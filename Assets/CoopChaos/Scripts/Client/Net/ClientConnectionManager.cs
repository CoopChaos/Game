using System;
using System.Text;
using Unity.Netcode;
using Unity.Netcode.Transports.UNET;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

namespace CoopChaos
{
    [RequireComponent(typeof(ConnectionManager))]
    public class ClientConnectionManager : MonoBehaviour
    {
        private const int TimeoutDuration = 10;

        private ConnectionManager connectionManager;
        
        private ConnectResult lastConnectResult;
        private DisconnectReason lastDisconnectReason;
        
        public event Action<ConnectResult> OnConnectingFinished;
        public event Action<DisconnectReason> OnDisconnected;

        
        public bool StartClient(string ipAddress, int port)
        {
            lastConnectResult = ConnectResult.Undefined;
            lastDisconnectReason = DisconnectReason.Undefined;
            
            NetworkManager.Singleton.NetworkConfig.NetworkTransport = connectionManager.NetworkTransport;
            connectionManager.NetworkTransport.SetConnectionData(ipAddress, (ushort) port);

            var token = ClientSettings.GetToken();

            NetworkManager.Singleton.NetworkConfig.ConnectionData = Encoding.UTF8.GetBytes(
                JsonUtility.ToJson(
                    new ConnectionPayload()
                    {
                        Token = token,
                        Username = "User" + (new System.Random()).Next()
                    }));

            NetworkManager.Singleton.NetworkConfig.ClientConnectionBufferTimeout = TimeoutDuration;

            if (!NetworkManager.Singleton.StartClient())
                return false;

            NetworkManager.Singleton.CustomMessagingManager.RegisterNamedMessageHandler(
                NetworkMessage.ConnectResult.ToString(),
                HandleConnectResultMessage);
            NetworkManager.Singleton.CustomMessagingManager.RegisterNamedMessageHandler(
                NetworkMessage.DisconnectReason.ToString(),
                HandleDisconnectReasonMessage);

            return true;
        }

        public void StopClient()
        {
            NetworkManager.Singleton.Shutdown();
            SceneManager.LoadScene("MainMenu");
        }

        public void OnNetworkReady()
        {
            if (!NetworkManager.Singleton.IsClient)
            {
                enabled = false;
            }
        }
        
        public void OnShutdown()
        {
            OnDisconnected?.Invoke(DisconnectReason.UserRequestedDisconnect);
        }

        private void Awake()
        {
            connectionManager = GetComponent<ConnectionManager>();
            Assert.IsNotNull(connectionManager);
        }

        private void Start()
        {
            NetworkManager.Singleton.OnClientDisconnectCallback += HandleOnClientDisconnect;
        }

        private void OnDestroy()
        {
            if (NetworkManager.Singleton != null)
            {
                NetworkManager.Singleton.CustomMessagingManager.UnregisterNamedMessageHandler(
                    NetworkMessage.ConnectResult.ToString());
                NetworkManager.Singleton.CustomMessagingManager.UnregisterNamedMessageHandler(
                    NetworkMessage.DisconnectReason.ToString());

                NetworkManager.Singleton.OnClientDisconnectCallback -= HandleOnClientDisconnect;
            }
        }

        private void HandleOnClientDisconnect(ulong clientId)
        {
            NetworkManager.Singleton.Shutdown();

            // handle disconnect on connecting
            if (lastConnectResult == ConnectResult.Undefined)
            {
                lastConnectResult = ConnectResult.Timeout;
            }
            
            if (SceneManager.GetActiveScene().name != "MainMenu")
            {
                SceneManager.LoadScene("MainMenu");
            }

            if (lastConnectResult != ConnectResult.Success)
            {
                OnConnectingFinished?.Invoke(lastConnectResult);
                return;
            }
            
            // handle disconnect on running
            if (lastDisconnectReason == DisconnectReason.Undefined)
            {
                lastDisconnectReason = DisconnectReason.Timeout;
            }
            
            OnDisconnected?.Invoke(lastDisconnectReason);
        }

        private void HandleConnectResultMessage(ulong clientID, FastBufferReader reader)
        {
            reader.ReadValueSafe(out ConnectResult status);
            
            if (status == ConnectResult.Success)
                OnConnectingFinished?.Invoke(status);
            
            lastConnectResult = status;
        }

        private void HandleDisconnectReasonMessage(ulong clientId, FastBufferReader reader)
        {
            reader.ReadValueSafe(out DisconnectReason status);
            lastDisconnectReason = status;
        }
    }
}