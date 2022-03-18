using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using CoopChaos.Shared;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;
using UnityEngine.Timeline;

namespace CoopChaos
{
    public class ServerGameStage : Stage
    {
        [SerializeField] private NetworkObject playerPrefab;
        [SerializeField] private Transform playerSpawn;

        public enum Roles
        {
            Pilot,
            Technician,
            Gunner,
        }

        private Dictionary<Guid, NetworkObject> players = new Dictionary<Guid, NetworkObject>();
        private Dictionary<ulong, ServerInteractableObjectBase> interactableObjects = new Dictionary<ulong, ServerInteractableObjectBase>();

        public override StageType Type => StageType.Game;

        public NetworkObject GetPlayerObjectByClientHash(Guid clientHash)
            => players[clientHash];
        
        
        public void RegisterInteractableObject(ServerInteractableObjectBase interactableObject)
        {
            Assert.IsTrue(!interactableObjects.ContainsKey(interactableObject.NetworkObjectId));
            interactableObjects.Add(interactableObject.NetworkObjectId, interactableObject);
        }
        
        public void InteractWith(ulong clientId, ulong interactableObjectId)
        {
            Assert.IsTrue(interactableObjects.ContainsKey(interactableObjectId));
            
            var interactableObject = interactableObjects[interactableObjectId];
            var player = GetPlayerObjectByClientHash(UserConnectionMapper.Singleton[clientId]);

            if (Vector2.Distance(interactableObject.gameObject.transform.position, player.transform.position) 
                > GameContextState.Singleton.GameContext.InteractRange)
            {
                Debug.LogWarning("Player interacted with object but is too far away");
                return;
            }
            
            interactableObject.Interact(clientId);
        }
        
        public override void OnNetworkSpawn()
        {
            if (!IsServer)
            {
                enabled = false;
                return;
            }

            NetworkManager.Singleton.OnClientConnectedCallback += HandleClientConnected;
            NetworkManager.Singleton.OnClientConnectedCallback += HandleClientDisconnected;

            List<Roles> roles = new List<Roles>();

            foreach(Roles key in Enum.GetValues(typeof(Roles)))
                roles.Add(key);

            // Shuffle List
            System.Random rng = new System.Random();
            
            int n = roles.Count;  
            while (n > 1) {  
                n--;  
                int k = rng.Next(n + 1);  
                Roles value = roles[k];  
                roles[k] = roles[n];  
                roles[n] = value;  
            }  

            
            int i = 0;
            foreach (NetworkClient client in NetworkManager.Singleton.ConnectedClientsList)
            {
                var player = Instantiate(playerPrefab, playerSpawn.position, playerSpawn.rotation);
                player.SpawnWithOwnership(client.ClientId);

                // random unity color
                var color = new Color(UnityEngine.Random.Range(0.4f, 1f), UnityEngine.Random.Range(0.4f, 1f), UnityEngine.Random.Range(0.4f, 1f));
                player.GetComponent<GameStageUser>().SetColor(color);
                
                players.Add(UserConnectionMapper.Singleton[client.ClientId], player);

                if(i < roles.Count)
                {
                    player.GetComponent<GameStageUser>().SetRoleClientRpc(roles[i]);
                    i++;
                }
            }
        }


        public override void OnNetworkDespawn()
        {
            UnregisterCallbacks();   
        }

        protected override void OnDestroy()
        {
            UnregisterCallbacks();
        }

        private void UnregisterCallbacks()
        {
            if (NetworkManager.Singleton != null)
            {
                NetworkManager.Singleton.OnClientConnectedCallback -= HandleClientConnected;
                NetworkManager.Singleton.OnClientConnectedCallback -= HandleClientDisconnected;
            }
        }

        private void HandleClientConnected(ulong clientId)
        {
            Guid clientHash = UserConnectionMapper.Singleton[clientId];
            if (players.ContainsKey(clientHash))
            {
                players[clientHash].ChangeOwnership(clientId);   
            }
            else
            {
                NetworkManager.DisconnectClient(clientId);   
            }
        }
        
        private void HandleClientDisconnected(ulong clientId)
        {
            Guid clientHash = UserConnectionMapper.Singleton[clientId];
            if (players.ContainsKey(clientHash))
            {
                players[clientHash].ChangeOwnership(clientId);
            }
        }
    }
}