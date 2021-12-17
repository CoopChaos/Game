using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Assertions;

namespace CoopChaos
{
    [RequireComponent(typeof(ServerDoorInteractable), typeof(ClientDoorInteractable), typeof(Collider2D))]
    public class DoorInteractableState : InteractableObjectStateBase
    {
        private Collider2D collider;
        private NetworkVariable<bool> open;

        public NetworkVariable<bool> Open => open;

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();

            open.OnValueChanged += HandleOpenChanged;
        }

        protected override void Awake()
        {
            base.Awake();
            
            collider = GetComponent<Collider2D>();
            Assert.IsNotNull(collider);
            
            open = new NetworkVariable<bool>(true);
        }

        private void HandleOpenChanged(bool open, bool oldOpen)
        {
            collider.enabled = !open;
        }
    }
}