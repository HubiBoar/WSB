using System.Text.Json;
using System.Text.Json.Serialization;

public sealed class BoolConverter : JsonConverter<bool>
{
    public static void Register(IServiceCollection services)
    {
        services.ConfigureHttpJsonOptions(opts =>
        {
                opts.SerializerOptions.NumberHandling = JsonNumberHandling.AllowReadingFromString;
                opts.SerializerOptions.Converters.Add(new BoolConverter());
        });
    }

    public override bool Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return reader.TokenType switch
        {
            JsonTokenType.True => true,
            JsonTokenType.False => false,
            JsonTokenType.String => reader.GetString() switch
            {
                "true" => true,
                "false" => false,
                _ => throw new JsonException()
            },
            _ => throw new JsonException()
        };
    }

    public override void Write(Utf8JsonWriter writer, bool value, JsonSerializerOptions options)
    {
        writer.WriteBooleanValue(value);
    }
}