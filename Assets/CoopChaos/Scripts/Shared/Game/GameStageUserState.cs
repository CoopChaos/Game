using Unity.Netcode;
using UnityEngine;

namespace CoopChaos
{
    public class GameStageUserState : NetworkBehaviour
    {
        [ServerRpc]
        public void SetTargetPositionServerRpc()
        {
        }
    }
}