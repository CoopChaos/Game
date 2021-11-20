using Unity.Netcode;
using UnityEngine;

namespace CoopChaos.Menu
{
    public class LobbyBehaviour : MonoBehaviour
    {
        private bool ready = false;
        
        public void Join()
        {
            
        }

        public bool ToggleReady()
        {
            ready = !ready;
            
            
            
            return ready;
        }
    }
}