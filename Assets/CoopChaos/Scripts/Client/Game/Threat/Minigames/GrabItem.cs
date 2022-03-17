using UnityEngine;
using UnityEngine.UI;

namespace CoopChaos
{
    public class GrabItem : BaseThreatMinigame
    {
        [SerializeField]
        private Button solverButton;

        public override void StartMinigame()
        {
            base.StartMinigame();
            solverButton.onClick.AddListener(ButtonClicked);
        }

        void ButtonClicked()
        {
            FinishMinigame();
        }
    }
}