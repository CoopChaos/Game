using Unity.Netcode;
using UnityEngine;

namespace CoopChaos
{
    public class StoryRoomState : InteractableObjectStateBase
    {
        private NetworkVariable<NetworkStringLarge> title;
        private NetworkVariable<NetworkStringLarge> description;
        private NetworkVariable<bool> canTriggerNext;

        public NetworkVariable<NetworkStringLarge> Title => title;
        public NetworkVariable<NetworkStringLarge> Description => description;
        public NetworkVariable<bool> CanTriggerNext => canTriggerNext;

        [ServerRpc(RequireOwnership = false)]
        public void TriggerNextFlightSequenceServerRpc()
        {
            FindObjectOfType<ServerStoryRoom>().TriggerNextFlightSequence();
            TriggerFlightSequenceFinishedClientRpc();
        }
        
        [ClientRpc]
        public void TriggerFlightSequenceFinishedClientRpc()
        {
            StartCoroutine(FindObjectOfType<AnimationManager>().WarpDrive());
        }
        
        protected override void Awake()
        {
            Debug.Log("AWAKE");
            
            description = new NetworkVariable<NetworkStringLarge>();
            title = new NetworkVariable<NetworkStringLarge>();
            canTriggerNext = new NetworkVariable<bool>(false);

            base.Awake();
       }
    }
}