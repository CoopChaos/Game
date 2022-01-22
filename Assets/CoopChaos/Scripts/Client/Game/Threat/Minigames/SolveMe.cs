using System.Transactions;
using UnityEngine;
using UnityEngine.UI;

namespace CoopChaos
{
    public class SolveMe : BaseThreatMinigame
    {
        public Button solverButton;

        public override void Start()
        {
            base.Start();
            
            solverButton.onClick.AddListener(ButtonClicked);
        }

        void ButtonClicked()
        {
            Debug.Log("Solve Me solved");
            FinishMinigame();
        }
    }
}