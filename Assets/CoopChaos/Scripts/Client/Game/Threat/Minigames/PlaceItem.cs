using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CoopChaos
{
    public class PlaceItem : BaseThreatMinigame
    {
        [SerializeField]
        private Button[] grabFields;

        [SerializeField]
        private Button[] placeFields;

        [SerializeField]
        private Sprite[] items;

        [SerializeField]
        private string[] itemNames;

        private int[] itemMap;
        private int[] itemPutMap;

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