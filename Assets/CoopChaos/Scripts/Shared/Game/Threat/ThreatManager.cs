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
        private LinkedList<GameObject> currentThreats;
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
            currentThreats = new LinkedList<GameObject>();

            SpawnThreat();
        }

        public GameObject SelectThreat()
        {   
            int randomIndex = Random.Range(0, threatPool.Length);
            return threatPool[randomIndex];
        }

        public async void SpawnThreat() {
            if(threatManagerState == ThreatManagerState.ThreatInProgress) return;

            GameObject threat = SelectThreat();

            LinkedListNode<GameObject> tnode = currentThreats.AddLast(Instantiate(threat, new Vector3(0f, 0f, 0f), Quaternion.identity, this.transform));

            ThreatDescriptionUI.enabled = true;
            ThreatDescriptionUI.text = tnode.Value.GetComponent<ThreatObject>().threatName + " " + tnode.Value.GetComponent<ThreatObject>().threatDescription ;

            StartCoroutine(StartThreatTimer(tnode.Value));

            NetworkObject[] networkObjects = tnode.Value.GetComponentsInChildren<NetworkObject>();
            
            // POSITION START

            for(int i = 0; i < tnode.Value.transform.childCount; i++) {
                tnode.Value.transform.GetChild(i).transform.position = GameObject.Find("SpawnPoints").transform.GetChild(i).transform.position;
            }

            // POSITION END

            foreach (NetworkObject no in networkObjects)
                no.Spawn();

            SetThreatStatus(ThreatManagerState.ThreatInProgress);
        }

        public IEnumerator StartThreatTimer(GameObject threat) {
            // yield time until damage
            yield return new WaitForSeconds(threat.GetComponent<ThreatObject>().threatTime);
            if(GetThreatStatus() == ThreatManagerState.ThreatInProgress) {
                SetThreatStatus(ThreatManagerState.ThreatFailed);
            }
            if(threat == null) yield return null;
            // yield time until game over
            yield return new WaitForSeconds(threat.GetComponent<ThreatObject>().threatTime);
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
            if (currentThreats.Count == 0) return true;
            else return false;
        }

        public void Update() {
            if (currentThreats.Count > 0) {
                foreach (GameObject threat in currentThreats) {
                    if(threat.GetComponent<ThreatObject>().Finished.Value) {
                        ThreatDescriptionUI.enabled = false;
                        Debug.Log("Threat Complete");
                        SetThreatStatus(ThreatManagerState.ThreatComplete);
                        currentThreats.Remove(threat);
                        SetThreatStatus(ThreatManagerState.ThreatGracePeriod);
                        StartCoroutine(StartGracePeriod()); // Return to idle
                    }
                }
            }
        }
    }
}
