using System.Collections.Generic;
using CoopChaos.Simulation.Components;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace CoopChaos
{
    [RequireComponent(typeof(RadarRoomState))]
    public class ClientRadarRoom : ClientInteractableObjectBase
    {
        [SerializeField] private GameObject highlight;
        [SerializeField] private GameObject radarContainer;
        [SerializeField] private GameObject radarPointPrefab;
        
        private GameObject radarMenu;
        private RadarRoomState radarRoomState;
        private List<GameObject> radarObjects;

        public override void OnNetworkSpawn()
        {        
            base.OnNetworkSpawn();

            radarRoomState.IsBlocked.OnValueChanged += HandleOpenChanged;

            radarRoomState.RadarEntities.OnListChanged += HandleListChanged;

            foreach (var entity in radarRoomState.RadarEntities)
            {
                HandleAdd(entity);
            }
        }

        protected override void Awake()
        {
            radarMenu = GameObject.Find("RadarMenu");
            radarRoomState = GetComponent<RadarRoomState>();
            radarObjects = new List<GameObject>();
            
            Assert.IsNotNull(radarMenu);
            Assert.IsNotNull(radarRoomState);
        }

        public override void Highlight()
        {
            highlight.SetActive(true);
        }
        
        public override void Unhighlight()
        {
            highlight.SetActive(false);
            radarMenu.SetActive(false);
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
                    value.X + rt.rect.width * 0.5f - radarRoomState.CenterX.Value,
                    value.Y + rt.rect.height * 0.5f - radarRoomState.CenterY.Value),
                Quaternion.identity,
                radarContainer.transform);
            
            elem.transform.SetParent(radarMenu.transform, false);
            elem.GetComponent<Image>().color = GetColor(value.Type);
            elem.transform.localScale = new Vector2(value.Size * 0.034f, value.Size * 0.034f);
            radarObjects.Add(elem);
        }

        private Color GetColor(DetectionType type)
        {
            switch (type)
            {
                case DetectionType.AliveShipObject:
                    return Color.green;
                case DetectionType.AliveProjectileObject:
                    return Color.red;
                case DetectionType.NaturalDeadObject:
                    return Color.yellow;
                default:
                    return Color.white;
            }
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

        private void HandleOpenChanged(bool open, bool oldOpen)
        {
            
        }
    }
}