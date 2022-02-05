using System;
using UnityEngine.Serialization;

namespace CoopChaos
{
    [Serializable]
    public class OccuranceUseCase
    {
        public float ProportionalChance;
        public OccuranceDescription OccuranceDescription;
    }
}