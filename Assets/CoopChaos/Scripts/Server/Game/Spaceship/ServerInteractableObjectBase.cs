using Unity.Netcode;
using UnityEngine;

namespace Yame
{
    public abstract class ServerInteractableObjectBase : NetworkBehaviour
    {
        public abstract void Interact(ulong clientId);
    }
}