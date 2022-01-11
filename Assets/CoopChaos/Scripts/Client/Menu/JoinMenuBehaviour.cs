using System;
using TMPro;
using Unity.Netcode;
using Unity.Netcode.Transports.UNET;
using UnityEngine;

namespace Yame.Menu
{
    public class JoinMenuBehaviour : MonoBehaviour
    {
        [SerializeField] private GameObject joinMenu;
        [SerializeField] private GameObject loadingPage;
        
        [SerializeField]
        private TMP_InputField ipAddress;
        
        [SerializeField]
        private ErrorMessageBehaviour errorMessage;

        private ClientConnectionManager clientConnectionManager;
        private ServerConnectionManager serverConnectionManager;
        private ConnectionManager connectionManager;
        
        public void OnSelectJoin()
        {
            if (clientConnectionManager.StartClient(ipAddress.text, 29909) == false)
            {
                errorMessage.SetErrorMessage("Failed to join");
            }
            else
            {
                joinMenu.SetActive(false);
                loadingPage.SetActive(true);
            }
        }
        
        public void OnSelectServer()
        {
            if (connectionManager.StartHost(ipAddress.text, 29909) == false)
            {
                errorMessage.SetErrorMessage("Failed to start server");
            }
            else
            {
                joinMenu.SetActive(false);
                loadingPage.SetActive(true);
            }
        }

        private void HandleOnConnectingFinished(ConnectResult result)
        {
            errorMessage.SetErrorMessage($"Failed to join ({result})");
            joinMenu.SetActive(true);
            loadingPage.SetActive(false);
        }
        
        private void HandleOnDisconnected(DisconnectReason reason)
        {
            errorMessage.SetErrorMessage($"Disconnected from server ({reason})");
            joinMenu.SetActive(true);
            loadingPage.SetActive(false);
        }

        private void Awake()
        {
            clientConnectionManager = FindObjectOfType<ClientConnectionManager>();
            serverConnectionManager = FindObjectOfType<ServerConnectionManager>();
            connectionManager = FindObjectOfType<ConnectionManager>();
            
            clientConnectionManager.OnDisconnected += HandleOnDisconnected;
            clientConnectionManager.OnConnectingFinished += HandleOnConnectingFinished;
        }

        private void OnDestroy()
        {
            clientConnectionManager.OnDisconnected -= HandleOnDisconnected;
            clientConnectionManager.OnConnectingFinished -= HandleOnConnectingFinished;
        }
    }
}