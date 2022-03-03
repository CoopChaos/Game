using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Yame.Threat;

namespace CoopChaos
{
    public delegate string ThreatMStateChange(ThreatManagerState state);

    public enum ThreatManagerState
    {
        ThreatIdle,
        ThreatInProgress,
        ThreatComplete,
        ThreatFailed
    }

    public class ThreatManager : NetworkBehaviour
    {
        public static ThreatManager Instance;
        private GameObject currentThreat;
        private event ThreatMStateChange ThreatMStateChangeEvent;
        private ThreatManagerState threatManagerState;


        [SerializeField]
        private GameObject ThreatUI;

        [SerializeField]
        private int threatTime = 30;

        [SerializeField]
        private GameObject[] threatPool;


        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            if (Instance == null) Instance = this;
            else if(Instance != this) {
                Destroy(gameObject);
            }
        }

        public GameObject SelectThreat()
        {
            int randomIndex = Random.Range(0, threatPool.Length);
            currentThreat = threatPool[randomIndex];
            return currentThreat;
        }

        public void SpawnThreat() {
            currentThreat = Instantiate(SelectThreat(), new Vector3(8.664088f, 16.46855f, -3.953443f), Quaternion.identity);

            NetworkObject[] networkObjects = currentThreat.GetComponentsInChildren<NetworkObject>();

            foreach (NetworkObject no in networkObjects)
                no.Spawn();

            SetThreatStatus(ThreatManagerState.ThreatInProgress);
        }

        public IEnumerator StartThreatTimer() {
            yield return new WaitForSeconds(threatTime);
            if(GetThreatStatus() == ThreatManagerState.ThreatInProgress) {
                SetThreatStatus(ThreatManagerState.ThreatFailed);
            }
        }

        private void SetThreatStatus(ThreatManagerState state) {
            ThreatMStateChangeEvent(state);
            threatManagerState = state;
        }

        public ThreatManagerState GetThreatStatus() {
            return threatManagerState;
        }

        public bool ThreatResolved() {
            if (currentThreat == null) return true;
            return currentThreat.GetComponent<ThreatObject>().Finished.Value;
        }

        public void Update() {
            if(currentThreat.GetComponent<ThreatObject>().Finished.Value) {
                Debug.Log("Threat Complete");
                SetThreatStatus(ThreatManagerState.ThreatComplete);
                currentThreat = null;
            }
        }
    }
}
