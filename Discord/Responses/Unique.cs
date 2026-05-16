namespace Discord.Responses;

/// <summary>
/// Represents a Discord object with an ID.
/// </summary>
public abstract record Unique
{
    /// <summary>
    /// Get the ID.
    /// </summary>
    /// <returns>The ID.</returns>
    [System.Text.Json.Serialization.JsonPropertyName("id")]
    public required string Id { get; init; }
}