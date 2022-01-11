namespace Yame
{
    public enum ConnectResult
    {
        Undefined,
        Success,                  // client successfully connected. This may also be a successful reconnect.
        Timeout,                  // client timed out while trying to connect.
        InvalidPayload,           // payload did not verify as valid
        ServerFull,               // can't join, server is already at capacity.
        UsernameDuplicate,        // username is already in use.
    }
}