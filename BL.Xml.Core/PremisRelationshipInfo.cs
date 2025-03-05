using System;
using System.Collections.Generic;
using System.Text;

namespace BL.Xml.Core
{
    /// <summary>
    /// Holds the data needed for creating a premis:relationship XElement.
    /// </summary>
    /// <remarks>
    /// Example premis:relationship XElement:
    /// <code>
    ///     &lt;premis:relationship&gt;
    ///         &lt;premis:relationshipType&gt;derivation&lt;/premis:relationshipType&gt;
    ///         &lt;premis:relationshipSubType&gt;migration&lt;/premis:relationshipSubType&gt;
    ///         &lt;premis:relatedObjectIdentification&gt;
    ///             &lt;premis:relatedObjectIdentifierType&gt;ARK&lt;/premis:relatedObjectIdentifierType&gt;
    ///             &lt;premis:relatedObjectIdentifierValue&gt;ark://vdc/635197248000000011&lt;/premis:relatedObjectIdentifierValue&gt;
    ///         &lt;/premis:relatedObjectIdentification&gt;
    ///         &lt;premis:relatedEventIdentification&gt;
    ///             &lt;premis:relatedEventIdentifierType&gt;local&lt;/premis:relatedEventIdentifierType&gt;
    ///             &lt;premis:relatedEventIdentifierValue&gt;event001&lt;/premis:relatedEventIdentifierValue&gt;
    ///         &lt;/premis:relatedEventIdentification&gt;
    ///     &lt;/premis:relationship&gt;
    /// </code>
    /// </remarks>
    public sealed class PremisRelationshipInfo
    {
        /// <summary>
        /// relationshipType XElement value.
        /// </summary>
        public string RelationshipType { get; set; }

        /// <summary>
        /// relationshipSubType XElement value.
        /// </summary>
        public string RelationshipSubType { get; set; }

        /// <summary>
        /// relatedObjectIdentifierType XElement value.
        /// </summary>
        public string RelatedObjectIdentifierType { get; set; }

        /// <summary>
        /// relatedObjectIdentifierValue XElement value.
        /// </summary>
        public string RelatedObjectIdentifierValue { get; set; }

        /// <summary>
        /// relatedEventIdentifierType XElement value.
        /// </summary>
        public string RelatedEventIdentifierType { get; set; }

        /// <summary>
        /// relatedEventIdentifierValue XElement value.
        /// </summary>
        public string RelatedEventIdentifierValue { get; set; }
    }
}
