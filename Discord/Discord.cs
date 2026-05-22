using Discord.Params;
using Discord.Requests;
using Discord.Responses;
using HttpRequestMessageHandler.Interfaces;
using HttpUtility = System.Web.HttpUtility;
using IDiscord = Discord.Interfaces.IDiscord;
using static Discord.DiscordHttpResponseMessageHandlers;
using static Discord.PathFactory;

namespace Discord;

/// <inheritdoc cref="IDiscord" />
/// <param name="httpRequestMessageFactoryHandler">The <see cref="IHttpRequestMessageFactoryHandler"/>.</param>
/// <param name="parameter">The credentials containing the authentication information of the user agent.</param>
public sealed class Discord(IHttpRequestMessageFactoryHandler httpRequestMessageFactoryHandler, string parameter)
    : HttpRequestMessageHandler.HttpRequestMessageFactories(Scheme, parameter, Uri), IDiscord
{
    private const string Scheme = "Bot";
    private const string Uri = "https://discordapp.com/api";

    /// <inheritdoc/>
    /// <exception cref="TaskCanceledException">The cancellation token was cancelled.</exception>
    public async Task<User> GetCurrentUserAsync(CancellationToken cancellationToken) =>
        await httpRequestMessageFactoryHandler.SendAsync<User>(
            GetGetCurrentUserHttpRequestMessageFactory,
            cancellationToken);

    /// <inheritdoc/>
    /// <exception cref="TaskCanceledException">The cancellation token was cancelled.</exception>
    public async Task<Message?> GetLatestMessageAsync(
        GetLatestMessageAsyncParams getLatestMessageAsyncParams,
        CancellationToken cancellationToken)
    {
        var httpRequestMessageFactory = GetGetLatestMessageHttpRequestMessageFactory(getLatestMessageAsyncParams);

        var guildMessages = await httpRequestMessageFactoryHandler.SendAsync<SearchGuildMessages>(
            httpRequestMessageFactory,
            cancellationToken);

        return guildMessages.Messages.SingleOrDefault()?.SingleOrDefault();
    }

    /// <inheritdoc/>
    /// <exception cref="TaskCanceledException">The cancellation token was cancelled.</exception>
    public async Task<IEnumerable<Message>> GetChannelMessagesAfterAsync(
        GetChannelMessagesAfterAsyncParams getChannelMessagesAfterAsyncParams,
        CancellationToken cancellationToken)
    {
        var httpRequestMessageFactory =
            GetGetChannelMessagesAfterHttpRequestMessageFactory(getChannelMessagesAfterAsyncParams);

        return await httpRequestMessageFactoryHandler.SendAsync<IEnumerable<Message>>(
            httpRequestMessageFactory,
            cancellationToken);
    }

    /// <inheritdoc/>
    /// <exception cref="TaskCanceledException">The cancellation token was cancelled.</exception>
    public async Task<Channel> ModifyChannelNameAsync(
        ModifyChannelNameAsyncParams modifyChannelNameAsyncParams,
        CancellationToken cancellationToken)
    {
        var httpRequestMessageFactory = GetModifyChannelNameHttpRequestMessageFactory(modifyChannelNameAsyncParams);

        return await httpRequestMessageFactoryHandler.SendAsync<Channel>(httpRequestMessageFactory, cancellationToken);
    }

    /// <inheritdoc/>
    /// <exception cref="TaskCanceledException">The cancellation token was cancelled.</exception>
    public async Task<string?> GetUserVoiceStateChannelIdAsync(
        GetUserVoiceStateChannelIdAsyncParams getUserVoiceStateChannelIdAsyncParams,
        CancellationToken cancellationToken)
    {
        var httpRequestMessageFactory =
            GetGetUserVoiceStateChannelIdHttpRequestMessageFactory(getUserVoiceStateChannelIdAsyncParams);

        return await httpRequestMessageFactoryHandler.SendAsync(
            httpRequestMessageFactory,
            HandleGetUserVoiceStateChannelIdAsyncAsync,
            cancellationToken);
    }

    /// <inheritdoc/>
    /// <exception cref="TaskCanceledException">The cancellation token was cancelled.</exception>
    public async Task<Message> CreateMessageAsync(
        CreateMessageAsyncParams createMessageAsyncParams,
        CancellationToken cancellationToken)
    {
        var httpRequestMessageFactory = GetCreateMessageHttpRequestMessageFactory(createMessageAsyncParams);

        return await httpRequestMessageFactoryHandler.SendAsync<Message>(httpRequestMessageFactory, cancellationToken);
    }

    /// <inheritdoc/>
    /// <exception cref="TaskCanceledException">The cancellation token was cancelled.</exception>
    public async Task DeleteMessageAsync(
        DeleteMessageAsyncParams deleteMessageAsyncParams,
        CancellationToken cancellationToken)
    {
        var httpRequestMessageFactory = GetDeleteMessageHttpRequestMessageFactory(deleteMessageAsyncParams);

        await httpRequestMessageFactoryHandler.SendAsync(
            httpRequestMessageFactory,
            HandleDeleteMessageAsyncAsync,
            cancellationToken);
    }
    
    #region HttpRequestMessageFactories

    private HttpRequestMessage GetGetCurrentUserHttpRequestMessageFactory()
    {
        string[] paths =
        [
            "users",
            "@me"
        ];

        var uriBuilder = UriBuilderFactory.GetUriBuilder(paths);

        return HttpRequestMessageFactory.GetHttpRequestMessage(uriBuilder);
    }

    private Func<HttpRequestMessage> GetGetLatestMessageHttpRequestMessageFactory(
        GetLatestMessageAsyncParams getLatestMessageAsyncParams) => () =>
    {
        string[] paths =
        [
            ..ToGuildsPaths(getLatestMessageAsyncParams.GuildId),
            "messages",
            "search"
        ];

        var uriBuilder = UriBuilderFactory.GetUriBuilder(paths);

        var query = HttpUtility.ParseQueryString(uriBuilder.Query);

        query["limit"] = "1";
        query["channel_id"] = getLatestMessageAsyncParams.ChannelId;
        query["author_id"] = getLatestMessageAsyncParams.AuthorId;
        query["sort_by"] = "timestamp";
        query["include_nsfw"] = "true";

        uriBuilder.Query = query.ToString();

        return HttpRequestMessageFactory.GetHttpRequestMessage(uriBuilder);
    };

    private Func<HttpRequestMessage> GetGetChannelMessagesAfterHttpRequestMessageFactory(
        GetChannelMessagesAfterAsyncParams getChannelMessagesAfterAsyncParams) => () =>
    {
        var paths = ToMessagesPaths(getChannelMessagesAfterAsyncParams.ChannelId);

        var uriBuilder = UriBuilderFactory.GetUriBuilder(paths);

        var query = HttpUtility.ParseQueryString(uriBuilder.Query);

        query["after"] = getChannelMessagesAfterAsyncParams.MessageId;

        uriBuilder.Query = query.ToString();

        return HttpRequestMessageFactory.GetHttpRequestMessage(uriBuilder);
    };

    private Func<HttpRequestMessage> GetModifyChannelNameHttpRequestMessageFactory(
        ModifyChannelNameAsyncParams modifyChannelNameAsyncParams) => () =>
    {
        var paths = ToChannelsPaths(modifyChannelNameAsyncParams.ChannelId);

        var uriBuilder = UriBuilderFactory.GetUriBuilder(paths);

        var inputValue = new ModifyChannelName
        {
            Name = modifyChannelNameAsyncParams.Name
        };

        return HttpRequestMessageFactory.GetHttpRequestMessage(uriBuilder, HttpMethod.Patch, inputValue);
    };

    private Func<HttpRequestMessage> GetGetUserVoiceStateChannelIdHttpRequestMessageFactory(
        GetUserVoiceStateChannelIdAsyncParams getUserVoiceStateChannelIdAsyncParams) => () =>
    {
        string[] paths =
        [
            ..ToGuildsPaths(getUserVoiceStateChannelIdAsyncParams.GuildId),
            "voice-states",
            getUserVoiceStateChannelIdAsyncParams.UserId
        ];

        var uriBuilder = UriBuilderFactory.GetUriBuilder(paths);

        return HttpRequestMessageFactory.GetHttpRequestMessage(uriBuilder);
    };

    private Func<HttpRequestMessage> GetCreateMessageHttpRequestMessageFactory(
        CreateMessageAsyncParams createMessageAsyncParams) => () =>
    {
        var paths = ToMessagesPaths(createMessageAsyncParams.ChannelId);

        var uriBuilder = UriBuilderFactory.GetUriBuilder(paths);

        var inputValue = new CreateMessage
        {
            Content = createMessageAsyncParams.Content
        };

        return HttpRequestMessageFactory.GetHttpRequestMessage(uriBuilder, HttpMethod.Post, inputValue);
    };

    private Func<HttpRequestMessage> GetDeleteMessageHttpRequestMessageFactory(
        DeleteMessageAsyncParams deleteMessageAsyncParams) => () =>
    {
        string[] paths =
        [
            ..ToMessagesPaths(deleteMessageAsyncParams.ChannelId),
            deleteMessageAsyncParams.MessageId
        ];

        var uriBuilder = UriBuilderFactory.GetUriBuilder(paths);

        return HttpRequestMessageFactory.GetHttpRequestMessage(uriBuilder, HttpMethod.Delete);
    };

    #endregion
}
