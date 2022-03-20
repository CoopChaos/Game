using System;
using System.Collections.Generic;
using CoopChaos;
using CoopChaos.CoopChaos.Scripts.Shared.Game.Spaceship;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Serialization;

namespace Yame.Threat
{
    public class ThreatObject : InteractableObjectStateBase
    {
        [SerializeField] private String threatName;
        [SerializeField] private String threatDescription;

        [SerializeField] private int threatTime;

        [SerializeField] private BaseThreatMinigame[] mingames;

        public NetworkVariable<int> numTasksTotal;
        public NetworkVariable<int> numTasksFinished;

        private NetworkVariable<bool> timeConstrained;
        private NetworkVariable<float> timeToSolve;

        private NetworkVariable<bool> finished;

        public NetworkVariable<bool> Finished => finished;
        public Dictionary<string, ServerDeviceInteractableBase> threatObjectives = new Dictionary<string, ServerDeviceInteractableBase>();

        private String[] threatObjectivesString;
        
        public BaseThreatMinigame[] Minigames => mingames;

        public int ThreatTime => threatTime;
        public string ThreatName => threatName;
        public string ThreatDescription => threatDescription;

        public event Action<ulong> ActivateEvent;

        [ClientRpc]
        public void ActivateClientRpc()
        {
            ActivateEvent?.Invoke(0);
        }

        [ClientRpc]
        public void CommunicateTaskInfosToClientClientRpc(String title, String description)
        {
            this.threatName = title;
            this.threatDescription = description;
            // this.threatObjectivesString = objectives;
        }

        protected override void Awake()
        {
            base.Awake();

            timeConstrained = new NetworkVariable<bool>();
            timeToSolve = new NetworkVariable<float>();
            finished = new NetworkVariable<bool>();
        }
    }
}