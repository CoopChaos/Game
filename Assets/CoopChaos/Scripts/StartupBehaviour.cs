using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Yame
{
    // used to automatically transition from startup scene to first scene
    public class StartupBehaviour : MonoBehaviour
    {
        private void Start()
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
}