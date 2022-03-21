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
        }

        void ButtonClicked()
        {
            FinishMinigame();
        }
    }
}