using System;
using Unity.Netcode;
using UnityEngine;

namespace CoopChaos
{
    public class SpaceshipState : NetworkBehaviour
    {
        private NetworkVariable<int> numThreatsActive;
        private NetworkVariable<float> health;

        public NetworkVariable<float> Health => health;
        public NetworkVariable<int> NumThreatsActive => numThreatsActive;

        public override void OnNetworkSpawn()
        {
        }

        private void Awake()
        {
            health = new NetworkVariable<float>();
            numThreatsActive = new NetworkVariable<int>();
        }
    }
}