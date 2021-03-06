using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using CoopChaos;
using CoopChaos.Shared;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;

namespace CoopChaos
{
    [RequireComponent(typeof(ConnectionManager))]
    public class ServerConnectionManager : MonoBehaviour
    {
        private const int MaxConnectPayload = 1024;
        
        [SerializeField] 
        private NetworkObject[] globalNetworkObjectPrefabs;
        
        private List<NetworkObject> globalNetworkObjects = new List<NetworkObject>();
        private ConnectionManager connectionManager;

        public bool StartServer(string ipAddress, int port)
        {
            NetworkManager.Singleton.NetworkConfig.NetworkTransport = connectionManager.NetworkTransport;
            connectionManager.NetworkTransport.SetConnectionData(ipAddress, (ushort) port);
            
            return NetworkManager.Singleton.StartServer();
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

        public void OnShutdown()
        {
            foreach (var globalNetworkObject in globalNetworkObjects)
            {
                Destroy(globalNetworkObject);
            }
            
            globalNetworkObjects.Clear();
        }

        private void Start()
        {
            connectionManager = GetComponent<ConnectionManager>();
            
            Assert.IsNotNull(connectionManager);
            
            NetworkManager.Singleton.ConnectionApprovalCallback += ApprovalCheck;
            NetworkManager.Singleton.OnServerStarted += ServerStartedHandler;
        }

        private void OnDestroy()
        {
            if (NetworkManager.Singleton != null)
            {
                NetworkManager.Singleton.ConnectionApprovalCallback -= ApprovalCheck;
                NetworkManager.Singleton.OnServerStarted -= ServerStartedHandler;
            }
        }

        private void ServerStartedHandler()
        {
            foreach (NetworkObject networkObjectPrefab in globalNetworkObjectPrefabs)
            {
                var networkObject = Instantiate(networkObjectPrefab);
                networkObject.Spawn();
                globalNetworkObjects.Add(networkObject);
            }
            
            // we need to manually handle local host because it is not handled by approval check
            if (NetworkManager.Singleton.IsHost)
            {
                var clientHash = UserConnectionMapper.TokenToClientHash(ClientSettings.GetToken());
                PrepareUser(NetworkManager.Singleton.LocalClientId, clientHash, "User-Host");
            }
            
            NetworkManager.Singleton.SceneManager.LoadScene("Lobby", LoadSceneMode.Single);
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
            if (UserConnectionMapper.Singleton.Count >= GameContextState.Singleton.GameContext.MaxUserCount)
            {
                CustomMessagingHelper.StartSend()
                    .Write(ConnectResult.ServerFull)
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
                    .Write(ConnectResult.InvalidPayload)
                    .Send(clientId, NetworkMessage.ConnectResult);

                StartCoroutine(WaitToDisconnect(clientId));
                return;
            }

            // we hash the token, so we can easily share it with other clients without worrying
            // about the client knowing the token
            var clientHash = UserConnectionMapper.TokenToClientHash(connectionPayload.Token);

            // ensure user is only connected once and old connection is disconnected
            if (UserConnectionMapper.Singleton.Contains(clientHash))
            {
                var oldClientId = UserConnectionMapper.Singleton[clientHash];
                
                CustomMessagingHelper.StartSend()
                    .Write(DisconnectReason.LoggedInAgain)
                    .Send(oldClientId, NetworkMessage.DisconnectReason);
                
                NetworkManager.Singleton.DisconnectClient(oldClientId);
            }
            
            // connection seems fine so we accpet it
            CustomMessagingHelper.StartSend()
                .Write(ConnectResult.Success)
                .Send(clientId, NetworkMessage.ConnectResult);
            
            connectionApprovedCallback(true, null, true, Vector3.zero, Quaternion.identity);
            PrepareUser(clientId, clientHash, connectionPayload.Username);
        }

        private void PrepareUser(ulong clientId, Guid clientHash, string username)
        {
            var user = NetworkManager.Singleton.ConnectedClients[clientId].PlayerObject
                .GetComponent<ServerUserPersistentBehaviour>();

            user.UserModel = new ServerUserModel(username, clientId, clientHash);
            UserConnectionMapper.Singleton.Add(clientHash, clientId);
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