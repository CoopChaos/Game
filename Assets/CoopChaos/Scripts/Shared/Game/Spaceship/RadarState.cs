using CoopChaos.CoopChaos.Scripts.Shared.Game.Spaceship;
using CoopChaos.Rooms;
using Unity.Netcode;
using UnityEngine;

namespace CoopChaos
{
    //[RequireComponent(typeof(ServerRadarRoom), typeof(ClientRadar))]
    public class RadarState : NetworkBehaviour
    {
        private NetworkList<RadarEntity> radarEntities;

        public NetworkList<RadarEntity> RadarEntities => radarEntities;
        
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
