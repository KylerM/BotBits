using PlayerIOClient;

namespace BotBits.Events
{
    /// <summary>
    ///     Occurs when the player initially joins the room. Contains world information such as title and world content.
    /// </summary>
    /// <seealso cref="ReceiveEvent{T}" />
    [ReceiveEvent("init")]
    public sealed class InitEvent : PlayerEvent<InitEvent>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="InitEvent" /> class.
        /// </summary>
        /// <param name="message">The EE message.</param>
        /// <param name="client"></param>
        internal InitEvent(BotBitsClient client, Message message)
            : base(client, message, 6, true)
        {
            this.Owner = message.GetString(0);
            this.WorldName = message.GetString(1);
            this.Plays = message.GetInt(2);
            this.Favorites = message.GetInt(3);
            this.Likes = message.GetInt(4);
            this.EncryptedToken = message.GetString(5);
            // 6: UserId
            this.Smiley = (Smiley) message.GetInt(7);
            this.AuraShape = (AuraShape) message.GetInt(8);
            this.AuraColor = (AuraColor)message.GetInt(9);
            // 9: Gold border
            this.SpawnX = message.GetDouble(11);
            this.SpawnY = message.GetDouble(12);
            this.ChatColor = message.GetUInt(13);
            this.Username = message.GetString(14);
            this.CanEdit = message.GetBoolean(15);
            this.IsOwner = message.GetBoolean(16);
            this.Favorited = message.GetBoolean(17);
            this.Liked = message.GetBoolean(18);
            this.WorldWidth = message.GetInt(19);
            this.WorldHeight = message.GetInt(20);
            this.GravityMultiplier = message.GetDouble(21);
            this.BackgroundColor = message.GetUInt(22);
            this.Visible = message.GetBoolean(23);
            this.HideLobby = message.GetBoolean(24);
            this.AllowSpectating = message.GetBoolean(25);
            this.RoomDescription = message.GetString(26);
            this.CurseLimit = message.GetInt(27);
            this.ZombieLimit = message.GetInt(28);
            this.Campaign = message.GetBoolean(29);
            this.CrewId = message.GetString(30);
            this.CrewName = message.GetString(31);
            this.CanChangeWorldOptions = message.GetBoolean(32);
            this.WorldStatus = (WorldStatus) message.GetInt(33);
            this.Badge = message.GetBadge(34);
            this.CrewMember = message.GetBoolean(35);
            this.MinimapEnabled = message.GetBoolean(36);
            this.LobbyPreviewEnabled = message.GetBoolean(37);
        }

        public AuraColor AuraColor { get; set; }

        public bool LobbyPreviewEnabled { get; set; }

        public bool MinimapEnabled { get; set; }

        public bool CrewMember { get; set; }

        public WorldStatus WorldStatus { get; set; }

        public Badge Badge { get; set; }

        public bool CanChangeWorldOptions { get; set; }

        public string CrewName { get; set; }

        public string CrewId { get; set; }

        public bool Campaign { get; set; }

        public bool Liked { get; set; }

        public bool Favorited { get; set; }

        public int ZombieLimit { get; set; }

        public int CurseLimit { get; set; }

        public string RoomDescription { get; set; }

        public bool AllowSpectating { get; set; }

        public uint ChatColor { get; set; }

        public bool HideLobby { get; set; }

        public AuraShape AuraShape { get; set; }

        public Smiley Smiley { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this <see cref="InitEvent" /> is visible.
        /// </summary>
        /// <value>
        ///     <c>true</c> if visible; otherwise, <c>false</c>.
        /// </value>
        public bool Visible { get; set; }

        /// <summary>
        ///     Gets or sets the color of the background.
        /// </summary>
        /// <value>
        ///     The color of the background.
        /// </value>
        public uint BackgroundColor { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this player is allowed to edit.
        /// </summary>
        /// <value><c>true</c> if this instance can edit; otherwise, <c>false</c>.</value>
        public bool CanEdit { get; set; }

        /// <summary>
        ///     Gets or sets the encryption option of the world.
        /// </summary>
        /// <value>The encryption.</value>
        public string EncryptedToken { get; set; }

        /// <summary>
        ///     Gets or sets the gravity of the world.
        /// </summary>
        /// <value>The gravity.</value>
        public double GravityMultiplier { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this player owns the world.
        /// </summary>
        /// <value><c>true</c> if this player is the owner; otherwise, <c>false</c>.</value>
        public bool IsOwner { get; set; }

        /// <summary>
        ///     Gets or sets the width of the world.
        /// </summary>
        /// <value>The width of the room.</value>
        public int WorldWidth { get; set; }

        /// <summary>
        ///     Gets or sets the height of the world.
        /// </summary>
        /// <value>The height of the room.</value>
        public int WorldHeight { get; set; }

        /// <summary>
        ///     Gets or sets the spawn x coordinate.
        /// </summary>
        /// <value>The spawn x.</value>
        public double SpawnX { get; set; }

        /// <summary>
        ///     Gets or sets the spawn y coordinate.
        /// </summary>
        /// <value>The spawn y.</value>
        public double SpawnY { get; set; }

        /// <summary>
        ///     Gets or sets the username.
        /// </summary>
        /// <value>The username.</value>
        public string Username { get; set; }

        /// <summary>
        ///     Gets or sets the current woots of the world.
        /// </summary>
        /// <value>The current woots.</value>
        public int Favorites { get; set; }

        /// <summary>
        ///     Gets or sets the owner username of the world.
        /// </summary>
        /// <value>The owner username.</value>
        public string Owner { get; set; }

        /// <summary>
        ///     Gets or sets the plays of the world.
        /// </summary>
        /// <value>The plays.</value>
        public int Plays { get; set; }

        /// <summary>
        ///     Gets or sets the total woots of the world.
        /// </summary>
        /// <value>The total woots.</value>
        public int Likes { get; set; }

        /// <summary>
        ///     Gets or sets the name of the world.
        /// </summary>
        /// <value>The name of the world.</value>
        public string WorldName { get; set; }

        /// <summary>
        ///     Gets the block x.
        /// </summary>
        /// <value>The block x.</value>
        public int SpawnBlockX
        {
            get { return WorldUtils.PosToBlock(this.SpawnX); }
        }

        /// <summary>
        ///     Gets the block y.
        /// </summary>
        /// <value>The block y.</value>
        public int SpawnBlockY
        {
            get { return WorldUtils.PosToBlock(this.SpawnY); }
        }
    }
}