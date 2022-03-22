using System;
using UnityEngine;

namespace CoopChaos
{
    [Serializable]
    public class EnemySpecification
    {
        public float EnemySize;
        public float SpawnXOffset;
        
        public Vector2[] PatrolPoints;
        public float Speed;
        public float ShootInterval;
    }
    
    // AsteroidOccuranceDescription
    [CreateAssetMenu(fileName = "FightOccuranceDescription", menuName = "CoopChaos/FightOccuranceDescription", order = 1)]
    public class FightOccuranceDescription : OccuranceDescription
    {
        public EnemySpecification[] EnemySpecifications;
    }
}