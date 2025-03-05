using System;
using System.Collections.Generic;
using System.Text;

namespace BL.Xml.Core
{
    /// <summary>
    /// This class defines the data needed for creating an MdWrap XElement.
    /// </summary>
    /// <remarks>
    /// Example mdWrap XElement:
    /// <code>
    ///     &lt;mets:mdWrap MDTYPE="OTHER" OTHERMDTYPE="BLRELATIVEPOSITION"&gt;
    ///     &lt;/mets:mdWrap&gt;
    /// </code>
    /// </remarks>
    // ReSharper disable ClassNeverInstantiated.Global
    public sealed class MdWrapInfo
        // ReSharper restore ClassNeverInstantiated.Global
    {
        /// <summary>
        /// MDTYPE XAttribute value.
        /// </summary>
        public string MdType { get; set; }

        /// <summary>
        /// OTHERMDTYPE XAttribute value.
        /// </summary>
        public string OtherMdType { get; set; }
    }
}
