
using System.Collections.Generic;
using CoopChaos.CoopChaos.Scripts.Shared.Game.Spaceship;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Assertions;

namespace CoopChaos
{
    public class ClientRadar : NetworkBehaviour
    {
        [SerializeField] private GameObject radarPointPrefab;
        [SerializeField] private GameObject radarContainer;

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
            
            radarState.RadarEntities.OnListChanged += HandleListChanged;

            foreach (var entity in radarState.RadarEntities)
            {
                HandleAdd(entity);
            }
        }

        private void HandleListChanged(NetworkListEvent<RadarEntity> changeEvent)
        {
            switch (changeEvent.Type)
            {
                case NetworkListEvent<RadarEntity>.EventType.Add:
                    HandleAdd(changeEvent.Value);
                    break;
                case NetworkListEvent<RadarEntity>.EventType.RemoveAt:
                    HandleRemove(changeEvent.Index);
                    break;
                case NetworkListEvent<RadarEntity>.EventType.Clear:
                    HandleClear();
                    break;
                case NetworkListEvent<RadarEntity>.EventType.Value:
                    HandleValueChange(changeEvent.Index, changeEvent.Value);
                    break;
            }
        }
        
        private void HandleClear()
        {
            foreach (var radarObject in radarObjects)
            {
                Destroy(radarObject);
            }
            
            radarObjects.Clear();
        }

        private void HandleAdd(RadarEntity value)
        {
            var rt = radarContainer.GetComponent<RectTransform>();
            var elem = Instantiate(
                radarPointPrefab, 
                new Vector2(
                    value.X + rt.rect.width * 0.5f,
                    value.Y + rt.rect.height * 0.5f),
                Quaternion.identity,
                radarContainer.transform);
            elem.transform.SetParent(radarMenu.transform, false);
            radarObjects.Add(elem);
        }
        
        private void HandleRemove(int index)
        {
            Destroy(radarObjects[index]);
            radarObjects.RemoveAt(index);
        }
        
        private void HandleValueChange(int index, RadarEntity value)
        {
            radarObjects[index].transform.localPosition = new Vector2(value.X, value.X);
        }

        private void Awake()
        {
            radarMenu = GameObject.Find("RadarMenu");
            radarState = GetComponent<RadarState>();
            radarObjects = new List<GameObject>();
            
            Assert.IsNotNull(radarMenu);
            Assert.IsNotNull(radarState);
        }
    }
}
