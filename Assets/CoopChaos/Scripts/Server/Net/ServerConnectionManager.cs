using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CoopChaos.Shared;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Serialization;

namespace CoopChaos
{
    public class ServerConnectionManager : MonoBehaviour
    {
        private const int MaxConnectPayload = 1024;

        [FormerlySerializedAs("phaseManager")] [FormerlySerializedAs("gameManager")] [SerializeField] private NetworkObject manager;
        
        private ConnectionManager connectionManager;
        private NetworkObject initialPhase;

        private Dictionary<ulong, Guid> playerIdToGuid = new Dictionary<ulong, Guid>();
        private IServerPhase currentPhase;

        public void OnNetworkReady()
        {
            if (!NetworkManager.Singleton.IsServer)
            {
                enabled = false;
                return;
            }
            
            NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnect;
        }

        private void Start()
        {
            connectionManager = GetComponent<ConnectionManager>();
            NetworkManager.Singleton.ConnectionApprovalCallback += ApprovalCheck;
            NetworkManager.Singleton.OnServerStarted += ServerStartedHandler;
        }

        private void ServerStartedHandler()
        {
            // when server started, directly start first phase
            var phase = Instantiate(initialPhase);
            phase.Spawn();
            currentPhase = phase.GetComponent<ServerLobbyPhase>();
            currentPhase.OnPhaseChanged += PhaseChangedHandler;
        }
        
        private void PhaseChangedHandler(IServerPhase newPhase)
        {
            currentPhase.OnPhaseChanged -= PhaseChangedHandler;
            currentPhase = newPhase;
            currentPhase.OnPhaseChanged += PhaseChangedHandler;
        }

        private void ApprovalCheck(byte[] connectionData, ulong clientId, NetworkManager.ConnectionApprovedDelegate connectionApprovedCallback)
        {
            if (connectionData.Length > MaxConnectPayload)
            {
                connectionApprovedCallback(false, 0, false, null, null);
                return;
            }

            // Approval check happens for Host too, but obviously we want it to be approved
            if (clientId == NetworkManager.Singleton.LocalClientId)
            {
                connectionApprovedCallback(true, null, true, null, null);
                return;
            }
            
            // Test for over-capacity connection. This needs to be done asap, to make sure we refuse connections asap and don't spend useless time server side
            // on invalid users trying to connect
            // todo this is currently still spending too much time server side.
            if (GameContext.Singleton.MaxPlayerCount >= playerIdToGuid.Count)
            {
                // TODO-FIXME:Netcode Issue #796. We should be able to send a reason and disconnect without a coroutine delay.
                // TODO:Netcode: In the future we expect Netcode to allow us to return more information as part of
                // the approval callback, so that we can provide more context on a reject. In the meantime we must provide the extra information ourselves,
                // and then manually close down the connection.
                SendServerToClientConnectResult(clientId, ConnectStatus.ServerFull);
                SendServerToClientSetDisconnectReason(clientId, ConnectStatus.ServerFull);
                StartCoroutine(WaitToDisconnect(clientId));
                return;
            }
            
            string payload = System.Text.Encoding.UTF8.GetString(connectionData);
            var connectionPayload = JsonUtility.FromJson<ConnectionPayload>(payload); // https://docs.unity3d.com/2020.2/Documentation/Manual/JSONSerialization.html

            if (playerIdToGuid.ContainsValue(connectionPayload.Guid))
            {
                var oldClientId = playerIdToGuid.First(p => p.Value == connectionPayload.Guid).Key;
                
                // kicking old client to leave only current
                SendServerToClientSetDisconnectReason(oldClientId, ConnectStatus.LoggedInAgain);
                NetworkManager.Singleton.DisconnectClient(oldClientId);
            }

            var status = currentPhase.CanPlayerConnect(connectionPayload.Guid, connectionPayload.Username);
            if (status != ConnectStatus.Success)
            {
                SendServerToClientConnectResult(clientId, ConnectStatus.ServerFull);
                SendServerToClientSetDisconnectReason(clientId, ConnectStatus.ServerFull);
                StartCoroutine(WaitToDisconnect(clientId));
                return;
            }

            SendServerToClientConnectResult(clientId, ConnectStatus.Success);

            playerIdToGuid.Add(clientId, connectionPayload.Guid);
            connectionApprovedCallback(true, null, true, Vector3.zero, Quaternion.identity);
        }
        
        /// <summary>
        /// Responsible for the Server->Client custom message of the connection result.
        /// </summary>
        /// <param name="clientID"> id of the client to send to </param>
        /// <param name="status"> the status to pass to the client</param>
        public void SendServerToClientConnectResult(ulong clientID, ConnectStatus status)
        {
            var writer = new FastBufferWriter(sizeof(ConnectStatus), Allocator.Temp);
            writer.WriteValueSafe(status);
            NetworkManager.Singleton.CustomMessagingManager.SendNamedMessage(nameof(NetworkMessage.ConnectResult), clientID, writer);
        }
        
        /// <summary>
        /// Sends a DisconnectReason to the indicated client. This should only be done on the server, prior to disconnecting the client.
        /// </summary>
        /// <param name="clientID"> id of the client to send to </param>
        /// <param name="status"> The reason for the upcoming disconnect.</param>
        public void SendServerToClientSetDisconnectReason(ulong clientID, ConnectStatus status)
        {
            var writer = new FastBufferWriter(sizeof(ConnectStatus), Allocator.Temp);
            writer.WriteValueSafe(status);
            NetworkManager.Singleton.CustomMessagingManager.SendNamedMessage(nameof(NetworkMessage.DisconnectReason), clientID, writer);
        }
        
        private IEnumerator WaitToDisconnect(ulong clientId)
        {
            yield return new WaitForSeconds(0.5f);
            NetworkManager.Singleton.DisconnectClient(clientId);
        }
        
        private void OnClientDisconnect(ulong clientId)
        {
            if(playerIdToGuid.TryGetValue(clientId, out var guid))
            {
                playerIdToGuid.Remove(clientId);
                currentPhase.DisconnectPlayer(guid);
            }

            if( clientId == NetworkManager.Singleton.LocalClientId )
            {
                // the ServerGameNetPortal may be initialized again, which will cause its OnNetworkSpawn to be called again.
                // Consequently we need to unregister anything we registered, when the NetworkManager is shutting down.
                NetworkManager.Singleton.OnClientDisconnectCallback -= OnClientDisconnect;
            }
        }
    }
}