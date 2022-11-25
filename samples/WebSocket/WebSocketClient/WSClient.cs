using Newtonsoft.Json;
using System.Reactive.Linq;
using Websocket.Client;

namespace WebSocketClient
{
    /// <summary>
    /// https://github.com/Marfusios/websocket-client
    /// </summary>
    public class WSClient : IAsyncDisposable
    {
        private readonly Uri _url = new("ws://127.0.0.1:6001");
        private readonly string _channel;

        public WSClient(string channel)
        {
            if (string.IsNullOrEmpty(channel))
                throw new ArgumentNullException(nameof(channel));

            _channel = channel;
            Client = new WebsocketClient(_url)
            {
                Name = _channel,
                ReconnectTimeout = TimeSpan.FromSeconds(120),
                ErrorReconnectTimeout = TimeSpan.FromSeconds(30)
            };

            Client.Start();
        }

        public IWebsocketClient? Client { get; }

        public void Send(Message message)
        {
            if (message == null)
                return;

            Client?.Send(JsonConvert.SerializeObject(message, Formatting.None));
        }

        public void Subscribe(Action<Message> onNext, Action<Exception> onError, Action onCompleted)
        {
            Client?.MessageReceived
                .Where(msg => !string.IsNullOrEmpty(msg.Text))
                .Where(msg => msg.Text.Contains(_channel))
                .Subscribe(msg => onNext?.Invoke(JsonConvert.DeserializeObject<Message>(msg.Text)), onError, onCompleted);
        }

        public void Subscribe(Action<Message> onNext) => Subscribe(onNext, ex => { }, () => { });

        public void Subscribe(Action<Message> onNext, Action<Exception> onError) => Subscribe(onNext, onError, () => { });

        public async ValueTask DisposeAsync()
        {
            await Client?.Stop(System.Net.WebSockets.WebSocketCloseStatus.NormalClosure, string.Empty);
            Client?.Dispose();
        }
    }
}