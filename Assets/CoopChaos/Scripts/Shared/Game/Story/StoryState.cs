using System;
using Unity.Netcode;
using UnityEngine;

namespace DefaultNamespace
{
    public class StoryState : NetworkBehaviour
    {
        private NetworkVariable<float> flighsequenceProgress;

        // number between 0.0 and 1.0
        public NetworkVariable<float> FlighsequenceProgress => flighsequenceProgress;
        public event Action<FlightSequenceInfo> FlightSequenceFinished;

        [ClientRpc]
        public void NotifyFlightSequenceFinishedClientRpc(FlightSequenceInfo info)
        {
            FlightSequenceFinished?.Invoke(info);
        }

        private void Awake()
        {
            flighsequenceProgress = new NetworkVariable<float>();
        }
    }
}