namespace
TwitchNet.Clients
{
    public enum
    ClientState
    {
        /// <summary>
        /// The client is disconnected.        
        /// </summary>
        Disconnected = 0,

        /// <summary>
        /// The client is currently disconnecting.
        /// </summary>
        Disconnecting = 1,

        /// <summary>
        /// The client is connected.
        /// </summary>
        Connected = 2,

        /// <summary>
        /// The client is currently connecting.
        /// </summary>
        Connecting = 3,

        /// <summary>
        /// The client is disconnected and all managaed resources have been released.
        /// </summary>
        Disposed = 4,

        /// <summary>
        /// The client is currently being disconnecting and being disposed.
        /// </summary>
        Disposing = 5,
    }
}
