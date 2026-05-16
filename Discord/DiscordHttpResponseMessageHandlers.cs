using static System.Net.HttpStatusCode;
using VoiceState = Discord.Responses.VoiceState;
using static HttpRequestMessageHandler.HttpResponseMessageHandlers;

namespace Discord;

/// <summary>
/// <see cref="HttpResponseMessage"/> handlers used in <see cref="Discord"/>.
/// </summary>
public static class DiscordHttpResponseMessageHandlers
{
    /// <summary>
    /// Ensure <see cref="HttpResponseMessage"/> was successful or status code was <see cref="NotFound"/>. If successful, deserialize the <see cref="HttpContent"/> to <see cref="VoiceState"/> and return the channel ID if available.
    /// </summary>
    /// <param name="httpResponseMessage">The <see cref="HttpResponseMessage"/>.</param>
    /// <param name="cancellationToken"> The cancellation token to cancel operation.</param>
    /// <returns>The task object representing the asynchronous operation.</returns>
    /// <exception cref="HttpRequestException">The HTTP response is unsuccessful and not <see cref="NotFound"/>.</exception>
    /// <exception cref="System.Text.Json.JsonException">Deserialization was unsuccessful.</exception>
    /// <exception cref="InvalidOperationException">Deserialization was unsuccessful.</exception>
    public static async Task<string?> HandleGetUserVoiceStateChannelIdAsyncAsync(
        HttpResponseMessage httpResponseMessage,
        CancellationToken cancellationToken) => IsNotFound(httpResponseMessage)
        ? null
        : (await HandleAsync<VoiceState>(httpResponseMessage, cancellationToken)).ChannelId;

    /// <summary>
    /// Ensure <see cref="HttpResponseMessage"/> was successful or status code was <see cref="NotFound"/>. Returns the <see cref="HttpResponseMessage"/>.
    /// </summary>
    /// <param name="httpResponseMessage">The <see cref="HttpResponseMessage"/>.</param>
    /// <param name="cancellationToken"> The cancellation token to cancel operation.</param>
    /// <returns>The task object representing the asynchronous operation.</returns>
    /// <exception cref="HttpRequestException">The HTTP response is unsuccessful and not <see cref="NotFound"/>.</exception>
    public static async Task<HttpResponseMessage> HandleDeleteMessageAsyncAsync(
        HttpResponseMessage httpResponseMessage,
        CancellationToken cancellationToken)
    {
        if (!IsNotFound(httpResponseMessage))
        {
            await HandleAsync(httpResponseMessage, cancellationToken);
        }

        return httpResponseMessage;
    }

    private static bool IsNotFound(HttpResponseMessage httpResponseMessage) =>
        httpResponseMessage.StatusCode == NotFound;
}