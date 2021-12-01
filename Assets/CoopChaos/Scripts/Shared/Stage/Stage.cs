using Unity.Netcode;
using UnityEngine;

namespace CoopChaos
{
    public abstract class Stage : NetworkBehaviour
    {
        public abstract StageType Type { get; }

        private static GameObject currentStage;

        protected virtual void Start()
        {
            if (currentStage != null)
            {
                if (currentStage == gameObject)
                    return;
                
                if (currentStage.GetComponent<Stage>().Type == Type)
                {
                    Destroy(gameObject);
                    return;
                }
                
                Destroy(currentStage);
            }

            currentStage = gameObject;
            DontDestroyOnLoad(gameObject);
        }
        
        protected virtual void OnDestroy()
        {
            base.OnDestroy();
            if (currentStage == gameObject)
                currentStage = null;
        }
    }
}