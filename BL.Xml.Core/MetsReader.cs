using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Xml.Linq;

namespace BL.Xml.Core
{
    /// <summary>
    /// Generic methods for reading a mets file
    /// </summary>
    public static class MetsReader
    {
        #region Find XElement by name

        /// <summary>
        /// Find an XElement by name that is a child of the given xElementToSearch or the xElementToSearch itself
        /// </summary>
        /// <param name="xElementToSearch">The XElement to search in</param>
        /// <param name="namespace">The Namespace of the XElement to search for</param>
        /// <param name="elementName">The name of the XElement to search for</param>
        /// <returns>The found XElement (or null if not found)</returns>
        public static XElement FindElementByName(XElement xElementToSearch, XNamespace @namespace, string elementName)
        {
            ValidatorHelperMethods.ValidateXElement(xElementToSearch, "xElementToSearch");
            ValidatorHelperMethods.ValidateXNamespace(@namespace, "namespace");
            ValidatorHelperMethods.ValidateString(elementName, "elementName");

            return (from e in xElementToSearch.DescendantsAndSelf()
                where e.Name == @namespace + elementName
                select e).FirstOrDefault();
        }

        /// <summary>
        /// Find an XElement by name that is in xDocumentToSearch
        /// </summary>
        /// <param name="xDocumentToSearch">The XDocument to search in</param>
        /// <param name="namespace">The Namespace of the XElement to search for</param>
        /// <param name="elementName">The name of the XElement to search for</param>
        /// <returns>The found XElement (or null if not found)</returns>
        public static XElement FindElementByName(XDocument xDocumentToSearch, XNamespace @namespace, string elementName)
        {
            ValidatorHelperMethods.ValidateXDocument(xDocumentToSearch, "xDocumentToSearch");
            return FindElementByName(xDocumentToSearch.Root, @namespace, elementName);
        }

        #endregion

        #region Find XElements by name

        /// <summary>
        /// Find a list of XElements by name that are children of xElementToSearch including the element itself
        /// </summary>
        /// <param name="xElementToSearch">The XElement to search in</param>
        /// <param name="namespace">The Namespace of the XElement to search for</param>
        /// <param name="elementName">The name of the XElement to search for</param>
        /// <returns>A list of found XElements (empty list if not found)</returns>
        public static List<XElement> FindElementsByName(XElement xElementToSearch, XNamespace @namespace,
            string elementName)
        {
            ValidatorHelperMethods.ValidateXElement(xElementToSearch, "xElementToSearch");
            ValidatorHelperMethods.ValidateXNamespace(@namespace, "namespace");
            ValidatorHelperMethods.ValidateString(elementName, "elementName");

            return (from e in xElementToSearch.DescendantsAndSelf()
                where e.Name == @namespace + elementName
                select e).ToList();
        }

        /// <summary>
        /// Find a list of XElements by name that are children of xDocumentToSearch
        /// </summary>
        /// <param name="xDocumentToSearch">The XDocument you want to search in</param>
        /// <param name="namespace">The Namespace of the XElement to search for</param>
        /// <param name="elementName">The Name of the XElement to search for</param>
        /// <returns>A list of found XElements (empty list if not found)</returns>
        public static List<XElement> FindElementsByName(XDocument xDocumentToSearch, XNamespace @namespace,
            string elementName)
        {
            ValidatorHelperMethods.ValidateXDocument(xDocumentToSearch, "xDocumentToSearch");
            return FindElementsByName(xDocumentToSearch.Root, @namespace, elementName);
        }

        #endregion

        #region Find XAttribute by name

        /// <summary>
        /// Find the first XAttribute by name that is part of xElementToSearch or the elements children
        /// </summary>
        /// <param name="xElementToSearch">The XElement to search in</param>
        /// <param name="attributeName">The name of the XAttribute to look for</param>
        /// <returns>The found XAttribute (null if not found)</returns>
        /// <remarks>The namespace is NOT supplied and is ignored. So, an XAttribute with name "hRef" will match "xlink:hRef"</remarks>
        public static XAttribute FindAttributeByName(XElement xElementToSearch, string attributeName)
        {
            ValidatorHelperMethods.ValidateXElement(xElementToSearch, "xElementToSearch");
            ValidatorHelperMethods.ValidateString(attributeName, "attributeName");

            // This is done in two stages to aid with debugging
            var attributeList = (from x in xElementToSearch.DescendantsAndSelf().Attributes()
                select x).ToList();

            // return attributeList.FirstOrDefault();
            return attributeList.FirstOrDefault(x => x.Name.LocalName == attributeName);
        }

        /// <summary>
        /// Find the first XAttribute by name that is part of a child within the given XDocument
        /// </summary>
        /// <param name="xDocumentToSearch">The XDocument you want to search in</param>
        /// <param name="attributeName">The Name of the XXAttribute to look for</param>
        /// <returns>The found XAttribute (null if not found)</returns>
        /// <remarks>The namespace is NOT supplied and is ignored. So, "hRef" will match "xlink:hRef"</remarks>
        public static XAttribute FindAttributeByName(XDocument xDocumentToSearch, string attributeName)
        {
            ValidatorHelperMethods.ValidateXDocument(xDocumentToSearch, "xDocumentToSearch");
            return FindAttributeByName(xDocumentToSearch.Root, attributeName);
        }

        #endregion

        #region Find XElement by XAttribute name and value

        /// <summary>
        /// Find an XElement that has an XAttribute with the given name and value. The XElement will
        /// be a child of the xElementToSearch or the xElementToSearch itself
        /// </summary>
        /// <param name="xElementToSearch">The XElement to search in</param>
        /// <param name="attributeName">The Name of the XElement to search for</param>
        /// <param name="attributeValue">The value of the XElement to search for</param>
        /// <returns>The found XElement (or null if not found)</returns>
        public static XElement FindElementByAttributeNameAndValue(
            XElement xElementToSearch,
            string attributeName,
            string attributeValue)
        {
            ValidatorHelperMethods.ValidateXElement(xElementToSearch, "xElementToSearch");
            ValidatorHelperMethods.ValidateString(attributeName, "attributeName");
            ValidatorHelperMethods.ValidateString(attributeValue, "attributeValue");

            return (from e in xElementToSearch.DescendantsAndSelf()
                where e.Attributes().FirstOrDefault(
                          a => a.Name.LocalName == attributeName &&
                               a.Value.Equals(attributeValue, StringComparison.CurrentCultureIgnoreCase)) != null
                select e).FirstOrDefault();
        }

        /// <summary>
        /// Find an XElement that has an XAttribute with the given name and value. The XElement will
        /// be contained in xDocumentToSearch
        /// </summary>
        /// <param name="xDocumentToSearch">The XDocument to search in</param>
        /// <param name="attributeName">The Name of the XElement to search for</param>
        /// <param name="attributeValue">The value of the XElement to search for</param>
        /// <returns>The found XElement (or null if not found)</returns>
        public static XElement FindElementByAttributeNameAndValue(
            XDocument xDocumentToSearch,
            string attributeName,
            string attributeValue)
        {
            ValidatorHelperMethods.ValidateXDocument(xDocumentToSearch, "xDocumentToSearch");

            return FindElementByAttributeNameAndValue(xDocumentToSearch.Root, attributeName, attributeValue);
        }

        #endregion

        #region Find XElements by XAttribute name and value

        /// <summary>
        /// Find an XElement that has an XAttribute with the given name and value. The XElement will
        /// be a child of the xElementToSearch or the xElementToSearch itself
        /// </summary>
        /// <param name="xElementToSearch">The XElement to search in</param>
        /// <param name="attributeName">The name of the XElement to search for</param>
        /// <param name="attributeValue">The value of the XElement to search for</param>
        /// <returns>The found XElement (or null if not found)</returns>
        public static List<XElement> FindElementsByAttributeNameAndValue(
            XElement xElementToSearch,
            string attributeName,
            string attributeValue)
        {
            ValidatorHelperMethods.ValidateXElement(xElementToSearch, "xElementToSearch");
            ValidatorHelperMethods.ValidateString(attributeName, "attributeName");
            ValidatorHelperMethods.ValidateString(attributeValue, "attributeValue");

            return (from e in xElementToSearch.DescendantsAndSelf()
                where e.Attributes().FirstOrDefault(
                          a => a.Name.LocalName == attributeName &&
                               a.Value.Equals(attributeValue, StringComparison.CurrentCultureIgnoreCase)) != null
                select e).ToList();
        }

        /// <summary>
        /// Find an XElement that has an XAttribute with the given name and value. The XElement will
        /// be a child of the xElementToSearch or the xElementToSearch itself
        /// </summary>
        /// <param name="xDocumentToSearch">The XDocument to search in</param>
        /// <param name="attributeName">The name of the XElement to search for</param>
        /// <param name="attributeValue">The value of the XElement to search for</param>
        /// <returns>The found XElement (or null if not found)</returns>
        public static List<XElement> FindElementsByAttributeNameAndValue(
            XDocument xDocumentToSearch,
            string attributeName,
            string attributeValue)
        {
            ValidatorHelperMethods.ValidateXDocument(xDocumentToSearch, "xDocumentToSearch");

            return FindElementsByAttributeNameAndValue(xDocumentToSearch.Root, attributeName, attributeValue);
        }


        /// <summary>
        /// Find a list of XElement that has an XAttribute with the given name and value. The XElement will
        /// be contained in xDocumentToSearch
        /// </summary>
        /// <param name="xDocumentToSearch">The XDocument to search in</param>
        /// <param name="namespace">The namespace to use in the search</param>
        /// <param name="elementName">The element name to search for</param>
        /// <param name="attributeName">The Name of the XElement to search for</param>
        /// <param name="attributeValue">The value of the XElement to search for</param>
        /// <returns>The found XElement (or null if not found)</returns>
        public static List<XElement> FindElementsByElementNameAttributeNameAndValue(
            XDocument xDocumentToSearch,
            XNamespace @namespace,
            string elementName,
            string attributeName,
            string attributeValue)
        {
            ValidatorHelperMethods.ValidateXDocument(xDocumentToSearch, "xDocumentToSearch");

            if (xDocumentToSearch.Root == null)
                return null;

            return (from e in xDocumentToSearch.Root.DescendantsAndSelf()
                where e.Name == @namespace + elementName &&
                      e.Attributes().FirstOrDefault(
                          a => a.Name.LocalName == attributeName &&
                               a.Value.Equals(attributeValue, StringComparison.CurrentCultureIgnoreCase)) != null
                select e).ToList();

        }

        /// <summary>
        /// Find a list of parent XElements from within XDocumentToSearch that have a child of the given name and an XAttribute with the given name and value. 
        /// </summary>
        /// <param name="xDocumentToSearch">The XDocument to search in</param>
        /// <param name="namespace">The namespace to use in the search</param>
        /// <param name="childElementName">The child element name to search for</param>
        /// <param name="attributeName">The name of the attribute to search for</param>
        /// <param name="attributeValue">The value of the attribute to search for</param>
        /// <returns>The list of parent  XElements (or null if not found)</returns>
        public static List<XElement> FindParentElementsByChildElementNameAttributeNameAndValue(
            XDocument xDocumentToSearch,
            XNamespace @namespace,
            string childElementName,
            string attributeName,
            string attributeValue)
        {
            ValidatorHelperMethods.ValidateXDocument(xDocumentToSearch, "xDocumentToSearch");

            if (xDocumentToSearch.Root == null)
                return null;

            return (from e in xDocumentToSearch.Root.DescendantsAndSelf()
                where e.Name == @namespace + childElementName &&
                      e.Attributes().FirstOrDefault(
                          a => a.Name.LocalName == attributeName &&
                               a.Value.Equals(attributeValue, StringComparison.CurrentCultureIgnoreCase)) != null
                select e.Parent).ToList();
        }

        /// <summary>
        /// Find a list of XElements from within XDocumentToSearch that have the given elemnt name and the given child element name
        /// </summary>
        /// <param name="xDocumentToSearch">The XDocument to search in</param>
        /// <param name="namespace">The namespace to use in the search</param>
        /// <param name="elementName">The child element name to search for</param>
        ///  <param name="childElementName">The child element name to search for</param>
        /// <returns>The list of parent  XElements (or null if not found)</returns>
        public static List<XElement> FindElementsByElementNameAndChildElementName(
            XDocument xDocumentToSearch,
            XNamespace @namespace,
            string elementName,
            string childElementName)
        {
            ValidatorHelperMethods.ValidateXDocument(xDocumentToSearch, "xDocumentToSearch");

            if (xDocumentToSearch.Root == null)
                return null;

            return (from e in xDocumentToSearch.Root.DescendantsAndSelf()
                let childElement = e.Elements().FirstOrDefault()
                where childElement != null &&
                      (e.Name == @namespace + elementName &&
                       childElement.Name.LocalName.Equals(childElementName))
                select e).ToList();
        }

        /// <summary>
        /// Find a list of parent XElements from within XDocumentToSearch that have a child elememt with a given name. The XElement will
        /// be contained in xDocumentToSearch
        /// </summary>
        /// <param name="xElementToSearch">The XElement to search in</param>
        /// <param name="namespace">The Namespace of the child XElement to search for</param>
        /// <param name="elementName">The name of the child XElement to search for</param>
        /// <returns>The list of parent  XElements (or null if not found)</returns>
        public static List<XElement> FindParentElementsByChildName(XElement xElementToSearch, XNamespace @namespace,
            string elementName)
        {
            ValidatorHelperMethods.ValidateXElement(xElementToSearch, "xElementToSearch");
            ValidatorHelperMethods.ValidateXNamespace(@namespace, "namespace");
            ValidatorHelperMethods.ValidateString(elementName, "elementName");

            return (from e in xElementToSearch.DescendantsAndSelf()
                where e.Name == @namespace + elementName
                select e.Parent).ToList();
        }

        #endregion

        #region Find XElement by XElement name, XAttribute name and value

        /// <summary>
        /// Find an XElement that has an XAttribute with the given name and value. The XElement will
        /// be a child of the xElementToSearch or the xElementToSearch itself
        /// </summary>
        /// <param name="xElementToSearch">The XElement to search in</param>
        /// <param name="namespace">The Namespace of the XElement to search for</param>
        /// <param name="elementName">The name of the XElement to search for</param>
        /// <param name="attributeName">The name of the XAttribute to search for</param>
        /// <param name="attributeValue">The value of the XAttribute to search for</param>
        /// <returns>The found XElement (or null if not found)</returns>
        public static XElement FindElementByElementNameAttributeNameAndValue(
            XElement xElementToSearch,
            XNamespace @namespace,
            string elementName,
            string attributeName,
            string attributeValue)
        {
            ValidatorHelperMethods.ValidateXElement(xElementToSearch, "xElementToSearch");
            ValidatorHelperMethods.ValidateXNamespace(@namespace, "namespace");
            ValidatorHelperMethods.ValidateString(elementName, "elementName");
            ValidatorHelperMethods.ValidateString(attributeName, "attributeName");
            ValidatorHelperMethods.ValidateString(attributeValue, "attributeValue");

            return (from e in xElementToSearch.DescendantsAndSelf()
                where e.Name == @namespace + elementName &&
                      e.Attributes().FirstOrDefault(
                          a => a.Name.LocalName == attributeName &&
                               a.Value.Equals(attributeValue, StringComparison.CurrentCultureIgnoreCase)) != null
                select e).FirstOrDefault();
        }

        /// <summary>
        /// Find an XElement that has an XAttribute with the given name and value. The XElement will
        /// be a child of xDocumentToSearch
        /// </summary>
        /// <param name="xDocumentToSearch">The XDocument to search in</param>
        /// <param name="namespace">The Namespace of the XElement to search for</param>
        /// <param name="elementName">The name of the XElement to search for</param>
        /// <param name="attributeName">The name of the XAttribute to search for</param>
        /// <param name="attributeValue">The value of the XAttribute to search for</param>
        /// <returns>The found xelement (or null if not found)</returns>
        public static XElement FindElementByElementNameAttributeNameAndValue(
            XDocument xDocumentToSearch,
            XNamespace @namespace,
            string elementName,
            string attributeName,
            string attributeValue)
        {
            ValidatorHelperMethods.ValidateXDocument(xDocumentToSearch, "xDocumentToSearch");

            return FindElementByElementNameAttributeNameAndValue(xDocumentToSearch.Root, @namespace, elementName,
                attributeName, attributeValue);
        }

        #endregion

        #region Miscellaneous Methods



        /// <summary>
        /// Find the struct map XElement that has the specified "TYPE" XAttribute
        /// </summary>
        /// <param name="xDocumentToSearch">The XDocument to search in</param>
        /// <param name="typeAttributeValue">The "TYPE" XAttribute value to look for</param>
        /// <returns>The found XElement for the required struct map (null if not found)</returns>
        public static XElement GetStructMapTypeElement(XDocument xDocumentToSearch, string typeAttributeValue)
        {
            return FindElementByElementNameAttributeNameAndValue(
                xDocumentToSearch,
                Namespaces.Mets,
                "structMap",
                "TYPE",
                typeAttributeValue
            );
        }

        /// <summary>
        /// Find the struct map XElement that has the specified "USE" XAttribute
        /// </summary>
        /// <param name="xDocumentToSearch">The XDocument to search in</param>
        /// <param name="useAttributeValue">The "USE" XAttribute value to look for</param>
        /// <returns>The found XElement for the required fileGrp (null if not found)</returns>
        public static XElement GetFileGroupUseElement(XDocument xDocumentToSearch, string useAttributeValue)
        {
            return FindElementByElementNameAttributeNameAndValue(
                xDocumentToSearch,
                Namespaces.Mets,
                "fileGrp",
                "USE",
                useAttributeValue
            );
        }

        #endregion

        #region Ark Methods

        /// <summary>
        /// Find an ArkId for a specific amdSec that is a child of xElementToSearch
        /// </summary>
        /// <param name="xElementToSearch">The XElement to search in</param>
        /// <param name="digiprovMdId">The ID of the digiprovMD of the 'amdSec' that contains the ArkId to find</param>
        /// <returns>The found ArkId (null if none found)</returns>
        public static string GetArkIdFromAmdSec(XElement xElementToSearch, string digiprovMdId)
        {
            ValidatorHelperMethods.ValidateXElement(xElementToSearch, "xElementToSearch");
            ValidatorHelperMethods.ValidateString(digiprovMdId, "digiprovMdId");

            var amdSecId = digiprovMdId.Contains("-") ? digiprovMdId.Split('-')[0] : digiprovMdId;

            var amdSec = FindElementByElementNameAttributeNameAndValue(
                xElementToSearch,
                Namespaces.Mets,
                "amdSec",
                "ID",
                amdSecId);

            if (amdSec == null)
            {
                throw new ApplicationException(
                    String.Format("Could not find an amdSec that has an ID of {0} (using {1})", amdSecId,
                        digiprovMdId));
            }

            var objectIdentifierValueElement = FindElementByName(amdSec, Namespaces.Premis, "objectIdentifierValue");
            if (objectIdentifierValueElement == null)
            {
                throw new ApplicationException(String.Format(
                    "Could not find an objectIdentifierValue element for the amdSec {0}", amdSecId, digiprovMdId));
            }

            return (string) objectIdentifierValueElement;
        }

        /// <summary>
        /// Find an ArkId for a specific amdSec contained in xDocumentToSearch
        /// </summary>
        /// <param name="xDocumentToSearch">The XDocument to search in</param>
        /// <param name="digiprovMdId">The ID of the digiprovMD of the 'amdSec' that contains the ArkId to find</param>
        /// <returns>The found ArkId (null if none found)</returns>
        public static string GetArkIdFromAmdSec(XDocument xDocumentToSearch, string digiprovMdId)
        {
            ValidatorHelperMethods.ValidateXDocument(xDocumentToSearch, "xDocumentToSearch");

            return GetArkIdFromAmdSec(xDocumentToSearch.Root, digiprovMdId);
        }

        /// <summary>
        /// Gets the ArkId for a Mets XDocument. This is found in the 'metsHdr' XElement.
        /// </summary>
        /// <param name="xDocumentToSearch">The XDocument to search in</param>
        /// <returns>The ArkId (null if not found)</returns>
        public static string GetMetsArk(XDocument xDocumentToSearch)
        {
            ValidatorHelperMethods.ValidateXDocument(xDocumentToSearch, "xDocumentToSearch");

            // This could be done in one select, but the following just make sure we are getting the right one
            var metsHdrElement = FindElementByName(xDocumentToSearch, Namespaces.Mets, "metsHdr");
            var metsArkId = FindElementByName(metsHdrElement, Namespaces.Mets, "metsDocumentID");

            return metsArkId == null ? null : metsArkId.Value;
        }

        #endregion

        #region Get Content Entries

        /// <summary>
        /// Find MetsEntry information for the Content from xDocumentToSearch
        /// </summary>
        /// <param name="xDocumentToSearch">The XDocument you want to search in</param>
        /// <param name="usage">The value of the "USE" XAttribute of the fileGrp you are looking for</param>
        /// <param name="searchLinkedAmdSec">Indicates whether or not the MetsEntry class is to be built using using the mets:file data 
        /// or from the linked AmdSec</param>
        /// <returns>A dictionary of MetsEntry content files</returns>
        public static Dictionary<string, MetsEntry> GetContentEntries(XDocument xDocumentToSearch, string usage,
            bool searchLinkedAmdSec = false)
        {
            ValidatorHelperMethods.ValidateXDocument(xDocumentToSearch, "xDocumentToSearch");

            // Get the required fileSec "mets:fileGrp USE=usage"
            var fileGrpElement =
                FindElementByElementNameAttributeNameAndValue(xDocumentToSearch, Namespaces.Mets, "fileGrp", "USE",
                    usage);
            if (fileGrpElement == null)
            {
                throw new ApplicationException(String.Format("Could not find a fileGrp element with USE of {0}",
                    usage));
            }

            var fileElements = FindElementsByName(fileGrpElement, Namespaces.Mets, "file");
            if (fileElements.Count == 0)
            {
                throw new ApplicationException(
                    String.Format("Could not find any file elements in the fileGrp with USE of {0}", usage));
            }

            return fileElements.Select(xElement =>
                BuildMetsEntry(xElement, usage, xDocumentToSearch, searchLinkedAmdSec)).ToDictionary(value => value.Id);
        }

        #endregion

        #region Build mets entry

        /// <summary>
        /// Construct a MetsEntry from data in fileElement or from the linked amdSec in xDocumentContainingLinkedAmdSecs
        /// </summary>
        /// <param name="fileElement">This is the file XElement of the XElement in question</param>
        /// <param name="usage">This is used PURELY to populate one of the fields in the MetsEntry</param>
        /// <param name="xDocumentContainingLinkedAmdSecs">If we are linking, this XDocument contains the amd sec to link to</param>
        /// <param name="searchLinkedAmdSec">Shall we get the data from the linked Amd Sec? True if Yes</param>
        /// <returns>An MdEntry containing all of the available data</returns>
        public static MetsEntry BuildMetsEntry(XElement fileElement, string usage,
            XDocument xDocumentContainingLinkedAmdSecs = null, bool searchLinkedAmdSec = false)
        {
            ValidatorHelperMethods.ValidateXElement(fileElement, "fileElement");
            if (searchLinkedAmdSec)
            {
                // If we are searching linked amdSec, then xDocumentContainingLinkedAmdSecs must be valid
                ValidatorHelperMethods.ValidateXDocument(xDocumentContainingLinkedAmdSecs,
                    "xDocumentContainingLinkedAmdSecs");
            }

            // Get the ID attribute value
            var id = (string) fileElement.Attribute("ID");
            if (String.IsNullOrEmpty(id))
            {
                throw new ApplicationException("Cannot find the ID attribute when building a mets entry.");
            }

            // Sanity check the value
            // TODO: Is this necessary?
            var admId = (string) fileElement.Attribute("ADMID") ?? String.Empty;
            admId = admId.Trim();
            if (admId.Contains(" "))
            {
                var splitArray = admId.Split(' ');
                admId = splitArray[0];
            }

            // Find the url (this is in a child XElement and it should ignore the namespace. Therefore, use FindAttributeByName)
            var urlAttribute = FindAttributeByName(fileElement, "href");
            var url = urlAttribute != null ? urlAttribute.Value : String.Empty;

            // Place holder values
            string checksumValue;
            string checkSumType;
            long size;
            string mimeType;
            var arkId = String.Empty;

            if (searchLinkedAmdSec)
            {
                var linkedAmdSecElement =
                    FindElementByAttributeNameAndValue(xDocumentContainingLinkedAmdSecs, "ID", admId);

                var checksumElement = FindElementByName(linkedAmdSecElement, Namespaces.Premis, "messageDigest");
                checksumValue = checksumElement != null ? checksumElement.Value : String.Empty;

                var checkSumTypeElement =
                    FindElementByName(linkedAmdSecElement, Namespaces.Premis, "messageDigestAlgorithm");
                checkSumType = checkSumTypeElement != null ? checkSumTypeElement.Value : String.Empty;

                var sizeAttribute = FindElementByName(linkedAmdSecElement, Namespaces.Premis, "size");
                size = sizeAttribute != null ? Int64.Parse(sizeAttribute.Value) : 0;

                var mimeTypeAttribute = FindElementByName(linkedAmdSecElement, Namespaces.Premis, "formatName");
                mimeType = mimeTypeAttribute != null ? mimeTypeAttribute.Value : String.Empty;

                // ReSharper disable PossibleNullReferenceException
                arkId = GetArkIdFromAmdSec(xDocumentContainingLinkedAmdSecs.Root, admId);
                // ReSharper restore PossibleNullReferenceException
            }
            else
            {
                // The following values are found directly in the supplied fileElement
                // NB Ark is NOT found
                var checksumAttribute = fileElement.Attribute("CHECKSUM");
                checksumValue = checksumAttribute != null ? checksumAttribute.Value : String.Empty;

                var checkSumTypeAttribute = fileElement.Attribute("CHECKSUMTYPE");
                checkSumType = checkSumTypeAttribute != null ? checkSumTypeAttribute.Value : String.Empty;

                var sizeAttribute = fileElement.Attribute("SIZE");
                size = sizeAttribute != null ? Int64.Parse(sizeAttribute.Value) : 0;

                var mimeTypeAttribute = fileElement.Attribute("MIMETYPE");
                mimeType = mimeTypeAttribute != null ? mimeTypeAttribute.Value : String.Empty;
            }

            // Put all of the found attribute values into the MetsEntry return value
            return new MetsEntry
            {
                Id = id,
                Url = url,
                ChecksumValue = checksumValue,
                ArkId = arkId,
                CheckSumType = checkSumType,
                Size = size,
                AmdId = admId,
                FileGroup = usage,
                MimeType = mimeType
            };
        }

        #endregion

        #region Useful parts

        public static XDocument ReadMetsXml(string path)
        {
            if (String.IsNullOrEmpty(path))
                throw new ArgumentException("METS file path not specified");

            if (!File.Exists(path))
                throw new ArgumentException(String.Format("METS file not found at location '{0}'.", path));

            using (var s = File.OpenRead(path))
            {
                return XDocument.Load(s);
            }
        }

        public static XElement GetDlsVersioning(XDocument metsDocument)
        {
            //<metsHdr ADMID="amd999-src001" />
            //<amdSec ID="amd999">
            //  <sourceMD ID="amd999-src001">
            //    <mdWrap>
            //      <xmlData>
            //        <versioningData>
            //          <key>ABC123</key>
            //          <mdhash>XYZ987</mdhash>
            //        </versioningData>
            //      </xmlData>
            //    </mdWrap>
            //  </sourceMD>
            //</amdSec>
            return (
                from h in metsDocument.Descendants()
                    .Where(n => n.Name.LocalName == "metsHdr")
                    .Attributes()
                    .Where(n => n.Name.LocalName == "ADMID")
                    .SelectMany(n => n.Value.Split())
                join a in metsDocument.Descendants().Where(n => n.Name.LocalName == "amdSec")
                        .Elements()
                        .Where(n => n.Name.LocalName == "sourceMD")
                    on h.ToLowerInvariant() equals a.Attributes().Single(n => n.Name.LocalName == "ID")
                        .Value
                where a.Descendants().SingleOrDefault(n => n.Name.LocalName == "versioningData") != null
                select a.Descendants().SingleOrDefault(n => n.Name.LocalName == "versioningData")
            ).SingleOrDefault();

        }

        public static XElement GetMarcXml(XDocument doc)
        {
            if (null == doc)
                throw new ArgumentNullException("doc", "No submission XML document specified.");

            if (null == doc.Root)
                throw new ArgumentNullException("doc", "XML contains no root element.");

            // get DMDIDs for root logical div
            var dmdIds =
                doc.Root.Elements()
                    .Where(n => 0 == String.Compare(n.Name.LocalName, "structMap", StringComparison.OrdinalIgnoreCase))
                    .Where(e => 0 == String.Compare((string) e.Attribute("TYPE"), "logical",
                                    StringComparison.OrdinalIgnoreCase))
                    .Elements()
                    .Single(n => 0 == String.Compare(n.Name.LocalName, "div", StringComparison.OrdinalIgnoreCase))
                    .Attribute("DMDID").Value.Split(' ');

            // get the MARC dmdSecs
            var dmdSecs =
                doc.Root
                    .Elements()
                    .Where(n => 0 == String.Compare(n.Name.LocalName, "dmdSec", StringComparison.OrdinalIgnoreCase));

            var mdWrap = (from d in dmdSecs.Elements()
                    .Where(e => 0 == String.Compare(e.Name.LocalName, "mdWrap", StringComparison.OrdinalIgnoreCase)
                                && e.Attribute("MDTYPE").Value == "MARC")
                let element = d.Parent
                where element != null
                let id = element.Attribute("ID").Value
                where dmdIds.Contains(id)
                select d);

            // get the MARC XML
            return mdWrap.Descendants().FirstOrDefault(e =>
                0 == String.Compare(e.Name.LocalName, "record", StringComparison.OrdinalIgnoreCase));
        }

        public static string GetWorkflowName(XDocument submission)
        {
            return (string) submission.Elements().Single(e => e.Name.LocalName == "mets").Attribute("TYPE");
        }

        public static string GetLArk(XDocument submission)
        {
            return (string) submission
                .Descendants()
                .Where(e => e.Name.LocalName == "structMap")
                .Single(s => ((string) s.Attribute("TYPE")).ToUpperInvariant() == "LOGICAL")
                .Descendants()
                .First(e => e.Name.LocalName == "div")
                .Attribute("CONTENTIDS");
        }

        public static string GetMdArk(XDocument submission)
        {
            return (from l in submission.Descendants().Where(n => n.Name.LocalName == "structMap")
                        .Where(
                            n =>
                                n.Attributes().Select(a => a.Value.ToLower()).SingleOrDefault() == "logical")
                        .Elements()
                        .Where(n => n.Name.LocalName == "div")
                        .Attributes()
                        .Where(a => a.Name.LocalName == "DMDID")
                        .SelectMany(a => a.Value.Split())
                    join d in submission.Descendants().Where(n => n.Name.LocalName == "dmdSec")
                        on l equals (string) d.Attributes().SingleOrDefault(n => n.Name.LocalName == "ID")
                    where d.Elements().Any(n => n.Name.LocalName == "mdRef")
                    select (string) d.Elements().Where(n => n.Name.LocalName == "mdRef")
                        .Attributes()
                        .SingleOrDefault(n => n.Name.LocalName == "href"))
                .SingleOrDefault();
        }

        public static XElement GetMdArkContainer(XDocument submission)
        {
            return (from l in submission.Descendants().Where(n => n.Name.LocalName == "structMap")
                    .Where(
                        n =>
                            n.Attributes().Select(a => a.Value.ToLower()).SingleOrDefault() == "logical")
                    .Elements()
                    .Where(n => n.Name.LocalName == "div")
                    .Attributes()
                    .Where(a => a.Name.LocalName == "DMDID")
                    .SelectMany(a => a.Value.Split())
                join d in submission.Descendants().Where(n => n.Name.LocalName == "dmdSec")
                    on l equals (string) d.Attributes().SingleOrDefault(n => n.Name.LocalName == "ID")
                where d.Elements().Any(n => n.Name.LocalName == "mdRef")
                select d.Elements().SingleOrDefault(n => n.Name.LocalName == "mdRef")).SingleOrDefault();
        }

        #endregion

        public static object MergeConferenceData(object mods)
        {
            throw new NotImplementedException();
        }
    }
}
