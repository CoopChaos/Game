using System;
using System.Collections.Generic;
using System.Linq;
using CoopChaos.Simulation;
using DefaultNamespace;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Assertions;
using Random = System.Random;

namespace CoopChaos
{
    public class ServerFlightSequenceManager : MonoBehaviour
    {
        private int depthProgress;
        private FlightSequenceDescription flightSequence;
        private IOccurance runningOccurance;
        private SimulationBehaviour simulation;

        private StoryState state;

        public event Action<IOccurance> NewOccuranceEvent; 
        public event Action FlightSequenceFinishedEvent;

        public void LoadFlightSequence(FlightSequenceDescription flightSequence)
        {
            depthProgress = 0;
            this.flightSequence = flightSequence;

            LoadNextOccurance();
        }
        
        public bool IsFlightSequenceFinished()
        {
            return depthProgress >= flightSequence.Depth;
        }

        private void LoadNextOccurance()
        {
            if (runningOccurance != null)
            {
                runningOccurance.Remove();
            }

            runningOccurance = OccuranceFactory.Create(DetermineOccurance(flightSequence.UseCases));
            runningOccurance.Start(simulation);
            
            NewOccuranceEvent?.Invoke(runningOccurance);
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
            state = FindObjectOfType<StoryState>();
            Assert.IsNotNull(state);

            simulation = FindObjectOfType<SimulationBehaviour>();
            Assert.IsNotNull(simulation);
        }

        private void Update()
        {
            if (runningOccurance != null)
            {
                var finished = runningOccurance.Update();
                if (finished == false)
                {
                    Debug.Log("TESTTEST");
                    ++depthProgress;
                    state.FlighsequenceProgress.Value = (float) flightSequence.Depth / depthProgress;

                    if (IsFlightSequenceFinished())
                    {
                        FlightSequenceFinishedEvent?.Invoke();
                    }
                    else
                    {
                        LoadNextOccurance();
                    }
                }
            }
        }
    }
}