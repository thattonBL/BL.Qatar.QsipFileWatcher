using System;
using System.Collections.Generic;
using System.Text;

namespace BL.Xml.Core
{
    /// <summary>
    /// Holds the data needed for creating a premis:event XElement.
    /// </summary>
    /// <remarks>
    /// Example premis:event XElement:
    /// <code>
    ///     &lt;premis:event&gt;
    ///         &lt;premis:eventIdentifier&gt;
    ///             &lt;premis:eventIdentifierType&gt;local&lt;/premis:eventIdentifierType&gt;
    ///             &lt;premis:eventIdentifierValue&gt;event001&lt;/premis:eventIdentifierValue&gt;
    ///         &lt;/premis:eventIdentifier&gt;
    ///         &lt;premis:eventType&gt;WellFormedness&lt;/premis:eventType&gt;
    ///         &lt;premis:eventDateTime&gt;2013-11-11T14:05:23&lt;/premis:eventDateTime&gt;
    ///         &lt;premis:eventOutcomeInformation&gt;
    ///             &lt;premis:eventOutcome&gt;Success&lt;/premis:eventOutcome&gt;
    ///         &lt;/premis:eventOutcomeInformation&gt;
    ///         &lt;premis:linkingAgentIdentifier&gt;
    ///             &lt;premis:linkingAgentIdentifierType&gt;local&lt;/premis:linkingAgentIdentifierType&gt;
    ///             &lt;premis:linkingAgentIdentifierValue&gt;agent001&lt;/premis:linkingAgentIdentifierValue&gt;
    ///         &lt;/premis:linkingAgentIdentifier&gt;
    ///     &lt;/premis:event&gt;
    /// </code>
    /// </remarks>
    public class PremisEventInfo
    {
        /// <summary>
        /// eventIdentifier XElement value.
        /// </summary>
        public string EventIdentifierType { get; set; }

        /// <summary>
        /// eventIdentifierValue XElement value.
        /// </summary>
        public string EventIdentifierValue { get; set; }

        /// <summary>
        /// eventType XElement value.
        /// </summary>
        public string EventType { get; set; }

        /// <summary>
        /// eventDateTime XElement value.
        /// </summary>
        public string EventDateTime { get; set; }

        /// <summary>
        /// eventOutcome XElement value.
        /// </summary>
        public string EventOutcome { get; set; }

        /// <summary>
        /// linkingAgentIdentifierType XElement value.
        /// </summary>
        public string LinkingAgentIdentifierType { get; set; }

        /// <summary>
        /// linkingAgentIdentifierValue XElement value.
        /// </summary>
        public string LinkingAgentIdentifierValue { get; set; }
    }
}
