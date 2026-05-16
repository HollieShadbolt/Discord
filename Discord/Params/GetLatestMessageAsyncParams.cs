namespace Discord.Params;

/// <summary>
/// Params for <see cref="Interfaces.IDiscord.GetLatestMessageAsync"/>.
/// </summary>
public sealed record GetLatestMessageAsyncParams
{
    /// <summary>
    /// Get the ID of the guild.
    /// </summary>
    /// <returns>The ID of the guild.</returns>
    public required string GuildId { get; init; }

    /// <summary>
    /// Get the ID of the channel.
    /// </summary>
    /// <returns>The ID of the channel.</returns>
    public required string ChannelId { get; init; }

    /// <summary>
    /// Get the ID of the author.
    /// </summary>
    /// <returns>The ID of the author.</returns>
    public required string AuthorId { get; init; }
}