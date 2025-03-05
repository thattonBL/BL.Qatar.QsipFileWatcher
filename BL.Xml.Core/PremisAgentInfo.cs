using System;
using System.Collections.Generic;
using System.Text;

namespace BL.Xml.Core
{
    /// <summary>
    /// Holds the data needed for creating a premis:agent XElement.
    /// </summary>
    /// <remarks>
    /// Example premis:agent XElement:
    /// <code>
    ///     &lt;premis:agent&gt;
    ///         &lt;premis:agentIdentifier&gt;
    ///             &lt;premis:agentIdentifierType&gt;local&lt;/premis:agentIdentifierType&gt;
    ///             &lt;premis:agentIdentifierValue&gt;agent001&lt;/premis:agentIdentifierValue&gt;
    ///         &lt;/premis:agentIdentifier&gt;
    ///         &lt;premis:agentName&gt;JHove;1.5&lt;/premis:agentName&gt;
    ///         &lt;premis:agentType&gt;software&lt;/premis:agentType&gt;
    ///     &lt;/premis:agent&gt;
    /// </code>
    /// </remarks>
    public sealed class PremisAgentInfo
    {
        /// <summary>
        /// agentIdentifierType XElement value.
        /// </summary>
        public string AgentIdentifierType { get; set; }

        /// <summary>
        /// agentIdentifierValue XElement value.
        /// </summary>
        public string AgentIdentifierValue { get; set; }

        /// <summary>
        /// agentName XElement value.
        /// </summary>
        public string AgentName { get; set; }

        /// <summary>
        /// agentType XElement value.
        /// </summary>
        public string AgentType { get; set; }
    }
}
