using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CoopChaos
{
    public class BaseThreatMinigame : MonoBehaviour, IThreatMinigame
    {
        public Canvas miniGameContainer;
        private bool minigameFinished;

        protected bool minigameStarted;
        public virtual void Start()
        {
            // ensure container is disabled at game start
            this.minigameFinished = false;
            this.miniGameContainer.enabled = false;
            this.minigameStarted = false;
        }

        public bool IsFinished()
        {
            return this.minigameFinished;
        }

        public bool IsOpen()
        {
            return this.miniGameContainer.enabled;
        }

        public virtual void StartMinigame()
        {
            this.miniGameContainer.enabled = true;
        }

        public virtual void PauseMinigame()
        {
            this.miniGameContainer.enabled = false;
        }

        public void FinishMinigame()
        {
            this.minigameFinished = true;
            this.miniGameContainer.enabled = false;
        }
    }
}
