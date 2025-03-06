using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Qatar.QsipFileWatcher.Model;

public class QSipPackageDataModel
{
    public QSipPackageDataModel(string renamedCsvPath)
    {
        SubmissionCsvPath = renamedCsvPath;
    }
    
    public string SubmissionCsvPath { get; set; } 
}
