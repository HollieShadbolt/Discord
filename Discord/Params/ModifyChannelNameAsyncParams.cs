namespace Discord.Params;

/// <summary>
/// Params for <see cref="Interfaces.IDiscord.ModifyChannelNameAsync"/>.
/// </summary>
public sealed record ModifyChannelNameAsyncParams
{
    /// <summary>
    /// Get the ID of the channel.
    /// </summary>
    /// <returns>The ID of the channel.</returns>
    public required string ChannelId { get; init; }

    /// <summary>
    /// Get the new channel name.
    /// </summary>
    /// <returns>The new channel name.</returns>
    public required string Name { get; init; }
}