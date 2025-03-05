using System;
using System.Collections.Generic;
using System.Text;

namespace BL.Xml.Core
{
    /// <summary>
    /// Holds the data needed for creating a metshdr XElement.
    /// </summary>
    /// <remarks>
    /// Example metsHdr XElement:
    /// <code>
    /// &lt;metsHdr CREATEDATE="2013-11-20T12:11:15Z" ADMID="amd00000001-source"&gt;
    ///     &lt;agent ROLE="CREATOR" TYPE="OTHER" OTHERTYPE="SOFTWARE"&gt;
    ///         &lt;name&gt;Google.Scheduler;1.0&lt;/name&gt;
    ///     &lt;/agent&gt;
    ///     &lt;metsDocumentID&gt;ark:/81055/dvdc_100018114985.0x000001&lt;/metsDocumentID&gt;
    /// &lt;/metsHdr&gt;
    /// </code>
    /// </remarks>
    // ReSharper disable ClassNeverInstantiated.Global
    public sealed class MetsHdrInfo
        // ReSharper restore ClassNeverInstantiated.Global
    {
        /// <summary>
        /// ADMID XAttribute value (can be empty or missing - if so, it is omitted).
        /// </summary>
        public string AdmId { get; set; }

        /// <summary>
        /// Mets:name XElement value.
        /// </summary>
        public string AgentName { get; set; }

        /// <summary>
        /// metsDocumentID XElement value (NB this has a default).
        /// </summary>
        public string DocumentId { get; set; }

        /// <summary>
        /// Creates an instance of the MetsHdrInfo class.
        /// </summary>
        public MetsHdrInfo()
        {
            // The default
            DocumentId = "METS DOCUMENT ARK GOES HERE";
        }
    }
}
