namespace Shared;

public static partial class Sockets
{
    public enum Command
    {
        Start,
        StartOk,
        Unknown
    }

    public sealed record Message
    {
        public static readonly Message Start   = new (Command.Start);
        public static readonly Message StartOk = new (Command.StartOk);
        public static readonly Message Unknown = new (Command.Unknown);

        public Command Command { get; }
        public string Payload { get; }

        public Message(Command command, string payload)
        {
            Command = command;
            Payload = payload;
        }

        public Message(Command command)
        {
            Command = command;
            Payload = string.Empty;
        }
    }
}