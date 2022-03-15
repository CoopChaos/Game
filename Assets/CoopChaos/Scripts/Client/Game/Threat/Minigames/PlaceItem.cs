using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CoopChaos
{
    public class PlaceItem : BaseThreatMinigame
    {
        [Serializable]
        public struct ObjectFieldRelation {
            public Button item;
            public Button slot;
        }

        Dictionary<Button, Button> ofr = new Dictionary<Button, Button>();
        

        [SerializeField]
        public ObjectFieldRelation[] objectFieldRelations;

        private Button currentItem;

        public override void StartMinigame()
        {
            base.StartMinigame();
            
            foreach (var objectFieldRelation in objectFieldRelations)
            {
                objectFieldRelation.item.onClick.AddListener(() => ItemClicked(objectFieldRelation.item));
                objectFieldRelation.slot.onClick.AddListener(() => SlotClicked(objectFieldRelation.slot));
                ofr.Add(objectFieldRelation.slot, objectFieldRelation.item);
            }
        }

        private void SlotClicked(Button slot)
        {
            if(ofr[slot] == currentItem) {
                ofr[slot].enabled = false;
                slot.enabled = false;

                ofr.Remove(slot);
            }

            if(ofr.Count <= 0) {
                FinishMinigame();
            }
        }

        private void ItemClicked(Button item)
        {
            currentItem = item;
        }
    }
}