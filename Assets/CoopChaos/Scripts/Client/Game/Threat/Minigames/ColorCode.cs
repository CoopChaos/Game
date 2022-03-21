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

        int currentIdx;

        [SerializeField] bool isViewer;

        string[] colors = new string[] { "Gelb", "Orange", "Rot", "Grün", "Blau", "Pink" };
        int[] nums;
        [SerializeField] string correctCodeInput;

        [SerializeField]
        private Text CodeDisplay;
    
        public override void StartMinigame()
        {
            base.StartMinigame();
            currentIdx = 0;

            nums = correctCodeInput.Split(' ').Select(int.Parse).ToArray();
            Debug.Log(nums.ToString());
            if(isViewer)
            {
                CodeDisplay.text = GenerateCodeDisplay(nums);
            } else {
                for (int i = 0; i < buttons.Length; i++)
                {
                    int x = i;
                    Button b = buttons[i];
                    b.onClick.AddListener(() => ButtonClicked(x));
                }
            }
        }

        private string GenerateCodeDisplay(int[] nums)
        {
            string code = "Folgende Reihenfolge ist die Loesung ";
            for (int i = 0; i < nums.Length; i++)
            {
                code += colors[nums[i]];
                if(i != nums.Length - 1)
                {
                    code += " ";
                }
            }
            return code;
        }

        void Update()
        {
            if(currentIdx >= buttons.Length && !isViewer) FinishMinigame();
        }
        void ButtonClicked(int i)
        {
            if(isViewer) return;
            if(nums[currentIdx] == i)
            {;
                currentIdx++;
            } else {
                currentIdx = 0;
            }
        }
    }
}