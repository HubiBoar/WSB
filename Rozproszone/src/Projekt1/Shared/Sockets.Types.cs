namespace Shared;

public static partial class Sockets
{
    public sealed record Token
    (
        string Login,
        string Password,
        bool IsAdmin
    );

    public interface ITokenMessage : IMessage
    {
        public Token Token { get; }
    }

    public interface IMessage
    {
        public static abstract string Command { get; }
    }
}