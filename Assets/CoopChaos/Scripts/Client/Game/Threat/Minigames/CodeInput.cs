using System.Collections.Generic;
using System.Transactions;
using UnityEngine;
using UnityEngine.UI;

namespace CoopChaos
{
    public class CodeInput : BaseThreatMinigame
    {
        Button[] buttons;
        int ctr;
        int size = 6;
    
        public override void StartMinigame()
        {
            base.StartMinigame();
            ctr = 1;
            size = 6;
            buttons = GetComponentsInChildren<Button>();
            Debug.Log(buttons.Length);
            for (int i = 0; i < size; i++)
            {
                // TODO: fix loop
                buttons[i].onClick.AddListener(() => ButtonClicked(buttons[i]));
                int rnd = 1;
                buttons[i].GetComponentInChildren<Text>().text = rnd.ToString();
            }
        }

        void ButtonClicked(Button b)
        {
            if(b.GetComponentInChildren<Text>().text == ctr.ToString())
            {
                ctr++;
                b.enabled = false;
                if(ctr == size) {
                    FinishMinigame();
                }
            }
            else
            {
            }
        }
    }
}