namespace Server;

internal sealed partial class Account
{
    public sealed class Token
    {
        public string Value { get; }

        internal Token(string value)
        {
            Value = value;
        }

        internal Token(string login, string password)
        {
            Value = $"{login}||{password}";
        }

        public void Deconstruct(out string login, out string password)
        {
            var splitValue = Value.Split("||");

            login = splitValue[0];
            password = splitValue[1];
        }

        public override bool Equals(object? obj)
        {
            return obj is Token token && token.Value == Value;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }
    }
}