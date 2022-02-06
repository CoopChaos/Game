using System;
using System.Collections.Generic;
using UnityEngine;

namespace CoopChaos
{
    public class ServerOccuranceManager : MonoBehaviour
    {
        private List<IOccurance> currentOccurances = new List<IOccurance>();

        public void LoadNewOccurance(OccuranceDescription occuranceDescription)
        {
            currentOccurances.Add(OccuranceFactory.Create(occuranceDescription));
        }

        public void ClearOccurances()
        {
            foreach (var currentOccurance in currentOccurances)
            {
                currentOccurance.Remove();
            }
            
            currentOccurances.Clear();
        }

        private void Update()
        {
            foreach (var occurance in currentOccurances)
            {
                occurance.Update();
            }
        }
    }
}