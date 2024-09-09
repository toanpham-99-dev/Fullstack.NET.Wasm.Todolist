using WorkManagermentWeb.EventHandler.Events;

namespace WorkManagermentWeb.EventHandler.Services
{
    /// <summary>
    /// IEventPusher
    /// </summary>
    public interface IEventPusher
    {
        /// <summary>
        /// Publish
        /// </summary>
        /// <typeparam name="TEvent"></typeparam>
        /// <param name="event"></param>
        /// <returns></returns>
        Task Publish<TEvent>(TEvent @event)
            where TEvent : IEvent;
    }
}
