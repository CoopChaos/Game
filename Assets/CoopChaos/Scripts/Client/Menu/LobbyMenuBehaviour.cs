using Unity.Netcode;
using UnityEngine;

namespace CoopChaos.Menu
{
    public class LobbyMenuBehaviour : MonoBehaviour
    {
        [SerializeField] private GameObject joinMenu;

        private ClientConnectionManager clientConnectionManager;
        private LobbyStageState lobbyStageState;

        public void OnSelectDisconnect()
        {
            clientConnectionManager.StopClient();
        }

        public void OnSelectToggleReady()
        {
            
        }
    }
}