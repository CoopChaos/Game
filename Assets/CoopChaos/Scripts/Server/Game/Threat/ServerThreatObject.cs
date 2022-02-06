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
        private String[] objectivesString;

        private bool threatCompleted = false;

        private void Update()
        {
            int compCounter = 0;
            // threadCompleted is true when every sub objective is finished
            foreach (var i in threatObject.threatObjectives)
            {
                threatCompleted = i.Value.deviceInteractableState.Fulfilled.Value;
                if (i.Value.deviceInteractableState.Fulfilled.Value) compCounter++;
            }

            threatObject.numTasksFinished.Value = compCounter;

            if (threatCompleted)
            {
                threatObject.Finished.Value = true;
            }
        }

        private void Start()
        {
            threatObject.threatObjectives = GetComponentsInChildren<ServerDeviceInteractableBase>().ToDictionary(
                i => i.name,
                i => i.GetComponent<ServerDeviceInteractableBase>());
            
            Debug.Log("--- NEW THREAT ---");
            Debug.Log("Objectives: " + threatObject.threatObjectives.Count);

            objectivesString = new string[threatObject.threatObjectives.Count];

            foreach (var serverDeviceInteractable in threatObject.threatObjectives)
            {
                Debug.Log(serverDeviceInteractable.Value.deviceInteractableState.TaskDescription);
                objectivesString[0] = serverDeviceInteractable.Value.deviceInteractableState.TaskDescription;
            }
            
            threatObject.threatName = "TestThreat";
            threatObject.threatDescription = "TestObjectives";
            threatObject.Finished.Value = false;
            threatObject.Finished.OnValueChanged = OnFinishChanged;
            threatObject.numTasksTotal.Value = threatObject.threatObjectives.Count;
            
            threatObject.CommunicateTaskInfosToClientClientRpc(threatObject.threatName, threatObject.threatDescription);
        }

        private void OnFinishChanged(bool previousvalue, bool newvalue)
        {
            threatObject.CommunicateTaskInfosToClientClientRpc(threatObject.threatName, threatObject.threatDescription);
            
            if(previousvalue == false && newvalue == true) Debug.Log("Threat completed");
        }

        protected void Awake()
        {
            threatObject = GetComponent<ThreatObject>();
        }

    }
}