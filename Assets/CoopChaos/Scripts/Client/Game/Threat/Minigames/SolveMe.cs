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
            solverButton.onClick.AddListener(ButtonClicked);
            Debug.Log("Started");
        }

        void ButtonClicked()
        {
            Debug.Log("Solve Me solved");
            FinishMinigame();
        }
    }
}