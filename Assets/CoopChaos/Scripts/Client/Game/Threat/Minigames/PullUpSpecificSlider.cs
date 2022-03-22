using System.Collections.Generic;
using System.Transactions;
using UnityEngine;
using UnityEngine.UI;

namespace CoopChaos
{
    public class PullUpSpecificSlider : BaseThreatMinigame
    {
        Slider chosen;
        [SerializeField] Slider[] sliders;
    
        public override void StartMinigame()
        {
            base.StartMinigame();
            if(!minigameStarted) {
                int rnd = Random.Range(0, sliders.Length);
                chosen = sliders[rnd];
                chosen.onValueChanged.AddListener((c) => SliderChanged(c));
                chosen.value = 0;
                minigameStarted = true;
            }
        }

        private void SliderChanged(float c)
        {
            if(c == 100) {
                FinishMinigame();
            }
        }
    }
}