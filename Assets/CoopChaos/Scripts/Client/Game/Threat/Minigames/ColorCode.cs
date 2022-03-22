using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CoopChaos
{
    public class ColorCode : BaseThreatMinigame
    {
        [SerializeField] Button[] buttons;

        [SerializeField] private TextMeshProUGUI DeviceDisplay;

        int currentIdx;

        [SerializeField] bool isViewer;

        string[] colors = new string[] { "Gelb", "Orange", "Rot", "Grün", "Blau", "Pink" };
        [SerializeField] private Sprite[] colorSprites;

        [SerializeField] private Image[] colorSequence;
        int[] nums;
        [SerializeField] string correctCodeInput;

        [SerializeField]
        private Text CodeDisplay;
    
        public override void StartMinigame()
        {
            base.StartMinigame();

            if(!minigameStarted)
            {
                currentIdx = 0;

                nums = correctCodeInput.Split(' ').Select(int.Parse).ToArray();
                Debug.Log(nums.ToString());
                if(isViewer)
                {
                    GenerateCodeDisplay(nums);
                } else {
                    for (int i = 0; i < buttons.Length; i++)
                    {
                        int x = i;
                        Button b = buttons[i];
                        b.onClick.AddListener(() => ButtonClicked(x, b));
                    }
                }
                minigameStarted = true;
            }
        }

        private IEnumerator SetButtonClickedAction(Button b, bool ok)
        {
            DeviceDisplay.text = ok ? "OK" : "WRONG";
            yield return new WaitForSeconds(0.2f);
            if(!ok) {
                DeviceDisplay.text = "RESET";
                yield return new WaitForSeconds(0.2f);
            }
            DeviceDisplay.text = "";
        }

        private void GenerateCodeDisplay(int[] nums)
        {
            for (int i = 0; i < nums.Length; i++)
            {
                colorSequence[i].sprite = colorSprites[nums[i]];
            }
            return;
        }

        void Update()
        {
            if(currentIdx >= buttons.Length && !isViewer) FinishMinigame();
        }
        void ButtonClicked(int i, Button b)
        {
            if(isViewer) return;
            if(nums[currentIdx] == i) {
                StartCoroutine(SetButtonClickedAction(b, true));
                currentIdx++;
            } else {
                StartCoroutine(SetButtonClickedAction(b, false));
                currentIdx = 0;
            }
        }
    }
}