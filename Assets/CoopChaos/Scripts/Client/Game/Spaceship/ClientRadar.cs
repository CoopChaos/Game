
using System.Collections.Generic;
using CoopChaos.CoopChaos.Scripts.Shared.Game.Spaceship;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Assertions;

namespace CoopChaos
{
    [RequireComponent(typeof(RadarState))]
    public class ClientRadar : NetworkBehaviour
    {
        [SerializeField] private GameObject radarPointPrefab;

        private RadarState radarState;
        private GameObject radarMenu;
        private List<GameObject> radarObjects;

        public override void OnNetworkSpawn()
        {
            if (!IsClient)
            {
                enabled = false;
                return;
            }
            
            radarState.OnRadarUpdate += HandleRadarUpdate;
        }

        private void HandleRadarUpdate()
        {
            Debug.Log("Client Handle update");
            foreach (var radarObject in radarObjects)
            {             
                Destroy(radarObject);
            }

            
            
            foreach (var entity in radarState.RadarEntities)
            {
                Debug.Log("Draw");
                var elem= Instantiate(radarPointPrefab, new Vector2(entity.X, entity.Y), Quaternion.identity);
                elem.transform.SetParent(radarMenu.transform, false);
                radarObjects.Add(elem);
            }
            
        }

        private void Awake()
        {
            radarMenu = GameObject.Find("RadarMenu");
            radarState = GetComponent<RadarState>();
            radarObjects = new List<GameObject>();


            Assert.IsNotNull(radarMenu);
        }
    }
}
