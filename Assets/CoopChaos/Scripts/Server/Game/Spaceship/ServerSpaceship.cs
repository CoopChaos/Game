using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Assertions;

namespace CoopChaos
{
    public class ServerSpaceship : NetworkBehaviour
    {
        private ServerGameStage serverGameStage;
        private Dictionary<int, ServerInteractableObjectBase> interactableObjects = new Dictionary<int, ServerInteractableObjectBase>();

        public void InteractWith(ulong clientId, int interactableObjectId)
        {
            Assert.IsTrue(interactableObjects.ContainsKey(interactableObjectId));
            var interactableObject = interactableObjects[interactableObjectId];
            var player = serverGameStage.GetPlayerObjectByClientHash(UserConnectionMapper.Singleton[clientId]);

            if (Vector2.Distance(interactableObject.gameObject.transform.position, player.transform.position) 
                > GameContext.Singleton.InteractRange)
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

            serverGameStage = FindObjectOfType<ServerGameStage>();
            Assert.IsNotNull(serverGameStage);
        }

        private void Start()
        {
            interactableObjects = GetComponentsInChildren<InteractableObjectStateBase>().ToDictionary(
                i => i.InteractableObjectId,
                i => i.GetComponent<ServerInteractableObjectBase>());
        }
    }
}