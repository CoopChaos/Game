using System.Collections.Generic;
using CoopChaos.Simulation;
using UnityEngine;

namespace CoopChaos
{
    [Occurance(OccuranceType.Fight)]
    public class FightOccurance : IOccurance
    {
        private OccuranceDescription description;
        
        public FightOccurance(OccuranceDescription description)
        {
            this.description = description;
        }
        
        public void Start(SimulationBehaviour simulation)
        {
        }

        public void Update()
        {
            throw new System.NotImplementedException();
        }

        public void Remove()
        {
            throw new System.NotImplementedException();
        }
    }
}