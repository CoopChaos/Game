using System;

namespace CoopChaos
{
    [Serializable]
    public class FlightSequenceDescription
    {
        public int Depth;
        public OccuranceUseCase[] UseCases;
    }
}