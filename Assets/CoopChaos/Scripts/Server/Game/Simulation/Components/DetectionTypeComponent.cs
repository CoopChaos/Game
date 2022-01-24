namespace CoopChaos.Simulation.Components
{
    public enum DetectionType
    {
        NaturalDeadObject,
        AliveShipObject,
        AliveProjectileObject
    }

    // type of object seen in radar
    public class DetectionTypeComponent
    {
        public DetectionType Type { get; set; }
    }
}