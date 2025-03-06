using EventBus.Events;

namespace BL.Qatar.QsipFileWatcher.ExternalEvents;

public record QsipPackageRequestIntegrationEvent : IntegrationEvent  
{
    public static string EVENT_NAME = "QsipPackageRequest.IntegrationEvent";

    public string CsvFilePath { get; init; }

    public QsipPackageRequestIntegrationEvent(string csvFilePath)
    {
        CsvFilePath = csvFilePath;
    }
}
