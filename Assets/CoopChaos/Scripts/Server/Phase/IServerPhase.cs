using System;
using CoopChaos.Shared;

namespace CoopChaos
{
    public interface IServerPhase
    {
        PhaseType Type { get; }
        event Action<IServerPhase> OnPhaseChanged;

        ConnectStatus CanPlayerConnect(Guid playerId, string username);
        
        void ConnectPlayer(Guid playerId, PlayerModel playerModel);
        void DisconnectPlayer(Guid playerId);
    }
}