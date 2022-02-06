using CoopChaos.CoopChaos.Scripts.Shared.Game.Spaceship;
using CoopChaos.Rooms;
using Unity.Netcode;
using UnityEngine;

namespace CoopChaos
{
    [RequireComponent(typeof(ServerRadar), typeof(ClientRadar))]
    public class RadarState : NetworkBehaviour
    {
        private float radarMaxRange = 250;
        private NetworkList<RadarEntity> radarEntities;

        public NetworkList<RadarEntity> RadarEntities => radarEntities;
        public float RadarMaxRange => radarMaxRange;
        
        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
        }

        protected void Awake()
        {
            radarEntities = new NetworkList<RadarEntity>();
        }
    }
}
