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

        int currentItem = 0;

        int correctCount = 0;

        public override async void StartMinigame()
        {
            base.StartMinigame();

            itemMap = new int[grabFields.Length];
            itemPutMap = new int[placeFields.Length];

            for (int i = 0; i < grabFields.Length; i++)
            {
                int rnd = UnityEngine.Random.Range(0, items.Length);

                itemMap[i] = rnd;
                itemPutMap[i] = rnd;
            }

            for (int i = 0; i < grabFields.Length; i++)
            {
                int x = i;
                grabFields[i].GetComponent<Image>().sprite = items[itemMap[i]];
                grabFields[i].onClick.AddListener(() => { OnGrabFieldClick(x); });
            }

            for (int i = 0; i < placeFields.Length; i++)
            {
                int x = i;
                placeFields[i].onClick.AddListener(() => { OnPlaceFieldClick(x); });
            }
        }

        private void OnPlaceFieldClick(int x)
        {
            if(itemPutMap[x] == currentItem)
            {
                placeFields[x].GetComponent<Image>().sprite = items[itemPutMap[x]];
                correctCount++;
                placeFields[x].interactable = false;
            }
        }

        private void OnGrabFieldClick(int i)
        {
            currentItem = itemMap[i];
            Debug.Log("Clicked on grab field " + i);
        }

        void ButtonClicked()
        {
            FinishMinigame();
        }

        void Update()
        {
            if (correctCount == grabFields.Length)
            {
                FinishMinigame();
            }
        }
    }
}