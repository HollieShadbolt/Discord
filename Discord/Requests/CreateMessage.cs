namespace Discord.Requests;

/// <summary>
/// Represents a Create Message request.
/// </summary>
public sealed record CreateMessage
{
    /// <summary>
    /// Contents of the message.
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("content")]
    public required string Content { get; init; }
}