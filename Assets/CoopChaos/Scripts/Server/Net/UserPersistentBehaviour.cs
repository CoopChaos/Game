using JetBrains.Annotations;
using Unity.Netcode;

namespace CoopChaos
{
    // behaviour of user over the whole connection
    public class UserPersistentBehaviour : NetworkBehaviour
    {
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