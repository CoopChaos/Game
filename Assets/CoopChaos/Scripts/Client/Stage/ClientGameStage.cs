namespace CoopChaos
{
    public class ClientGameStage : Stage
    {
        public override StageType Type => StageType.Game;

        public override void OnNetworkSpawn()
        {
            if (!IsClient)
            {
                enabled = false;
                return;
            }
        }
    }
}