using Unity.Netcode;
using UnityEngine;
using UnityEngine.Assertions;

namespace CoopChaos
{
    [RequireComponent(typeof(DoorInteractableState))]
    public class ClientDoorInteractable : ClientInteractableObjectBase
    {
        [SerializeField] private GameObject highlight;
        [SerializeField] private GameObject doorLeft;
        [SerializeField] private GameObject doorRight;
        
        [SerializeField] private float openingFactor = 1f;


        private DoorInteractableState doorInteractableState;

        public override void Highlight()
        {
            highlight.SetActive(true);
        }

        public override void Unhighlight()
        {
            highlight.SetActive(false);
        }

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();

            doorInteractableState.Open.OnValueChanged += HandleOpenChanged;
        }
        
        private void HandleOpenChanged(bool open, bool oldOpen)
        {
            // move door sprites to left and right
            doorLeft.transform.position = new Vector3(doorLeft.transform.position.x - (open ? 1 : -1) * openingFactor, doorLeft.transform.position.y, doorLeft.transform.position.z);
            doorRight.transform.position = new Vector3(doorRight.transform.position.x + (open ? 1 : -1) * openingFactor, doorRight.transform.position.y, doorRight.transform.position.z);

        }

        protected override void Awake()
        {
            base.Awake();
            doorInteractableState = GetComponent<DoorInteractableState>();
        }
    }
}