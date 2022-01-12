using System.Collections.Generic;
using CoopChaos.CoopChaos.Scripts.Shared.Game.Spaceship;
using Unity.Netcode;
using UnityEngine;

namespace Yame.Threat
{
    public class ThreatObject : NetworkBehaviour
    {
        // TODO: make this a network varaible
        public List<DeviceInteractableState> objectives;
        private string threatDescription;
        
        private NetworkVariable<bool> timeConstrained;
        private NetworkVariable<float> timeToSolve;

        private NetworkVariable<bool> finished;

        public NetworkVariable<bool> Finished => finished;


    }
}