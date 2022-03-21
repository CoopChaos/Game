using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Assertions;

namespace CoopChaos
{
    public class ClientSpaceship : NetworkBehaviour
    {
        private SpaceshipState spaceshipState;
        [SerializeField] private GameObject LastHealthBarPrefab;
        [SerializeField] private GameObject HealthbarPrefab;
        [SerializeField] private GameObject Anchor;
        [SerializeField] private GameObject HealthbarContainer;



        public override void OnNetworkSpawn()
        {
            if (!IsClient)
            {
                enabled = false;
                return;
            }
            
            spaceshipState.Health.OnValueChanged += HandleHealthChanged;
            // Debug.Log(spaceshipState.Health.Value);
            // for(int i = 0; i < spaceshipState.Health.Value; i += 100)
            // {
            //     Debug.Log("health: " + i);
            //     var healthbar = Instantiate(HealthbarPrefab, HealthbarContainer.transform);
            //     healthbar.transform.localPosition = new Vector3(i * 2, 0, 0);
            // }
        }
        
        private void HandleHealthChanged(float health, float newHealth)
        {
            //Debug.Log(((RectTransform)HealthbarPrefab.transform).rect.width);
            float healthbarWidth = ((RectTransform)HealthbarPrefab.transform).rect.width;
            float lastHealthWidth = ((RectTransform)LastHealthBarPrefab.transform).rect.width;

            var lastHealthBar = Instantiate(LastHealthBarPrefab, HealthbarContainer.transform);
            lastHealthBar.transform.localPosition = new Vector3(Anchor.transform.localPosition.x, Anchor.transform.localPosition.y, 0);
            
            for (int i = 100; i < spaceshipState.Health.Value; i+=100)
            {
                var healthBar = Instantiate(HealthbarPrefab, HealthbarContainer.transform);
                float offsetX = Anchor.transform.localPosition.x + healthbarWidth * i/100;
                healthBar.transform.localPosition = new Vector3(offsetX, Anchor.transform.localPosition.y, 0);
            }
            // for(int i = 100; i < spaceshipState.Health.Value; i += 100)
            // {
            //     var healthbar = Instantiate(HealthbarPrefab, LastHealthBarPrefab.transform);
            //     float xOffset = Anchor.transform.position.x + lastHealthWidth;
            //     healthbar.transform.position = new Vector3(xOffset, Anchor.transform.position.y, -200);
            // }
            StartCoroutine(FindObjectOfType<AnimationManager>().Shake());
        }

        private void Awake()
        {
            spaceshipState = GetComponent<SpaceshipState>();
            Assert.IsNotNull(spaceshipState);

            

        }
    }
}