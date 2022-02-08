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
        
        public string Title => description.Title;
        public string Description => description.Description;
        
        public void Start(SimulationBehaviour simulation)
        {
        }

        public bool Update()
        {
            return false;
        }

        public void Remove()
        {
        }
    }
}