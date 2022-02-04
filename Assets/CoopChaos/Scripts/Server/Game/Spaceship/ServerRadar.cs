using CoopChaos.CoopChaos.Scripts.Shared.Game.Spaceship;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Assertions;

namespace CoopChaos
{
    [RequireComponent(typeof(RadarState))]
    public class ServerRadar : NetworkBehaviour
    {
        private RadarState radarState;

        public override void OnNetworkSpawn()
        {
            if (!IsServer)
            {
                enabled = false;
                return;
            }
            
            
            Debug.Log("Server Radar: Added RadarEntity");
            radarState.RadarEntities.Add(new RadarEntity(10, 20, 0));
            radarState.RadarEntities.Add(new RadarEntity(20, 20, 0));
            radarState.RadarEntities.Add(new RadarEntity(40, 10, 0));
            radarState.RadarEntities.Add(new RadarEntity(60, 60, 0));
            radarState.RadarEntities.RemoveAt(2);
            radarState.RadarEntities[0] = new RadarEntity(0, 0, 0);
        }

        private void Awake()
        {
            radarState = GetComponent<RadarState>();
            Assert.IsNotNull(radarState);
        }
    }
}
