using System.Collections;
using System.Collections.Generic;

namespace Yame
{
    public interface IOccurance
    {
        IEnumerable<IOccuranceObject> Objects { get; }
        IOccuranceSpaceship Spaceship { get; }
        
        void Start();
        void Update(float deltaTime);
    }
}