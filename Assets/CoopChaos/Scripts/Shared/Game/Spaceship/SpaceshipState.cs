using System;
using Unity.Netcode;
using UnityEngine;

namespace CoopChaos
{
    public class SpaceshipState : NetworkBehaviour
    {
        private NetworkVariable<float> health;

        public NetworkVariable<float> Health => health;

        public override void OnNetworkSpawn()
        {
        }

        private void Awake()
        {
            health = new NetworkVariable<float>();
        }
    }
}