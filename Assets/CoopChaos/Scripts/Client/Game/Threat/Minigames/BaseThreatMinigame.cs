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
        public virtual void Start()
        {
            // ensure container is disabled at game start
            this.minigameFinished = false;
            this.miniGameContainer.enabled = false;
        }

        public bool IsFinished()
        {
            return this.minigameFinished;
        }

        public virtual void StartMinigame()
        {
            this.minigameFinished = false;
            this.miniGameContainer.enabled = true;
        }

        public void FinishMinigame()
        {
            this.minigameFinished = true;
            this.miniGameContainer.enabled = false;
        }
    }
}
