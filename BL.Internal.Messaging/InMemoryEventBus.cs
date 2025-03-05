using BL.Internal.Messaging.Interfaces;

namespace BL.Internal.Messaging;

public sealed class InMemoryEventBus(IInMemoryMessageQueue queue) : IInMemoryEventBus
{
    public async Task PublishAsync<T>(T integrationEvent, CancellationToken cancellationToken = default) where T : class, IInternalEvent
    {
        await queue.Writer.WriteAsync(integrationEvent, cancellationToken);
    }
}
