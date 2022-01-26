using Unity.Netcode;
using UnityEngine;
using UnityEngine.Assertions;
using Yame;

namespace CoopChaos.CoopChaos.Scripts.Shared.Game.Spaceship
{
    public class DeviceInteractableTimerState : DeviceInteractableBaseState
    {
        private NetworkVariable<float> timeToFulFill;
        public NetworkVariable<float> TimeToFulFill => timeToFulFill;

        protected override void Awake()
        {
            base.Awake();
            
            timeToFulFill = new NetworkVariable<float>(10);
        }
    }
}