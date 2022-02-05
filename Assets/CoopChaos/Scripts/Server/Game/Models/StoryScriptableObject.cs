using UnityEngine;

namespace CoopChaos
{
    [CreateAssetMenu(fileName = "New Story", menuName = "CoopChaos/Story")]
    public class StoryScriptableObject : ScriptableObject
    {
        public string Name;
        public string Description;
        
        public FlightSequenceDescription[] FlightSequences;
    }
}