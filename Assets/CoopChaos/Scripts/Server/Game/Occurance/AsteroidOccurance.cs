using System.Collections.Generic;
using CoopChaos.Simulation;
using CoopChaos.Simulation.Components;
using CoopChaos.Simulation.Factories;
using DefaultEcs;
using UnityEngine;
using Random = System.Random;

namespace CoopChaos
{
    [Occurance(OccuranceType.Asteroid)]
    public class AsteroidOccurance : IOccurance
    {
        private const float offset = 300;
        
        private SimulationBehaviour simulation;
        private AsteroidOccuranceDescription description;
        private SimulationBehaviour simulationBehaviour;

        private EntitySet asteroids;
        private float yEnd;

        public AsteroidOccurance(OccuranceDescription description)
        {
            this.description = (AsteroidOccuranceDescription)description;
        }
        
        public string Title => description.Title;
        public string Description => description.Description;
        
        public void Start(SimulationBehaviour simulation)
        {
            this.simulation = simulation;

            var spaceship = simulation.World.PlayerSpaceship;
            ref var spaceshipObject = ref spaceship.Value.Get<ObjectComponent>();

            var layers = description.Length / (description.MaxAsteroidSize + description.DistanceBetweenAsteroids);

            var leftBound = spaceshipObject.X - 5000;
            var rightBound = spaceshipObject.X + 5000;

            var yStart = offset + spaceshipObject.Y;

            var random = new Random();

            int i = 0;

            for (int layer = 0; layer < layers; ++layer)
            {
                var y = layer * (description.MaxAsteroidSize + description.DistanceBetweenAsteroids);

                for (float x = leftBound;
                     x < rightBound;
                     x += (description.MaxAsteroidSize + description.DistanceBetweenAsteroids))
                {
                    ++i;
                    var asteroid = simulation.World.CreateAsteroid(x, y + yStart, spaceshipObject.Mass / 2.0f, 
                        (float)(random.NextDouble() * (description.MaxAsteroidSize - description.MinAsteroidSize) + description.MinAsteroidSize));
                    
                    asteroid.Set<HiddenAsteroidComponent>();
                }
            }
            
            asteroids = simulation.World.Native.GetEntities()
                .With<HiddenAsteroidComponent>()
                .AsSet();

            yEnd = offset + yStart + (layers + 1) * (description.MaxAsteroidSize + description.DistanceBetweenAsteroids);
        }

        public bool Update()
        {
            var spaceship = simulation.World.PlayerSpaceship;
            ref var spaceshipObject = ref spaceship.Value.Get<ObjectComponent>();

            return spaceshipObject.Y < yEnd;
        }

        public void Remove()
        {
            foreach (var asteroid in asteroids.GetEntities())
            {
                asteroid.Dispose();
            }
        }

        private class HiddenAsteroidComponent
        {
        }
    }
}