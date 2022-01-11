using System;
using Unity.Netcode;
using UnityEngine.Assertions;

namespace Yame
{
    public abstract class InteractableObjectStateBase : NetworkBehaviour
    {
        private static int interactableObjectIdCounter = 1;
        public int InteractableObjectId { get; private set; }

        public override void OnNetworkSpawn()
        {
            Assert.IsTrue(InteractableObjectId != 0, "InteractableObjectId must be set before OnNetworkSpawn");
        }

        protected virtual void Awake()
        {
            InteractableObjectId = interactableObjectIdCounter++;
        }
    }
}