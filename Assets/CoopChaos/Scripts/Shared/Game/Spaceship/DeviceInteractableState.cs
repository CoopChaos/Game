using Unity.Netcode;
using UnityEngine;
using UnityEngine.Assertions;
using Yame;

namespace CoopChaos.CoopChaos.Scripts.Shared.Game.Spaceship
{
    [RequireComponent(typeof(ServerDeviceInteractable), typeof(ClientDeviceInteractable))]
    public class DeviceInteractableState : InteractableObjectStateBase
    {
        // prevent someone else from doing the same task concurrently
        // tasks with multiple people recieve multiple interactables
        private NetworkVariable<bool> claimed;
        
        // if whatever lies behind the interactable is finished, it is marked
        // fulfilled so no accidental reclaim could happen
        private NetworkVariable<bool> fulfilled;

        private NetworkVariable<ulong> clientId;

        public NetworkVariable<bool> Claimed => claimed;
        public NetworkVariable<bool> Fulfilled => fulfilled;
        public NetworkVariable<ulong> ClientId => clientId;

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();

            claimed.OnValueChanged += HandleOpenChanged;
        }

        protected override void Awake()
        {
            base.Awake();
            
            claimed = new NetworkVariable<bool>(true);
            fulfilled = new NetworkVariable<bool>(true);
        }

        private void HandleOpenChanged(bool open, bool oldOpen)
        {
            if (open)
            {
                Debug.Log("--- Interactable Claimed ---");
            }
            else
            {
                if (fulfilled.Value)
                {
                    Debug.Log("--- Interactable freed fulfilled ---");
                }
                else
                {
                    Debug.Log("--- Interactable freed unfulfilled ---");
                }
            }
        }
    }
}