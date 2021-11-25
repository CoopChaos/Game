using System;
using System.Collections.Generic;
using System.Linq;
using CoopChaos.Shared;
using Unity.Netcode;
using UnityEngine.SceneManagement;

namespace CoopChaos
{
    public class ServerLobbyPhase : NetworkBehaviour, IServerPhase
    {
        private Dictionary<Guid, PlayerModel> players = new Dictionary<Guid, PlayerModel>();

        public PhaseType Type => PhaseType.Lobby;
        public event Action<IServerPhase> OnPhaseChanged;

        public override void OnNetworkSpawn()
        {
            if (!IsServer)
            {
                return;
            }

            NetworkManager.Singleton.SceneManager.LoadScene("Lobby", LoadSceneMode.Single);
        }

        public ConnectStatus CanPlayerConnect(Guid playerId, string username)
        {
            if (players.Any(p => p.Value.Username == username))
            {
                return ConnectStatus.UsernameDuplicate;
            }

            return ConnectStatus.Success;
        }

        public void ConnectPlayer(Guid playerId, PlayerModel playerModel)
        {
            players.Add(playerId, playerModel);
        }

        public void DisconnectPlayer(Guid playerId)
        {
            players.Remove(playerId);
        }
    }
}