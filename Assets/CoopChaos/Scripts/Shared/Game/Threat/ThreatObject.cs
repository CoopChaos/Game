using System;
using System.Collections.Generic;
using CoopChaos;
using CoopChaos.CoopChaos.Scripts.Shared.Game.Spaceship;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Serialization;

namespace Yame.Threat
{
    public class ThreatObject : NetworkBehaviour
    {
        // TODO: make this a network varaible
        public String threatName;
        public String threatDescription;
        public NetworkVariable<int> numTasksTotal;
        public NetworkVariable<int> numTasksFinished;

        private NetworkVariable<bool> timeConstrained;
        private NetworkVariable<float> timeToSolve;

        private NetworkVariable<bool> finished;

        public NetworkVariable<bool> Finished => finished;
        
        public Dictionary<string, ServerDeviceInteractableBase> threatObjectives = new Dictionary<string, ServerDeviceInteractableBase>();
        private String[] threatObjectivesString;
        
        [ClientRpc]
        public void CommunicateTaskInfosToClientClientRpc(
            String title,
            String description)
        {
            this.threatName = title;
            this.threatDescription = description;
            // this.threatObjectivesString = objectives;
        }

    }
}