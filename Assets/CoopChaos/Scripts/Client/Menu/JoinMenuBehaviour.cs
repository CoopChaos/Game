using System;
using System.Net;
using TMPro;
using Unity.Netcode;
using Unity.Netcode.Transports.UNET;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace CoopChaos.Menu
{
    public class JoinMenuBehaviour : MonoBehaviour
    {
        [SerializeField] private GameObject joinMenu;
        [SerializeField] private GameObject loadingPage;

        [SerializeField] private Button cancelButton;
        
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
            ipAddress.text = GetLocalIPAddress();

            IPAddress[] ipv4Addresses = Array.FindAll(
                Dns.GetHostEntry(string.Empty).AddressList,
                a => true);
                
            Debug.Log($"Found {ipv4Addresses.Length} IPv4 addresses");
            foreach (var ipAddress in ipv4Addresses)
            {
                Debug.Log($"IPv4 address: {ipAddress}");
            }


            cancelButton.onClick.AddListener(() =>
            {
                clientConnectionManager.StopClient();
                joinMenu.SetActive(true);
                loadingPage.SetActive(false);
            });
            
            clientConnectionManager.OnDisconnected += HandleOnDisconnected;
            clientConnectionManager.OnConnectingFinished += HandleOnConnectingFinished;
        }

        private string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }

            return "";
        }

        private string GetGlobalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6)
                {
                    return ip.ToString();
                }
            }

            return "";
        }

        private void OnDestroy()
        {
            clientConnectionManager.OnDisconnected -= HandleOnDisconnected;
            clientConnectionManager.OnConnectingFinished -= HandleOnConnectingFinished;
        }
    }
}