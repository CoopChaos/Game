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
    public delegate void ThreatMStateChange(ThreatManagerState state);

    public enum ThreatManagerState
    {
        ThreatIdle,
        ThreatInProgress,
        ThreatComplete,
        ThreatFailed,
        ThreatMalicious,
        ThreatGracePeriod
    }

    public class ThreatManager : NetworkBehaviour
    {
        public static ThreatManager Instance;
        private LinkedList<NetworkObject> currentThreats;
        public event ThreatMStateChange ThreatMStateChangeEvent;
        private ThreatManagerState threatManagerState;


        [SerializeField]
        private Text ThreatUI;

        [SerializeField]
        private Text ThreatDescriptionUI;

        [SerializeField]
        private NetworkObject[] threatPool;

        public override void OnNetworkSpawn() {
            base.OnNetworkSpawn();

            if (!IsServer) {
                enabled = false;
                return;
            }
        }

        public void SpawnThreat() 
        {
            Debug.Log("Spawning threat");
            
            if (threatManagerState == ThreatManagerState.ThreatInProgress) 
                return;
            
            var threat = currentThreats.AddLast(InstantiateThreat()).Value;
            var threatObject = threat.GetComponent<ThreatObject>();
            
            SetupMinigamesInThreat(threat.transform);
            
            ThreatDescriptionUI.enabled = true;
            ThreatDescriptionUI.text = threatObject.ThreatName + " " + threatObject.ThreatDescription;

            StartCoroutine(StartThreatTimer(threatObject));
            SetThreatStatus(ThreatManagerState.ThreatInProgress);
        }
        
        public ThreatManagerState GetThreatStatus() {
            return threatManagerState;
        }

        public bool ThreatResolved() {
            if (currentThreats.Count == 0) return true;
            else return false;
        }

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);

            ThreatDescriptionUI.enabled = false;
            
            if (Instance == null) Instance = this;
            else if(Instance != this) {
                Destroy(gameObject);
            }

            currentThreats = new LinkedList<NetworkObject>();
        }

        private void Update() 
        {
            if (currentThreats.Count > 0)
            {
                RemoveThreatsIfFinished();
            }
        }

        private NetworkObject InstantiateThreat()
        {   
            int randomIndex = Random.Range(0, threatPool.Length);
            var threat = Instantiate(threatPool[randomIndex], new Vector3(0f, 0f, 0f), Quaternion.identity, this.transform);
            threat.name = threatPool[randomIndex].name;
            return threat;
        }

        private void SetupMinigamesInThreat(Transform threatTransform)
        {
            foreach (var minigame in threatTransform.GetComponent<ThreatObject>().Minigames)
            {
                var minigameObject = Instantiate(minigame, threatTransform);
                minigameObject.GetComponent<NetworkObject>().Spawn();
            }
            
            var spawnPointsObject = GameObject.Find("SpawnPoints");

            var spawnPoints = Enumerable.Range(0, spawnPointsObject.transform.childCount)
                .OrderBy(i => Random.Range(0f, 1f))
                .Select(i => spawnPointsObject.transform.GetChild(i))
                .ToList();

            for (int i = 0; i < threatTransform.childCount; ++i)
            {
                threatTransform.GetChild(i).position = spawnPoints[i].position;
                
                /*
                threatTransform.GetChild(i).GetComponent<NetworkObject>()
                    .Spawn();    
                */
            }
        }

        private IEnumerator StartThreatTimer(ThreatObject threat)
        {
            // yield time until damage
            yield return new WaitForSeconds(threat.ThreatTime);
            
            if (GetThreatStatus() == ThreatManagerState.ThreatInProgress) {
                SetThreatStatus(ThreatManagerState.ThreatFailed);
            }
            
            // yield time until game over
            yield return new WaitForSeconds(threat.ThreatTime);
            
            if (GetThreatStatus() == ThreatManagerState.ThreatFailed) {
                SetThreatStatus(ThreatManagerState.ThreatMalicious);
            }
        }

        private IEnumerator StartGracePeriod() {
            yield return new WaitForSeconds(10);
            SetThreatStatus(ThreatManagerState.ThreatIdle);
        }

        private void SetThreatStatus(ThreatManagerState state) {
            ThreatUI.text = state.ToString();
            ThreatMStateChangeEvent(state);
            threatManagerState = state;
            SetThreatStatusClientRpc(state);
        }

        [ClientRpc]
        private void SetThreatStatusClientRpc(ThreatManagerState state) {
            ThreatUI.text = state.ToString();
            ThreatMStateChangeEvent(state);
            threatManagerState = state;
        }

        private void RemoveThreatsIfFinished()
        {
            foreach (NetworkObject threat in currentThreats)
            {
                if (threat.GetComponent<ThreatObject>().Finished.Value)
                {
                    RemoveFinishedThreat(threat);
                }
            }
        }

        private void RemoveFinishedThreat(NetworkObject threat)
        {
            ThreatDescriptionUI.enabled = false;

            Debug.Log("Threat Complete");

            SetThreatStatus(ThreatManagerState.ThreatComplete);
            currentThreats.Remove(threat);

            SetThreatStatus(ThreatManagerState.ThreatGracePeriod);
            StartCoroutine(StartGracePeriod()); // Return to idle
        }
    }
}
