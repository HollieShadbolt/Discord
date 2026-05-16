namespace DiscordTests;

public static class DiscordHttpResponseMessageHandlersTests
{
    [Test]
    public static async Task HandleGetUserVoiceStateChannelIdAsyncAsync_TestAsync()
    {
        // Arrange
        var expectedChannelId = Guid.NewGuid().ToString();

        var inputValue = new Discord.Responses.VoiceState
        {
            ChannelId = expectedChannelId
        };

        var jsonContent = System.Net.Http.Json.JsonContent.Create(inputValue);

        var httpResponseMessage = new HttpResponseMessage
        {
            StatusCode = System.Net.HttpStatusCode.OK,
            Content = jsonContent
        };

        // Act
        var actualChannelId =
            await Discord.DiscordHttpResponseMessageHandlers.HandleGetUserVoiceStateChannelIdAsyncAsync(
                httpResponseMessage,
                CancellationToken.None);

        // Assert
        Assert.That(actualChannelId, Is.EqualTo(expectedChannelId));
    }

    [Test]
    public static void HandleGetUserVoiceStateChannelIdAsyncAsync_HttpRequestException_Test()
    {
        // Arrange
        var httpResponseMessage = new HttpResponseMessage
        {
            StatusCode = System.Net.HttpStatusCode.InternalServerError
        };

        // Act & Assert
        Assert.ThrowsAsync<HttpRequestException>(() =>
            Discord.DiscordHttpResponseMessageHandlers.HandleGetUserVoiceStateChannelIdAsyncAsync(
                httpResponseMessage,
                CancellationToken.None));
    }

    [Test]
    public static void HandleGetUserVoiceStateChannelIdAsyncAsync_JsonException_Test()
    {
        // Arrange
        var content = Guid.NewGuid().ToString();

        var stringContent = new StringContent(content);

        var httpResponseMessage = new HttpResponseMessage
        {
            StatusCode = System.Net.HttpStatusCode.OK,
            Content = stringContent
        };

        // Act & Assert
        Assert.ThrowsAsync<System.Text.Json.JsonException>(() =>
            Discord.DiscordHttpResponseMessageHandlers.HandleGetUserVoiceStateChannelIdAsyncAsync(
                httpResponseMessage,
                CancellationToken.None));
    }

    [Test]
    public static async Task HandleDeleteMessageAsyncAsync_TestAsync()
    {
        // Arrange
        var expectedHttpResponseMessage = new HttpResponseMessage
        {
            StatusCode = System.Net.HttpStatusCode.OK
        };

        // Act
        var actualChannelId =
            await Discord.DiscordHttpResponseMessageHandlers.HandleDeleteMessageAsyncAsync(
                expectedHttpResponseMessage,
                CancellationToken.None);

        // Assert
        Assert.That(actualChannelId, Is.EqualTo(expectedHttpResponseMessage));
    }

    [Test]
    public static void HandleDeleteMessageAsyncAsync_HttpRequestException_TestAsync()
    {
        // Arrange
        var httpResponseMessage = new HttpResponseMessage
        {
            StatusCode = System.Net.HttpStatusCode.InternalServerError
        };

        // Act & Assert
        Assert.ThrowsAsync<HttpRequestException>(() =>
            Discord.DiscordHttpResponseMessageHandlers.HandleDeleteMessageAsyncAsync(
                httpResponseMessage,
                CancellationToken.None));
    }
}