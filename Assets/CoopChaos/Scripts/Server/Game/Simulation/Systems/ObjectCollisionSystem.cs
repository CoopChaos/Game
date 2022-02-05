using System.Collections.Generic;
using CoopChaos.Simulation.Components;
using DefaultEcs;
using UnityEngine;

namespace CoopChaos.Simulation.Systems
{
    public class ObjectCollisionSystem : ISystem
    {
        /*
         * This system is responsible for detecting collisions between objects.
         *
         * if two objects are colliding they take damage depending on their mass
         * and the amount of force they are exerting on each other.
         *
         * the smaller object is getting destroyed on collision and the velocity of the larger
         * object is reduced
         * 
         */

        private CoopChaosWorld world;
        private EntitySet changedEntities;
        private EntitySet entities;

        public ObjectCollisionSystem(CoopChaosWorld world)
        {
            this.world = world;
            
            changedEntities = world.Native.GetEntities()
                .WhenChanged<ObjectComponent>()
                .AsSet();
            
            entities = world.Native.GetEntities()
                .With<ObjectComponent>()
                .AsSet();
        }
        
        public void Update(float deltaTime)
        {
            foreach (var entitiyA in changedEntities.GetEntities())
            {
                ref var ocA = ref entitiyA.Get<ObjectComponent>();

                foreach (var entityB in entities.GetEntities())
                {
                    ref var ocB = ref entityB.Get<ObjectComponent>();

                    var dx = ocA.X - ocB.X;
                    var dy = ocA.Y - ocB.Y;
                    
                    var distance = Mathf.Sqrt(dx * dx + dy * dy);
                    
                    if (distance < ocA.Size + ocB.Size)
                    {
                        HandleCollision(entitiyA, ref ocA, entityB, ref ocB);
                    }
                }
            }
        }

        private void HandleCollision(
            Entity entityA,
            ref ObjectComponent ocA,
            Entity entityB,
            ref ObjectComponent ocB)
        {
            var dx = ocA.VelocityX - ocB.VelocityX;
            var dy = ocA.VelocityY - ocB.VelocityY;
            
            var velocityDifference = Mathf.Sqrt(dx * dx + dy * dy);
            var velocityDifferenceOrder = Mathf.Log(velocityDifference);

            // if A is smaller than B, A takes damage and B is destroyed
            var smallerEntity = ocA.Mass < ocB.Mass ? entityA : entityB;
            var ocSmaller = ocA.Mass < ocB.Mass ? ref ocA : ref ocB;
            
            var largerEntity = ocA.Mass < ocB.Mass ? entityB : entityA;

            largerEntity.Set(new DamageComponent()
            {
                Damage = velocityDifferenceOrder * ocSmaller.Mass
            });
            
            smallerEntity.Set<DestroyComponent>();
        }
    }
}