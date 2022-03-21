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
        private ThreatObjectState threatObjectState;
        
        protected void Awake()
        {
            threatObjectState = GetComponent<ThreatObjectState>();
        }
    }
}