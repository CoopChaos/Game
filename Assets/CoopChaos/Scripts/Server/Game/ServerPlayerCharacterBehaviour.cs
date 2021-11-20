using Unity.Netcode;
using UnityEngine;

namespace CoopChaos
{
    public class ServerPlayerCharacterBehaviour : NetworkBehaviour
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