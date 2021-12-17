using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;

namespace CoopChaos
{
    [RequireComponent(typeof(Rigidbody2D), typeof(PlayerInput))]
    public class GameStageUser : NetworkBehaviour
    {
        [SerializeField] private float speed = 12f;
        [SerializeField] private GameObject characterCamera;

        private GameStageUserApi api;
        private InteractableObjectBehaviour currentInteractable;
        private SpaceshipState spaceshipState;

        private Rigidbody2D rigidbody;
        private InputAction moveInputAction;
        private InputAction interactInputAction;

        public void SetColor(Color color)
        {
            GetComponentInChildren<SpriteRenderer>().color = color;
        }

        public void OnInteract()
        {
            api.Interact(currentInteractable.InteractableObjectId);
        }

        public override void OnNetworkSpawn()
        {
            Debug.Assert(characterCamera != null, "Character camera is null");

            if (!IsClient || !IsOwner)
            {
                enabled = false;
                characterCamera.SetActive(false);
                return;
            }
            
            api = GetComponent<GameStageUserApi>();
            spaceshipState = FindObjectOfType<SpaceshipState>();
            rigidbody = GetComponent<Rigidbody2D>();
            
            Assert.IsNotNull(api);
            Assert.IsNotNull(spaceshipState);
            Assert.IsNotNull(rigidbody);
            
            var playerInput = GetComponent<PlayerInput>();
            Assert.IsNotNull(playerInput);
            
            moveInputAction = playerInput.actions["move"];
            interactInputAction = playerInput.actions["interact"];
            
        }

        private void FixedUpdate()
        {
            Vector2 moveInput = moveInputAction.ReadValue<Vector2>();
            rigidbody.velocity += moveInput * Time.deltaTime * speed;
        }

        private void Update()
        {
            HighlightCloseInteractableObjects();
        }

        private void HighlightCloseInteractableObjects()
        {
            var interactableObjects = FindObjectsOfType<InteractableObjectBehaviour>();

            InteractableObjectBehaviour closestInteractableObject = null;
            float closestDistance = float.MaxValue;

            foreach (var interactableObject in interactableObjects)
            {
                var distance = Vector2.Distance(transform.position, interactableObject.transform.position);

                if (distance < closestDistance)
                {
                    closestInteractableObject = interactableObject;
                    closestDistance = distance;
                }
            }

            if (currentInteractable != null && currentInteractable != closestInteractableObject)
            {
                currentInteractable.Unhighlight();
                currentInteractable = null;
            }

            if (closestInteractableObject != null && closestDistance < GameContext.Singleton.InteractRange)
            {
                closestInteractableObject.Highlight();
                currentInteractable = closestInteractableObject;
            }
        }

        private void Awake()
        {
            Assert.IsNotNull(characterCamera);
        }
    }
}