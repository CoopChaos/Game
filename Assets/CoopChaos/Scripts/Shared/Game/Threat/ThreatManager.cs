using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

namespace CoopChaos
{
    public class ThreatManager : NetworkBehaviour
    {
        public static ThreatManager Instance;

        public GameObject SampleThreat;
        public GameObject ThreatUI;
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            if (Instance == null) Instance = this;
            else if(Instance != this) {
                Destroy(gameObject);
            }
        }

        public void Test() {
            Debug.Log("ThreatManager.Test()");
        }

        public void SpawnThreat() {
            GameObject o = Instantiate(SampleThreat, new Vector3(8.664088f, 16.46855f, -3.953443f), Quaternion.identity);

            NetworkObject[] networkObjects = o.GetComponentsInChildren<NetworkObject>();

            foreach (NetworkObject no in networkObjects)
                no.Spawn();
        }
    }
}
