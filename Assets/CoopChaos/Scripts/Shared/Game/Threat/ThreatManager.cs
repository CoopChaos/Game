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
            
            SpawnThreat();
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

        public NetworkObject SelectThreat()
        {   
            int randomIndex = Random.Range(0, threatPool.Length);
            return threatPool[randomIndex];
        }

        public void SpawnThreat() {
            if (threatManagerState == ThreatManagerState.ThreatInProgress) 
                return;

            NetworkObject threat = SelectThreat();

            NetworkObject go = Instantiate(threat, new Vector3(0f, 0f, 0f), Quaternion.identity, this.transform);

            LinkedListNode<NetworkObject> tnode = currentThreats.AddLast(go);

            go.Spawn();

            ThreatDescriptionUI.enabled = true;
            ThreatDescriptionUI.text = tnode.Value.GetComponent<ThreatObject>().threatName + " " + tnode.Value.GetComponent<ThreatObject>().threatDescription ;

            StartCoroutine(StartThreatTimer(tnode.Value));

            // POSITION START
            LinkedList<int> positions = new LinkedList<int>();

            for(int i = 0; i < tnode.Value.transform.childCount; i++) {
                int rnd = 0;

                do {
                    rnd = Random.Range(0, GameObject.Find("SpawnPoints").transform.childCount);
                } while(positions.Contains(rnd));

                tnode.Value.transform.GetChild(i).transform.position = GameObject.Find("SpawnPoints").transform.GetChild(rnd).transform.position;
                positions.AddLast(rnd);
            }
            
            SetThreatStatus(ThreatManagerState.ThreatInProgress);
        }

        public IEnumerator StartThreatTimer(NetworkObject threat) {
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
            SetThreatStatusClientRpc(state);
        }

        [ClientRpc]
        private void SetThreatStatusClientRpc(ThreatManagerState state) {
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
                foreach (NetworkObject threat in currentThreats) {
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
