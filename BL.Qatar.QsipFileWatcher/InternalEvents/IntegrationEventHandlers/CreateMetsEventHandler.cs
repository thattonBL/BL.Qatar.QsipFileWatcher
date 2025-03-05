using BL.Internal.Messaging.Interfaces;
using BL.Qatar.QsipFileWatcher.InternalEvents.IntegrationEvents;
using BL.Qatar.QsipFileWatcher.Services;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Qatar.QsipFileWatcher.InternalEvents.IntegrationEventHandlers;

public class CreateMetsEventHandler(IQatarMetsBuilder metsBuilder, IInMemoryEventBus inMemoryEventBus,
        ILogger<ValidateAndRenameSubmissionEventHandler> logger) : INotificationHandler<CreateMetsEvent>
{
    public async Task Handle(CreateMetsEvent @event, CancellationToken cancellation)
    {
        Console.WriteLine("Handling the CreateMets Event..." + @event.Id);

        //Do the Validation

        //Rename the files

        //await inMemoryEventBus(new CreateMetsEvent)
    }
}
