using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CoopChaos;
using Unity.Netcode;
using UnityEngine;

namespace Yame.Threat
{
    public class ServerThreatManager : NetworkBehaviour
    {
        private ThreatManagerState state;
        private LinkedList<NetworkObject> currentThreats;
        public event ThreatMStateChange ThreatMStateChangeEvent;

        [SerializeField]
        private NetworkObject[] threatPool;
        
        public override void OnNetworkSpawn() 
        {
            base.OnNetworkSpawn();

            if (!IsServer) {
                enabled = false;
                return;
            }
            
            SpawnThreat();
        }

        public void SpawnThreat() 
        {
            Debug.Log("Spawning threat");
            
            if (state.Status.Value == ThreatManagerStatus.ThreatInProgress) 
                return;
            
            var threat = currentThreats.AddLast(InstantiateThreat()).Value;
            var threatObject = threat.GetComponent<ThreatObjectState>();
            
            StartCoroutine(StartThreatTimer(threatObject));
            SetThreatStatus(ThreatManagerStatus.ThreatInProgress);
        }
        
        public ThreatManagerStatus GetThreatStatus() 
        {
            return state.Status.Value;
        }

        public bool ThreatResolved()
            => currentThreats.Count == 0;

        private void Awake()
        {
            currentThreats = new LinkedList<NetworkObject>();
            state = GetComponent<ThreatManagerState>();
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
            threat.Spawn();
            return threat;
        }

        private IEnumerator StartThreatTimer(ThreatObjectState threat)
        {
            // yield time until damage
            yield return new WaitForSeconds(threat.ThreatTime);
            
            if (GetThreatStatus() == ThreatManagerStatus.ThreatInProgress) {
                SetThreatStatus(ThreatManagerStatus.ThreatFailed);
            }
            
            // yield time until game over
            yield return new WaitForSeconds(threat.ThreatTime);
            
            if (GetThreatStatus() == ThreatManagerStatus.ThreatFailed) {
                SetThreatStatus(ThreatManagerStatus.ThreatMalicious);
            }
        }

        private IEnumerator StartGracePeriod() 
        {
            yield return new WaitForSeconds(10);
            SetThreatStatus(ThreatManagerStatus.ThreatIdle);
        }

        private void SetThreatStatus(ThreatManagerStatus status) 
        {
            state.Status.Value = status;
            ThreatMStateChangeEvent?.Invoke(status);
        }

        private void RemoveThreatsIfFinished()
        {
            foreach (NetworkObject threat in currentThreats.ToList())
            {
                if (threat.GetComponent<ThreatObjectState>().Finished.Value)
                {
                    Debug.Log("Finished threat removed");
                    RemoveFinishedThreat(threat);
                }
            }
        }

        private void RemoveFinishedThreat(NetworkObject threat)
        {
            SetThreatStatus(ThreatManagerStatus.ThreatComplete);
            currentThreats.Remove(threat);

            SetThreatStatus(ThreatManagerStatus.ThreatGracePeriod);
            StartCoroutine(StartGracePeriod()); // Return to idle
        }
    }
}