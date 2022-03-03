using System;
using System.Collections.Generic;
using System.Linq;
using CoopChaos.Simulation;
using CoopChaos.Simulation.Components;
using CoopChaos.Simulation.Events;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

namespace CoopChaos
{
    public class ServerSpaceship : NetworkBehaviour
    {
        private SimulationBehaviour simulation;
        private ServerGameStage serverGameStage;
        private SpaceshipState spaceshipState;

        private ThreatManager threatManager;

        public override void OnNetworkSpawn()
        {
            if (!IsServer)
            {
                enabled = false;
                return;
            }
        }

        private void Awake()
        {
            serverGameStage = FindObjectOfType<ServerGameStage>();
            Assert.IsNotNull(serverGameStage);
            
            simulation = FindObjectOfType<SimulationBehaviour>();
            Assert.IsNotNull(simulation);
            
            spaceshipState = GetComponent<SpaceshipState>();
            Assert.IsNotNull(spaceshipState);

            threatManager = FindObjectOfType<ThreatManager>();
            Assert.IsNotNull(threatManager);

            threatManager.ThreatMStateChangeEvent += OnThreatMStateChange;
        }

        private void Start()
        {
            simulation.World.Native.Subscribe<PlayerSpaceshipDamageEvent>(HandleDamageEvent);
        }

        private void HandleDamageEvent(in PlayerSpaceshipDamageEvent e)
        {
            ref var oc = ref e.Entity.Get<ObjectComponent>();
            spaceshipState.Health.Value = oc.Health;

            if(spaceshipState.NumThreatsActive.Value == 0) {
                spaceshipState.NumThreatsActive.Value = 1;
                threatManager.SpawnThreat();
            }
            
            if (spaceshipState.Health.Value < 0)
            {
                NetworkManager.Singleton.SceneManager.LoadScene("GameOverDie", LoadSceneMode.Single);
            }
        }

        private void OnThreatMStateChange(ThreatManagerState state)
        {
            if(state == ThreatManagerState.ThreatFailed) {
                spaceshipState.Health.Value -= 1;
            } else if (state == ThreatManagerState.ThreatInProgress) {
                Debug.Log("Spaceship noticed Threat in progress");
            } else if (state == ThreatManagerState.ThreatComplete) {
                // Maybe we can restore health here
                Debug.Log("Spaceship noticed Threat complete");
            }
        }
    }
}