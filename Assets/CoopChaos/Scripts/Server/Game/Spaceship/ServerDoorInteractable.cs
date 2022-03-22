using System;
using System.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Assertions;

namespace CoopChaos
{
    [RequireComponent(typeof(DoorInteractableState))]
    public class ServerDoorInteractable : ServerInteractableObjectBase
    {
        private DoorInteractableState doorInteractableState;
        private int counter = 0;
        
        public override void Interact(ulong clientId)
        {
            ++counter;
            doorInteractableState.Open.Value = !doorInteractableState.Open.Value;

            if (!doorInteractableState.Open.Value)
            {
                StartCoroutine(DelayedClose(counter));
            }
        }

        private IEnumerator DelayedClose(int lastCounter)
        {
            yield return new WaitForSeconds(1);

            if (counter == lastCounter)
            {
                doorInteractableState.Open.Value = true;
            }
        }

        private void Awake()
        {
            doorInteractableState = GetComponent<DoorInteractableState>();
            Assert.IsNotNull(doorInteractableState);
        }

        protected override void Start()
        {
            base.Start();
        }
    }
}