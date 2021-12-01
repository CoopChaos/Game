namespace CoopChaos
{
    public enum ConnectStatus
    {
        Undefined,
        InvalidPayload,           // payload did not verify as valid
        Success,                  // client successfully connected. This may also be a successful reconnect.
        ServerFull,               // can't join, server is already at capacity.
        UsernameDuplicate,        // username is already in use.
        LoggedInAgain,            // logged in on a separate client, causing this one to be kicked out.
        UserRequestedDisconnect,  // Intentional Disconnect triggered by the user.
        GenericDisconnect,        // server disconnected, but no specific reason given.
    }
}