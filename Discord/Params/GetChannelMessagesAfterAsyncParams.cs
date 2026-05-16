namespace Discord.Params;

/// <summary>
/// Params for <see cref="Interfaces.IDiscord.GetChannelMessagesAfterAsync"/>.
/// </summary>
public sealed record GetChannelMessagesAfterAsyncParams
{
    /// <summary>
    /// Get the ID of the channel.
    /// </summary>
    /// <returns>The ID of the channel.</returns>
    public required string ChannelId { get; init; }

    /// <summary>
    /// Get the ID of the message.
    /// </summary>
    /// <returns>The ID of the message.</returns>
    public required string MessageId { get; init; }
}