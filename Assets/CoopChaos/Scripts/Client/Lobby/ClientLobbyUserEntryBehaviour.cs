using System;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

namespace CoopChaos
{
    public class ClientLobbyUserEntryBehaviour : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI entryText;

        private string baseText;
        private bool lastReady;
        
        public void SetReady(bool ready)
        {
            lastReady = ready;
            UpdateText();
        }
        
        public void SetUser(LobbyStageState.UserModel user)
        {
            baseText = user.Username;
            UpdateText();
        }

        private void UpdateText()
        {
            entryText.text = (lastReady ? "(Ready)" : "(Not Ready)") + " " + baseText;
        }

        private void Awake()
        {
            Assert.IsNotNull(entryText);
        }
    }
}