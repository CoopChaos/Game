using System;
using Unity.Netcode;
using Unity.Netcode.Transports.UNET;
using UnityEngine;
using UnityEngine.Assertions;

namespace CoopChaos
{
    [RequireComponent(typeof(ClientConnectionManager), typeof(ServerConnectionManager), typeof(UserConnectionMapper))]
    public class ConnectionManager : MonoBehaviour
    {
        [SerializeField] 
        private UNetTransport networkTransport;

        private ServerConnectionManager serverConnectionManager;
        private ClientConnectionManager clientConnectionManager;

        public static ConnectionManager Instance { get; private set; }

        public UNetTransport NetworkTransport => networkTransport;
        
        private void Awake()
        {
            Assert.IsNull(Instance);
            Instance = this;
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