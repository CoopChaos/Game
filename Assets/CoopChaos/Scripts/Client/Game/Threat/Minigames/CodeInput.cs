using System.Collections.Generic;
using System.Transactions;
using UnityEngine;
using UnityEngine.UI;

namespace CoopChaos
{
    public class CodeInput : BaseThreatMinigame
    {
        [SerializeField] Button[] buttons;
        LinkedList<int> nums = new LinkedList<int>();
        int ctr;
        int size = 6;

        // This is for the cooperative color code input game
        [SerializeField] bool colorCoopMode;
        [SerializeField] bool isColorCoopModeViewer;

        [SerializeField] int[] correctCodeInput;

        Queue<int> correctCode;

        Color[] colors = new Color[] { Color.red, Color.blue, Color.green, Color.yellow, Color.cyan, Color.magenta, Color.cyan, Color.gray, Color.white, Color.black };
    
        public override void StartMinigame()
        {
            base.StartMinigame();
            correctCode = new Queue<int>(correctCodeInput);
            ctr = 1;
            size = buttons.Length;
            Debug.Log(buttons.Length);
            for (int i = 0; i < size; i++)
            {
                Button b = buttons[i];
                // TODO: fix loop
                b.onClick.AddListener(() => ButtonClicked(b));
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
            Debug.Log("Init Finished");
        }

        void ButtonClicked(Button b)
        {
            if (colorCoopMode)
            {
                if(correctCode.Peek().ToString() == b.GetComponentInChildren<Text>().text)
                {
                    correctCode.Dequeue();
                    b.GetComponent<Image>().color = Color.green;
                    if (correctCode.Count == 0)
                    {
                        Debug.Log("Finished");
                    }
                } else
                {
                    Debug.Log("Wrong");
                }
            } else {
                int num = int.Parse(b.GetComponentInChildren<Text>().text);
                Debug.Log("Button clicked: " + num);
                if(ctr == size) {
                    FinishMinigame();
                } else if (num == ctr) {
                    ctr++;
                }
            }
        }
    }
}