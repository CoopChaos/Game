using System;

namespace CoopChaos
{
    public class OccuranceAttribute : Attribute
    {
        public OccuranceAttribute(OccuranceType occuranceType)
        {
            OccuranceType = occuranceType;
        }
        
        public OccuranceType OccuranceType { get; }
    }
}