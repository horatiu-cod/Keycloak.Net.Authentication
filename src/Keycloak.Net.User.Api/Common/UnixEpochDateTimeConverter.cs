using System.Text.Json;
using System.Text.Json.Serialization;


namespace Keycloak.Net.User.Api.Common;

internal class UnixEpochDateTimeConverter : JsonConverter<DateTime>
{
    // Assuming the timestamp is in seconds. For milliseconds, adjust accordingly.
    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {

        long seconds = reader.GetInt64();
        var dt = DateTimeOffset.FromUnixTimeMilliseconds(seconds);
        return dt.DateTime.Date;
    }

    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
    {
        var dateTimeOfSet = new DateTimeOffset(value).ToUniversalTime();
        var milliseconds = dateTimeOfSet.ToUnixTimeMilliseconds();
        writer.WriteNumberValue(milliseconds);
    }
}
