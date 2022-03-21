using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CoopChaos
{
    public class GrabItem : BaseThreatMinigame
    {
        [SerializeField]
        private Button[] solverButton;

        [SerializeField]
        private Sprite[] items;

        [SerializeField]
        private string[] itemNames;

        [SerializeField]
        private TextMeshProUGUI instruction;

        int correctItem = 0;

        public override void StartMinigame()
        {
            base.StartMinigame();

            correctItem = Random.Range(0, solverButton.Length);

            instruction.text = "Nehme " + itemNames[correctItem] + " aus dem Regal.";

            for (int i = 0; i < solverButton.Length; i++)
            {
                solverButton[i].GetComponent<Image>().sprite = items[i];
                if(i == correctItem)
                {
                    solverButton[i].onClick.AddListener(() => ButtonClicked());
                }
            }
        }

        void ButtonClicked()
        {
            FinishMinigame();
        }
    }
}