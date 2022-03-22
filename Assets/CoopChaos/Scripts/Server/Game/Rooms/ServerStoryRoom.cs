using System;
using System.Collections;
using Unity.Netcode;
using UnityEngine;

namespace CoopChaos
{
    public class ServerStoryRoom : ServerInteractableObjectBase
    {
        private ServerFlightSequenceManager flightSequenceManager;
        private ServerStory story;
        private StoryRoomState state;

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            
            if (!IsServer)
            {
                enabled = false;
                return;
            }
        }

        public void TriggerNextFlightSequence()
        {
            Debug.Log("Triggering next flightsequence");
            state.CanTriggerNext.Value = false;
            story.LoadNextFlightSequence();
        }

        private void Awake()
        {
            flightSequenceManager = FindObjectOfType<ServerFlightSequenceManager>();

            story = FindObjectOfType<ServerStory>();
            state = FindObjectOfType<StoryRoomState>();

            flightSequenceManager.NewOccuranceEvent += ocurrance =>
            {
                state.Title.Value = ocurrance.Title;
                state.Description.Value = ocurrance.Description;
            };

            flightSequenceManager.FlightSequenceFinishedEvent += () =>
            {
                state.CanTriggerNext.Value = true;
            };
        }
    }
}