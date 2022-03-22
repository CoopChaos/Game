using System;
using System.Collections.Generic;
using System.Linq;
using CoopChaos;
using CoopChaos.Shared;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

namespace CoopChaos
{
    [RequireComponent(typeof(LobbyStageState))]
    public class ServerLobbyStage : Stage
    {
        private LobbyStageState state;
        
        [SerializeField]
        private NetworkObject lobbyUserPrefab;

        public override StageType Type => StageType.Lobby;

        public void ToggleUserReady(ulong clientId)
        {
            var clientHash = UserConnectionMapper.Singleton[clientId];
            int clientIndex = state.Users.IndexWhere(u => u.ClientHash == clientHash);
            
            Assert.IsTrue(clientIndex != -1);
            
            state.Users[clientIndex] = new LobbyStageState.UserModel(
                state.Users[clientIndex].Id,
                state.Users[clientIndex].ClientHash,
                !state.Users[clientIndex].Ready,
                state.Users[clientIndex].RawUsername);

            if (state.Users.All(u => u.Ready) && GameContextState.Singleton.GameContext.MinUserCount <= state.Users.Count)
            {
                NetworkManager.SceneManager.LoadScene("StartScreen", LoadSceneMode.Single);
            }
        }

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            
            Assert.IsNotNull(lobbyUserPrefab);
            
            if (!IsServer)
            {
                enabled = false;
                return;
            }
            
            NetworkManager.Singleton.OnClientConnectedCallback += HandleClientConnected;
            NetworkManager.Singleton.OnClientDisconnectCallback += HandleClientDisconnected;
            
            state = GetComponent<LobbyStageState>();
            Assert.IsNotNull(state);
            
            foreach (var client in NetworkManager.Singleton.ConnectedClientsList)
            {
                AddLobbyUser(client.ClientId);
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
                NetworkManager.Singleton.OnClientDisconnectCallback -= HandleClientDisconnected;
            }
        }

        private void HandleClientConnected(ulong clientId)
        {
            AddLobbyUser(clientId);
        }
        

        private void HandleClientDisconnected(ulong clientId)
        {
            state.Users.RemoveAt(state.Users.IndexWhere(u => u.Id == clientId));
        }

        private void AddLobbyUser(ulong clientId)
        {
            var client = NetworkManager.Singleton.ConnectedClients[clientId];
            
            var lobbyUser = Instantiate(lobbyUserPrefab);
            lobbyUser.SpawnWithOwnership(clientId, true);
            
            var user = NetworkManager.Singleton.ConnectedClients[client.ClientId].PlayerObject
                .GetComponent<ServerUserPersistentBehaviour>();
            state.Users.Add(new LobbyStageState.UserModel(user.UserModel.ClientId, user.UserModel.ClientHash, false, user.UserModel.Username));
        }
    }
}