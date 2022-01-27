using DefaultEcs;

namespace CoopChaos.Simulation
{
    public interface ISystem
    {
        // dependency injection over property
        World World { get; set; }
        
        void Update(float deltaTime);
    }
}