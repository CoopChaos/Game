using UnityEngine;

namespace CoopChaos
{
    // AsteroidOccuranceDescription
    [CreateAssetMenu(fileName = "AsteroidOccuranceDescription", menuName = "CoopChaos/AsteroidOccuranceDescription", order = 1)]
    public class AsteroidOccuranceDescription : OccuranceDescription
    {
        public float Length;
        public float MinAsteroidSize;
        public float MaxAsteroidSize;
        public float DistanceBetweenAsteroids;
    }
}