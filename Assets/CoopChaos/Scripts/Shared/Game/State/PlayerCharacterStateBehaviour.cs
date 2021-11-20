using Unity.Netcode;
using UnityEngine;

namespace CoopChaos
{
    public class PlayerCharacterStateBehaviour : NetworkBehaviour
    {
        [ServerRpc]
        public void SetTargetPositionServerRpc()
        {
        }
        
    }
}