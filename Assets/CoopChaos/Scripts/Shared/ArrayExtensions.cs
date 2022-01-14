using Unity.Netcode;
using UnityEngine;

namespace CoopChaos
{
    public static class ArrayExtensions
    {
        public static T FindComponentInBehaviour<T>(this MonoBehaviour[] array) where T : Component
        {
            foreach (var monoBehaviour in array)
            {
                var component = monoBehaviour.GetComponent<T>();
                if (component != null)
                {
                    return component;
                }
            }
            
            return null;
        }
    }
}