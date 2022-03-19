using System.Collections.Generic;
using UnityEngine;

namespace CoopChaos
{
    public class EnemyPatrolComponent
    {
        public List<Vector2> PatrolPoints { get; set; }

        public float PatrolSpeed { get; set; }
        public int PatrolPointIndex { get; set; }
        public Vector2 PatrolPosition { get; set; }
    }
}