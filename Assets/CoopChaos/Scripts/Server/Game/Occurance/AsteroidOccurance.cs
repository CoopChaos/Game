using System.Collections.Generic;
using CoopChaos.Simulation;

namespace CoopChaos
{
    [Occurance(OccuranceType.Asteroid)]
    public class AsteroidOccurance : IOccurance
    {
        private AsteroidOccuranceDescription description;
        private SimulationBehaviour simulationBehaviour;

        public AsteroidOccurance(OccuranceDescription description)
        {
            this.description = (AsteroidOccuranceDescription)description;
        }
        
        public void Start(SimulationBehaviour simulation)
        {
            
        }

        public void Update()
        {
        }

        public void Remove()
        {
            
        }
    }
}