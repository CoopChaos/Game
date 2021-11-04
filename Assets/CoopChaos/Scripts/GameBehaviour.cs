using System;
using Unity.Netcode;
using UnityEngine;

namespace CoopChaos.Scripts
{
    public class GameBehaviour : MonoBehaviour
    {
        private void Start()
        {
            NetworkManager.Singleton.StartClient();
        }
    }
}