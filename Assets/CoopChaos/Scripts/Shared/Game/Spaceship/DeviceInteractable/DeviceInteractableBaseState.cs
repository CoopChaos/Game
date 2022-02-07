using Unity.Netcode;
using UnityEngine;
using UnityEngine.Assertions;
using Yame;

namespace CoopChaos.CoopChaos.Scripts.Shared.Game.Spaceship
{
    public class DeviceInteractableBaseState : InteractableObjectStateBase
    {
        // prevent someone else from doing the same task concurrently
        // tasks with multiple people recieve multiple interactables
        private NetworkVariable<bool> claimed;
        
        // if whatever lies behind the interactable is finished, it is marked
        // fulfilled so no accidental reclaim could happen
        private NetworkVariable<bool> fulfilled;

        private NetworkVariable<ulong> clientId;
        
        private string taskDescription;

        public NetworkVariable<bool> Claimed => claimed;
        public NetworkVariable<bool> Fulfilled => fulfilled;
        public NetworkVariable<ulong> ClientId => clientId;
        
        public string TaskDescription => taskDescription;

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();

            claimed.OnValueChanged += HandleOpenChanged;
        }

        protected override void Awake()
        {
            base.Awake();
            
            claimed = new NetworkVariable<bool>(false);
            fulfilled = new NetworkVariable<bool>(false);
            clientId = new NetworkVariable<ulong>();
            taskDescription = "Lorem Ipsum Dolor";
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