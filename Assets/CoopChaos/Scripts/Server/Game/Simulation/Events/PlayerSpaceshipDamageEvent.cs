using DefaultEcs;

namespace CoopChaos.Simulation.Events
{
    public class PlayerSpaceshipDamageEvent
    {
        public float Damage { get; set; }
        public Entity Entity { get; set; }
    }
}