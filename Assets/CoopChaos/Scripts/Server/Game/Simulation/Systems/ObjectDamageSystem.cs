using CoopChaos.Simulation.Components;
using CoopChaos.Simulation.Events;
using DefaultEcs;

namespace CoopChaos.Simulation.Systems
{
    public class ObjectDamageSystem : ISystem
    {
        private World world;
        private EntitySet damagedEntities;
        
        public ObjectDamageSystem(World world)
        {
            this.world = world;

            damagedEntities = World.GetEntities()
                .With<ObjectComponent>()
                .WhenAdded<DamageComponent>()
                .AsSet();
        }
        
        public World World { get; set; }
        
        public void Update(float deltaTime)
        {
            foreach (var entity in damagedEntities.GetEntities())
            {
                ref var damage = ref entity.Get<DamageComponent>();
                ref var oc = ref entity.Get<ObjectComponent>();

                oc.Health -= damage.Damage;

                if (entity.Has<PlayerSpaceshipComponent>())
                {
                    World.Publish(new PlayerSpaceshipDamageEvent()
                    {
                        Damage = damage.Damage,
                        Entity = entity
                    });
                }

                if (oc.Health <= 0)
                {
                    oc.Health = 0;
                    entity.Set<DestroyComponent>();
                }
                
                entity.Remove<DamageComponent>();
            }
        }
    }
}