using BL.Qatar.QsipFileWatcher.Model;
using BL.Qatar.QsipFileWatcher.Models;
using EventBus.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Qatar.QsipFileWatcher.ExternalEvents;

public record QsipPackageRequestIntegrationEvent : IntegrationEvent  
{
    public static string EVENT_NAME = "QsipPackageRequest.IntegrationEvent";

    public QSipPackageDataModel QsipDataModel { get; init; }

    public QsipPackageRequestIntegrationEvent(QSipPackageDataModel qsipDataModel)
    {
        QsipDataModel = qsipDataModel;
    }
}
