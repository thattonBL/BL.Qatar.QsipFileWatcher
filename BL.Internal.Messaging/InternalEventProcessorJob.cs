using BL.Internal.Messaging.Interfaces;
using MediatR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace BL.Internal.Messaging;

public class InternalEventProcessorJob(IInMemoryMessageQueue queue, IPublisher publisher, ILogger<InternalEventProcessorJob> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await foreach (IInternalEvent integrationEvent in queue.Reader.ReadAllAsync(stoppingToken))
        {
            try
            {
                logger.LogInformation($"Publishing {integrationEvent.Id}");

                await publisher.Publish(integrationEvent, stoppingToken);

                logger.LogInformation($"Processed {integrationEvent.Id}");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Unable to publish Integration Event with Id:{integrationEvent.Id}");
            }
        }
    }
}
