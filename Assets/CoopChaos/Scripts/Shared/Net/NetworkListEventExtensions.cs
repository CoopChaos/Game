using Unity.Netcode;

namespace CoopChaos
{
    public enum AbstractNetworkListEvent
    {
        Undefined,
        
        Change,
        Add,
        Remove,
        Clear
    }
    
    public static class NetworkListEventExtensions
    {
        public static AbstractNetworkListEvent ToAbstract<T>(this NetworkListEvent<T>.EventType type)
            => type switch
            {
                NetworkListEvent<T>.EventType.Add => AbstractNetworkListEvent.Add,
                NetworkListEvent<T>.EventType.Clear => AbstractNetworkListEvent.Clear,
                NetworkListEvent<T>.EventType.Insert => AbstractNetworkListEvent.Add,
                NetworkListEvent<T>.EventType.Remove => AbstractNetworkListEvent.Remove,
                NetworkListEvent<T>.EventType.Value => AbstractNetworkListEvent.Change,
                NetworkListEvent<T>.EventType.RemoveAt => AbstractNetworkListEvent.Remove,
                _ => AbstractNetworkListEvent.Undefined
            };
    }
}