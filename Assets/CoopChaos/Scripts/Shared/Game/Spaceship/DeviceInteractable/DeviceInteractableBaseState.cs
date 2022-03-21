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

        public bool IsRoleBound;

        public PlayerRoles Role;
        
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

        [ServerRpc(RequireOwnership = false)]
        public void ClaimServerRpc(ulong clientId)
        {
            if (claimed.Value)
            {
                Debug.LogWarning("Tried to claim an already claimed interactable");
                return;
            }

            claimed.Value = true;
            this.clientId.Value = clientId;
        }

        [ServerRpc(RequireOwnership = false)]
        public void FulfillServerRpc()
        {
            if (!claimed.Value)
            {
                Debug.LogWarning("Tried to fulfill an unclaimed interactable");
                return;
            }

            Debug.Log("Threat fullfilled");
            fulfilled.Value = true;
        }

        [ServerRpc(RequireOwnership = false)]
        public void UnclaimServerRpc()
        {
            if (!claimed.Value)
            {
                Debug.LogWarning("Tried to reclaim an unclaimed interactable");
                return;
            }

            claimed.Value = false;
            fulfilled.Value = false;
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