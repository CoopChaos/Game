using System;
using CoopChaos.Shared;
using Unity.Netcode;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;

namespace CoopChaos
{
    [RequireComponent(typeof(Rigidbody2D), typeof(PlayerInput), typeof(GameStageUserApi))]
    public class GameStageUser : NetworkBehaviour
    {
        [SerializeField] private float speed = 150f;
        [SerializeField] private GameObject characterCamera;
        [SerializeField] private GameObject circle;

        private GameObject spaceshipControlMenu;
        private GameObject radarMenu;
        private GameObject cannonControlMenu;
        
        private Canvas pauseMenu;

        private GameStageUserApi api;
        private ClientInteractableObjectBase currentInteractable;
        private SpaceshipState spaceshipState;

        private Rigidbody2D rigidbody;
        private Vector2 movement;
        private InputAction moveInputAction;
        private InputAction interactInputAction;

        private InputAction pauseInputAction;

        public void SetColor(Color color)
        {
            GetComponentInChildren<SpriteRenderer>().color = color;
        }

        public void OnInteract()
        {
            if (currentInteractable != null)
            {
                api.InteractServerRpc(currentInteractable.NetworkObjectId);
                if (currentInteractable is ClientSpaceshipControlRoom)
                {
                    spaceshipControlMenu.SetActive(!spaceshipControlMenu.activeSelf);
                } 
                else if (currentInteractable is ClientRadarRoom)
                {
                    radarMenu.SetActive(!radarMenu.activeSelf);
                }
                else if (currentInteractable is ClientCannonRoom)
                {
                    cannonControlMenu.SetActive(!cannonControlMenu.activeSelf);
                }
            }
        }

        public override void OnNetworkSpawn()
        {
            Debug.Assert(characterCamera != null, "Character camera is null");

            var playerInput = GetComponent<PlayerInput>();
            Assert.IsNotNull(playerInput);
            
            if (!IsClient || !IsOwner)
            {
                playerInput.enabled = false;

                enabled = false;
                characterCamera.SetActive(false);
                
                return;
            }
            
            moveInputAction = playerInput.actions["move"];
            interactInputAction = playerInput.actions["interact"];
            pauseInputAction = playerInput.actions["pause"];
        }

        public void OnPause()
        {
            if(pauseMenu.enabled) {
                moveInputAction.Enable();
                interactInputAction.Enable();
            } else {
                moveInputAction.Disable();
                interactInputAction.Disable();
            }

            pauseMenu.enabled = !pauseMenu.enabled;
            var playerInput = GetComponent<PlayerInput>();
        }

        private void FixedUpdate()
        {
            //rigidbody.MovePosition(rigidbody.position + movement * speed * Time.fixedDeltaTime);
            //Vector2 moveInput = moveInputAction.ReadValue<Vector2>();
            rigidbody.velocity += movement * speed * Time.fixedDeltaTime ; 
            
            // Movement
            //rigidbody.AddForce(movement * speed);
            
        }
        
        

        private void Update()
        {
            // Input
            movement = moveInputAction.ReadValue<Vector2>();
            
            HighlightCloseInteractableObjects();
        }

        private void HighlightCloseInteractableObjects()
        {
            var interactableObjects = FindObjectsOfType<ClientInteractableObjectBase>();

            ClientInteractableObjectBase closestInteractableObject = null;
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

            if (closestDistance > GameContextState.Singleton.GameContext.InteractRange)
            {
                closestInteractableObject = null;
            }

            if (currentInteractable != closestInteractableObject)
            {
                if (currentInteractable != null)
                {
                    currentInteractable.Unhighlight();
                    currentInteractable = null;
                }

                if (closestInteractableObject != null)
                {
                    closestInteractableObject.Highlight();
                    currentInteractable = closestInteractableObject;
                }
            }
        }

        private void Awake()
        {
            Assert.IsNotNull(characterCamera);
            
            api = GetComponent<GameStageUserApi>();
            spaceshipState = FindObjectOfType<SpaceshipState>();
            rigidbody = GetComponent<Rigidbody2D>();
            
            pauseMenu = GameObject.Find("PauseMenu").GetComponent<Canvas>();
            pauseMenu.enabled = false;

            spaceshipControlMenu = GameObject.Find("SpaceshipControlMenu");
            spaceshipControlMenu.SetActive(false);
            
            radarMenu = GameObject.Find("RadarMenu");
            radarMenu.SetActive(false);

            cannonControlMenu = GameObject.Find("CannonControlMenu");
            cannonControlMenu.SetActive(false);
            
            Assert.IsNotNull(pauseMenu);
            Assert.IsNotNull(api);
            Assert.IsNotNull(spaceshipState);
            Assert.IsNotNull(rigidbody);
            Assert.IsNotNull(spaceshipControlMenu);
            Assert.IsNotNull(radarMenu);
            Assert.IsNotNull(cannonControlMenu);
        }
    }
}