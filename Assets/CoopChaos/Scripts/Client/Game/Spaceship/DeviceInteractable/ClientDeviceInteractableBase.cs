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
            
            deviceSprite.SetActive(!claim);
        }
        
        public override void Highlight()
        {
            throw new NotImplementedException();
        }

        public override void Unhighlight()
        {
            throw new NotImplementedException();
        }

        protected override void Awake()
        {
            base.Awake();
            
            // TODO reimplement highlight
        }
    }
}