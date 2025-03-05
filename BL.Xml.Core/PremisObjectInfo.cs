using System;
using System.Collections.Generic;
using System.Text;

namespace BL.Xml.Core
{
    /// <summary>
    /// Holds the data needed for creating a premis:object XElement.
    /// </summary>
    /// <remarks>
    /// Example premis:object XElement:
    /// <code>
    ///     &lt;premis:object xsi:type="premis:file"&gt;
    ///         &lt;premis:objectIdentifier&gt;
    ///             &lt;premis:objectIdentifierType&gt;ARK&lt;/premis:objectIdentifierType&gt;
    ///             &lt;premis:objectIdentifierValue&gt;ark://vdc/635197248000000004&lt;/premis:objectIdentifierValue&gt;
    ///         &lt;/premis:objectIdentifier&gt;
    ///         &lt;premis:objectCharacteristics&gt;
    ///             &lt;premis:compositionLevel&gt;0&lt;/premis:compositionLevel&gt;
    ///             &lt;premis:fixity&gt;
    ///                 &lt;premis:messageDigestAlgorithm&gt;MD5&lt;/premis:messageDigestAlgorithm&gt;
    ///                 &lt;premis:messageDigest&gt;c7076ad10051649b6a6911a3558cf7f6&lt;/premis:messageDigest&gt;
    ///             &lt;/premis:fixity&gt;
    ///             &lt;premis:size&gt;147145&lt;/premis:size&gt;
    ///             &lt;premis:format&gt;
    ///                 &lt;premis:formatDesignation&gt;
    ///                     &lt;premis:formatName&gt;image/jp2&lt;/premis:formatName&gt;
    ///                 &lt;/premis:formatDesignation&gt;
    ///             &lt;/premis:format&gt;
    ///         &lt;/premis:objectCharacteristics&gt;
    ///         &lt;premis:originalName&gt;BL_A0017259116_00000005.jp2&lt;/premis:originalName&gt;
    ///     &lt;/premis:object&gt;
    /// </code>
    /// </remarks>
    public sealed class PremisObjectInfo
    {
        /// <summary>
        /// xsi:type XAttribute value.
        /// </summary>
        public string PremisObjectType { get; set; }

        /// <summary>
        /// objectIdentifierType XElement value.
        /// </summary>
        public string ObjectIdentifierType { get; set; }

        /// <summary>
        /// objectIdentifierValue XElement value.
        /// </summary>
        public string ObjectIdentifierValue { get; set; }

        /// <summary>
        /// compositionLevel XElement value.
        /// </summary>
        public string CompositionLevel { get; set; }

        /// <summary>
        /// messageDigestAlgorithm XElement value (usually md5).
        /// </summary>
        public string MessageDigestAlgorithm { get; set; }

        /// <summary>
        /// messageDigest XElement value (the hash).
        /// </summary>
        public string MessageDigest { get; set; }

        /// <summary>
        /// size XElement value.
        /// </summary>
        public string Size { get; set; }

        /// <summary>
        /// formatName XElement Value (MIME type).
        /// </summary>
        public string FormatName { get; set; }

        /// <summary>
        /// originalName XElement value (the filename).
        /// </summary>
        public string PremisOriginalName { get; set; }
    }
}
