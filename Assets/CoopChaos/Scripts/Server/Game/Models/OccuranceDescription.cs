using UnityEngine;

namespace CoopChaos
{
    [CreateAssetMenu(fileName = "OccuranceDescription", menuName = "CoopChaos/OccuranceDescription", order = 1)]
    public class OccuranceDescription : ScriptableObject
    {
        public OccuranceType OccuranceType;

        public string Title;
        public string Description;
    }
}