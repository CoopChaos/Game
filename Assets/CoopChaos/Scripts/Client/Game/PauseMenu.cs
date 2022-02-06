using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

namespace CoopChaos
{
    public class PauseMenu : MonoBehaviour
    {
        [SerializeField]
        private Button pauseButton;
        // Start is called before the first frame update
        void Start()
        {
            pauseButton.onClick.AddListener(OnClickLeaveGame);
        }

        private void OnClickLeaveGame()
        {
            Application.Quit();
        }
    }
}
