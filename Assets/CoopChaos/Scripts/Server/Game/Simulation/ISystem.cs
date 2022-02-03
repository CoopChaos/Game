using DefaultEcs;

namespace CoopChaos.Simulation
{
    public interface ISystem
    {
        void Update(float deltaTime);
    }
}