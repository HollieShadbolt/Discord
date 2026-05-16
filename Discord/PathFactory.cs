namespace Discord;

/// <summary>
/// A path factory.
/// </summary>
public static class PathFactory
{
    /// <summary>
    /// Get the guilds paths.
    /// </summary>
    /// <param name="guildId">The ID of the guild.</param>
    /// <returns>The guilds paths.</returns>
    public static string[] ToGuildsPaths(string guildId) =>
    [
        "guilds",
        guildId
    ];

    /// <summary>
    /// Get the messages paths.
    /// </summary>
    /// <param name="channelId">The ID of the channel.</param>
    /// <returns>The messages paths.</returns>
    public static string[] ToMessagesPaths(string channelId) =>
    [
        ..ToChannelsPaths(channelId),
        "messages"
    ];

    /// <summary>
    /// Get the channels paths.
    /// </summary>
    /// <param name="channelId">The ID of the channel.</param>
    /// <returns>The channels paths.</returns>
    public static string[] ToChannelsPaths(string channelId) =>
    [
        "channels",
        channelId
    ];
}