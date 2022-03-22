using System.Transactions;
using UnityEngine;
using UnityEngine.UI;

namespace CoopChaos
{
    public class SolveMe : BaseThreatMinigame
    {
        public Button solverButton;

        public override void StartMinigame()
        {
            base.StartMinigame();
            if(!minigameStarted) {
                solverButton.onClick.AddListener(ButtonClicked);
                Debug.Log("Started");
                minigameStarted = true;
            }
        }

        void ButtonClicked()
        {
            Debug.Log("Solve Me solved");
            FinishMinigame();
        }
    }
}