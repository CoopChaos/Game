using UnityEngine;

namespace CoopChaos
{
    [CreateAssetMenu(fileName = "OccuranceDescription", menuName = "CoopChaos/OccuranceDescription", order = 1)]
    public class OccuranceDescription : ScriptableObject
    {
        public OccuranceType OccuranceType;
    }
}