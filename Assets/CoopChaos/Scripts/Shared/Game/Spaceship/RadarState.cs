using System;
using CoopChaos.CoopChaos.Scripts.Shared.Game.Spaceship;
using Unity.Netcode;
using UnityEngine;

namespace CoopChaos
{
    [RequireComponent(typeof(ServerRadar), typeof(ClientRadar))]
    public class RadarState : NetworkBehaviour
    {
        private NetworkList<RadarEntity> radarEntities;
        public event Action OnRadarUpdate;

        public NetworkList<RadarEntity> RadarEntities => radarEntities;
        
        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
        }

        protected void Awake()
        {
            radarEntities = new NetworkList<RadarEntity>();
        }

        [ClientRpc]
        public void UpdateRadarClientRpc()
        {
            OnRadarUpdate?.Invoke();
        }

        
    }
}
