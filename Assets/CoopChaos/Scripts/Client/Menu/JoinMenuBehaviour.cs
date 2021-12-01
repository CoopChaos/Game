using System;
using TMPro;
using UnityEngine;

namespace CoopChaos.Menu
{
    public class JoinMenuBehaviour : MonoBehaviour
    {
        [SerializeField]
        private TMP_InputField ipAddress;

        [SerializeField] 
        private GameObject lobbyMenu;
        
        private ClientConnectionManager clientConnectionManager;
        private ServerConnectionManager serverConnectionManager;

        public void OnSelectJoin()
        {
            clientConnectionManager.StartClient(ipAddress.text, 29909);

            gameObject.SetActive(false);
            lobbyMenu.SetActive(true);
        }
        
        public void OnSelectServer()
        {
            serverConnectionManager.StartServer(ipAddress.text, 29909);
            gameObject.SetActive(false);
        }

        private void Awake()
        {
            clientConnectionManager = FindObjectOfType<ClientConnectionManager>();
            serverConnectionManager = FindObjectOfType<ServerConnectionManager>();
        }
    }
}