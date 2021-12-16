using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;

namespace CoopChaos
{
    [RequireComponent(typeof(Rigidbody2D), typeof(PlayerInput))]
    public class ClientPlayerCharacterBehaviour : NetworkBehaviour
    {
        [SerializeField] private float speed = 12f;
        [SerializeField] private GameObject characterCamera;
        
        private Rigidbody2D rigidbody;
        private InputAction moveInputAction;
        private InputAction interactInputAction;

        public void SetColor(Color color)
        {
            GetComponentInChildren<SpriteRenderer>().color = color;
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

            var playerInput = GetComponent<PlayerInput>();
            moveInputAction = playerInput.actions["move"];
            interactInputAction = playerInput.actions["interact"];
            rigidbody = GetComponent<Rigidbody2D>(); 
        }

        private void FixedUpdate()
        {
            Vector2 moveInput = moveInputAction.ReadValue<Vector2>();
            rigidbody.velocity += moveInput * Time.deltaTime * speed;
        }

        private void Awake()
        {
            Assert.IsNotNull(characterCamera);
        }
    }
}