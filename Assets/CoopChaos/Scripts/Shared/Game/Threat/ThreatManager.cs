using System.Collections;
using System.Collections.Generic;
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
        private GameObject currentThreat;
        public event ThreatMStateChange ThreatMStateChangeEvent;
        private ThreatManagerState threatManagerState;


        [SerializeField]
        private Text ThreatUI;

        [SerializeField]
        private Text ThreatDescriptionUI;

        [SerializeField]
        private GameObject[] threatPool;


        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            ThreatDescriptionUI.enabled = false;
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
            if(threatManagerState == ThreatManagerState.ThreatInProgress) return;
            currentThreat = Instantiate(SelectThreat(), new Vector3(0f, 0f, 0f), Quaternion.identity);

            ThreatDescriptionUI.enabled = true;
            ThreatDescriptionUI.text = currentThreat.GetComponent<ThreatObject>().threatName + " " + currentThreat.GetComponent<ThreatObject>().threatDescription ;

            StartCoroutine(StartThreatTimer());

            NetworkObject[] networkObjects = currentThreat.GetComponentsInChildren<NetworkObject>();

            foreach (NetworkObject no in networkObjects)
                no.Spawn();

            SetThreatStatus(ThreatManagerState.ThreatInProgress);
        }

        public IEnumerator StartThreatTimer() {
            // yield time until damage
            yield return new WaitForSeconds(currentThreat.GetComponent<ThreatObject>().threatTime);
            if(GetThreatStatus() == ThreatManagerState.ThreatInProgress) {
                SetThreatStatus(ThreatManagerState.ThreatFailed);
            }
            if(currentThreat == null) yield return null;
            // yield time until game over
            yield return new WaitForSeconds(currentThreat.GetComponent<ThreatObject>().threatTime);
            if(GetThreatStatus() == ThreatManagerState.ThreatFailed) {
                SetThreatStatus(ThreatManagerState.ThreatMalicious);
            }
        }

        public IEnumerator StartGracePeriod() {
            yield return new WaitForSeconds(10);
            SetThreatStatus(ThreatManagerState.ThreatIdle);
        }

        private void SetThreatStatus(ThreatManagerState state) {
            ThreatUI.text = state.ToString();
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
            if (currentThreat != null) {
                if(currentThreat.GetComponent<ThreatObject>().Finished.Value) {
                    ThreatDescriptionUI.enabled = false;
                    Debug.Log("Threat Complete");
                    SetThreatStatus(ThreatManagerState.ThreatComplete);
                    currentThreat = null;
                    SetThreatStatus(ThreatManagerState.ThreatGracePeriod);
                    StartCoroutine(StartGracePeriod()); // Return to idle
                }
            }
        }
    }
}
