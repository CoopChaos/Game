using System;
using System.Collections.Generic;
using System.Linq;
using CoopChaos;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

namespace CoopChaos
{
    [RequireComponent(typeof(LobbyStageState))]
    public class ServerLobbyStage : NetworkBehaviour
    {
        private LobbyStageState state;

        public override void OnNetworkSpawn()
        {
            if (!IsServer)
            {
                enabled = false;
                return;
            }
            
            NetworkManager.Singleton.OnClientConnectedCallback += HandleClientConnected;
            NetworkManager.Singleton.OnClientDisconnectCallback += HandleClientDisconnected;
            
            state = GetComponent<LobbyStageState>();
            state.OnToggleUserReady += HandleToggleUserReady;

            foreach (var client in NetworkManager.Singleton.ConnectedClientsList)
            {
                var user = NetworkManager.Singleton.ConnectedClients[client.ClientId]
                    .PlayerObject.GetComponent<UserPersistentBehaviour>();
                state.Users.Add(new LobbyStageState.UserModel(client.ClientId, false, user.name));
            }
        }

        public override void OnNetworkDespawn()
        {
            UnregisterCallbacks();
        }

        public override void OnDestroy()
        {
            UnregisterCallbacks();
        }

        private void UnregisterCallbacks()
        {
            NetworkManager.Singleton.OnClientConnectedCallback -= HandleClientConnected;
            NetworkManager.Singleton.OnClientDisconnectCallback -= HandleClientDisconnected;
            state.OnToggleUserReady -= HandleToggleUserReady;
        }

        private void HandleToggleUserReady(ulong clientId)
        {
            int clientIndex = state.Users.IndexWhere(u => u.ClientId == clientId);
            
            Assert.IsTrue(clientIndex != -1);
            
            state.Users[clientIndex] = new LobbyStageState.UserModel(
                state.Users[clientIndex].ClientId,
                !state.Users[clientIndex].Ready,
                state.Users[clientIndex].RawUsername);
            
            state.UserReadyChangedClientRpc(clientId);

            if (state.Users.All(u => u.Ready) && GameContext.Singleton.MinUserCount <= state.Users.Count)
            {
                NetworkManager.SceneManager.LoadScene("Game", LoadSceneMode.Single);
            }
        }

        private void HandleClientConnected(ulong clientId)
        {
            var user = NetworkManager.Singleton.ConnectedClients[clientId].PlayerObject.GetComponent<UserPersistentBehaviour>();
            
            state.Users.Add(new LobbyStageState.UserModel(clientId, false, user.name));
            state.UserConnectedClientRpc(clientId);
        }

        private void HandleClientDisconnected(ulong clientId)
        {
            state.Users.RemoveAt(state.Users.IndexWhere(u => u.ClientId == clientId));
            state.UserDisconnectedClientRpc(clientId);
        }
    }
}