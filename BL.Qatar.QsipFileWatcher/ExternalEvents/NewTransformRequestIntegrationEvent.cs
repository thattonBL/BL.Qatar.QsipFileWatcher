using BL.Qatar.QsipFileWatcher.Models;
using EventBus.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Qatar.QsipFileWatcher.ExternalEvents;

public record NewTransformRequestIntegrationEvent : IntegrationEvent
{
    public static string EVENT_NAME = "NewTransformRequest.IntegrationEvent";

    public TransformRequestDataModel TransformRequest { get; init; }

    public NewTransformRequestIntegrationEvent(TransformRequestDataModel transformRequest)
    {
        TransformRequest = transformRequest;
    }
}
