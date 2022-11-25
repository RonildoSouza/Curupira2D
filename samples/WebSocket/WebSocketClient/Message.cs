namespace WebSocketClient
{
    public class Message
    {
        public Message(string channel, string text)
        {
            if (string.IsNullOrEmpty(channel))
                throw new ArgumentNullException(nameof(channel));

            if (string.IsNullOrEmpty(text))
                throw new ArgumentNullException(nameof(text));

            Channel = channel;
            Text = text;
        }

        public string Channel { get; }
        public string Text { get; }

        public override string ToString() => $"{nameof(Channel)}: {Channel} | {nameof(Text)}: {Text}";
    }
}