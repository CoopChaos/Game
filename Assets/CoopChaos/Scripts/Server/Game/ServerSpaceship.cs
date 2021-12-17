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
        private Dictionary<int, InteractableObjectStateBase> interactableObjects
            = new Dictionary<int, InteractableObjectStateBase>();

        public void InteractFromPlayer(ulong clientId, int interactableObjectId)
        {
            Assert.IsTrue(interactableObjects.ContainsKey(interactableObjectId));
            interactableObjects[interactableObjectId].Interact(clientId);
        }

        private void Start()
        {
            interactableObjects = GetComponentsInChildren<InteractableObjectStateBase>().ToDictionary(
                i => i.InteractableObjectId,
                i => i);
        }
    }
}