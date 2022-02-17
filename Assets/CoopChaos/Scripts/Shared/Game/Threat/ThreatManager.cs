using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CoopChaos
{
    public class ThreatManager : MonoBehaviour
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
            Instantiate(ThreatUI, new Vector3(0, 0, 0), Quaternion.identity);
            Instantiate(SampleThreat, new Vector3(0, 0, 0), Quaternion.identity);
        }
    }
}
