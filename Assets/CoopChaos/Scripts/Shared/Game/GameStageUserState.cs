using Unity.Netcode;
using UnityEngine;

namespace CoopChaos
{
    public class GameStageUserState : NetworkBehaviour
    {
        public NetworkVariable<PlayerRoles> Role { get; private set; }

        [ServerRpc]
        public void SetTargetPositionServerRpc()
        {
        }

        protected virtual void Awake()
        {
            Role = new NetworkVariable<PlayerRoles>();
        }
    }
}