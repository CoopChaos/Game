using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using Random = System.Random;

namespace CoopChaos
{
    public class ServerFlightSequence : MonoBehaviour
    {
        
        private FlightSequenceDescription flightSequence;
        private ServerOccuranceManager serverOccuranceManager;
        
        public void LoadFlightSequence(FlightSequenceDescription flightSequence)
        {
            this.flightSequence = flightSequence;
            
            
        }

        private OccuranceDescription DetermineOccurance(OccuranceUseCase[] useCase)
        {
            Random rnd = new Random();
            
            float selectedLimit = useCase.Sum(u => u.ProportionalChance);
            float selected = rnd.Next(0, (int)selectedLimit);
            
            for (int i = 0; i < useCase.Length; i++)
            {
                selected -= useCase[i].ProportionalChance;
                if (selected < 0)
                {
                    return useCase[i].OccuranceDescription;
                }
            }
            
            return useCase[useCase.Length - 1].OccuranceDescription;
        }

        private void Awake()
        {
            serverOccuranceManager = FindObjectOfType<ServerOccuranceManager>();
            Assert.IsNotNull(serverOccuranceManager);
        }
    }
}