using Unity.Netcode;

namespace Yame.Threat
{
    public class ServerThreatObject : NetworkBehaviour
    {
        private ThreatObject threatObject;

        public void Update()
        {
            foreach (var o in threatObject.objectives)
            {
                if (!o.Fulfilled.Value)
                {
                    threatObject.Finished.Value = false;
                    break;
                }

                threatObject.Finished.Value = true;
            }
        }
    }
}