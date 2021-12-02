using System;
using System.Text;
using Unity.Netcode;
using Unity.Netcode.Transports.UNET;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CoopChaos
{
    [RequireComponent(typeof(ConnectionManager))]
    public class ClientConnectionManager : MonoBehaviour
    {
        private const int TimeoutDuration = 10;

        private ConnectionManager connectionManager;
        
        public event Action<ConnectStatus> OnDisconnected;
        public event Action<ConnectStatus> OnConnected;
        
        public void StartClient(string ipAddress, int port)
        {
            NetworkManager.Singleton.NetworkConfig.NetworkTransport = connectionManager.NetworkTransport;

            connectionManager.NetworkTransport.ConnectAddress = ipAddress;
            connectionManager.NetworkTransport.ConnectPort = port;

            var token = ClientSettings.GetToken();

            NetworkManager.Singleton.NetworkConfig.ConnectionData = Encoding.UTF8.GetBytes(
                JsonUtility.ToJson(
                    new ConnectionPayload()
                    {
                        Token = token,
                        Username = "User" + (new System.Random()).Next()
                    }));

            NetworkManager.Singleton.NetworkConfig.ClientConnectionBufferTimeout = TimeoutDuration;
            NetworkManager.Singleton.StartClient();
            
            NetworkManager.Singleton.CustomMessagingManager.RegisterNamedMessageHandler(
                NetworkMessage.ConnectResult.ToString(),
                HandleConnectResultMessage);
            NetworkManager.Singleton.CustomMessagingManager.RegisterNamedMessageHandler(
                NetworkMessage.DisconnectReason.ToString(),
                HandleDisconnectResultMessage);
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

        private void Awake()
        {
            connectionManager = GetComponent<ConnectionManager>();
        }

        private void HandleConnectResultMessage(ulong clientID, FastBufferReader reader)
        {
            reader.ReadValueSafe(out ConnectStatus status);
            OnConnected?.Invoke(status);
            
            if (status != ConnectStatus.Success)
                OnDisconnected?.Invoke(status);
        }

        private void HandleDisconnectResultMessage(ulong clientId, FastBufferReader reader)
        {
            reader.ReadValueSafe(out ConnectStatus status);
            OnDisconnected?.Invoke(status);
        }
    }
}