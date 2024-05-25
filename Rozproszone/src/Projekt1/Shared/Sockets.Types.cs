namespace Shared;

public static partial class Sockets
{
    public sealed record Message
    {
        public static readonly Message Start   = new ("Start");
        public static readonly Message StartOk = new ("StartOk");
        public static readonly Message Unknown = new ("Unknown");

        public string Command { get; }
        public string Payload { get; }

        public Message(string command, string payload)
        {
            Command = command;
            Payload = payload;
        }

        public Message(string command)
        {
            Command = command;
            Payload = string.Empty;
        }
    }
}