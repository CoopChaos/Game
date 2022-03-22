using System;
using System.Collections.Generic;
using System.Linq;
using CoopChaos.CoopChaos.Scripts.Shared.Game.Spaceship;
using Unity.Netcode;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Yame.Threat
{
    public class ServerThreatObject : NetworkBehaviour
    {
        private ThreatObjectState state;
        private String[] objectivesString;

        private GameObject[] minigamePhase2Objects; // only use this for temporarilly storing the minigame objects for view
        
        private int numTasksFinished;
        private int numTasksTotal;

        private bool inPhase2;
        
        private bool threatCompleted = false;

        public virtual void Update()
        {
            
        }

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();

            if (!IsServer) 
            {
                enabled = false;
                return;
            }

            state.threatObjectives = GetComponentsInChildren<ServerDeviceInteractableBase>().ToDictionary(
                i => i.name,
                i => i.GetComponent<ServerDeviceInteractableBase>());
            
            Debug.Log("--- NEW THREAT ---");
            Debug.Log("Objectives: " + state.threatObjectives.Count);

            objectivesString = new string[state.threatObjectives.Count];

            state.Finished.Value = false;
            state.Finished.OnValueChanged = OnFinishChanged;

            PrepareMinigames();
        }

        protected void Awake()
        {
            state = GetComponent<ThreatObjectState>();
        }

        private void OnFinishChanged(bool previousvalue, bool newvalue)
        {
            state.CommunicateTaskInfosToClientClientRpc(state.threatName, state.threatDescription);
            
            if (previousvalue == false && newvalue == true) 
                Debug.Log("Threat completed");
        }

        private void PrepareMinigames()
        {
            SpawnMinigamePhase(transform.GetComponent<ThreatObjectState>().Minigames);
        }

        private void SpawnMinigamePhase(GameObject[] minigames)
        {
            var spawnPointsObject = GameObject.Find("SpawnPoints");

            var spawnPoints = Enumerable.Range(0, spawnPointsObject.transform.childCount)
                .OrderBy(i => Random.Range(0f, 1f))
                .Select(i => spawnPointsObject.transform.GetChild(i))
                .ToList();

            for (int i = 0; i < minigames.Length; ++i)
            {
                var minigameObject = Instantiate(minigames[i], spawnPoints[i].position, Quaternion.identity, transform);
                minigameObject.GetComponent<NetworkObject>().Spawn();

                var device = minigameObject.GetComponent<DeviceInteractableBaseState>();
                device.Fulfilled.OnValueChanged += (_, fullfilled) => HandleMinigameFullfilled(fullfilled, device);
            }

            if(transform.GetComponent<ThreatObjectState>().SetpTwoViewOnly) {
                minigamePhase2Objects = new GameObject[transform.GetComponent<ThreatObjectState>().MinigamesPhase2.Length];

                for (int i = 0; i < transform.GetComponent<ThreatObjectState>().MinigamesPhase2.Length; ++i)
                {
                    var minigameObject = Instantiate(transform.GetComponent<ThreatObjectState>().MinigamesPhase2[i], spawnPoints[minigames.Length + i].position, Quaternion.identity, transform);
                    minigamePhase2Objects[i] = minigameObject;
                    minigameObject.GetComponent<NetworkObject>().Spawn();

                    var device = minigameObject.GetComponent<DeviceInteractableBaseState>();
                    device.Fulfilled.OnValueChanged += (_, fullfilled) => HandleMinigameFullfilled(fullfilled, device);
                }
            }
            
            numTasksFinished = 0;
            numTasksTotal = minigames.Length;
        }
        
        private void HandleMinigameFullfilled(bool fullfilled, DeviceInteractableBaseState minigame)
        {
            if (fullfilled)
            {
                numTasksFinished++;
                
                if (numTasksFinished == numTasksTotal)
                {
                    if(transform.GetComponent<ThreatObjectState>().TwoStepThreat && !inPhase2) {
                        Debug.Log("Phase 1 finished");
                        inPhase2 = true;
                        SpawnMinigamePhase(transform.GetComponent<ThreatObjectState>().MinigamesPhase2);
                    } else {
                        if(transform.GetComponent<ThreatObjectState>().SetpTwoViewOnly) {
                            for (int i = 0; i < transform.GetComponent<ThreatObjectState>().MinigamesPhase2.Length; ++i)
                            {
                                minigamePhase2Objects[i].GetComponent<NetworkObject>().Despawn();
                            }
                        }
                        Debug.Log("Threat completed");
                        state.Finished.Value = true;
                    }
                }

                Destroy(minigame.gameObject);
                Debug.Log("Minigame fullfilled");
            }
        }
    }
}