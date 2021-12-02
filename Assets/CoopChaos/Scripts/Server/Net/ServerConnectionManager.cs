using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using CoopChaos;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Serialization;

namespace CoopChaos
{
    [RequireComponent(typeof(ConnectionManager))]
    public class ServerConnectionManager : MonoBehaviour
    {
        private const int MaxConnectPayload = 1024;

        [SerializeField] private NetworkObject initialStage;

        private MD5 md5 = MD5.Create();
        
        private ConnectionManager connectionManager;

        public void StartServer(string ipAddress, int port)
        {
            NetworkManager.Singleton.NetworkConfig.NetworkTransport = connectionManager.NetworkTransport;

            connectionManager.NetworkTransport.ConnectAddress = ipAddress;
            connectionManager.NetworkTransport.ConnectPort = port;
            
            NetworkManager.Singleton.StartServer();
        }

        public void OnNetworkReady()
        {
            if (!NetworkManager.Singleton.IsServer)
            {
                enabled = false;
                return;
            }
            
            NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnect;
        }

        private void Awake()
        {
            connectionManager = GetComponent<ConnectionManager>();
            
            NetworkManager.Singleton.ConnectionApprovalCallback += ApprovalCheck;
            NetworkManager.Singleton.OnServerStarted += ServerStartedHandler;
        }

        private void OnDestroy()
        {
            NetworkManager.Singleton.ConnectionApprovalCallback -= ApprovalCheck;
            NetworkManager.Singleton.OnServerStarted -= ServerStartedHandler;
        }

        private void ServerStartedHandler()
        {
            var stage = Instantiate(initialStage);
            stage.Spawn();
        }

        private void ApprovalCheck(byte[] connectionData, ulong clientId, 
            NetworkManager.ConnectionApprovedDelegate connectionApprovedCallback)
            // connectionApproved needs to be called OR client can be disconnected explicitly
        {
            // ensure we do not accept large connection payloads
            if (connectionData.Length > MaxConnectPayload)
            {
                connectionApprovedCallback(false, 0, false, null, null);
                return;
            }
            
            // ensure we always accept local host
            if (clientId == NetworkManager.Singleton.LocalClientId)
            {
                // TODO: not sure if this results in intended behaviour. we might need to register this client
                // and send a connect message but this was the way it was shown in the example project
                connectionApprovedCallback(true, null, true, null, null);
                return;
            }
            
            // ensure we have room for another connection
            if (ServerGameContext.Singleton.MaxUserCount >= UserConnectionMapper.Singleton.Count)
            {
                CustomMessagingHelper.StartSend()
                    .Write(ConnectStatus.ServerFull)
                    .Send(clientId, NetworkMessage.ConnectResult);

                StartCoroutine(WaitToDisconnect(clientId));
                return;
            }
            
            string payload = System.Text.Encoding.UTF8.GetString(connectionData);
            var connectionPayload = JsonUtility.FromJson<ConnectionPayload>(payload);

            // ensure user sent a valid payload
            if (connectionPayload.Verify() == false)
            {
                CustomMessagingHelper.StartSend()
                    .Write(ConnectStatus.InvalidPayload)
                    .Send(clientId, NetworkMessage.ConnectResult);

                StartCoroutine(WaitToDisconnect(clientId));
                return;
            }

            // we hash the token, so we can easily share it with other clients without worrying
            // about the client knowing the token
            var tokenHash = new Guid(md5.ComputeHash(connectionPayload.Token.ToByteArray()));

            // ensure user is only connected once and old connection is disconnected
            if (UserConnectionMapper.Singleton.Contains(tokenHash))
            {
                var oldClientId = UserConnectionMapper.Singleton[tokenHash];
                
                CustomMessagingHelper.StartSend()
                    .Write(ConnectStatus.LoggedInAgain)
                    .Send(oldClientId, NetworkMessage.DisconnectReason);
                
                NetworkManager.Singleton.DisconnectClient(oldClientId);
            }
            
            // connection seems fine so we accpet it
            CustomMessagingHelper.StartSend()
                .Write(ConnectStatus.Success)
                .Send(clientId, NetworkMessage.ConnectResult);
            
            UserConnectionMapper.Singleton.Add(tokenHash, clientId);
            connectionApprovedCallback(true, null, true, Vector3.zero, Quaternion.identity);
        }

        private void OnClientDisconnect(ulong clientId)
        {
            UserConnectionMapper.Singleton.Remove(clientId);

            if (clientId == NetworkManager.Singleton.LocalClientId)
            {
                // the ServerGameNetPortal may be initialized again, which will cause its OnNetworkSpawn to be called again.
                // Consequently we need to unregister anything we registered, when the NetworkManager is shutting down.
                NetworkManager.Singleton.OnClientDisconnectCallback -= OnClientDisconnect;
            }
        }

        // this might seem dirty, but is currently the only way to ensure
        // the client receives the disconnect reason. see Netcode Issue #796
        private IEnumerator WaitToDisconnect(ulong clientId)
        {
            yield return new WaitForSeconds(0.5f);
            NetworkManager.Singleton.DisconnectClient(clientId);
        }
    }
}