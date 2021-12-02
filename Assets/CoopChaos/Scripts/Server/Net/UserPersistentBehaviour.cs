using System;
using JetBrains.Annotations;
using Unity.Netcode;

namespace CoopChaos
{
    // behaviour of user over the whole connection
    public class UserPersistentBehaviour : NetworkBehaviour
    {
        public ServerUserModel UserModel { get; set; }

        public override void OnNetworkSpawn()
        {
            if (!IsServer)
            {
                enabled = false;
                return;
            }
        }
    }
}