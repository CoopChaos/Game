using Unity.Netcode;

namespace CoopChaos
{
    public class RadarRoomState : InteractableObjectStateBase
    {
        private NetworkVariable<bool> isBlocked;
        public NetworkVariable<bool> IsBlocked => isBlocked;

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
        }
    }
}