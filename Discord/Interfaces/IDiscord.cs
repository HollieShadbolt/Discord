using Discord.Params;
using Discord.Responses;

namespace Discord.Interfaces;

/// <summary>
/// A Discord bot instance.
/// </summary>
public interface IDiscord
{
    /// <summary>
    /// Returns the <see cref="User"/> of the requester’s account.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
    /// <returns>The task object representing the asynchronous operation.</returns>
    Task<User> GetCurrentUserAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Returns the latest <see cref="Message"/> if available.
    /// </summary>
    /// <param name="getLatestMessageAsyncParams">The <see cref="GetLatestMessageAsyncParams"/>.</param>
    /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
    /// <returns>The task object representing the asynchronous operation.</returns>
    Task<Message?> GetLatestMessageAsync(
        GetLatestMessageAsyncParams getLatestMessageAsyncParams,
        CancellationToken cancellationToken);

    /// <summary>
    /// Retrieves the messages in a channel.
    /// </summary>
    /// <param name="getChannelMessagesAfterAsync">The <see cref="GetChannelMessagesAfterAsyncParams"/>.</param>
    /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
    /// <returns>The task object representing the asynchronous operation.</returns>
    Task<IEnumerable<Message>> GetChannelMessagesAfterAsync(
        GetChannelMessagesAfterAsyncParams getChannelMessagesAfterAsync,
        CancellationToken cancellationToken);

    /// <summary>
    /// Update a channel's name. Returns the updated <see cref="Channel"/>.
    /// </summary>
    /// <param name="modifyChannelNameAsyncParams">The <see cref="ModifyChannelNameAsyncParams"/>.</param>
    /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
    /// <returns>The task object representing the asynchronous operation.</returns>
    Task<Channel> ModifyChannelNameAsync(
        ModifyChannelNameAsyncParams modifyChannelNameAsyncParams,
        CancellationToken cancellationToken);

    /// <summary>
    /// Returns the channel ID of the specified user's voice state if available.
    /// </summary>
    /// <param name="getUserVoiceStateChannelIdAsyncParams">The <see cref="GetUserVoiceStateChannelIdAsyncParams"/>.</param>
    /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
    /// <returns>The task object representing the asynchronous operation.</returns>
    Task<string?> GetUserVoiceStateChannelIdAsync(
        GetUserVoiceStateChannelIdAsyncParams getUserVoiceStateChannelIdAsyncParams,
        CancellationToken cancellationToken);

    /// <summary>
    /// Post a message to a guild text or DM channel. Returns the <see cref="Message"/>.
    /// </summary>
    /// <param name="createMessageAsyncParams">The <see cref="CreateMessageAsyncParams"/>.</param>
    /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
    /// <returns>The task object representing the asynchronous operation.</returns>
    Task<Message> CreateMessageAsync(
        CreateMessageAsyncParams createMessageAsyncParams,
        CancellationToken cancellationToken);

    /// <summary>
    /// Delete a message.
    /// </summary>
    /// <param name="deleteMessageAsyncParams">The <see cref="DeleteMessageAsyncParams"/>.</param>
    /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
    /// <returns>The task object representing the asynchronous operation.</returns>
    Task DeleteMessageAsync(DeleteMessageAsyncParams deleteMessageAsyncParams, CancellationToken cancellationToken);
}