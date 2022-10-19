
namespace PollingPoc.Domain.Interfaces
{
    /// <summary>
    /// IMessageBroker pattern implementation for creation of instances of that will be use WebScoket connection.
    /// </summary>
    public interface IMessageBroker
    {
        /// <summary>
        /// This method publishes a message through WebSocket connection.
        /// </summary>
        /// <param name="subject">The subject of the message.</param>
        /// <param name="message">The message of the publication.</param>
        void Publish(string subject = null, object message = null);
    }
}
