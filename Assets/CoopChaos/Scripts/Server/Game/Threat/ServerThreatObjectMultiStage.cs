using UnityEngine;

namespace Yame.Threat
{
    public class ServerThreatObjectMultiStage : ServerThreatObject {
        [SerializeField] private GameObject[] stage1Threats;
        [SerializeField] private GameObject[] stage2Threats;


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
                SetStageActiveState(stage1Threats, false);
                SetStageActiveState(stage2Threats, true);
            }
        }

        private void SetThreatActiveState(GameObject threat, bool active) {
            threat.GetComponentInChildren<SpriteRenderer>().enabled = active;
            threat.GetComponentInChildren<BoxCollider2D>().enabled = active;
        }

        private void SetStageActiveState(GameObject[] threats, bool active) {
            foreach (var threat in threats) SetThreatActiveState(threat, active);
        }

        public override void Start()
        {
            base.Start();

            SetStageActiveState(stage1Threats, true);
            SetStageActiveState(stage2Threats, false);
        }
    }
}