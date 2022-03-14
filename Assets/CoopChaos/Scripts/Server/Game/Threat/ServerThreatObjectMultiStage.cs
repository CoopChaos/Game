using UnityEngine;

namespace Yame.Threat
{
    public class ServerThreatObjectMultiStage : ServerThreatObject {
        private GameObject[] stage1Threats;
        private GameObject[] stage2Threats;


        private bool threatCompleted = false;

        public override void Update()
        {
            base.Update();

            bool stage1Completed = true;

            foreach (var threat in stage1Threats)
            {
                stage1Completed = threat.GetComponent<ServerDeviceInteractableBase>().DeviceInteractableState.Fulfilled.Value;
                if (!stage1Completed) break;
            }

            if(stage1Completed) {
                foreach (var threat in stage1Threats)
                {
                    threat.SetActive(false);
                }

                foreach (var threat in stage2Threats)
                {
                    threat.SetActive(true);
                }
            }
        }

        public override void Start()
        {
            base.Start();

            foreach (var threat in stage1Threats)
            {
                threat.SetActive(true);
            }

            foreach (var threat in stage2Threats)
            {
                threat.SetActive(false);
            }
        }
    }
}