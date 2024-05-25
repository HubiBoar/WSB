using System.Text.Json;
using Shared;

namespace Server.Public;

public sealed record Account(string Name, string Surname, int Balance)
{
    internal Account(Server.Account account) : this(account.Name, account.Surname, account.Balance) {}
}

public sealed record Token(string Value)
{

}

public sealed record PayloadWithToken(Token Token)
{
    private sealed record Json(string Token);

    internal static Sockets.Message ToMessage(string command, Token token)
    {
        return new Sockets.Message(command, JsonSerializer.Serialize(new Json(token.Value)));
    }

    internal static PayloadWithToken FromMessage(Sockets.Message message)
    {
        var tokenValue = JsonSerializer.Deserialize<Json>(message.Payload)!;
        var serverToken = new Server.Account.Token(tokenValue.Token);

        return new (new Token(serverToken.Value));
    }
}

public sealed record PayloadWithToken<T>(Token Token, T Payload)
{
    private sealed record Json(string Token, string Payload);
    internal static Sockets.Message ToMessage(string command, PayloadWithToken<T> payload)
    {
        return new Sockets.Message(command, JsonSerializer.Serialize(new Json(payload.Token.Value, JsonSerializer.Serialize(payload.Payload))));
    }

    internal static Sockets.Message ToMessage(string command, Token token, T payload)
    {
        return ToMessage(command, new PayloadWithToken<T>(token, payload));
    }


    internal static PayloadWithToken<T> FromMessage(Sockets.Message message)
    {
        var (tokenValue, payload) = JsonSerializer.Deserialize<Json>(message.Payload)!;
        var serverToken = new Server.Account.Token(tokenValue);

        return new (new Token(serverToken.Value), JsonSerializer.Deserialize<T>(payload)!);
    }
}

public static class MessageExtensions
{
    public static PayloadWithToken<T> GetPayload<T>(this Sockets.Message message)
    {
        return PayloadWithToken<T>.FromMessage(message);
    }

    public static Token GetToken(this Sockets.Message message)
    {
        return PayloadWithToken.FromMessage(message).Token;
    }
}