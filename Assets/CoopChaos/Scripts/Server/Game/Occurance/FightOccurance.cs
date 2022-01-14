using System.Collections.Generic;
using UnityEngine;

namespace CoopChaos
{
    public class FightOccurance : IOccurance
    {
        private List<IOccuranceObject> objects;
        private IOccuranceSpaceship spaceship;
        
        public FightOccurance(IOccuranceSpaceship spaceship)
        {
            this.spaceship = spaceship;
            objects = new List<IOccuranceObject>();
        }

        public IEnumerable<IOccuranceObject> Objects => objects;
        public IOccuranceSpaceship Spaceship => spaceship;

        public void Start()
        {
        }

        public void Update(float deltaTime)
        {
        }
    }
}