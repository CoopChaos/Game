using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;

namespace Yame
{
    public class ServerGameStage : Stage
    {
        [SerializeField] private NetworkObject playerPrefab;
        [SerializeField] private Transform playerSpawn;

        private Dictionary<Guid, NetworkObject> players = new Dictionary<Guid, NetworkObject>();

        public override StageType Type => StageType.Game;

        public NetworkObject GetPlayerObjectByClientHash(Guid clientHash)
            => players[clientHash];

        public override void OnNetworkSpawn()
        {
            if (!IsServer)
            {
                enabled = false;
                return;
            }

            NetworkManager.Singleton.OnClientConnectedCallback += HandleClientConnected;
            NetworkManager.Singleton.OnClientConnectedCallback += HandleClientDisconnected;
            
            foreach (NetworkClient client in NetworkManager.Singleton.ConnectedClientsList)
            {
                var player = Instantiate(playerPrefab, playerSpawn.position, playerSpawn.rotation);
                player.SpawnWithOwnership(client.ClientId);
                
                // random unity color
                var color = new Color(UnityEngine.Random.Range(0.4f, 1f), UnityEngine.Random.Range(0.4f, 1f), UnityEngine.Random.Range(0.4f, 1f));
                player.GetComponent<GameStageUser>().SetColor(color);
                
                players.Add(UserConnectionMapper.Singleton[client.ClientId], player);
            }
        }

        public override void OnNetworkDespawn()
        {
            UnregisterCallbacks();   
        }

        protected override void OnDestroy()
        {
            UnregisterCallbacks();
        }

        private void UnregisterCallbacks()
        {
            if (NetworkManager.Singleton != null)
            {
                NetworkManager.Singleton.OnClientConnectedCallback -= HandleClientConnected;
                NetworkManager.Singleton.OnClientConnectedCallback -= HandleClientDisconnected;
            }
        }

        private void HandleClientConnected(ulong clientId)
        {
            Guid clientHash = UserConnectionMapper.Singleton[clientId];
            if (players.ContainsKey(clientHash))
            {
                players[clientHash].ChangeOwnership(clientId);   
            }
            else
            {
                NetworkManager.DisconnectClient(clientId);   
            }
        }
        
        private void HandleClientDisconnected(ulong clientId)
        {
            Guid clientHash = UserConnectionMapper.Singleton[clientId];
            if (players.ContainsKey(clientHash))
            {
                players[clientHash].ChangeOwnership(clientId);
            }
        }
    }
}