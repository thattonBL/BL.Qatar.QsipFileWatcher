using System;
using System.Collections.Generic;
using System.Text;

namespace BL.Xml.Core
{
    /// <summary>
    /// Holds the data needed for creating a dmdSec XElement.
    /// </summary>
    /// <remarks>
    /// Example dmdSec XElement:
    /// <code>
    ///     &lt;mets:dmdSec ID="dmd0001"&gt;
    ///         &lt;mets:mdRef MDTYPE="MARC" LOCTYPE="ARK" href="ark:/81055/dvdc_100018114986.0x000001" /&gt;
    ///     &lt;/mets:dmdSec&gt;
    /// </code>
    /// </remarks>
    // ReSharper disable ClassNeverInstantiated.Global
    public sealed class DmdSecInfo
        // ReSharper restore ClassNeverInstantiated.Global
    {
        /// <summary>
        /// ID XAttribute value. Formatted as "dmd{0:0000}".
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// MDTYPE XAttribute value (e.g. "MARC", "MODS", "OTHER").
        /// </summary>
        public string MdType { get; set; }

        /// <summary>
        /// OTHERMDTYPE XAttribute value used if MdType is "OTHER".
        /// </summary>
        public string OtherMdType { get; set; }

        /// <summary>
        /// LOCTYPE XAttribue value. (e.g. "ARK", "OTHER").
        /// </summary>
        public string LocType { get; set; }

        /// <summary>
        /// OTHERLOCTYPE XAttribute value if specified. If not, it is ommitted. (e.g. "AlphId").
        /// </summary>
        public string OtherLocType { get; set; }

        /// <summary>
        /// href XAttribute value (can be empty or missing in which case an empty string is inserted).
        /// </summary>
        public string HRef { get; set; }
    }
}
