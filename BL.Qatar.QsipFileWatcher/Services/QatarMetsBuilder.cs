using BL.Sip.Utilities.Core;
using BL.Xml.Core;
using Microsoft.Extensions.Logging;
using System.Text;
using System.Xml.Linq;

namespace BL.Qatar.QsipFileWatcher.Services;

public class QatarMetsBuilder : IQatarMetsBuilder
{
    private readonly IArkMinterService _arkMinter;
    private readonly ILogger<QatarMetsBuilder> _logger;

    public QatarMetsBuilder(IArkMinterService arkMinter, ILogger<QatarMetsBuilder> logger)
    {
        _arkMinter = arkMinter;
        _logger = logger;
    }

    /// <summary>Create a Mets Document</summary>
    /// <param name="collection">The collection name of the entity</param>
    /// <param name="softwareNameVersion">The software and version name used to create this mets</param>
    /// <param name="acquisitionPremisEventOutcomeDetailNote">The acquisition Premis EventOutcome Detail Note to be added</param>
    /// TODO: <param name="ContentFileList">The content Files for the submission that we need to represent in the mets</param>
    /// <param name="workflowEntityArkId">The arkId/itemUID of the workflowEntity</param>
    /// <param name="dArk">The digital ark to use in the mets Header element</param>
    /// <param name="contentFiles"></param>
    /// <param name="metadataModels">The track Models that are serialised to MARC XML</param>
    /// <param name="productMetadata">If we are creating mets for an album this will be populated</param>
    /// <returns>The created xDocument</returns>
    /// <remarks>This method is called from its own action group</remarks>
    public XDocument CreateMetsDocument(
        string collection,
        string softwareNameVersion,
        string acquisitionPremisEventOutcomeDetailNote,
        string workflowEntityArk,
        string metsDocumentArk,
        List<ContentFile> contentFiles,
        List<ContentFile> associatedFiles)
    //List<FileMetadataModel> metadataModels,
    //ProductMetadataModel productMetadata = null)
    {
        if (contentFiles.Count == 0)
        {
            throw new ArgumentException("submission has 0 master files");
        }

        var xDocument = MetsBuilder.CreateMetsStructure();

        var nextDmdSecNumber = 1;
        var amdSecNumber = 1;
        var fileElementNumber = 1;

        var namespacePrefixIdentifierMappings = GetNamespacePrefixIdentifierMappings();
        var namespaceDeclarationInfo = new NamespaceDeclarationInfo
        {
            NamespaceDeclarations = namespacePrefixIdentifierMappings
        };

        var metsRootElement = MetsBuilder.CreateMetsElementAndNamespacesAndSchemaLocations(collection, namespaceDeclarationInfo);

        xDocument.Add(metsRootElement);
        AddMetsHdr(softwareNameVersion, metsRootElement, metsDocumentArk);

        ////mint arks for dmdSec elements
        var dmdArks = MintArks(contentFiles.Count + 1);
        //add dmdSec for work
        AddDmdSec(nextDmdSecNumber++, metsRootElement, dmdArks[0]);

        //we need these
        var recordingArks = MintArks(contentFiles.Count + 1);
        // var for the donor name
        string submissionDonor;

        /*if (productMetadata != null)
        {
            productMetadata.LogicalRootArk = workflowEntityArk;
            productMetadata.LogicalRecordingArk = recordingArks[0];
            productMetadata.DmdProductArk = dmdArks[0];
            submissionDonor = productMetadata.DonorName;
        }
        else
        {
            metadataModels[0].LogicalRootArk = workflowEntityArk;
            metadataModels[0].LogicalRecordingArk = recordingArks[0];
            metadataModels[0].DmdProductArk = dmdArks[0];
            submissionDonor = metadataModels[0].DonorName;
        }*/

        //add dmdSec for contentFiles not associated files*/
        // We will have to loop with multiple files
        for (var i = 0; i < contentFiles.Count; i++)
        {
            // We need the arks in the model before we serialize to SAMI MARC
            /*if (productMetadata != null)
            {
                metadataModels[i].LogicalRecordingArk = recordingArks[i + 1];
            }
            metadataModels[i].DmdRecordingArk = dmdArks[i + 1];*/
            AddDmdSec(nextDmdSecNumber++, metsRootElement, dmdArks[i + 1]);
        }
        //AddSamiMARC(nextDmdSecNumber++, metsRootElement, metadataModels, productMetadata);

        var rightsAmdId = AddAmdSecForBlRightsDeclarationInfo(Namespaces.Mets, workflowEntityArk, amdSecNumber++, metsRootElement, "submissionDonor");

        AddFiles(acquisitionPremisEventOutcomeDetailNote, workflowEntityArk, contentFiles, associatedFiles, recordingArks, metsRootElement, rightsAmdId,
            ref amdSecNumber, ref fileElementNumber);

        return xDocument;
    }

    /*public XElement AddSamiMARC(int dmdSecNumber, XElement metsRootElement, List<FileMetadataModel> metadataModels, ProductMetadataModel productModel)
    {
        var dmdSecInfo = new DmdSecInfo
        {
            Id = dmdSecNumber,
            MdType = "MARC"
        };

        var dmdSecElement = CreateDmdSecElement(dmdSecInfo);
        dmdSecElement.Elements().First().Add(CreateMarcDataForTrack(metadataModels, productModel));

        metsRootElement.Add(dmdSecElement);

        return dmdSecElement;
    }*/

    /// <summary>Gets the dictionary of values to be used in declaring the required namespaces</summary>
    /// <returns>Returns the dictionary which represents the namespace prefix identifier mappings for an ejournals mets document</returns>
    private static Dictionary<string, string> GetNamespacePrefixIdentifierMappings()
    {
        var namespacePrefixIdentifierMappings = new Dictionary<string, string>
        {
                {"mets", Namespaces.Mets.NamespaceName},
                {"premis", Namespaces.Premis.NamespaceName},
                {"xlink", Namespaces.Xlink.NamespaceName},
                {"mods", Namespaces.Mods.NamespaceName}
                //{"rts", Namespaces.Rights.NamespaceName}
            };
        return namespacePrefixIdentifierMappings;
    }

    /// <summary>
    /// Add the MetsHdr sec into the mets file
    /// </summary>
    /// <param name="softwareNameVersion">The software name that was used</param>
    /// <param name="metsNamespaceElement">The element to add this sec to</param>
    /// <param name="dArk">The digital ark to use in the mets header</param>
    private static void AddMetsHdr(string softwareNameVersion, XContainer metsNamespaceElement, string dArk)
    {
        var metsHdrInfo = new MetsHdrInfo
        {
            AdmId = String.Empty,
            AgentName = softwareNameVersion,
            DocumentId = dArk,
        };

        var metsHdrElement = MetsBuilder.CreateMetsHdrElement(metsHdrInfo);
        metsNamespaceElement.Add(metsHdrElement);
    }

    private static XElement AddDmdSec(int dmdSecNumber, XElement metsRootElement, string ark)
    {
        var dmdSecInfo = new DmdSecInfo
        {
            Id = dmdSecNumber,
            MdType = "OTHER",
            OtherMdType = "SAMI",
            LocType = "ARK",
            HRef = ark,
        };

        var dmdSecElement = MetsBuilder.CreateDmdSecElement(dmdSecInfo);

        metsRootElement.Add(dmdSecElement);

        return dmdSecElement;
    }

    //had to copy and change this from core since its not working correct
    public static XElement CreateDmdSecElement(DmdSecInfo dmdSecInfo)
    {
        if (dmdSecInfo == null)
        {
            throw new ArgumentNullException("dmdSecInfo", "dmdSecInfo cannot be missing.");
        }

        ValidatorHelperMethods.ValidateIntUpperAndLower(dmdSecInfo.Id, "dmdSecInfo.Id", 9999, 0);
        ValidatorHelperMethods.ValidateString(dmdSecInfo.MdType, "dmdSecInfo.MdType");

        var idString = $"dmd{dmdSecInfo.Id:00000000}";

        XElement dmdSecElement;
        if (dmdSecInfo.MdType == "MODS" || dmdSecInfo.MdType == "MARC")
        {
            dmdSecElement = new XElement(Namespaces.Mets + "dmdSec", new XAttribute("ID", idString),
                new XElement(Namespaces.Mets + "mdWrap", new XAttribute("MDTYPE", dmdSecInfo.MdType)));

            return dmdSecElement;
        }

        if (dmdSecInfo.MdType == "OTHER" && dmdSecInfo.OtherMdType != "SAMI")
        {
            var mdWrap = new XElement(Namespaces.Mets + "mdWrap", new XAttribute("MDTYPE", "OTHER"),
                new XElement(Namespaces.Mets + "xmlData"));

            if (!String.IsNullOrEmpty(dmdSecInfo.OtherMdType))
            {
                mdWrap.SetAttributeValue("OTHERMDTYPE", dmdSecInfo.OtherMdType);
            }

            dmdSecElement = new XElement(Namespaces.Mets + "dmdSec", new XAttribute("ID", idString),
            mdWrap);
            return dmdSecElement;
        }

        dmdSecElement = new XElement(Namespaces.Mets + "dmdSec", new XAttribute("ID", idString));

        var mdRefElement = new XElement(Namespaces.Mets + "mdRef",
            new XAttribute("MDTYPE", dmdSecInfo.MdType),
            new XAttribute("LOCTYPE", dmdSecInfo.LocType));

        if (!String.IsNullOrEmpty(dmdSecInfo.OtherLocType) && dmdSecInfo.LocType == "OTHER")
        {
            mdRefElement.SetAttributeValue("OTHERLOCTYPE", dmdSecInfo.OtherLocType);
        }

        if (!String.IsNullOrEmpty(dmdSecInfo.HRef))
        {
            mdRefElement.SetAttributeValue(Namespaces.Xlink + "href", dmdSecInfo.HRef);
        }

        if (!string.IsNullOrEmpty(dmdSecInfo.OtherMdType))
        {
            mdRefElement.SetAttributeValue("OTHERMDTYPE", dmdSecInfo.OtherMdType);
        }

        dmdSecElement.Add(mdRefElement);

        return dmdSecElement;
    }

    /*public XElement CreateMarcDataForTrack(List<FileMetadataModel> metadataModels, ProductMetadataModel productMetadata)
    {
        var metsNs = Namespaces.Mets;
        var marcNs = Namespaces.Marc;

        XElement productMarcRecord;
        if (productMetadata == null)
        {
            productMarcRecord = XDocument.Parse(metadataModels[0].SerializeAsMarcProductXmlString()).Elements().First();
        }
        else
        {
            productMarcRecord = XDocument.Parse(productMetadata.SerializeAsMarcProductXmlString()).Elements().First();
        }

        List<XElement> recordingMarcXmls = new List<XElement>();
        foreach (var recModel in metadataModels)
        {
            recordingMarcXmls.Add(XDocument.Parse(recModel.SerializeAsMarcRecordXmlString()).Elements().First());
        }

        var tempRoot = new XElement("tempRoot",
            new XAttribute(XNamespace.Xmlns + "mets", metsNs),
            new XElement(metsNs + "xmlData",
                new XElement(marcNs + "collection", new XAttribute(XNamespace.Xmlns + "marc", marcNs),
                    productMarcRecord.Elements().First(),
                    recordingMarcXmls.Select(s => s)
                    )));

        return tempRoot.Elements().First();
    }*/


    public string AddAmdSecForBlRightsDeclarationInfo(XNamespace metsNs, string workflowEntityArk, int amdSecNumber, XElement metsRootXElement, string donorName)
    {
        //TODO: May need to look again if we have to support other formats we should maybe gather all the data we require at the beginning and store it.
        //var myRightsAndProvenance = ddexHelper.GetRightsAndProvenance(ddexDoc);

        //The Creator, Our Organisation reference along with the unique ark indentifier are added in this section
        //We need to ensure that the Creator Name is UTF-8 encodable
        byte[] creatorBytes = Encoding.Default.GetBytes("Except as otherwise permitted under your national copyright law this material may not be copied or distributed further.");

        //These are new namespaces
        XNamespace odrNs = "http://www.w3.org/ns/odrl/2/";
        XNamespace dcNs = "http://purl.org/dc/terms/";
        XNamespace xsiNs = "http://www.w3.org/2001/XMLSchema-instance";

        //Create the amdSec node within a temporary root node to allow use of namespaces
        var tempRoot = new XElement("tempRoot",
                new XAttribute(XNamespace.Xmlns + "mets", metsNs),
                new XElement(metsNs + "amdSec", new XAttribute("ID", $"amd{amdSecNumber:00000000}"),
                    new XElement(metsNs + "rightsMD", new XAttribute("ID", $"amd{amdSecNumber:00000000}-rights01"),
                        new XElement(metsNs + "mdWrap", new XAttribute("MDTYPE", "METSRIGHTS"),
                            new XElement(metsNs + "xmlData",
                                new XElement(odrNs + "Policy",
                                    new XAttribute("type", "http://www.w3.org/ns/odrl/2/set"),
                                    new XAttribute("uid", workflowEntityArk),
                                    new XAttribute("inheritFrom", "http://bl.uk/rights/raqs/17.4/"),
                                    new XAttribute(xsiNs + "schemaLocation", "http://www.w3.org/ns/odrl/2/ http://www.w3.org/ns/odrl/2/ODRL21.xsd"),
                                    new XAttribute(XNamespace.Xmlns + "odrl", odrNs),
                                    new XAttribute(XNamespace.Xmlns + "dc", dcNs),
                                    new XElement(dcNs + "rights", new XAttribute("id", "attribution"), Encoding.UTF8.GetString(creatorBytes)),
                                    new XElement(dcNs + "contributor", donorName),
                                    new XElement(dcNs + "provenance", "British Library MSP Portal")
                                    ))))));

        //If you wanna see it then use this
        //Console.WriteLine(tempRoot);
        //Take the contents of the temporary root node and put it into the mets after the last amdSec node
        metsRootXElement.Add(tempRoot.Elements().FirstOrDefault());
        return $"amd{amdSecNumber:00000000}-rights01";
    }

    private void AddFiles(string acquisitionPremisEventOutcomeDetailNote, string workflowEntityArkId, List<ContentFile> contentFiles, List<ContentFile> associatedFiles, List<string> arkList, XElement metsRootElement, string rightsAmdId, ref int amdSecNumber, ref int fileElementNumber)
    {
        var myDmdSecNumber = 2;

        var originalFileGroup = CreateFileGrp("Original");
        var fileSecElement = CreateFileSecElement();
        fileSecElement.Add(originalFileGroup);

        var audio = CreateFileGrp("Audio");
        originalFileGroup.Add(audio);

        var logicalDivs = new List<XElement>();
        var nextLogicalId = 2;
        var arkId = 0;

        var divArks = arkList; //MintArks(contentFiles.Count);

        //at this point the contentFiles collection should only contain original master audio files
        foreach (var file in contentFiles)
        {
            file.MimeType = MimeTypes.MimeTypeByExtension(Path.GetExtension(file.FullPath));

            var fileElement = AddFile(acquisitionPremisEventOutcomeDetailNote, file, metsRootElement, out var filePointer, ref amdSecNumber,
                ref fileElementNumber, true);
            audio.Add(fileElement);

            var logicalDiv = CreateLogicalDiv(nextLogicalId, myDmdSecNumber, divArks[arkId], "Recording");
            nextLogicalId++;
            arkId++;
            myDmdSecNumber++;
            logicalDiv.Add(filePointer);
            logicalDivs.Add(logicalDiv);
        }

        #region other files types images etc

        arkId = 0;

        var imageArks = MintArks(associatedFiles.Count);

        var image = CreateFileGrp("Image");
        originalFileGroup.Add(image);

        foreach (var file in associatedFiles)
        {
            file.MimeType = MimeTypes.MimeTypeByExtension(Path.GetExtension(file.FullPath));

            var fileElement = AddFile(acquisitionPremisEventOutcomeDetailNote, file, metsRootElement, out var filePointer, ref amdSecNumber,
                ref fileElementNumber, false);
            image.Add(fileElement);

            var logicalDiv = CreateLogicalDiv(nextLogicalId, null, imageArks[arkId], GetDivTypeFromMime(file.MimeType));
            nextLogicalId++;
            arkId++;
            logicalDiv.Add(filePointer);
            logicalDivs.Add(logicalDiv);
        }
        /*
        if (supplementaryFiles.Count > 0)
        {
            var suppFileGroup = CreateFileGrp("Supplementary");
            fileSecElement.Add(suppFileGroup);

            foreach (var file in supplementaryFiles)
            {
                file.MimeType = MimeTypes.MimeTypeByExtension(Path.GetExtension(file.FullPath));

                var fileElement = AddFile(acquisitionPremisEventOutcomeDetailNote, file, metsRootElement, out var filePointer, ref amdSecNumber,
                    ref fileElementNumber, false);
                suppFileGroup.Add(fileElement);

                var logicalDiv = CreateLogicalDiv(nextLogicalId, null, divArks[arkId], GetDivTypeFromMime(file.MimeType));
                nextLogicalId++;
                arkId++;
                logicalDiv.Add(filePointer);
                logicalDivs.Add(logicalDiv);
            }
        }*/
        #endregion

        metsRootElement.Add(fileSecElement);

        AddLogicalStructMap(workflowEntityArkId, rightsAmdId, metsRootElement, logicalDivs);
    }

    private XElement AddFile(string acquisitionPremisEventOutcomeDetailNote, ContentFile file, XElement metsRootElement, out XElement filePointer, ref int amdSecNumber, ref int fileElementNumber, bool audio)
    {
        //var fileSystem = new FileSystem();
        //below must be read/transformed from the ddex??
        //TODO: Ensure ContentFiles passed in already have Hash and HashAlgorithm set also they need FileSize setting
        //file.HashAlgorithm = "SHA256";
        //file.Hash = FileSystem.GetHash(file.FullPath, file.HashAlgorithm);

        var acquisitionPremisEvent = CreateAcquisitionPremisEvent(amdSecNumber, acquisitionPremisEventOutcomeDetailNote);
        AddAmdSecForContentFile(amdSecNumber, file, metsRootElement, acquisitionPremisEvent);

        var fileElement = CreateFileElement(amdSecNumber, file.FullPath, fileElementNumber, true);

        //create file elements
        if (audio)
        {
            var begin = "00:00:00:00";//all recordings start at 0 for now
            var end = "duration";

            filePointer = CreateFilePointerWithArea(fileElementNumber, begin, end);
        }
        else
        {
            filePointer = CreateFilePointer(fileElementNumber);
        }

        fileElementNumber++;
        amdSecNumber++;
        return fileElement;
    }

    /// <summary>
    /// Creates an acquisition premis event
    /// </summary>
    /// <param name="amdSecNumber">The integer part of the amd Sec Id attribute value</param>
    /// <param name="acquisitionText">The acquisition text to be used within the premis event's outcome detail note</param>
    /// <returns>An instance of an XElement representing a digital provenance element containing an acquisition premis event</returns>
    private static XElement CreateAcquisitionPremisEvent(int amdSecNumber, string acquisitionText)
    {
        var amdIdAttributeValue = $"amd{amdSecNumber:00000000}";
        var digiprovIdAttributeValue = amdIdAttributeValue + "-event01";

        XElement acquisitionPremisElement = MetsBuilder.CreateDigiProvenanceMetadataElement(digiprovIdAttributeValue, "PREMIS:EVENT");

        var premisEventInfoWithOutcomeDetail = new PremisEventInfoWithOutcomeDetail
        {
            EventDateTime = $"{DateTime.Now:s}",
            EventIdentifierType = "local",
            EventIdentifierValue = "event01",
            EventType = "Acquisition",
            EventOutcome = "Success",
            EventOutcomeDetailNote = acquisitionText,
            LinkingAgentIdentifierType = "",
            LinkingAgentIdentifierValue = ""
        };

        XElement premisElement = PremisBuilder.CreatePremisEventElementWithOutcomeDetailWithoutAgentIdentifier(premisEventInfoWithOutcomeDetail);

        acquisitionPremisElement.Descendants().Last().Add(premisElement);
        return acquisitionPremisElement;
    }

    /// <summary>Creates the amdSec for the pdf and adds it to the XDocument</summary>
    /// <param name="amdSecNumber">The number to allocate to this amd sec</param>
    /// <param name="contentFile">The contentFile instance which describes the file</param>
    /// <param name="metsRootElement">The element to add this amdSec to</param>
    /// <param name="digiProvenanceElement">An XElement to represent a digital provence element that should be added</param>
    private static void AddAmdSecForContentFile(int amdSecNumber, ContentFile contentFile, XContainer metsRootElement, XElement digiProvenanceElement)
    {
        var amdIdAttributeValue = $"amd{amdSecNumber:00000000}";
        var digiprovIdAttributeValue = amdIdAttributeValue + "-object01";

        var amdSecInfo = new AmdSecInfo
        {
            Id = amdSecNumber,
            Md = "digiprovMD",
            MdId = digiprovIdAttributeValue
        };
        var amdSecElement = MetsBuilder.CreateAmdSecElement(amdSecInfo);

        var mdWrapInfo = new MdWrapInfo
        {
            MdType = "PREMIS:OBJECT",
            OtherMdType = String.Empty
        };

        var mdWrapElement = MetsBuilder.CreateMdWrapElement(mdWrapInfo);

        amdSecElement.Descendants().Last().Add(mdWrapElement);

        var premisObjectInfo = new PremisObjectInfo
        {
            PremisObjectType = "premis:file",
            ObjectIdentifierType = "ARK",
            //TODO: check the ark format. Where is the content fileId coming from?
            ObjectIdentifierValue = contentFile.ID,
            CompositionLevel = "0",
            MessageDigestAlgorithm = contentFile.HashAlgorithm,
            MessageDigest = contentFile.Hash,
            Size = contentFile.FileSize.ToString(),
            FormatName = contentFile.MimeType,
            PremisOriginalName = contentFile.FullPath
        };

        var premisObjectElement = PremisBuilder.CreatePremisObject(premisObjectInfo);
        amdSecElement.Descendants().Last().Add(premisObjectElement);
        amdSecElement.Elements().Last().AddAfterSelf(digiProvenanceElement);
        metsRootElement.Add(amdSecElement);
    }

    private static void AddLogicalStructMap(string workflowEntityArk, string rightsAmdId, XElement rootElement, List<XElement> divs)
    {
        var structMapInfo = new StructMapInfo
        {
            Type = "LOGICAL"
        };
        var logicalStructMapElement = MetsBuilder.CreateStructMapElement(structMapInfo);

        var logicalStructMapDivInfo = new StructMapDivInfo
        {
            Id = "log00000001",
            ContentIds = workflowEntityArk,
            Type = "Work",
            DmdId = "dmd00000001",
            AdmId = rightsAmdId
        };
        var workDiv = MetsBuilder.CreateStructMapDivElement(logicalStructMapDivInfo);

        logicalStructMapElement.Add(workDiv);
        workDiv.Add(divs);

        rootElement.Add(logicalStructMapElement);
    }

    /// <summary>
    /// Create a fileGrp element for a given content file
    /// </summary>
    /// <param name="amdSecNumber">The amdSec Id (integer part) that describes the content file</param>
    /// <param name="fileElementNumber">An integer that fortms the numeric part of the File elements ID attribute</param>
    /// <param name="contentFile">The content file which is described by the file group</param>
    /// <param name="referenceToPremisEventRequired">A boolean to indicate whether a reference should be added to the file admid attribute pointing at an acquisition premis event</param>
    /// <returns>An instance of an XElement representing the filegroup element</returns>
    public XElement CreateFileElement(int amdSecNumber, string contentFileUrl, int fileElementNumber, bool referenceToPremisEventRequired = false)
    {
        var amdIdAttributeValue = String.Format(referenceToPremisEventRequired ? "amd{0:00000000}-object01 amd{0:00000000}-event01" : "amd{0:00000000}-object01", amdSecNumber);

        //var isExternalContentContentFile =
        //    String.Equals(contentFile.Filename, PurchasedMetsBuilder.ExternalContentFilename, StringComparison.CurrentCultureIgnoreCase);
        var isExternalContentContentFile = false;

        var fileInfo = new BL.Xml.Core.FileInfo
        {
            Id = fileElementNumber,
            AdmId = amdIdAttributeValue,
            // ReSharper disable once ConditionIsAlwaysTrueOrFalse
            LocType = isExternalContentContentFile ? "OTHER" : "URL",
            // ReSharper disable once ConditionIsAlwaysTrueOrFalse
            OtherLocType = isExternalContentContentFile ? "EXTERNALCONTENT" : String.Empty,   // if there is no content, data is external
                                                                                              // ReSharper disable once ConditionIsAlwaysTrueOrFalse
            HRef = isExternalContentContentFile ? "Content url to be inserted here" : contentFileUrl
        };

        var fileElement = MetsBuilder.CreateFileElement(fileInfo);

        return fileElement;
    }

    public XElement CreateFilePointerWithArea(int fileElementNumber, string begin, string end)
    {
        var fileIdAttributeValue = $"file{fileElementNumber:00000000}";
        var fptr = new XElement(Namespaces.Mets + "fptr");

        var area = new XElement(Namespaces.Mets + "area",
            new XAttribute("FILEID", fileIdAttributeValue),
            new XAttribute("BETYPE", "SMPTE-25"),
            new XAttribute("BEGIN", begin),
            new XAttribute("END", end));
        fptr.Add(area);

        return fptr;
    }

    /// <summary>
    /// Creates the premis events for the validation reports
    /// </summary>
    /// <param name="fileElementNumber">The integer to be used as the numeric part of the FileID attribute</param>
    /// <returns>AN XElement representing the file pointer</returns>
    /*public XDocument CreateValidationPremisEvents(XDocument metsFile, List<ValidationReport> validationReports)
    {
        var AgentType = "Software";
        var IdentifierType = "local";
        var _eventType = "validation";

        foreach (var validationReport in validationReports)
        {
            var amdSecElement = metsFile.Descendants()
                                 .Where(e => e.Name.LocalName == "amdSec")
                                 .Single(
                                     e =>
                                     e.Descendants()
                                      .Where(d => d.Name.LocalName == "objectIdentifierValue")
                                      .Any(a => a.Value == validationReport.ContentFileUid));

            var amdSecIdAttribute = amdSecElement.Attributes().Single(a => a.Name.LocalName == "ID");


            //Create a lookup for the key last value pair in use for the digiprovMD Id attribute of the selected amdSec
            //e.g. KEY: object Value: 01 or KEY: Event VALUE:004 etc
            var ids =
                amdSecElement.Elements()
                      .Where(e => e.Name.LocalName == "digiprovMD")
                      .Attributes()
                      .Where(a => a.Name.LocalName == "ID")
                      .Select(v => v.Value)
                      .Select(v => v.Split(new[] { "-" }, StringSplitOptions.RemoveEmptyEntries).Last())
                      .Select(
                          v => Regex.Split(v, "([a-z]+)|([0-9]+)")
                                    .Where(r => !string.IsNullOrWhiteSpace(r))
                                    .ToList())
                      .ToLookup(v => v.First(), v => v.LastOrDefault());

            //Increment value determined above or use default starting value for eventId or agentId
            var eventId =
                ids.Where(k => k.Key == "event")
                   .SelectMany(v => v)
                   .Select(Increment)
                   .OrderBy(v => v)
                   .LastOrDefault() ?? "001";

            var agentId =
                ids.Where(k => k.Key == "agent")
                   .SelectMany(v => v)
                   .Select(Increment)
                   .OrderBy(v => v)
                   .LastOrDefault() ?? "001";

            //Create and add the digiProv element for validation event
            var digiProvEventElementIdAttributeValue = String.Format("{0}-event{1}", amdSecIdAttribute.Value, eventId);
            var digiProvEventElement = new XElement(Namespaces.Mets + "digiprovMD", new XAttribute("ID", digiProvEventElementIdAttributeValue));
            var mdWrapInfo = new MdWrapInfo
            {
                MdType = "PREMIS:EVENT",
                OtherMdType = String.Empty,
            };
            var mdWrapElement = MetsBuilder.CreateMdWrapElement(mdWrapInfo);
            digiProvEventElement.Add(mdWrapElement);

            var premisValidationEventInfo = new PremisEventInfo
            {
                EventDateTime = String.Format("{0:s}", DateTime.Now),
                EventType = _eventType,
                EventOutcome = validationReport.Status,
                EventIdentifierType = IdentifierType,
                EventIdentifierValue = string.Format("event{0}", eventId),
                LinkingAgentIdentifierType = IdentifierType,
                LinkingAgentIdentifierValue = string.Format("agent{0}", agentId)
            };

            var digiProvPremisEventElement = PremisBuilder.CreatePremisEventElement(premisValidationEventInfo);
            mdWrapElement.Descendants().Last().Add(digiProvPremisEventElement);
            amdSecElement.Add(digiProvEventElement);

            //Create and add the digiProv element for validation agent
            var digiProvAgentElementIdAttributeValue = String.Format("{0}-agent{1}", amdSecIdAttribute.Value, agentId);
            var digiProvAgentElement = new XElement(Namespaces.Mets + "digiprovMD", new XAttribute("ID", digiProvAgentElementIdAttributeValue));
            mdWrapInfo = new MdWrapInfo
            {
                MdType = "PREMIS:AGENT",
                OtherMdType = String.Empty,
            };

            mdWrapElement = MetsBuilder.CreateMdWrapElement(mdWrapInfo);
            digiProvAgentElement.Add(mdWrapElement);

            var premisValidationAgentInfo = new PremisAgentInfo
            {
                AgentName = validationReport.Agent,
                AgentType = AgentType,
                AgentIdentifierType = IdentifierType,
                AgentIdentifierValue = string.Format("agent{0}", agentId)
            };

            var digiProvValidationAgentElement = PremisBuilder.CreatePremisAgentElement(premisValidationAgentInfo);
            mdWrapElement.Descendants().Last().Add(digiProvValidationAgentElement);
            amdSecElement.Add(digiProvAgentElement);

            //Update the fileElements admid attribute value

            var fileElement = metsFile.Descendants()
                                      .Where(e => e.Name.LocalName == "file")
                                      .SingleOrDefault(e => e.Attribute("ADMID").Value.StartsWith(amdSecIdAttribute.Value));

            if (fileElement == null)
            {
                _logger.LogError(string.Format("Unable to save PREMIS events into mets file because there was no file file Element with admId attribute starting with '{0}'", amdSecIdAttribute.Value));
                return null;
            }

            var fileElementAdmidAttribute = fileElement.Attributes().Single(a => a.Name.LocalName == "ADMID");
            var fileElementAdmidAttributeValue = fileElementAdmidAttribute.Value;
            fileElementAdmidAttributeValue = String.Format("{0} {1} {2}", fileElementAdmidAttributeValue, digiProvEventElementIdAttributeValue, digiProvAgentElementIdAttributeValue);
            fileElementAdmidAttribute.Value = fileElementAdmidAttributeValue;
        }
        return metsFile;
    }*/

    /// <summary>
    /// Creates a file pointer element
    /// </summary>
    /// <param name="fileElementNumber">The integer to be used as the numeric part of the FileID attribute</param>
    /// <returns>AN XElement representing the file pointer</returns>
    public static XElement CreateFilePointer(int fileElementNumber)
    {
        var fileIdAttributeValue = $"file{fileElementNumber:00000000}";
        return new XElement(Namespaces.Mets + "fptr", new XAttribute("FILEID", fileIdAttributeValue));
    }


    /// <summary>
    /// Create a fileGrp element for a given content file
    /// </summary>
    /// <param name="use">THe value of the fileGrp usage attribute</param>
    /// <returns>An instance of an XElement representing the filegroup element</returns>
    public XElement CreateFileGrp(string use)
    {
        var fileGrpInfo = new FileGrpInfo
        {
            Use = use
        };

        //the CreateFileSecElement actually produces a file group element when called with the optional second parameter set to false the default value
        var fileGrpElement = MetsBuilder.CreateFileSecElement(fileGrpInfo);

        return fileGrpElement;
    }

    /// <summary>
    /// Creates an empty fileSec element
    /// </summary>
    /// <returns>An XElement representing the fileSec elemnt</returns>
    private static XElement CreateFileSecElement()
    {
        return new XElement(Namespaces.Mets + "fileSec");
    }

    public static string Increment(string last)
    {
        var pos = last.IndexOfAny("0123456789".ToCharArray());

        var id = int.Parse(last.Substring(pos)) + 1;

        return id.ToString("D3");
    }

    private List<string> MintArks(int count)
    {
        List<string> arks = new List<string>();
        try
        {
            arks = _arkMinter.GetArkList(count);
        }
        catch (Exception ex)
        {
            throw new Exception($"System Error. Could not mint arks", ex);
        }
        return arks;
    }

    private static XElement CreateLogicalDiv(int logicalId, int? dmdId, string ark, string type)
    {
        var logicalStructMapDivInfo = new StructMapDivInfo
        {
            Id = $"log{logicalId:00000000}",
            ContentIds = ark,
            Type = type
        };

        if (dmdId.HasValue)
        {
            logicalStructMapDivInfo.DmdId = $"dmd{dmdId:00000000}";
        }

        var div = MetsBuilder.CreateStructMapDivElement(logicalStructMapDivInfo);

        return div;
    }

    private static int GenerateId(XContainer doc, string element, string prefix)
    {
        var biggestId = doc.Descendants()
            .Where(x => x.Name.LocalName == element && x.Attributes().Any(a => a.Name == "ID" && a.Value.StartsWith(prefix))).Max(y => y.Attribute("ID").Value);

        if (biggestId == null)
        {
            return 1;
        }

        var biggestSectionId = biggestId.Replace(prefix, string.Empty);
        var biggestSectionNumber = int.Parse(biggestSectionId);
        biggestSectionNumber++;
        return biggestSectionNumber;
    }
    private static int GenerateNewValueId(XContainer doc, string element, string prefix)
    {
        var biggestId = doc.Descendants()
            .Where(x => x.Name.LocalName == element && x.Value.StartsWith(prefix)).Max(y => y.Value);

        if (biggestId == null)
        {
            return 1;
        }

        var biggestSectionId = biggestId.Replace(prefix, string.Empty);
        var biggestSectionNumber = int.Parse(biggestSectionId);
        biggestSectionNumber++;
        return biggestSectionNumber;
    }

    /// <summary>
    /// Create a new Agent premis element
    /// </summary>
    /// <param name="newId">the new incremented id for the agent element</param>
    /// <param name="agentElement">the element from the transform</param>
    /// <returns></returns>
    private static XElement CreateAgentDigiProvElement(string newId, XElement agentElement)
    {
        var digiProvRoot = MetsBuilder.CreateDigiProvenanceMetadataElement(newId, "PREMIS:AGENT");
        digiProvRoot.Elements().First().Elements().First().Add(agentElement);
        return digiProvRoot;
    }

    private static XElement CreatePremisLinkingAgentIdentifierElement(string linkingAgentIdentifierType, string linkingAgentIdentifierValue)
    {
        return new XElement(Namespaces.Premis + "linkingAgentIdentifier",
        new XElement(Namespaces.Premis + "linkingAgentIdentifierType", linkingAgentIdentifierType),
            new XElement(Namespaces.Premis + "linkingAgentIdentifierValue", linkingAgentIdentifierValue));
    }

    /// <summary>
    /// Creates a new Premis event element  
    /// </summary>
    /// <param name="newId">the new incremented id for the event element</param>
    /// <param name="eventElement">the element from the transform</param>
    /// <returns></returns>
    private static XElement CreateEventDigiProvElement(string newId, XElement eventElement)
    {
        var digiProvRoot = MetsBuilder.CreateDigiProvenanceMetadataElement(newId, "PREMIS:EVENT");
        digiProvRoot.Elements().First().Elements().First().Add(eventElement);
        return digiProvRoot;
    }

    public static void AddDroidPremisEvents(XElement mets, Dictionary<string, MetsEntry> files, string version, string sigFile)
    {
        foreach (var f in files)
        {
            var amdId = f.Value.AmdId.Substring(0, f.Value.AmdId.IndexOf("-", StringComparison.Ordinal));

            var amdSec = mets.Descendants().SingleOrDefault(d =>
                d.Name == Namespaces.Mets + "amdSec" &&
                d.Attributes().Any(a => a.Name == "ID" && a.Value == amdId));

            if (amdSec == null)
            {
                throw new ArgumentException($"amdSec with ID:{amdId} for file:{f.Value.Url} not found.");
            }

            var digiEventId = GenerateId(mets, "digiprovMD", $"{amdId}-event");
            var digiAgentId = GenerateId(mets, "digiprovMD", $"{amdId}-agent");

            var agentId1 = GenerateNewValueId(amdSec, "agentIdentifierValue", "agent");
            var premisAgent1 = PremisBuilder.CreatePremisAgentElement(new PremisAgentInfo
            {
                AgentIdentifierType = "local",
                AgentIdentifierValue = $"agent{agentId1:00}",
                AgentName = $"DROID;{version}",
                AgentType = "software"
            });
            var digi1 = CreateAgentDigiProvElement($"{amdId}-agent{digiAgentId:00}", premisAgent1);
            digiAgentId++;
            amdSec.Add(digi1);

            var agentId2 = GenerateNewValueId(amdSec, "agentIdentifierValue", "agent");
            var premisAgent2 = PremisBuilder.CreatePremisAgentElement(new PremisAgentInfo
            {
                AgentIdentifierType = "local",
                AgentIdentifierValue = $"agent{agentId2:00}",
                AgentName = $"DROID;{sigFile}",
                AgentType = "software"
            });
            var digi2 = CreateAgentDigiProvElement($"{amdId}-agent{digiAgentId:00}", premisAgent2);
            amdSec.Add(digi2);

            var eventId = GenerateNewValueId(amdSec, "eventIdentifierValue", "event");
            var premisEvent = PremisBuilder.CreatePremisEventElement(new PremisEventInfo
            {
                EventIdentifierType = "local",
                EventIdentifierValue = $"event{eventId:00}",
                EventType = "format identification",
                EventDateTime = $"{DateTime.Now:s}",
                EventOutcome = "success",
                LinkingAgentIdentifierType = "local",
                LinkingAgentIdentifierValue = $"agent{agentId1:00}"
            });
            premisEvent.Add(CreatePremisLinkingAgentIdentifierElement("local", $"agent{agentId2:00}"));
            var digi3 = CreateEventDigiProvElement($"{amdId}-event{digiEventId:00}", premisEvent);
            amdSec.Add(digi3);


            var file = mets.Descendants().SingleOrDefault(d =>
                d.Name == Namespaces.Mets + "file" &&
                d.Attributes().Any(a => a.Name == "ADMID" && a.Value.StartsWith(amdId)));

            if (file == null)
            {
                throw new Exception($"file element not found for ADMID {amdId}");
            }

            var admidAttribute = file.Attributes().SingleOrDefault(a => a.Name == "ADMID");

            if (admidAttribute == null)
            {
                throw new Exception($"ADMID attribute not found on file element for {f.Value.Url}");
            }

            admidAttribute.Value += $" {amdId}-agent{agentId1:00} {amdId}-agent{agentId2:00} {amdId}-event{eventId:00}";
        }
    }

    public static bool CheckIfElementHasBeenAdded(XDocument elementXml, XDocument docXml)
    {
        XElement node = elementXml.Descendants().ElementAt(0);
        XNodeEqualityComparer comparer = new XNodeEqualityComparer();
        return docXml.Descendants().Any(element => comparer.Equals(node, element));
    }

    public static void AddMediaMdPremisEvent(XElement mets, MetsEntry file, string version)
    {
        var amdId = file.AmdId.Substring(0, file.AmdId.IndexOf("-", StringComparison.Ordinal));

        var amdSec = mets.Descendants().SingleOrDefault(d =>
            d.Name == Namespaces.Mets + "amdSec" &&
            d.Attributes().Any(a => a.Name == "ID" && a.Value == amdId));

        if (amdSec == null)
        {
            throw new ArgumentException($"amdSec with ID:{amdId} for file:{file.Url} not found.");
        }

        var agentId = GenerateNewValueId(amdSec, "agentIdentifierValue", "agent");
        var digiAgentId = GenerateId(mets, "digiprovMD", $"{amdId}-agent");
        var premisAgent1 = PremisBuilder.CreatePremisAgentElement(new PremisAgentInfo
        {
            AgentIdentifierType = "local",
            AgentIdentifierValue = $"agent{agentId:00}",
            AgentName = $"MediaInfo;{version}",
            AgentType = "software"
        });
        var digi1 = CreateAgentDigiProvElement($"{amdId}-agent{digiAgentId:00}", premisAgent1);
        amdSec.Add(digi1);

        var eventId1 = GenerateNewValueId(amdSec, "eventIdentifierValue", "event");
        var digiEventId = GenerateId(mets, "digiprovMD", $"{amdId}-event");
        var premisEvent = PremisBuilder.CreatePremisEventElement(new PremisEventInfo
        {
            EventIdentifierType = "local",
            EventIdentifierValue = $"event{eventId1:00}",
            EventType = "characterisation",
            EventDateTime = $"{DateTime.Now:s}",
            EventOutcome = "success",
            LinkingAgentIdentifierType = "local",
            LinkingAgentIdentifierValue = $"agent{agentId:00}"
        });
        var digi2 = CreateEventDigiProvElement($"{amdId}-event{digiEventId:00}", premisEvent);
        amdSec.Add(digi2);


        var eventId2 = GenerateNewValueId(amdSec, "eventIdentifierValue", "event");
        digiEventId = GenerateId(mets, "digiprovMD", $"{amdId}-event");
        premisEvent = PremisBuilder.CreatePremisEventElement(new PremisEventInfo
        {
            EventIdentifierType = "local",
            EventIdentifierValue = $"event{eventId2:00}",
            EventType = "profile validation",
            EventDateTime = $"{DateTime.Now:s}",
            EventOutcome = "success",
            LinkingAgentIdentifierType = "local",
            LinkingAgentIdentifierValue = $"agent{agentId:00}"
        });
        var digi3 = CreateEventDigiProvElement($"{amdId}-event{digiEventId:00}", premisEvent);
        amdSec.Add(digi3);


        var fileElement = mets.Descendants().SingleOrDefault(d =>
            d.Name == Namespaces.Mets + "file" &&
            d.Attributes().Any(a => a.Name == "ADMID" && a.Value.StartsWith(amdId)));

        if (file == null)
        {
            throw new Exception($"file element not found for ADMID {amdId}");
        }

        var admidAttribute = fileElement.Attributes().SingleOrDefault(a => a.Name == "ADMID");

        if (admidAttribute == null)
        {
            throw new Exception($"ADMID attribute not found on file element for {file.Url}");
        }

        admidAttribute.Value += $" {amdId}-agent{agentId:00} {amdId}-event{eventId1:00} {amdId}-event{eventId2:00}";
    }

    /// <summary>
    /// Gets the value of the TYPE attribute for the logical div based on the Mime of the file
    /// TODO: Probably a better way of mapping this long term...
    /// </summary>
    /// <param name="MimeType"></param>
    /// <returns></returns>
    private static string GetDivTypeFromMime(string MimeType)
    {
        switch (MimeType)
        {
            case "application/pdf":
                return "Monograph";
            case "text/plain":
                return "Text";
            case "image/jpeg":
                return "Illustration";
            default:
                return "Error MimeType Not Found";
        }
    }
}
