using System;
using System.Collections.Generic;
using CoopChaos;
using CoopChaos.CoopChaos.Scripts.Shared.Game.Spaceship;
using Unity.Netcode;
using UnityEngine;

namespace Yame.Threat
{
    public class ThreatObject : NetworkBehaviour
    {
        // TODO: make this a network varaible
        public NetworkVariable<NetworkString> objectives;
        public NetworkString threatName;
        public NetworkString trheatObjectives;
        public NetworkVariable<int> numTasksTotal;
        public NetworkVariable<int> numTasksFinished;

        private NetworkVariable<bool> timeConstrained;
        private NetworkVariable<float> timeToSolve;

        private NetworkVariable<bool> finished;

        public NetworkVariable<bool> Finished => finished;


    }
}