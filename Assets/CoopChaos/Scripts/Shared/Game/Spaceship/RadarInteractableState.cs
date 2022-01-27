using Unity.Netcode;

namespace CoopChaos.CoopChaos.Scripts.Shared.Game.Spaceship
{
    public class RadarInteractableState : InteractableObjectStateBase
    {
        private NetworkVariable<bool> isBlocked;

        public NetworkVariable<bool> IsBlocked => isBlocked;
        

        protected override void Awake()
        {
            base.Awake();
            
            isBlocked = new NetworkVariable<bool>(false);
        }
    }
}