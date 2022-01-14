using System;
using System.Collections.Generic;
using CoopChaos.Shared;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

namespace CoopChaos
{
    public class ClientLobbyStage : Stage
    {
        [SerializeField] private GameObject userEntryPrefab;
        [SerializeField] private GameObject userEntryContainer;
        [SerializeField] private TextMeshProUGUI lobbyStatusText;
        
        private LobbyStageState state;
        private LobbyStageUserApi _lobbyUserApi;
        
        private Dictionary<Guid, ClientLobbyUserEntryBehaviour> userEntries = new Dictionary<Guid, ClientLobbyUserEntryBehaviour>(); 

        public override StageType Type => StageType.Lobby;

        // we load the lobbyuser lazy because it does not exist in scene directly on spawn of
        // ClientLobbyStage but has to be synchronized
        private LobbyStageUserApi LazyLobbyUserApi
        {
            get
            {
                if (_lobbyUserApi == null)
                {
                    foreach (var lobbyUser in FindObjectsOfType<LobbyStageUserApi>())
                    {
                        if (lobbyUser.IsOwner)
                        {
                            this._lobbyUserApi = lobbyUser;
                            break;
                        }
                    }

                    Assert.IsNotNull(_lobbyUserApi);
                }

                return _lobbyUserApi;
            }
        }

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            
            Assert.IsNotNull(userEntryPrefab);
            Assert.IsNotNull(userEntryContainer);
            Assert.IsNotNull(lobbyStatusText);
            
            if (!IsClient)
            {
                enabled = false;
                return;
            }

            state = GetComponent<LobbyStageState>();
            Assert.IsNotNull(state);
            
            UpdateLobbyUIState();

            state.Users.OnListChanged += HandleOnUsersListChanged;

            foreach (var userInLobby in state.Users)
            {
                AddUserEntry(userInLobby);
            }
        }

        public void OnSelectToggleReady()
        {
            LazyLobbyUserApi.ToggleReadyServerRpc();
        }

        public void OnSelectDisconnect()
        {
            FindObjectOfType<ClientConnectionManager>().StopClient();
        }
        
        private void HandleOnUsersListChanged(NetworkListEvent<LobbyStageState.UserModel> listEvent)
        {
            switch (listEvent.Type.ToAbstract())
            {
                case AbstractNetworkListEvent.Add:
                    if (userEntries.ContainsKey(listEvent.Value.ClientHash))
                        return;

                    AddUserEntry(listEvent.Value);
                    UpdateLobbyUIState();
                    
                    break;
                case AbstractNetworkListEvent.Change:
                    userEntries[listEvent.Value.ClientHash].SetReady(listEvent.Value.Ready);
                    
                    break;
                case AbstractNetworkListEvent.Clear:
                    userEntries.Clear();
                    
                    break;
                case AbstractNetworkListEvent.Remove:
                    Destroy(userEntries[listEvent.Value.ClientHash].gameObject);
                    userEntries.Remove(listEvent.Value.ClientHash);
                    UpdateLobbyUIState();
                    
                    break;
            }
        }

        private void AddUserEntry(LobbyStageState.UserModel user)
        {
            var entry = Instantiate(userEntryPrefab, userEntryContainer.transform).GetComponent<ClientLobbyUserEntryBehaviour>();
            entry.SetUser(user);
            userEntries[user.ClientHash] = entry;
        }

        private void UpdateLobbyUIState()
        {
            lobbyStatusText.SetText(state.Users.Count >= GameContextState.Singleton.GameContext.MinUserCount
                ? $"({state.Users.Count} / {GameContextState.Singleton.GameContext.MaxUserCount}) in Lobby (set ready to start)"
                : $"({state.Users.Count} / {GameContextState.Singleton.GameContext.MaxUserCount}) in Lobby - {GameContextState.Singleton.GameContext.MinUserCount - state.Users.Count} remaining");
        }
    }
}