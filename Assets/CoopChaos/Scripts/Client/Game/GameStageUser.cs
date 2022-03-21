using System;
using CoopChaos.Shared;
using Unity.Netcode;
using Unity.Netcode.Components;
// using UnityEditor.PackageManager;
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

        private Canvas pauseMenu;

        private GameStageUserApi api;
        private ClientInteractableObjectBase currentInteractable;
        
        private Rigidbody2D rigidbody;
        private Vector2 movement;
        private InputAction moveInputAction;
        private InputAction interactInputAction;

        private InputAction pauseInputAction;
        private Animator anim;


        enum AnimationState 
        {
            Down,
            Up,
            Left,
            Right,
            Idle
        }

        public void SetColor(Color color)
        {
            //GetComponentInChildren<SpriteRenderer>().color = color;
        }

        [ClientRpc]
        public void SetRoleClientRpc(PlayerRoles role)
        {
            Debug.Log(role.ToString());
            switch(role)
            {
                case PlayerRoles.Pilot:
                    anim.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("Animation/Pilot/Player_Triggers");
                    break;
                case PlayerRoles.Technician:
                    anim.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("Animation/Technician/Technician_Triggers");
                    break;
                case PlayerRoles.Gunner:
                    anim.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("Animation/Gunner/Gunner_Triggers");
                    break;
            }
        }

        public void OnInteract()
        {
            if (currentInteractable != null)
            {
                api.InteractServerRpc(currentInteractable.NetworkObjectId);
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

                Debug.Log($"DISABLE {this.NetworkObjectId}");
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
            if (pauseMenu.enabled) 
            {
                moveInputAction.Enable();
                interactInputAction.Enable();
            } 
            else
            {
                moveInputAction.Disable();
                interactInputAction.Disable();
            }

            pauseMenu.enabled = !pauseMenu.enabled;
            var playerInput = GetComponent<PlayerInput>();
        }

        private void FixedUpdate()
        {
            rigidbody.velocity += movement * speed * Time.fixedDeltaTime ; 
            

           
            if(movement.x > 0.1f)
                AnimationTrigger(AnimationState.Right);
            else if(movement.x < -0.1f)
                AnimationTrigger(AnimationState.Left);
            else if(movement.y > 0.1f)
                AnimationTrigger(AnimationState.Up);
            else if(movement.y < -0.1f)
                AnimationTrigger(AnimationState.Down);
            else
                AnimationTrigger(AnimationState.Idle);

        }

        private void AnimationTrigger(AnimationState state)
        {
            SetAnimationTriggers(state);
            AnimationTriggerServerRpc(state);
        }

        [ServerRpc]
        private void AnimationTriggerServerRpc(AnimationState state)
        {
            AnimationTriggerClientRpc(state);
        }

        [ClientRpc]
        private void AnimationTriggerClientRpc(AnimationState state)
        {
            if(IsOwner)
                return;

            SetAnimationTriggers(state);
        }
        

        private void SetAnimationTriggers(AnimationState trigger)
        {
            switch(trigger)
            {
                case AnimationState.Right:
                    anim.SetTrigger("Right");
                    anim.ResetTrigger("Left");
                    anim.ResetTrigger("Up");
                    anim.ResetTrigger("Down");
                    anim.ResetTrigger("Idle");
                    break;
                case AnimationState.Left:
                    anim.SetTrigger("Left");
                    anim.ResetTrigger("Right");
                    anim.ResetTrigger("Up");
                    anim.ResetTrigger("Down");
                    anim.ResetTrigger("Idle");
                    break;
                case AnimationState.Up:
                    anim.SetTrigger("Up");
                    anim.ResetTrigger("Right");
                    anim.ResetTrigger("Left");
                    anim.ResetTrigger("Down");
                    anim.ResetTrigger("Idle");
                    break;
                case AnimationState.Down:
                    anim.SetTrigger("Down");
                    anim.ResetTrigger("Right");
                    anim.ResetTrigger("Left");
                    anim.ResetTrigger("Up");
                    anim.ResetTrigger("Idle");
                    break;
                case AnimationState.Idle:
                    anim.SetTrigger("Idle");
                    anim.ResetTrigger("Right");
                    anim.ResetTrigger("Left");
                    anim.ResetTrigger("Up");
                    anim.ResetTrigger("Down");
                    break;
            }
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
            rigidbody = GetComponent<Rigidbody2D>();
            anim = GetComponentInChildren<Animator>();
            
            Assert.IsNotNull(api);
            Assert.IsNotNull(rigidbody);
        }
    }
}