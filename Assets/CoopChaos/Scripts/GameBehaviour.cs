using System;
using Unity.Netcode;
using UnityEngine;

namespace CoopChaos
{
    public class GameBehaviour : MonoBehaviour
    {
        private void Start()
        {
            NetworkManager.Singleton.StartClient();
        }
    }
}