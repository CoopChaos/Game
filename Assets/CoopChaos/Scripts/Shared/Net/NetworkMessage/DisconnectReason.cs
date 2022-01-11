namespace Yame
{
    public enum DisconnectReason
    {
        Undefined,
        Timeout,                  // Timeout waiting for a response from the server
        LoggedInAgain,            // logged in on a separate client, causing this one to be kicked out.
        UserRequestedDisconnect,  // Intentional Disconnect triggered by the user.
        GenericDisconnect,        // server disconnected, but no specific reason given.
    }
}