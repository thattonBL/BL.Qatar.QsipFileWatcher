using BL.Qatar.QsipFileWatcher.Models;
using EventBus.Events;

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
