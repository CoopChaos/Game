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
    
        public override void StartMinigame()
        {
            base.StartMinigame();
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
                b.GetComponentInChildren<Text>().text = rnd.ToString();
            }
            Debug.Log("Init Finished");
        }

        void ButtonClicked(Button b)
        {
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