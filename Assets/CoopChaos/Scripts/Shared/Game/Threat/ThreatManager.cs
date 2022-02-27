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

        [SerializeField]
        private GameObject[] threatPool;

        private GameObject currentThreat;

        [SerializeField]
        private GameObject ThreatUI;

        public event ThreatMStateChange ThreatMStateChangeEvent;

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

            ThreatMStateChangeEvent(ThreatManagerState.ThreatInProgress);
        }

        public bool ThreatResolved() {
            if (currentThreat == null) return true;
            return currentThreat.GetComponent<ThreatObject>().Finished.Value;
        }
    }
}
