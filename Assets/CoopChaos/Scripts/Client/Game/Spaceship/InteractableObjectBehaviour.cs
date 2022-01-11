using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace Yame
{
    // works as a proxy for objects in spaceship
    // every object in a spaceship that can be interacted with has a InteractableObjectBehaviour
    // which event handler is registered by the object
    public class InteractableObjectBehaviour : MonoBehaviour
    {   
        public int InteractableObjectId { get; set; }
        public Vector2 InteractionPoint => gameObject.transform.position;

        public event Action OnHighlight;
        public event Action OnUnhighlight;
        
        public void Highlight()
            => OnHighlight?.Invoke();

        public void Unhighlight()
            => OnUnhighlight?.Invoke();

        private void Start()
        {
            Assert.IsNotNull(OnHighlight, "OnHighlight is null");
            Assert.IsNotNull(OnUnhighlight, "OnUnhighlight is null");
        }
    }
}