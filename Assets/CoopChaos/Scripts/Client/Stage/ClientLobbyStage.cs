using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CoopChaos
{
    public class ClientLobbyStage : Stage
    {
        [SerializeField] private GameObject userEntryPrefab;
        [SerializeField] private GameObject userEntryContainer;
        [SerializeField] private TextMeshProUGUI lobbyStatusText;
        
        private LobbyStageState state;
        private LobbyStageApi api;
        private int maxPlayerCount = 8;

        private Dictionary<Guid, ClientLobbyUserEntryBehaviour> userEntries = new Dictionary<Guid, ClientLobbyUserEntryBehaviour>(); 

        public override StageType Type => StageType.Lobby;

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            
            if (!IsClient)
            {
                enabled = false;
                return;
            }

            state = GetComponent<LobbyStageState>();
            api = GetComponent<LobbyStageApi>();
            
            UpdateLobbyUIState();

            state.OnUserConnected += HandleOnUserConnected;
            state.OnUserDisconnected += HandleOnUserDisconnected;
            state.OnUserReadyChanged += HandleOnUserReadyChanged;

            foreach (var user in state.Users)
            {
                AddUserEntry(user);
            }
        }

        public void OnSelectToggleReady()
        {
            api.ToggleReadyServerRpc(ClientSettings.GetToken());
        }

        public void OnSelectDisconnect()
        {
            FindObjectOfType<ClientConnectionManager>().StopClient();
        }

        private void UpdateLobbyUIState()
        {
            lobbyStatusText.SetText($"({state.Users.Count} / {maxPlayerCount}) in Lobby - {maxPlayerCount - state.Users.Count} remaining");
        }

        private void HandleOnUserConnected(LobbyStageState.UserModel user)
        {
            if (userEntries.ContainsKey(user.ClientHash))
                return;
            
            AddUserEntry(user);
            UpdateLobbyUIState();
        }
        
        private void AddUserEntry(LobbyStageState.UserModel user)
        {
            var entry = Instantiate(userEntryPrefab, userEntryContainer.transform).GetComponent<ClientLobbyUserEntryBehaviour>();
            entry.SetUser(user);
            userEntries[user.ClientHash] = entry;
        }

        private void HandleOnUserDisconnected(Guid clientHash)
        {
            if (!userEntries.ContainsKey(clientHash))
                return;

            Destroy(userEntries[clientHash].gameObject);
            userEntries.Remove(clientHash);
            UpdateLobbyUIState();
        }

        private void HandleOnUserReadyChanged(Guid clientHash, bool isReady)
        {
            userEntries[clientHash].SetReady(isReady);
        }
    }
}