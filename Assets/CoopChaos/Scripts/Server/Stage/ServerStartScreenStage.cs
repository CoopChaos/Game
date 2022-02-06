using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Netcode;

namespace CoopChaos
{
    public class ServerStartScreenStage : Stage
    {
        private const float TimeToLoad = 5.0f;
        
        public override StageType Type => StageType.GameStart;

        protected override void Start()
        {
            base.Start();

            StartCoroutine(LoadSceneInTime(TimeToLoad));
        }
        private IEnumerator LoadSceneInTime(float time)
        {
            yield return new WaitForSeconds(time);
            NetworkManager.SceneManager.LoadScene("Game", LoadSceneMode.Single);
        }
    }
}