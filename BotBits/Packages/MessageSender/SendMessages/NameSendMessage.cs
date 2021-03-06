using PlayerIOClient;

namespace BotBits.SendMessages
{
    /// <summary>
    ///     Sent to change world name.
    /// </summary>
    /// <seealso cref="SendMessage{T}" />
    public sealed class NameSendMessage : SendMessage<NameSendMessage>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="NameSendMessage" /> class.
        /// </summary>
        /// <param name="roomName">Name of the world.</param>
        public NameSendMessage(string roomName)
        {
            this.RoomName = roomName;
        }

        /// <summary>
        ///     Gets or sets the name of the world.
        /// </summary>
        /// <value>
        ///     The name of the world.
        /// </value>
        public string RoomName { get; set; }

        /// <summary>
        ///     Gets the PlayerIO message representing the data in this <see cref="SendMessage{T}" />.
        /// </summary>
        /// <returns></returns>
        protected override Message GetMessage()
        {
            return Message.Create("name", this.RoomName);
        }
    }
}