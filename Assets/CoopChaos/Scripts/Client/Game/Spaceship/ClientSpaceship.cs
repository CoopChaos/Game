using System;
using Unity.Netcode;
using UnityEngine.Assertions;

namespace CoopChaos
{
    public class ClientSpaceship : NetworkBehaviour
    {
        private SpaceshipState spaceshipState;
        
        public override void OnNetworkSpawn()
        {
            if (!IsClient)
            {
                enabled = false;
                return;
            }
            
            spaceshipState.Health.OnValueChanged += HandleHealthChanged;
        }
        
        private void HandleHealthChanged(float health, float newHealth)
        {
            StartCoroutine(FindObjectOfType<AnimationManager>().Shake());
        }

        private void Awake()
        {
            spaceshipState = GetComponent<SpaceshipState>();
            Assert.IsNotNull(spaceshipState);
        }
    }
}