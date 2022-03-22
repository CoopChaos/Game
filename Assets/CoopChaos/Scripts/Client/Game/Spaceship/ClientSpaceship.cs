using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Assertions;
using Object = UnityEngine.Object;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

namespace CoopChaos
{
    public class ClientSpaceship : NetworkBehaviour
    {
        private SpaceshipState spaceshipState;
        
        [SerializeField] private GameObject LastHealthBarPrefab;
        [SerializeField] private GameObject HealthbarPrefab;
        [SerializeField] private GameObject HealthbarContainer;

        private List<GameObject> healthBarParts;
        
        const float OffsetBetweenHealthbars = 10;

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
            if (healthBarParts != null)
            {
                foreach (var healthBarPart in healthBarParts)
                {
                    Destroy(healthBarPart);
                }
                
                healthBarParts.Clear();
            }
            else 
            {
                healthBarParts = new List<GameObject>();
            }

            if (spaceshipState.Health.Value > 0)
            {
                var go = Instantiate(LastHealthBarPrefab, HealthbarContainer.transform);
                go.GetComponent<RectTransform>();

                healthBarParts.Add(go);

                for (int i = 0; i < spaceshipState.Health.Value / 100 - 1; ++i)
                {
                    go = Instantiate(HealthbarPrefab, HealthbarContainer.transform);
                    go.GetComponent<RectTransform>();

                    healthBarParts.Add(go);
                }
            }

            Debug.Log("Oldhealth: " + health + " newhealth: " + newHealth);
            if(!(health == 0 && newHealth > 0))
                StartCoroutine(FindObjectOfType<AnimationManager>().Shake());
        }

        private void Awake()
        {
            spaceshipState = GetComponent<SpaceshipState>();
            Assert.IsNotNull(spaceshipState);

            

        }
    }
}