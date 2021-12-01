using Unity.Netcode;

namespace CoopChaos
{
    public class ServerGameStage : NetworkBehaviour
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