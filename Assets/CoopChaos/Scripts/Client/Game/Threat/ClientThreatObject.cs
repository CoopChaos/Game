using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

namespace Yame.Threat
{
    public class ClientThreatObject : NetworkBehaviour
    {
        private ThreatObject threatObject;
        public Text threatTitle;
        public Text threatObjectives;
        public Canvas threatPanel;

        private void Update()
        {
            threatPanel.enabled = !threatObject.Finished.Value;
            threatTitle.text = threatObject.threatName.ToString();
            threatObjectives.text = threatObject.trheatObjectives.ToString();
        }
        
        protected void Awake()
        {
            threatObject = GetComponent<ThreatObject>();
        }
    }
}