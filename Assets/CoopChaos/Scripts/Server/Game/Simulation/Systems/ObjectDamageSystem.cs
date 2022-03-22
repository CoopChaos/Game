using CoopChaos.Simulation.Components;
using CoopChaos.Simulation.Events;
using DefaultEcs;

namespace CoopChaos.Simulation.Systems
{
    public class ObjectDamageSystem : ISystem
    {
        private CoopChaosWorld world;
        private EntitySet damagedEntities;
        
        public ObjectDamageSystem(CoopChaosWorld world)
        {
            this.world = world;

            damagedEntities = world.Native.GetEntities()
                .With<ObjectComponent>()
                .WhenAdded<DamageComponent>()
                .AsSet();
        }

        public void Update(float deltaTime)
        {
            foreach (var entity in damagedEntities.GetEntities())
            {
                ref var damage = ref entity.Get<DamageComponent>();
                ref var oc = ref entity.Get<ObjectComponent>();

                oc.Health -= damage.Damage;

                if (entity.Has<PlayerSpaceshipComponent>())
                {
                    world.Native.Publish(new PlayerSpaceshipDamageEvent()
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
                
                entity.NotifyChanged<ObjectComponent>();
                entity.Remove<DamageComponent>();
            }
        }
    }
}