using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Assertions;

namespace CoopChaos
{
    [RequireComponent(typeof(ClientConnectionManager), typeof(ServerConnectionManager))]
    public class ConnectionManager : MonoBehaviour
    {
        private NetworkManager networkManager = NetworkManager.Singleton;
        private ServerConnectionManager serverConnectionManager;
        private ClientConnectionManager clientConnectionManager;
        
        public static ConnectionManager Instance { get; private set; }
        
        public NetworkManager NetworkManager => networkManager;

        private void Awake()
        {
            Assert.IsNull(Instance);
            Instance = this;
            
            
        }

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
            
            networkManager.OnServerStarted += OnNetworkReady;
        }

        private void OnNetworkReady()
        {
            serverConnectionManager.OnNetworkReady();
            clientConnectionManager.OnNetworkReady();
        }
    }
}