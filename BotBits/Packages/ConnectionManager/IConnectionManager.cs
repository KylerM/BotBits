using JetBrains.Annotations;
using PlayerIOClient;

namespace BotBits
{
    public interface IConnectionManager
    {
        void SetConnection([NotNull] Connection connection, ConnectionArgs args);
        void SetConnection([NotNull] IConnection connection, ConnectionArgs args);
    }
}