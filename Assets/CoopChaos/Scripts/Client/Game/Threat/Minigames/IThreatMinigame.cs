using UnityEngine;

namespace CoopChaos
{
    public interface IThreatMinigame
    {
        public bool IsFinished();

        public void StartMinigame();

        void FinishMinigame();
    }
}