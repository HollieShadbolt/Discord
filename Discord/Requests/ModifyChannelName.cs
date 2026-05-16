namespace Discord.Requests;

/// <summary>
/// Represents a Modify Channel Name request.
/// </summary>
public sealed record ModifyChannelName
{
    /// <summary>
    /// The name of the channel.
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("name")]
    public required string Name { get; init; }
}