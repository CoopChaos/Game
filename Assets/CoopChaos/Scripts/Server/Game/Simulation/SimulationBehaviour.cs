using System;
using System.Collections.Generic;
using System.Linq;
using DefaultEcs;
using UnityEngine;

namespace CoopChaos.Simulation
{
    public class SimulationBehaviour : MonoBehaviour
    {
        private World world;
        private List<ISystem> systems;

        private Entity SpawnEntity()
        {
            return world.CreateEntity();
        }

        public World World => world;

        private void Awake()
        {
            world = new World();

            InstantiateSystems();
        }

        private void InstantiateSystems()
        {
            systems = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => typeof(ISystem).IsAssignableFrom(p) && !p.IsInterface && !p.IsAbstract)
                .Select(t => Activator.CreateInstance(t, world) as ISystem)
                .ToList();
        }
    }
}