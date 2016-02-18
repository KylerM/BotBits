using PlayerIOClient;

namespace BotBits.SendMessages
{
    /// <summary>
    ///     Sent to activate magenta key.
    /// </summary>
    /// 
    public sealed class MagentaKeySendMessage : SendMessage<MagentaKeySendMessage>
    {
        public MagentaKeySendMessage()
        {
        }

        public MagentaKeySendMessage(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        public int X { get; set; }
        public int Y { get; set; }

        /// <summary>
        ///     Gets the PlayerIO message representing the data in this <see cref="SendMessage{T}" />.
        /// </summary>
        /// <returns></returns>
        protected override Message GetMessage()
        {
            return Message.Create("pressKey", this.X, this.Y, "magenta");
        }
    }
}