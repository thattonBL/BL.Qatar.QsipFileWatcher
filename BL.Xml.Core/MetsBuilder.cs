using System;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace BL.Xml.Core
{
    public static class MetsBuilder
    {
        /// <summary>
        /// Create the Mets XDocument shell.
        /// </summary>
        /// <returns>A new XDocument.</returns>
        public static XDocument CreateMetsStructure()
        {
            return new XDocument(new XDeclaration("1.0", "utf-8", "yes"));
        }

        /// <summary>
        /// Create the mets XElement with the namespaces and a schemaLocation attribute on it for a given content stream.
        /// </summary>
        /// <param name="contentStream">The content stream to use.</param>
        /// <param name="namespaceDeclarationInfo">Contains the xml namespaces to be declared</param>
        /// <param name="schemaLocationMappingInfo">
        /// Contains the schema location hints to used within the schemaLocation attribute
        /// value
        /// </param>
        /// <returns>The newly created XElement.</returns>
        public static XElement CreateMetsElementAndNamespacesAndSchemaLocations(
            string contentStream,
            NamespaceDeclarationInfo namespaceDeclarationInfo = null,
            SchemaLocationMappingInfo schemaLocationMappingInfo = null)
        {
            var metsElement = new XElement(Namespaces.Mets + "mets");

            if (namespaceDeclarationInfo != null && namespaceDeclarationInfo.NamespaceDeclarations.Any())
            {
                metsElement.Add(new XAttribute(XNamespace.Xmlns + "xsi", "http://www.w3.org/2001/XMLSchema-instance"));
                foreach (var namespacePrefixIdentifier in namespaceDeclarationInfo.NamespaceDeclarations)
                {
                    metsElement.Add(new XAttribute(XNamespace.Xmlns + namespacePrefixIdentifier.Key, namespacePrefixIdentifier.Value));
                }
            }

            if (schemaLocationMappingInfo != null && schemaLocationMappingInfo.SchemaLocationMappings.Any())
            {
                var builderSchemaLocationAttributeValue = new StringBuilder();
                foreach (var namespaceIdentifierSchemaLocation in schemaLocationMappingInfo.SchemaLocationMappings)
                {
                    builderSchemaLocationAttributeValue.Append(string.Format("{0} {1} ", namespaceIdentifierSchemaLocation.Key, namespaceIdentifierSchemaLocation.Value));
                }
                metsElement.Add(new XAttribute(Namespaces.Xsie + "schemaLocation", builderSchemaLocationAttributeValue.ToString().Trim()));
            }

            metsElement.Add(new XAttribute("TYPE", contentStream));

            return metsElement;
        }

        /// <summary>
        /// Create the MetsHdr XElement with the supplied data values.
        /// </summary>
        /// <param name="metsHdrInfo">Class holding the necessary data.</param>
        /// <returns>The newly created XElement.</returns>
        public static XElement CreateMetsHdrElement(MetsHdrInfo metsHdrInfo)
        {
            var metsHdrElement = new XElement(Namespaces.Mets + "metsHdr", new XAttribute("CREATEDATE", FormatDateNowAsW3Cdtf()));
            if (!String.IsNullOrEmpty(metsHdrInfo.AdmId))
            {
                metsHdrElement.SetAttributeValue("ADMID", metsHdrInfo.AdmId);
            }

            var agentNameElement = new XElement(Namespaces.Mets + "name", metsHdrInfo.AgentName ?? String.Empty);
            var agentElement = new XElement(Namespaces.Mets + "agent",
                new XAttribute("ROLE", "CREATOR"),
                new XAttribute("TYPE", "OTHER"),
                new XAttribute("OTHERTYPE", "SOFTWARE"), agentNameElement);

            var documentIdElement = new XElement(Namespaces.Mets + "metsDocumentID", metsHdrInfo.DocumentId ?? String.Empty);

            metsHdrElement.Add(agentElement);
            metsHdrElement.Add(documentIdElement);

            return metsHdrElement;
        }

        /// <summary>
        /// Create the structMap XElement.
        /// </summary>
        /// <param name="structMapInfo">Contains the data for adding this section.</param>
        /// <returns>The newly created XElement.</returns>
        public static XElement CreateStructMapElement(StructMapInfo structMapInfo)
        {
            if (structMapInfo == null)
            {
                throw new ArgumentNullException("structMapInfo");
            }

            ValidatorHelperMethods.ValidateString(structMapInfo.Type, "structMapInfo.Type");

            var structMapElement =
                new XElement(Namespaces.Mets + "structMap",
                    new XAttribute("TYPE", structMapInfo.Type));

            return structMapElement;
        }

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
                    new XElement(Namespaces.Mets + "mdWrap", new XAttribute("MDTYPE", dmdSecInfo.MdType),
                        new XElement(Namespaces.Mets + "xmlData")));

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

        /// <summary>
        /// Create the fileSec XElement.
        /// </summary>
        /// <param name="fileGrpInfo">Contains the data for adding a fileGrp to this section.</param>
        /// <param name="createFileSecElement">If true, create the parent file sec XElement.</param>
        /// <remarks>
        /// Again, the createFileSec XElement is to allow for the fact that there is no single scenaro.
        /// So, it is possible that we can create a fileGrp, but it does not need an enclosing fileSec
        /// element. Adding USE="Image" is an example as this sec is a child of USE="Original"
        /// </remarks>
        /// <returns>The newly created XElement</returns>
        public static XElement CreateFileSecElement(FileGrpInfo fileGrpInfo, bool createFileSecElement = false)
        {
            if (fileGrpInfo == null)
            {
                throw new ArgumentNullException("fileGrpInfo");
            }

            ValidatorHelperMethods.ValidateString(fileGrpInfo.Use, "fileGrpInfo.Use");

            if (!createFileSecElement)
            {
                return new XElement(Namespaces.Mets + "fileGrp", new XAttribute("USE", fileGrpInfo.Use));
            }

            var fileSecElement =
                new XElement(Namespaces.Mets + "fileSec",
                    new XElement(Namespaces.Mets + "fileGrp", new XAttribute("USE", fileGrpInfo.Use)));

            return fileSecElement;
        }

        /// <summary>
        /// Create the fileSec xelement.
        /// </summary>
        /// <param name="fileInfo">Contains the data for adding this section.</param>
        /// <returns>The newly created xelement.</returns>
        public static XElement CreateFileElement(FileInfo fileInfo)
        {
            if (fileInfo == null)
            {
                throw new ArgumentNullException("fileInfo");
            }

            ValidatorHelperMethods.ValidateIntUpperAndLower(fileInfo.Id, "fileInfo.Id", 99999999, 0);
            ValidatorHelperMethods.ValidateString(fileInfo.AdmId, "fileInfo.AdmId");
            ValidatorHelperMethods.ValidateString(fileInfo.LocType, "fileInfo.LocType");
            ValidatorHelperMethods.ValidateString(fileInfo.HRef, "fileInfo.HRef");

            var idString = String.Format("file{0:00000000}", fileInfo.Id);

            var fileElement =
                String.IsNullOrEmpty(fileInfo.OtherLocType) ?
                    new XElement(Namespaces.Mets + "file",
                        new XAttribute("ID", idString),
                        new XAttribute("ADMID", fileInfo.AdmId),
                        new XElement(Namespaces.Mets + "FLocat",
                            new XAttribute("LOCTYPE", fileInfo.LocType),
                            new XAttribute(Namespaces.Xlink + "href", fileInfo.HRef)))
                    : new XElement(Namespaces.Mets + "file",
                        new XAttribute("ID", idString),
                        new XAttribute("ADMID", fileInfo.AdmId),
                        new XElement(Namespaces.Mets + "FLocat",
                            new XAttribute("LOCTYPE", fileInfo.LocType),
                            new XAttribute("OTHERLOCTYPE", fileInfo.OtherLocType),
                            new XAttribute(Namespaces.Xlink + "href", fileInfo.HRef)));

            return fileElement;
        }

        /// <summary>
        /// Create the structMap div XElement.
        /// </summary>
        /// <param name="structMapDivInfo">Contains the data for adding this section.</param>
        /// <returns>The newly created XElement.</returns>
        public static XElement CreateStructMapDivElement(StructMapDivInfo structMapDivInfo)
        {
            if (structMapDivInfo == null)
            {
                throw new ArgumentNullException("structMapDivInfo");
            }

            ValidatorHelperMethods.ValidateString(structMapDivInfo.Id, "structMapDivInfo.Id");
            ValidatorHelperMethods.ValidateString(structMapDivInfo.Type, "structMapDivInfo.Type");

            // TODO: Not sure if the other attribute values can be missing
            var structMapDivElement = new XElement(Namespaces.Mets + "div");
            structMapDivElement.SetAttributeValue("ID", structMapDivInfo.Id);
            structMapDivElement.SetAttributeValue("CONTENTIDS", structMapDivInfo.ContentIds);
            structMapDivElement.SetAttributeValue("TYPE", structMapDivInfo.Type);

            // Only add this element if it is specified
            if (!String.IsNullOrEmpty(structMapDivInfo.DmdId))
            {
                structMapDivElement.SetAttributeValue("DMDID", structMapDivInfo.DmdId);
            }

            // Only add this element if it is specified
            if (!String.IsNullOrEmpty(structMapDivInfo.AdmId))
            {
                structMapDivElement.SetAttributeValue("ADMID", structMapDivInfo.AdmId);
            }

            return structMapDivElement;
        }

        /// <summary>
        /// Create digiProv metadata element with no content
        /// </summary>
        /// <param name="idValue">The id of the digiProve Elemnt</param>
        /// <param name="mdTypeValue">The value of the mdWraps element's mdType attibute</param>
        /// <returns>An instance of an XElement representing the digIProve Element.</returns>
        public static XElement CreateDigiProvenanceMetadataElement(string idValue, string mdTypeValue)
        {
            var digiProvMdElement = new XElement(Namespaces.Mets + "digiprovMD", new XAttribute("ID", idValue));
            var mdWrapInfo = new MdWrapInfo
            {
                MdType = mdTypeValue,

            };
            var mdWrapElement = CreateMdWrapElement(mdWrapInfo);
            digiProvMdElement.Add(mdWrapElement);
            return digiProvMdElement;
        }

        /// <summary>
        /// Creates a wrap XElement based on a mdWrapInfo object.
        /// </summary>
        /// <param name="mdWrapInfo">Contains the data for this section.</param>
        /// <remarks>
        /// Generally used adding Amd Secs for Images or Alto files as there is likely to
        /// be a Premis object as a sub-element and this is generated in PremisBuilder.
        /// There doesn't seem to be a rule for all situations that will allow this method to be automatically
        /// called from something else.
        /// </remarks>
        /// <returns>A new mdWrap XElement.</returns>
        public static XElement CreateMdWrapElement(MdWrapInfo mdWrapInfo)
        {
            if (mdWrapInfo == null)
            {
                throw new ArgumentNullException("mdWrapInfo");
            }

            var mdWrapElement = new XElement(Namespaces.Mets + "mdWrap", new XAttribute("MDTYPE", mdWrapInfo.MdType));
            if (mdWrapInfo.MdType == "OTHER" && !String.IsNullOrEmpty(mdWrapInfo.OtherMdType))
            {
                mdWrapElement.SetAttributeValue("OTHERMDTYPE", mdWrapInfo.OtherMdType);
            }
            var xmlDataElement = new XElement(Namespaces.Mets + "xmlData");

            mdWrapElement.Add(xmlDataElement);
            return mdWrapElement;
        }

        /// <summary>
        /// Create a generic amd sec XElement.
        /// </summary>
        /// <param name="amdSecInfo">The parameters used for the section.</param>
        /// <returns>A new AmdSec XElement</returns>
        public static XElement CreateAmdSecElement(AmdSecInfo amdSecInfo)
        {
            if (amdSecInfo == null)
            {
                throw new ArgumentNullException("amdSecInfo");
            }

            ValidatorHelperMethods.ValidateIntUpperAndLower(amdSecInfo.Id, "amdSecInfo.Id", 99999999, 0);

            var idString = String.Format("amd{0:00000000}", amdSecInfo.Id);
            var amdSecElement = new XElement(Namespaces.Mets + "amdSec", new XAttribute("ID", idString));

            // If there is an Md values specified, create an element to go with it
            if (!String.IsNullOrEmpty(amdSecInfo.Md))
            {
                if (String.IsNullOrEmpty(amdSecInfo.MdId))
                {
                    throw new ArgumentException("amdSecInfo.MdId cannot be empty or missing if amdSecInfo.Md is not", "amdSecInfo");
                }
                var sourceMdElement = new XElement(Namespaces.Mets + amdSecInfo.Md, new XAttribute("ID", amdSecInfo.MdId));
                amdSecElement.Add(sourceMdElement);
            }

            return amdSecElement;
        }

        /// <summary>
        /// Get the DateTime.Now formatted for the MODS.
        /// </summary>
        /// <returns>DateTime.Now correctly formatted.</returns>
        public static string FormatDateNowAsW3Cdtf()
        {
            return FormatDateAsW3Cdtf(DateTime.Now);
        }

        /// <summary>
        /// Get the supplied date formatted for the MODS.
        /// </summary>
        /// <param name="dateToFormat">The date to be formatted.</param>
        /// <returns>The correctly formatted date.</returns>
        public static string FormatDateAsW3Cdtf(DateTime dateToFormat)
        {
            return dateToFormat.ToString("yyyy-MM-ddTHH:mm:ssZ");
        }
    }
}
