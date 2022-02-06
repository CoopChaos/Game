using System;
using System.Collections;
using System.Collections.Generic;
using CoopChaos.Server;
using DefaultNamespace;
using Unity.Netcode;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

namespace CoopChaos
{
    public class ServerStory : NetworkBehaviour
    {
        private StoryScriptableObject story;

        private ServerFlightSequenceManager serverFlightSequenceManager;
        private StoryState state;

        private IEnumerator flightSequenceEnumerator;

        public void LoadNextFlightSequence()
        {
            if (serverFlightSequenceManager.IsFlightSequenceFinished())
            {
                if (flightSequenceEnumerator.MoveNext())
                {
                    serverFlightSequenceManager.LoadFlightSequence((FlightSequenceDescription)flightSequenceEnumerator.Current);   
                }
                else
                {
                    NetworkManager.SceneManager.LoadScene("Gameover", LoadSceneMode.Single);
                }
            }
        }
        
        public override void OnNetworkSpawn()
        {
            if (!IsServer)
            {
                enabled = false;
                return;
            }
            
            story = ServerGameContext.Singleton.Story;

            flightSequenceEnumerator = story.FlightSequences.GetEnumerator();
            flightSequenceEnumerator.MoveNext();
            
            serverFlightSequenceManager.LoadFlightSequence((FlightSequenceDescription)flightSequenceEnumerator.Current);
        }

        private void Awake()
        {
            serverFlightSequenceManager = FindObjectOfType<ServerFlightSequenceManager>();
            Assert.IsNotNull(serverFlightSequenceManager);

            state = FindObjectOfType<StoryState>();
            Assert.IsNotNull(state);
        }
    }
}