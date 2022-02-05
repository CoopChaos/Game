using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace CoopChaos.Server
{
    public class ServerGameContext : MonoBehaviour
    {
        [SerializeField]
        private StoryScriptableObject story;
        
        public static ServerGameContext Singleton { get; private set; }
        
        public StoryScriptableObject Story => story;
        
        private void Awake()
        {
            Assert.IsNull(Singleton);
            Singleton = this;
            DontDestroyOnLoad(gameObject);
        }

        private void OnDestroy()
        {
            Singleton = null;
        }
    }
}