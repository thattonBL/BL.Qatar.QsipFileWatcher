using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Qatar.QsipFileWatcher.Models;

public class TransformRequestDataModel
{
    public TransformRequestDataModel(string reference, string masterFileUri, string callbackUrl, string transformationProfile = "default")
    {
        Reference = reference;
        MasterFileUri = masterFileUri;
        CallbackUrl = callbackUrl;
        TransformationProfile = transformationProfile;
    }

    public string Reference { get; set; }
    public string MasterFileUri { get; set; }
    public string CallbackUrl { get; set; }
    public string TransformationProfile { get; set; }
}
