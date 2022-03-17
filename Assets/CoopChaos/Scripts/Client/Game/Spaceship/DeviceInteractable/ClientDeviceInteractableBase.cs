using System;
using CoopChaos;
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
        }

        public override void Highlight()
        {
            highlight.SetActive(true);
        }
        
        public override void Unhighlight()
        {
            highlight.SetActive(false);
        }

        protected override void Awake()
        {
            base.Awake();
        }
    }
}