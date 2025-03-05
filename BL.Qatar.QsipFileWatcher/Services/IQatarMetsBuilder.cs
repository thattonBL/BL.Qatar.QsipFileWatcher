using BL.Sip.Utilities.Core;
using System.Xml.Linq;

namespace BL.Qatar.QsipFileWatcher.Services
{
    public interface IQatarMetsBuilder
    {
        string AddAmdSecForBlRightsDeclarationInfo(XNamespace metsNs, string workflowEntityArk, int amdSecNumber, XElement metsRootXElement, string donorName);
        XElement CreateFileElement(int amdSecNumber, string contentFileUrl, int fileElementNumber, bool referenceToPremisEventRequired = false);
        XElement CreateFileGrp(string use);
        XElement CreateFilePointerWithArea(int fileElementNumber, string begin, string end);
        XDocument CreateMetsDocument(string collection, string softwareNameVersion, string acquisitionPremisEventOutcomeDetailNote, string workflowEntityArk, string metsDocumentArk, List<ContentFile> contentFiles, List<ContentFile> associatedFiles);
    }
}