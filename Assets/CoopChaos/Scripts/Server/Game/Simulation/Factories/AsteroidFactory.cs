using CoopChaos.Simulation.Components;
using DefaultEcs;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

namespace CoopChaos.Simulation.Factories
{
    public static class AsteroidFactory
    {
        public static Entity CreateAsteroid(this World world, float x, float y, float mass, float size)
        {
            var entity = world.CreateEntity();
            TextField
            entity.Set(new ObjectComponent()
            {
                X = x,
                Y = y,
                VelocityX = 0f,
                VelocityY = 0f,
                Mass = mass,
                Size = size,
            });
            
            entity.Set(new DetectionTypeComponent()
            {
                Type = DetectionType.NaturalDeadObject
            });
            
            return entity;
        }
    }
}