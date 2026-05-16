namespace Discord.Responses;

/// <summary>
/// Represents a Search Guild Messages response.
/// </summary>
public sealed record SearchGuildMessages
{
    /// <summary>
    /// Get a nested array of messages.
    /// </summary>
    /// <returns>A nested array of messages.</returns>
    [System.Text.Json.Serialization.JsonPropertyName("messages")]
    public required IEnumerable<IEnumerable<Message>> Messages { get; init; }
}