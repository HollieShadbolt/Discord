namespace Discord.Responses;

/// <summary>
/// Represents a Voice State in Discord.
/// </summary>
public sealed record VoiceState
{
    /// <summary>
    /// Get the channel ID the user is connected to.
    /// </summary>
    /// <returns>The channel ID the user is connected to.</returns>
    [System.Text.Json.Serialization.JsonPropertyName("channel_id")]
    public string? ChannelId { get; init; }
}