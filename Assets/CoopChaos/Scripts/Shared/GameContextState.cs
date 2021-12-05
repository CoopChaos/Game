using System;
using Unity.Netcode;
using UnityEngine.Assertions;

namespace CoopChaos.Shared
{
    public class GameContextState : NetworkBehaviour
    {
        private NetworkVariable<GameContext> gameContext = new NetworkVariable<GameContext>();
        
        public static GameContextState Singleton { get; private set; } 
        
        // gamecontext can only be correctly set by the server (guaranteed by networkvariable)
        // changing gamecontext on the client will result in an error
        public GameContext GameContext
        {
            get => gameContext.Value;
            set => gameContext.Value = value;
        }

        private void Awake()
        {
            Assert.IsNull(Singleton);
            Singleton = this;
            DontDestroyOnLoad(gameObject);
        }

        public override void OnDestroy()
        {
            Singleton = null;
        }

        public override void OnNetworkSpawn()
        {
            if (IsServer)
                gameContext.Value = GameContext.Default;
        }
    }
}