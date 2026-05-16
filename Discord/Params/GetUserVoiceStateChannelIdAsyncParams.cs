namespace Discord.Params;

/// <summary>
/// Params for <see cref="Interfaces.IDiscord.GetUserVoiceStateChannelIdAsync"/>.
/// </summary>
public sealed record GetUserVoiceStateChannelIdAsyncParams
{
    /// <summary>
    /// Get the ID of the guild.
    /// </summary>
    /// <returns>The ID of the guild.</returns>
    public required string GuildId { get; init; }

    /// <summary>
    /// Get the ID of the user.
    /// </summary>
    /// <returns>The ID of the user.</returns>
    public required string UserId { get; init; }
}