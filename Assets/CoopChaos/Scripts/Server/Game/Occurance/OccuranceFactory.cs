using System;
using System.Collections.Generic;
using System.Linq;

namespace CoopChaos
{
    public static class OccuranceFactory
    {
        private static Dictionary<OccuranceType, Type> occuranceTypes = new Dictionary<OccuranceType, Type>();

        static OccuranceFactory()
        {
            // get all classes with OccuranceAttribute
            occuranceTypes = typeof(IOccurance).Assembly.GetTypes()
                .Where(t => t.GetCustomAttributes(typeof(OccuranceAttribute), false).Length > 0)
                .ToDictionary(t => ((OccuranceAttribute)t.GetCustomAttributes(typeof(OccuranceAttribute), false)[0]).OccuranceType);
        }
        
        public static IOccurance Create(OccuranceDescription occuranceDescription)
        {
            return (IOccurance)Activator.CreateInstance(occuranceTypes[occuranceDescription.OccuranceType], (Object)occuranceDescription);
        }
    }
}