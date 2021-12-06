using System;
using Unity.Netcode;
using UnityEngine;

namespace CoopChaos
{
    public abstract class Stage : NetworkBehaviour
    {
        public abstract StageType Type { get; }

        private static GameObject currentStage;

        public override void OnNetworkSpawn()
        {
            // DontDestroyOnLoad *must* not happen start or awake. if it is done in start, the scenemanager will try
            // to synchronize the stage before it is set to DontDestroyOnLoad in the client. Because DontDestroyOnLoad
            // places the object in a different scene the object will be in different scenes on client and server while
            // the initial synchronize is happening. this will result in failure on client side
            // DontDestroyOnLoad(gameObject);
        }

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
        }

        protected virtual void OnDestroy()
        {
            base.OnDestroy();
            if (currentStage == gameObject)
                currentStage = null;
        }
    }
}