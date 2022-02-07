using Unity.Netcode;
using UnityEngine;

namespace CoopChaos
{
    public class RadarRoomState : InteractableObjectStateBase
    {
        private NetworkVariable<bool> isBlocked;
        
        private NetworkVariable<float> centerX;
        private NetworkVariable<float> centerY;

        public NetworkVariable<bool> IsBlocked => isBlocked;
        public NetworkVariable<float> CenterX => centerX;
        public NetworkVariable<float> CenterY => centerY;

        private NetworkList<RadarEntity> radarEntities;
        public NetworkList<RadarEntity> RadarEntities => radarEntities;
        
        private float radarMaxRange = 250;
        public float RadarMaxRange => radarMaxRange;
        
        
        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
        }

        protected override void Awake()
        {
            base.Awake();
            
            radarEntities = new NetworkList<RadarEntity>();
            isBlocked = new NetworkVariable<bool>(false);
            centerX = new NetworkVariable<float>();
            centerY = new NetworkVariable<float>();
        }
    }
}