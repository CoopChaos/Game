using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CoopChaos;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace Yame.Threat
{
    public class ClientThreatManager : NetworkBehaviour
    {
        private ThreatManagerState state;
        
        [SerializeField]
        private Text ThreatUI;

        [SerializeField]
        private Text ThreatDescriptionUI;

        public override void OnNetworkSpawn() {
            base.OnNetworkSpawn();

            if (!IsClient) {
                enabled = false;
                return;
            }

            state = GetComponent<ThreatManagerState>();
            Assert.IsNotNull(state);
            
            Debug.Log("INITIALIZING CLIENT THREAT MANAGER");
            state.Status.OnValueChanged += HandleStatusChanged;
            HandleStatusChanged(state.Status.Value, state.Status.Value);
        }

        private void HandleStatusChanged(ThreatManagerStatus _, ThreatManagerStatus status)
        {
            Debug.Log("Status changed to " + status);

            ThreatUI.text = status.ToString();

            if(status == ThreatManagerStatus.ThreatComplete
                || status == ThreatManagerStatus.ThreatGracePeriod
                || status == ThreatManagerStatus.ThreatIdle) {
                ThreatDescriptionUI.text = "";
            } else {
                ThreatDescriptionUI.text = FindObjectOfType<ThreatObjectState>().ThreatDescription;
            }
        }
    }
}