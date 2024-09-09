using MediatR;

namespace WorkManagermentWeb.EventHandler
{
    /// <summary>
    /// MediatRMessage
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MediatRMessage<T> : INotification
    {
        /// <summary>
        /// MediatRMessage
        /// </summary>
        /// <param name="message"></param>
        public MediatRMessage(T message)
        {
            Message = message;
        }

        /// <summary>
        /// Message
        /// </summary>
        public T Message { get; set; }
    }
}
