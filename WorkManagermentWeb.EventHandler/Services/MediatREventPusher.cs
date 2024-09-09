using MediatR;
using WorkManagermentWeb.EventHandler.Events;

namespace WorkManagermentWeb.EventHandler.Services
{
    /// <summary>
    /// MediatREventPusher
    /// </summary>
    public class MediatREventPusher : IEventPusher
    {
        /// <summary>
        /// IMediator
        /// </summary>
        private readonly IMediator _mediator;

        /// <summary>
        /// MediatREventPusher
        /// </summary>
        /// <param name="mediator"></param>
        public MediatREventPusher(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Publish
        /// </summary>
        /// <typeparam name="TEvent"></typeparam>
        /// <param name="event"></param>
        /// <returns></returns>
        public async Task Publish<TEvent>(TEvent @event) where TEvent : IEvent
        {
            await _mediator.Publish(new MediatRMessage<TEvent>(@event)).ConfigureAwait(false);
        }
    }
}
