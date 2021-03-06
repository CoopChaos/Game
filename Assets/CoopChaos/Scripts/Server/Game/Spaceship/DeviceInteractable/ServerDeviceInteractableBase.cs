using System;
using CoopChaos;
using CoopChaos.CoopChaos.Scripts.Shared.Game.Spaceship;
using UnityEngine;
using UnityEngine.Assertions;

namespace Yame
{
    public class ServerDeviceInteractableBase : ServerInteractableObjectBase
    {
        private DeviceInteractableBaseState deviceInteractableState;

        public DeviceInteractableBaseState DeviceInteractableState => deviceInteractableState;

        // TODO: check if role is suited to interact with the device
        
        public override void Interact(ulong clientId)
        {
            base.Interact(clientId);
            
            if (deviceInteractableState.IsRoleBound) {
                if (FindObjectOfType<ServerGameStage>()
                    .GetPlayerObjectByClientHash(UserConnectionMapper.Singleton[clientId])
                    .GetComponent<GameStageUserState>()
                    .Role.Value != deviceInteractableState.Role) 
                {
                    Debug.LogWarning($"Player tried to interact with threat but has wrong role {clientId}");
                    return;
                }
            }

            deviceInteractableState.Claimed.Value = !deviceInteractableState.Claimed.Value;
            
            deviceInteractableState.ClientId.Value = deviceInteractableState.Claimed.Value
                ? clientId
                : 0;
        }

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();

            if (!IsServer) {
                enabled = false;
                return;
            }

            deviceInteractableState.Fulfilled.OnValueChanged = ReactFulfilled;
        }

        private void Awake()
        {
            deviceInteractableState = GetComponent<DeviceInteractableBaseState>();
            Assert.IsNotNull(deviceInteractableState);
        }

        protected override void Start()
        {
            base.Start();
        }

        private void ReactFulfilled(bool previousvalue, bool newvalue)
        {
            Debug.Log("SERVER noticed fulfillment");
        }
    }
}