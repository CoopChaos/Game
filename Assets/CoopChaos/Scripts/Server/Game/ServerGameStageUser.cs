using Unity.Netcode;
using UnityEngine;

namespace Yame
{
    public class ServerGameStageUser : NetworkBehaviour
    {
        public override void OnNetworkSpawn()
        {
            if (IsClient)
            {
                enabled = false;
            }
        }
    }
}
