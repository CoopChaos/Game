using System;
using CoopChaos.CoopChaos.Scripts.Shared.Game.Spaceship;
using UnityEngine;

namespace Yame
{
    public class ClientDeviceInteractableBase : ClientInteractableObjectBase
    {
        [SerializeField] protected GameObject deviceSprite;
        [SerializeField] protected GameObject highlight;

        protected bool claimedByMe = false;
        
        protected virtual void HandleClaimChanged(bool claim, bool oldClaim)
        {
            if (true)
            {
                Debug.Log("Claimed");
            }

            claimedByMe = true;
            
            deviceSprite.SetActive(!claim);
        }

        private void HandleOnHighlight()
        {
            highlight.SetActive(true);
        }
        
        private void HandleOnUnhighlight()
        {
            highlight.SetActive(false);
        }

        protected override void Awake()
        {
            base.Awake();
            
            interactable.OnHighlight += HandleOnHighlight;
            interactable.OnUnhighlight += HandleOnUnhighlight;
        }
    }
}