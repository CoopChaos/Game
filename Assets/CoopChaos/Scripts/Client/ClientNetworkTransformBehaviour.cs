using Unity.Netcode.Components;
using UnityEngine;

namespace CoopChaos.Scripts.Client
{
    public class ClientNetworkTransformBehaviour : NetworkTransform
    {
        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            CanCommitToTransform = IsOwner;
        }

        protected override void Update()
        {
            base.Update();
            
            if (NetworkManager.IsConnectedClient || NetworkManager.IsListening)
            {
                if (CanCommitToTransform)
                    TryCommitTransformToServer(transform, NetworkManager.LocalTime.Time);
            }
        }
    }
}