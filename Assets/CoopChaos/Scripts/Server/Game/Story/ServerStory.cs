using System.Collections;
using System.Collections.Generic;
using CoopChaos.Server;
using DefaultNamespace;
using Unity.Netcode;
using UnityEngine.Assertions;

namespace CoopChaos
{
    public class ServerStory : NetworkBehaviour
    {
        private StoryScriptableObject story;
        private ServerOccuranceManager serverOccuranceManager;

        private IEnumerator<FlightSequenceDescription> flightSequenceEnumerator;
        
        public override void OnNetworkSpawn()
        {
            if (!IsServer)
            {
                enabled = false;
                return;
            }
            
            serverOccuranceManager = FindObjectOfType<ServerOccuranceManager>();
            Assert.IsNotNull(serverOccuranceManager);

            story = ServerGameContext.Singleton.Story;

            flightSequenceEnumerator = (IEnumerator<FlightSequenceDescription>) story.FlightSequences.GetEnumerator();
            flightSequenceEnumerator.MoveNext();
            
            // flightSequenceEnumerator.Current.
        }
        
        
    }
}