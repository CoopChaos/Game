using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CoopChaos;
using TMPro;
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
        private TextMeshProUGUI ThreatUI;

        [SerializeField]
        private TextMeshProUGUI ThreatDescriptionUI;

        [SerializeField]
        private Image UIBackground;

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

            switch (status)
            {
                case ThreatManagerStatus.ThreatIdle:
                    ThreatUI.text = "Es besteht keine Gefahr";
                    break;
                case ThreatManagerStatus.ThreatInProgress:
                    ThreatUI.text = "Eine Bedrohung ist aufgetreten, beseitigt sie schnellstmoeglich!";
                    break;
                case ThreatManagerStatus.ThreatFailed:
                    ThreatUI.text = "Die Bedrohung hat Schaden angerichtet, beeilt euch!";
                    break;
                case ThreatManagerStatus.ThreatMalicious:
                    ThreatUI.text = "Die Bedrohung hat euer Schiff zerstoert!";
                    break;
                case ThreatManagerStatus.ThreatGracePeriod:
                    ThreatUI.text = "Bedrohung beseitigt, Zeit zum Durchatmen!";
                    break;
            }

            if(status == ThreatManagerStatus.ThreatComplete
                || status == ThreatManagerStatus.ThreatGracePeriod
                || status == ThreatManagerStatus.ThreatIdle) {
                ThreatDescriptionUI.text = "";
                UIBackground.enabled = false;
            } else {
                ThreatDescriptionUI.text = FindObjectOfType<ThreatObjectState>().ThreatDescription;
                UIBackground.enabled = true;
            }
        }
    }
}