using Unity.Netcode;
using UnityEngine;

namespace CoopChaos
{
    public class PlayerCharacterState : NetworkBehaviour
    {
        [ServerRpc]
        public void SetTargetPositionServerRpc()
        {
        }
    }
}