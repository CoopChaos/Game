using Unity.Netcode;
using UnityEngine;

namespace CoopChaos
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
