using System;
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
        [SerializeField] private RectTransform background;
        [SerializeField] private GameObject highlight;
        [SerializeField] private GameObject radarContainer;
        [SerializeField] private GameObject radarPointPrefab;
        [SerializeField] private GameObject radarMenu;

        private RadarRoomState radarRoomState;
        private List<GameObject> radarObjects;
        
        private bool highlighted = false;

        public override void Highlight()
        {
            highlighted = true;
            highlight.SetActive(true);
        }

        public override void Unhighlight()
        {
            highlighted = false;
            highlight.SetActive(false);
            radarMenu.SetActive(false);
        }
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
            radarRoomState = GetComponent<RadarRoomState>();
            radarObjects = new List<GameObject>();
            
            radarRoomState.InteractEvent += user =>
            {
                if(user == NetworkManager.Singleton.LocalClientId && highlighted)
                    radarMenu.SetActive(!radarMenu.activeSelf);
            };

            Assert.IsNotNull(radarRoomState);
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
            var parent = radarContainer.GetComponent<RectTransform>();
            
            var d = radarRoomState.RadarMaxRange * 2;
            var r = Math.Min(background.rect.width * 0.95f, background.rect.height * 0.95f) / d;

            var elem = Instantiate(
                radarPointPrefab, 
                new Vector2(0, 0),
                Quaternion.identity,
                radarContainer.transform);

            if (value.Type == DetectionType.AliveProjectileObject)
            {
                Debug.Log($"Found projectile: {value.X}, {value.Y}");
            }
            
            elem.GetComponent<Image>().color = GetColor(value.Type);
            elem.GetComponent<RectTransform>().sizeDelta = new Vector2(value.Size * r * 12, value.Size * r * 12);

            elem.GetComponent<RectTransform>().localPosition = new Vector3(
                (value.X - radarRoomState.CenterX.Value) * r, 
                (value.Y - radarRoomState.CenterY.Value) * r, 0);
            
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