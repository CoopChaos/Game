using System;
using Unity.Netcode;
using Unity.Netcode.Transports.UNET;
using UnityEngine;
using UnityEngine.Assertions;

namespace CoopChaos
{
    [RequireComponent(typeof(ClientConnectionManager), typeof(ServerConnectionManager))]
    public class ConnectionManager : MonoBehaviour
    {
        // we need to use the new unitytransport because the old unet transport causes
        // an exception when the client is unable to connect to the server without any timeout
        // event called
        [SerializeField]
        private UnityTransport networkTransport;

        private ServerConnectionManager serverConnectionManager;
        private ClientConnectionManager clientConnectionManager;

        public static ConnectionManager Instance { get; private set; }
        
        public UnityTransport NetworkTransport => networkTransport;

        // host starts the server and client on the same machine
        public bool StartHost(string ipAddress, int port)
        {
            NetworkManager.Singleton.NetworkConfig.NetworkTransport = NetworkTransport;
            NetworkTransport.SetConnectionData(ipAddress, (ushort) port);
            
            return NetworkManager.Singleton.StartHost();
        }

        // disconnects the client or stops the server
        public void Disconnect()
        {
            if (NetworkManager.Singleton.IsServer)
                serverConnectionManager.OnShutdown();
            
            if (NetworkManager.Singleton.IsClient)
                clientConnectionManager.OnShutdown();

            NetworkManager.Singleton.Shutdown();
        }

        private void Awake()
        {
            Assert.IsNull(Instance);
            Instance = this;
            
            serverConnectionManager = GetComponent<ServerConnectionManager>();
            clientConnectionManager = GetComponent<ClientConnectionManager>();
            
            Assert.IsNotNull(serverConnectionManager);
            Assert.IsNotNull(clientConnectionManager);
        }
        
        private void Start()
        {
            DontDestroyOnLoad(gameObject);
            
            NetworkManager.Singleton.OnServerStarted += OnNetworkReady;
        }

        private void OnNetworkReady()
        {
            serverConnectionManager.OnNetworkReady();
            clientConnectionManager.OnNetworkReady();
        }
    }
}