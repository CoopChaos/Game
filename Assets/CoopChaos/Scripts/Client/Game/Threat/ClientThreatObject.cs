using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
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
        private Dictionary<int, String> tasks = new Dictionary<int, string>();

        private void Update()
        {
            /*
            threatPanel.enabled = !threatObject.Finished.Value;
            threatTitle.text = threatObject.threatName
                               + "( " + threatObject.numTasksFinished
                               + " / " + threatObject.numTasksTotal + " )";
            threatObjectives.text = threatObject.threatDescription;
            */
        }
        
        protected void Awake()
        {
            threatObject = GetComponent<ThreatObject>();
        }
    }
}