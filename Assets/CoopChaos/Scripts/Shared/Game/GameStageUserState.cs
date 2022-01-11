using Unity.Netcode;
using UnityEngine;

namespace Yame
{
    public class GameStageUserState : NetworkBehaviour
    {
        [ServerRpc]
        public void SetTargetPositionServerRpc()
        {
        }
    }
}