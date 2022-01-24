using UnityEngine;

namespace CoopChaos.Simulation.Components
{
    public class ObjectComponent
    {
        public float Health { get; set; }
        
        public float X { get; set; }
        public float Y { get; set; }
        
        public float VelocityX { get; set; }
        public float VelocityY { get; set; }
        
        public float Mass { get; set; }
        public float Size { get; set; }
        
        public float GetVelocity()
        {
            return Mathf.Sqrt(VelocityX * VelocityX + VelocityY * VelocityY);
        }
    }
}