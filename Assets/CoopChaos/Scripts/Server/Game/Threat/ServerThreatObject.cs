using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;

namespace Yame.Threat
{
    public class ServerThreatObject : NetworkBehaviour
    {
        private ThreatObject threatObject;
        private Dictionary<string, ServerDeviceInteractable> objectives = new Dictionary<string, ServerDeviceInteractable>();

        private bool threatCompleted = false;

        private void Update()
        {
            // threadCompleted is true when every sub objective is finished
            foreach (var i in objectives)
            {
                threatCompleted = i.Value.deviceInteractableState.Fulfilled.Value;
            }

            if (threatCompleted)
            {
                threatObject.Finished.Value = true;
                Debug.Log("Threat completed");
            }
        }

        private void Start()
        {
            objectives = GetComponentsInChildren<ServerDeviceInteractable>().ToDictionary(
                i => i.name,
                i => i.GetComponent<ServerDeviceInteractable>());
            
            Debug.Log("--- NEW THREAT ---");
            Debug.Log("Objectives: " + objectives.Count);
            
            foreach (var serverDeviceInteractable in objectives)
            {
                Debug.Log(serverDeviceInteractable.Value.deviceInteractableState.TaskDescription);
            }

            threatObject.threatName = "TestThreat";
            threatObject.trheatObjectives = "TestObjectives";
            threatObject.Finished.Value = false;

        }
        
        protected void Awake()
        {
            threatObject = GetComponent<ThreatObject>();
        }

    }
}