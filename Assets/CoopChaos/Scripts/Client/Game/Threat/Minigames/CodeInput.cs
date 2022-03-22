using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CoopChaos
{
    public class CodeInput : BaseThreatMinigame
    {
        [SerializeField] Button[] buttons;

        [SerializeField] Sprite[] numbers;
        [SerializeField] Sprite[] numbersRed;
        [SerializeField] Sprite[] numbersGreen;
        int ctr = 1;
        int size = 6;

        // This is for the cooperative color code input game
        [SerializeField] bool colorCoopMode;
        [SerializeField] bool isColorCoopModeViewer;

        [SerializeField] int[] correctCodeInput;

        [SerializeField]
        private TextMeshProUGUI CodeDisplay;

        Color[] colors = new Color[] { Color.red, Color.blue, Color.green, Color.yellow, Color.cyan, Color.magenta, Color.cyan, Color.gray, Color.white, Color.black };
    
        private void Initialize()
        {
            ctr = 1;
            size = buttons.Length;
            Debug.Log(buttons.Length);

            CodeDisplay.text = "";

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
                    b.GetComponent<Image>().sprite = numbers[rnd-1];
                    b.GetComponentInChildren<Text>().text = rnd.ToString();
                }
            }
            minigameStarted = true;
        }

        public override void StartMinigame()
        {
            base.StartMinigame();
            if(!minigameStarted) Initialize();
            Debug.Log("Init Finished");
        }


        void Update()
        {
            if(ctr > size) FinishMinigame();
        }

        private IEnumerator SetButtonOnColor(Button b, int i, bool red)
        {
            b.GetComponent<Image>().sprite = red ? numbersRed[i] : numbersGreen[i];
            yield return new WaitForSeconds(0.5f);
            b.GetComponent<Image>().sprite = numbers[i];
        }

        void ButtonClicked(Button b)
        {
            int i = int.Parse(b.GetComponentInChildren<Text>().text);
            Debug.Log("Button clicked: " + i);

            if (colorCoopMode)
            {
                if(correctCodeInput[ctr] == i) ctr++;
            } else {
                if (i == ctr) {
                    CodeDisplay.text += i;
                    StartCoroutine(SetButtonOnColor(b, i-1, false));
                    ctr++;
                } else {
                    CodeDisplay.text = "";
                    StartCoroutine(SetButtonOnColor(b, i-1, true));
                    ctr = 1;
                }
            }
        }
    }
}