using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CoopChaos
{
    public class ServerStartScreenStage : Stage
    {
        private float timer = 5.0f;
        
        public override StageType Type => StageType.GameStart;

        protected override void Start()
        {
            base.Start();
            
            timer = 5.0f;
        }

        public void Update()
        {
            timer = timer - Time.deltaTime;
            Debug.Log(timer);

            if (timer <= 0.0f)
            {
                NetworkManager.SceneManager.LoadScene("Game", LoadSceneMode.Single);
            }
        }
    }
}