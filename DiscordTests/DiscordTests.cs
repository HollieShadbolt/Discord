using System.Net.Http.Json;
using Moq;

namespace DiscordTests;

[TestFixture]
public static class DiscordTests
{
    #region GetCurrentUserAsync

    [Test]
    public static async Task GetCurrentUser_TestAsync()
    {
        // Arrange
        var mockHttpRequestMessageFactoryHandler =
            new Mock<HttpRequestMessageHandler.Interfaces.IHttpRequestMessageFactoryHandler>();

        var id = Guid.NewGuid().ToString();

        var expectedUser = new Discord.Responses.User
        {
            Id = id
        };

        var cancellationTokenSource = new CancellationTokenSource();

        mockHttpRequestMessageFactoryHandler
            .Setup(httpRequestMessageFactoryHandler =>
                httpRequestMessageFactoryHandler.SendAsync<Discord.Responses.User>(It.IsAny<Func<HttpRequestMessage>>(),
                    cancellationTokenSource.Token))
            .ReturnsAsync(expectedUser);

        var parameter = Guid.NewGuid().ToString();

        var discord = new Discord.Discord(mockHttpRequestMessageFactoryHandler.Object, parameter);

        // Act
        var actualUser = await discord.GetCurrentUserAsync(cancellationTokenSource.Token);

        // Assert
        Assert.That(actualUser, Is.EqualTo(expectedUser));

        mockHttpRequestMessageFactoryHandler.Verify(
            httpRequestMessageFactoryHandler => httpRequestMessageFactoryHandler.SendAsync<Discord.Responses.User>(
                It.Is<Func<HttpRequestMessage>>(httpRequestMessage =>
                    VerifyGetCurrentUserAsyncHttpRequestMessageFactory(httpRequestMessage, parameter)),
                cancellationTokenSource.Token),
            Times.Exactly(1));

        mockHttpRequestMessageFactoryHandler.VerifyNoOtherCalls();
    }

    private static bool VerifyGetCurrentUserAsyncHttpRequestMessageFactory(
        Func<HttpRequestMessage> httpRequestMessageFactory,
        string parameter)
    {
        var httpRequestMessage = httpRequestMessageFactory();

        return httpRequestMessage.Method == HttpMethod.Get &&
               httpRequestMessage.Headers.Authorization?.Scheme == "Bot" &&
               httpRequestMessage.Headers.Authorization.Parameter == parameter &&
               httpRequestMessage.RequestUri?.ToString() == "https://discordapp.com/api/users/@me";
    }

    #endregion

    #region GetLatestMessageAsync

    [Test]
    public static async Task GetLatestMessage_TestAsync()
    {
        // Arrange
        var mockHttpRequestMessageFactoryHandler =
            new Mock<HttpRequestMessageHandler.Interfaces.IHttpRequestMessageFactoryHandler>();

        var id = Guid.NewGuid().ToString();

        var expectedMessage = new Discord.Responses.Message
        {
            Id = id
        };

        var guildMessages = new Discord.Responses.SearchGuildMessages
        {
            Messages =
            [
                [
                    expectedMessage
                ]
            ]
        };

        var cancellationTokenSource = new CancellationTokenSource();

        mockHttpRequestMessageFactoryHandler
            .Setup(httpRequestMessageFactoryHandler =>
                httpRequestMessageFactoryHandler.SendAsync<Discord.Responses.SearchGuildMessages>(
                    It.IsAny<Func<HttpRequestMessage>>(),
                    cancellationTokenSource.Token))
            .ReturnsAsync(guildMessages);

        var parameter = Guid.NewGuid().ToString();

        var discord = new Discord.Discord(mockHttpRequestMessageFactoryHandler.Object, parameter);

        var guildId = Guid.NewGuid().ToString();

        var channelId = Guid.NewGuid().ToString();

        var authorId = Guid.NewGuid().ToString();

        var getLatestMessageAsyncParams = new Discord.Params.GetLatestMessageAsyncParams
        {
            GuildId = guildId,
            ChannelId = channelId,
            AuthorId = authorId
        };

        // Act
        var actualMessage =
            await discord.GetLatestMessageAsync(getLatestMessageAsyncParams, cancellationTokenSource.Token);

        // Assert
        Assert.That(actualMessage, Is.EqualTo(expectedMessage));

        mockHttpRequestMessageFactoryHandler
            .Verify(
                httpRequestMessageFactoryHandler =>
                    httpRequestMessageFactoryHandler.SendAsync<Discord.Responses.SearchGuildMessages>(
                        It.Is<Func<HttpRequestMessage>>(httpRequestMessageFactory =>
                            VerifyGetLatestMessageAsyncHttpRequestMessageFactory(httpRequestMessageFactory,
                                getLatestMessageAsyncParams, parameter)),
                        cancellationTokenSource.Token),
                Times.Exactly(1));

        mockHttpRequestMessageFactoryHandler.VerifyNoOtherCalls();
    }

    private static bool VerifyGetLatestMessageAsyncHttpRequestMessageFactory(
        Func<HttpRequestMessage> httpRequestMessageFactory,
        Discord.Params.GetLatestMessageAsyncParams getLatestMessageAsyncParams,
        string token)
    {
        var httpRequestMessage = httpRequestMessageFactory();

        return httpRequestMessage.Method == HttpMethod.Get &&
               httpRequestMessage.Headers.Authorization?.Scheme == "Bot" &&
               httpRequestMessage.Headers.Authorization.Parameter == token &&
               httpRequestMessage.RequestUri?.ToString() == "https://discordapp.com/api/guilds/" +
               $"{getLatestMessageAsyncParams.GuildId}/messages/search?limit=1&channel_id=" +
               $"{getLatestMessageAsyncParams.ChannelId}&author_id={getLatestMessageAsyncParams.AuthorId}" +
               "&sort_by=timestamp";
    }

    #endregion

    #region GetChannelMessagesAfterAsync

    [Test]
    public static async Task GetChannelMessagesAfterAsync_TestAsync()
    {
        // Arrange
        var mockHttpRequestMessageFactoryHandler =
            new Mock<HttpRequestMessageHandler.Interfaces.IHttpRequestMessageFactoryHandler>();

        var id = Guid.NewGuid().ToString();

        var message = new Discord.Responses.Message
        {
            Id = id
        };

        Discord.Responses.Message[] expectedMessages =
        [
            message
        ];

        var cancellationTokenSource = new CancellationTokenSource();

        mockHttpRequestMessageFactoryHandler
            .Setup(httpRequestMessageFactoryHandler =>
                httpRequestMessageFactoryHandler.SendAsync<IEnumerable<Discord.Responses.Message>>(
                    It.IsAny<Func<HttpRequestMessage>>(),
                    cancellationTokenSource.Token))
            .ReturnsAsync(expectedMessages);

        var parameter = Guid.NewGuid().ToString();

        var discordHttpContentFactory = new Discord.Discord(mockHttpRequestMessageFactoryHandler.Object, parameter);

        var channelId = Guid.NewGuid().ToString();

        var messageId = Guid.NewGuid().ToString();

        var getChannelMessagesAfterAsyncParams = new Discord.Params.GetChannelMessagesAfterAsyncParams
        {
            ChannelId = channelId,
            MessageId = messageId
        };

        // Act
        var actualMessages = await discordHttpContentFactory.GetChannelMessagesAfterAsync(
            getChannelMessagesAfterAsyncParams,
            cancellationTokenSource.Token);

        // Assert
        Assert.That(actualMessages, Is.EqualTo(expectedMessages));

        mockHttpRequestMessageFactoryHandler
            .Verify(
                httpRequestMessageFactoryHandler =>
                    httpRequestMessageFactoryHandler.SendAsync<IEnumerable<Discord.Responses.Message>>(
                        It.Is<Func<HttpRequestMessage>>(httpRequestMessageFactory =>
                            VerifyGetChannelMessagesAfterAsyncHttpRequestMessageFactory(httpRequestMessageFactory,
                                getChannelMessagesAfterAsyncParams, parameter)),
                        cancellationTokenSource.Token),
                Times.Exactly(1));

        mockHttpRequestMessageFactoryHandler.VerifyNoOtherCalls();
    }

    private static bool VerifyGetChannelMessagesAfterAsyncHttpRequestMessageFactory(
        Func<HttpRequestMessage> httpRequestMessageFactory,
        Discord.Params.GetChannelMessagesAfterAsyncParams getChannelMessagesAfterAsyncParams,
        string parameter)
    {
        var httpRequestMessage = httpRequestMessageFactory();

        return httpRequestMessage.Method == HttpMethod.Get &&
               httpRequestMessage.Headers.Authorization?.Scheme == "Bot" &&
               httpRequestMessage.Headers.Authorization.Parameter == parameter &&
               httpRequestMessage.RequestUri?.ToString() == "https://discordapp.com/api/channels/" +
               $"{getChannelMessagesAfterAsyncParams.ChannelId}/messages?after=" +
               getChannelMessagesAfterAsyncParams.MessageId;
    }

    #endregion

    #region ModifyChannelNameAsync

    [Test]
    public static async Task ModifyChannelNameAsync_TestAsync()
    {
        // Arrange
        var mockHttpRequestMessageFactoryHandler =
            new Mock<HttpRequestMessageHandler.Interfaces.IHttpRequestMessageFactoryHandler>();

        var expectedChannel = new Discord.Responses.Channel();

        var cancellationTokenSource = new CancellationTokenSource();

        mockHttpRequestMessageFactoryHandler
            .Setup(httpRequestMessageFactoryHandler =>
                httpRequestMessageFactoryHandler.SendAsync<Discord.Responses.Channel>(
                    It.IsAny<Func<HttpRequestMessage>>(),
                    cancellationTokenSource.Token))
            .ReturnsAsync(expectedChannel);

        var parameter = Guid.NewGuid().ToString();

        var discord = new Discord.Discord(mockHttpRequestMessageFactoryHandler.Object, parameter);

        var channelId = Guid.NewGuid().ToString();

        var name = Guid.NewGuid().ToString();

        var modifyChannelNameAsyncParams = new Discord.Params.ModifyChannelNameAsyncParams
        {
            ChannelId = channelId,
            Name = name
        };

        // Act
        var actualChannel =
            await discord.ModifyChannelNameAsync(modifyChannelNameAsyncParams, cancellationTokenSource.Token);

        // Assert
        Assert.That(actualChannel, Is.EqualTo(expectedChannel));

        mockHttpRequestMessageFactoryHandler
            .Verify(
                httpRequestMessageFactoryHandler =>
                    httpRequestMessageFactoryHandler.SendAsync<Discord.Responses.Channel>(
                        It.Is<Func<HttpRequestMessage>>(httpRequestMessageFactory =>
                            VerifyModifyChannelNameAsyncHttpRequestMessageFactory(httpRequestMessageFactory,
                                modifyChannelNameAsyncParams,
                                parameter)),
                        cancellationTokenSource.Token),
                Times.Exactly(1));

        mockHttpRequestMessageFactoryHandler.VerifyNoOtherCalls();
    }

    private static bool VerifyModifyChannelNameAsyncHttpRequestMessageFactory(
        Func<HttpRequestMessage> httpRequestMessageFactory,
        Discord.Params.ModifyChannelNameAsyncParams modifyChannelNameAsyncParams,
        string token)
    {
        var httpRequestMessage = httpRequestMessageFactory();

        var task = httpRequestMessage.Content?.ReadFromJsonAsync<Discord.Requests.ModifyChannelName>();

        return httpRequestMessage.Method == HttpMethod.Patch &&
               httpRequestMessage.Headers.Authorization?.Scheme == "Bot" &&
               httpRequestMessage.Headers.Authorization.Parameter == token &&
               httpRequestMessage.RequestUri?.ToString() == "https://discordapp.com/api/channels/" +
               modifyChannelNameAsyncParams.ChannelId &&
               task?.Result?.Name == modifyChannelNameAsyncParams.Name;
    }

    #endregion

    #region GetUserVoiceStateChannelIdAsync

    [Test]
    public static async Task GetUserVoiceStateChannelIdAsync_TestAsync()
    {
        // Arrange
        var mockHttpRequestMessageFactoryHandler =
            new Mock<HttpRequestMessageHandler.Interfaces.IHttpRequestMessageFactoryHandler>();

        var cancellationTokenSource = new CancellationTokenSource();

        var expectedChannelId = Guid.NewGuid().ToString();

        mockHttpRequestMessageFactoryHandler
            .Setup(httpRequestMessageFactoryHandler => httpRequestMessageFactoryHandler.SendAsync(
                It.IsAny<Func<HttpRequestMessage>>(),
                It.IsAny<Func<HttpResponseMessage, CancellationToken, Task<string?>>>(), cancellationTokenSource.Token))
            .ReturnsAsync(expectedChannelId);

        var parameter = Guid.NewGuid().ToString();

        var discord = new Discord.Discord(mockHttpRequestMessageFactoryHandler.Object, parameter);

        var guildId = Guid.NewGuid().ToString();

        var userId = Guid.NewGuid().ToString();

        var getUserVoiceStateChannelIdAsyncParams = new Discord.Params.GetUserVoiceStateChannelIdAsyncParams
        {
            GuildId = guildId,
            UserId = userId
        };

        // Act
        var actualChannelId = await discord.GetUserVoiceStateChannelIdAsync(
            getUserVoiceStateChannelIdAsyncParams,
            cancellationTokenSource.Token);

        // Assert
        Assert.That(actualChannelId, Is.EqualTo(expectedChannelId));

        mockHttpRequestMessageFactoryHandler
            .Verify(
                httpRequestMessageFactoryHandler => httpRequestMessageFactoryHandler.SendAsync(
                    It.Is<Func<HttpRequestMessage>>(httpRequestMessageFactory =>
                        VerifyGetUserVoiceStateChannelIdAsyncHttpRequestMessageFactory(httpRequestMessageFactory,
                            getUserVoiceStateChannelIdAsyncParams,
                            parameter)),
                    It.IsAny<Func<HttpResponseMessage, CancellationToken, Task<string?>>>(),
                    cancellationTokenSource.Token),
                Times.Exactly(1));

        mockHttpRequestMessageFactoryHandler.VerifyNoOtherCalls();
    }

    private static bool VerifyGetUserVoiceStateChannelIdAsyncHttpRequestMessageFactory(
        Func<HttpRequestMessage> httpRequestMessageFactory,
        Discord.Params.GetUserVoiceStateChannelIdAsyncParams getUserVoiceStateChannelIdAsyncParams,
        string parameter)
    {
        var httpRequestMessage = httpRequestMessageFactory();

        return httpRequestMessage.Method == HttpMethod.Get &&
               httpRequestMessage.Headers.Authorization?.Scheme == "Bot" &&
               httpRequestMessage.Headers.Authorization.Parameter == parameter &&
               httpRequestMessage.RequestUri?.ToString() == "https://discordapp.com/api/guilds/" +
               $"{getUserVoiceStateChannelIdAsyncParams.GuildId}/voice-states/" +
               getUserVoiceStateChannelIdAsyncParams.UserId;
    }

    #endregion

    #region CreateMessageAsync

    [Test]
    public static async Task CreateMessageAsync_TestAsync()
    {
        // Arrange
        var mockHttpRequestMessageFactoryHandler =
            new Mock<HttpRequestMessageHandler.Interfaces.IHttpRequestMessageFactoryHandler>();

        var cancellationTokenSource = new CancellationTokenSource();

        var id = Guid.NewGuid().ToString();

        var expectedMessage = new Discord.Responses.Message
        {
            Id = id
        };

        mockHttpRequestMessageFactoryHandler
            .Setup(httpRequestMessageFactoryHandler =>
                httpRequestMessageFactoryHandler.SendAsync<Discord.Responses.Message>(
                    It.IsAny<Func<HttpRequestMessage>>(),
                    cancellationTokenSource.Token))
            .ReturnsAsync(expectedMessage);

        var parameter = Guid.NewGuid().ToString();

        var discord = new Discord.Discord(mockHttpRequestMessageFactoryHandler.Object, parameter);

        var channelId = Guid.NewGuid().ToString();

        var content = Guid.NewGuid().ToString();

        var createMessageAsyncParams = new Discord.Params.CreateMessageAsyncParams
        {
            ChannelId = channelId,
            Content = content
        };

        // Act
        var actualMessage = await discord.CreateMessageAsync(createMessageAsyncParams, cancellationTokenSource.Token);

        // Assert
        Assert.That(actualMessage, Is.EqualTo(expectedMessage));

        mockHttpRequestMessageFactoryHandler
            .Verify(
                httpRequestMessageFactoryHandler =>
                    httpRequestMessageFactoryHandler.SendAsync<Discord.Responses.Message>(
                        It.Is<Func<HttpRequestMessage>>(httpRequestMessageFactory =>
                            VerifyCreateMessageAsyncHttpRequestMessageFactory(httpRequestMessageFactory,
                                createMessageAsyncParams, parameter)), cancellationTokenSource.Token),
                Times.Exactly(1));

        mockHttpRequestMessageFactoryHandler.VerifyNoOtherCalls();
    }

    private static bool VerifyCreateMessageAsyncHttpRequestMessageFactory(
        Func<HttpRequestMessage> httpRequestMessageFactory,
        Discord.Params.CreateMessageAsyncParams createMessageAsyncParams,
        string parameter)
    {
        var httpRequestMessage = httpRequestMessageFactory();

        var task = httpRequestMessage.Content?.ReadFromJsonAsync<Discord.Requests.CreateMessage>();

        return httpRequestMessage.Method == HttpMethod.Post &&
               httpRequestMessage.Headers.Authorization?.Scheme == "Bot" &&
               httpRequestMessage.Headers.Authorization.Parameter == parameter &&
               httpRequestMessage.RequestUri?.ToString() == $"https://discordapp.com/api/channels/" +
               $"{createMessageAsyncParams.ChannelId}/messages" &&
               task?.Result?.Content == createMessageAsyncParams.Content;
    }

    #endregion

    #region DeleteMessageAsync

    [Test]
    public static async Task DeleteMessageAsync_TestAsync()
    {
        // Arrange
        var mockHttpRequestMessageFactoryHandler =
            new Mock<HttpRequestMessageHandler.Interfaces.IHttpRequestMessageFactoryHandler>();

        var cancellationTokenSource = new CancellationTokenSource();

        var parameter = Guid.NewGuid().ToString();

        var discord = new Discord.Discord(mockHttpRequestMessageFactoryHandler.Object, parameter);

        var channelId = Guid.NewGuid().ToString();

        var messageId = Guid.NewGuid().ToString();

        var deleteMessageAsyncParams = new Discord.Params.DeleteMessageAsyncParams
        {
            ChannelId = channelId,
            MessageId = messageId
        };

        // Act
        await discord.DeleteMessageAsync(deleteMessageAsyncParams, cancellationTokenSource.Token);

        // Assert
        mockHttpRequestMessageFactoryHandler
            .Verify(
                httpRequestMessageFactoryHandler => httpRequestMessageFactoryHandler.SendAsync(
                    It.Is<Func<HttpRequestMessage>>(httpRequestMessageFactory =>
                        VerifyDeleteMessageAsyncHttpRequestMessageFactory(httpRequestMessageFactory,
                            deleteMessageAsyncParams,
                            parameter)),
                    Discord.DiscordHttpResponseMessageHandlers.HandleDeleteMessageAsyncAsync,
                    cancellationTokenSource.Token),
                Times.Exactly(1));

        mockHttpRequestMessageFactoryHandler.VerifyNoOtherCalls();
    }

    private static bool VerifyDeleteMessageAsyncHttpRequestMessageFactory(
        Func<HttpRequestMessage> httpRequestMessageFactory,
        Discord.Params.DeleteMessageAsyncParams deleteMessageAsyncParams,
        string parameter)
    {
        var httpRequestMessage = httpRequestMessageFactory();

        return httpRequestMessage.Method == HttpMethod.Delete &&
               httpRequestMessage.Headers.Authorization?.Scheme == "Bot" &&
               httpRequestMessage.Headers.Authorization.Parameter == parameter &&
               httpRequestMessage.RequestUri?.ToString() == $"https://discordapp.com/api/channels/" +
               $"{deleteMessageAsyncParams.ChannelId}/messages/{deleteMessageAsyncParams.MessageId}";
    }

    #endregion
}