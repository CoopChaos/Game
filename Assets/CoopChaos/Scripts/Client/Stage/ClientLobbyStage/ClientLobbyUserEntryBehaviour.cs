using TMPro;
using UnityEngine;

namespace CoopChaos
{
    public class ClientLobbyUserEntryBehaviour : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI entryText;

        private string baseText;
        
        public void SetReady(bool ready)
        {
            entryText.text = (ready ? "(Ready)" : "(Not Ready)") + " " + baseText;
        }
        
        public void SetUser(LobbyStageState.UserModel user)
        {
            baseText = user.Username;
        }
    }
}