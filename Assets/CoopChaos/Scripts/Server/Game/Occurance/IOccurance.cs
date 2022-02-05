using System.Collections;
using System.Collections.Generic;
using CoopChaos.Simulation;

namespace CoopChaos
{
    public interface IOccurance
    {
        void Start(SimulationBehaviour simulation);
        void Update();
        void Remove();
    }
}