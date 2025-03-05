using System;
using System.Collections.Generic;
using System.Text;

namespace BL.Xml.Core
{
    /// <summary>
    /// Contains the data needed to create a structMap XElement.
    /// </summary>
    /// <remarks>
    /// Example structMap XElement:
    /// <code>
    ///     &lt;mets:structMap TYPE="LOGICAL"&gt;
    /// </code>
    /// </remarks>
    // ReSharper disable ClassNeverInstantiated.Global
    public sealed class StructMapInfo
        // ReSharper restore ClassNeverInstantiated.Global
    {
        /// <summary>
        /// TYPE XAttribute value.
        /// </summary>
        public string Type { get; set; }
    }
}
