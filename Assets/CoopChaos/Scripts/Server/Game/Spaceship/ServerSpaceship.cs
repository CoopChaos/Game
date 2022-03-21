using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CoopChaos.Simulation;
using CoopChaos.Simulation.Components;
using CoopChaos.Simulation.Events;
using DefaultEcs;
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
           
        }

        private void Start()
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

            Debug.Log("Awake");
            simulation.World.Native.Subscribe<PlayerSpaceshipDamageEvent>(HandleDamageEvent);
            simulation.World.Native.Subscribe<PlayerSpaceshipDestroyedEvent>(HandleDestroyedEvent);
            simulation.World.Native.Subscribe<PlayerSpaceshipSpawnEvent>(HandleSpawnEvent);
            
            if(simulation.World.PlayerSpaceship != null)
                HandleSpawnEvent(new PlayerSpaceshipSpawnEvent{Health = simulation.World.PlayerSpaceship.Value.Get<ObjectComponent>().Health});
        }

        private void HandleSpawnEvent(in PlayerSpaceshipSpawnEvent e)
        {
            spaceshipState.Health.Value = e.Health;
            Debug.Log("Spawned spaceship with health: " + e.Health);
        }

        private void HandleDamageEvent(in PlayerSpaceshipDamageEvent e)
        {
            ref var oc = ref e.Entity.Get<ObjectComponent>();
            spaceshipState.Health.Value = oc.Health;

            threatManager.SpawnThreat();
            
            if (spaceshipState.Health.Value < 0)
            {
                StartCoroutine(DeathRoutine());
            }
        }

        private void HandleDestroyedEvent(in PlayerSpaceshipDestroyedEvent e)
        {
            Debug.Log("DEATH");
            StartCoroutine(DeathRoutine());
        }
        
        private IEnumerator DeathRoutine()
        {
            spaceshipState.DeathAnimationClientRpc();
            Debug.Log("DEATHB");
            yield return new WaitForSeconds(2);
            Debug.Log("DEATHDONE");
            NetworkManager.Singleton.SceneManager.LoadScene("GameOverDie", LoadSceneMode.Single);
        }

        private void OnThreatMStateChange(ThreatManagerState state)
        {
            if(state == ThreatManagerState.ThreatFailed) {
                // decrement health by placeholder value
                spaceshipState.Health.Value -= 1;
            } else if (state == ThreatManagerState.ThreatInProgress) {
                Debug.Log("Spaceship noticed Threat in progress");
            } else if (state == ThreatManagerState.ThreatComplete) {
                // Maybe we can restore health here
                Debug.Log("Spaceship noticed Threat complete");
            } else if (state == ThreatManagerState.ThreatMalicious) {
                // destroy spaceship
                SimulationBehaviour s = FindObjectOfType<SimulationBehaviour>();
                s.World.PlayerSpaceship.Value.Set<DestroyComponent>();
            }
        }
    }
}