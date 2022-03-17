using System.Collections.Generic;
using System.Transactions;
using UnityEngine;
using UnityEngine.UI;

namespace CoopChaos
{
    public class CodeInput : BaseThreatMinigame
    {
        [SerializeField] Button[] buttons;
        int ctr = 1;
        int size = 6;

        // This is for the cooperative color code input game
        [SerializeField] bool colorCoopMode;
        [SerializeField] bool isColorCoopModeViewer;

        [SerializeField] int[] correctCodeInput;

        Color[] colors = new Color[] { Color.red, Color.blue, Color.green, Color.yellow, Color.cyan, Color.magenta, Color.cyan, Color.gray, Color.white, Color.black };
    
        public override void StartMinigame()
        {
            base.StartMinigame();
            ctr = 1;
            size = buttons.Length;
            Debug.Log(buttons.Length);

            LinkedList<int> nums = new LinkedList<int>();

            for (int i = 0; i < size; i++)
            {
                Button b = buttons[i];
                // TODO: fix loop
                b.onClick.AddListener(() => ButtonClicked(b));
                if(isColorCoopModeViewer) {
                    b.GetComponent<Image>().color = colors[correctCodeInput[i]];
                } else {
                    int rnd = Random.Range(1, buttons.Length+1);
                    while(nums.Contains(rnd))
                    {
                        rnd = Random.Range(1, buttons.Length+1);
                    }
                    nums.AddLast(rnd);

                    if(colorCoopMode) {
                        b.GetComponent<Image>().color = colors[rnd-1];
                        b.GetComponentInChildren<Text>().color = colors[rnd-1];
                    }
                    b.GetComponentInChildren<Text>().text = rnd.ToString();
                }
            }
            Debug.Log("Init Finished");
        }


        void Update()
        {
            if(ctr > size) FinishMinigame();
        }
        void ButtonClicked(Button b)
        {
            int i = int.Parse(b.GetComponentInChildren<Text>().text);
            Debug.Log("Button clicked: " + i);

            if (colorCoopMode)
            {
                if(correctCodeInput[ctr] == i) ctr++;
            } else {
                if (i == ctr) ctr++;
            }
        }
    }
}