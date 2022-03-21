using System;
using System.Transactions;
using UnityEngine;
using UnityEngine.UI;

namespace CoopChaos
{
    public class ConnectionFix : BaseThreatMinigame
    {
        public Button leak1;
        public Button leak2;
        public Button leak3;
        private int counter;

        public override void StartMinigame()
        {
            base.StartMinigame();
            counter = 0;
            leak1.onClick.AddListener(() => ButtonClicked(leak1));
            leak2.onClick.AddListener(() => ButtonClicked(leak2));
            leak3.onClick.AddListener(() => ButtonClicked(leak3));
            Debug.Log("Started");
        }

        private void Update()
        {
            if (counter == 3)
            {
                FinishMinigame();
            }
        }

        void ButtonClicked(Button btn)
        {
            btn.GetComponent<Image>().enabled = false;
            counter++;
        }
    }
}