using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Yame.Threat;

namespace CoopChaos
{
    public class ThreatManager : NetworkBehaviour
    {
        public static ThreatManager Instance;

        public GameObject SampleThreat;
        public GameObject currentThreat;
        public GameObject ThreatUI;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            if (Instance == null) Instance = this;
            else if(Instance != this) {
                Destroy(gameObject);
            }
        }

        public void SpawnThreat() {
            currentThreat = Instantiate(SampleThreat, new Vector3(8.664088f, 16.46855f, -3.953443f), Quaternion.identity);

            NetworkObject[] networkObjects = currentThreat.GetComponentsInChildren<NetworkObject>();

            foreach (NetworkObject no in networkObjects)
                no.Spawn();
        }

        public bool ThreatResolved() {
            if (currentThreat == null) return true;
            return currentThreat.GetComponent<ThreatObject>().Finished.Value;
        }
    }
}
