using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;
using Yame.Threat;
using CoopChaos.Simulation;
using CoopChaos.Simulation.Components;

namespace CoopChaos
{
    public delegate void ThreatMStateChange(ThreatManagerStatus status);

    public class ThreatManagerState : NetworkBehaviour
    {
        public NetworkVariable<ThreatManagerStatus> Status { get; private set; }

        private void Awake()
        {
            Status = new NetworkVariable<ThreatManagerStatus>();
        }
    }
}
