using CoopChaos.Simulation.Components;
using DefaultEcs;

namespace CoopChaos.Simulation
{
    public class CoopChaosWorld
    {
        private EntitySet spaceship;
        
        public CoopChaosWorld(World native)
        {
            Native = native;
            
            spaceship = Native.GetEntities()
                .With<PlayerSpaceshipComponent>()
                .AsSet();
        }

        public World Native { get; }

        public Entity? PlayerSpaceship
        {
            get
            {
                if (spaceship.TryGetSingleEntity(out var e))
                {
                    return e;
                }
                
                return null;
            }
        }
    }
}