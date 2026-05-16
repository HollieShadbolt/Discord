namespace Discord.Params;

/// <summary>
/// Params for <see cref="Interfaces.IDiscord.CreateMessageAsync"/>.
/// </summary>
public sealed record CreateMessageAsyncParams
{
    /// <summary>
    /// Get the ID of the channel.
    /// </summary>
    /// <returns>The ID of the channel.</returns>
    public required string ChannelId { get; init; }

    /// <summary>
    /// Get the message contents.
    /// </summary>
    /// <returns>The message contents.</returns>
    public required string Content { get; init; }
}