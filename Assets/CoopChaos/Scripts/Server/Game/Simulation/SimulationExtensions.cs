using DefaultEcs;

namespace CoopChaos.Simulation
{
    public static class SimulationExtensions
    {
        public static bool TryGetSingleEntity(this EntitySet entitySet, out Entity entity)
        {
            if (entitySet.Count == 1)
            {
                entity = entitySet.GetEntities()[0];
                return true;
            }

            entity = default;
            return false;
        }
    }
}