using System;
using System.Collections.Generic;
using System.Text;

namespace BL.Xml.Core
{
    /// <summary>
    /// This class defines the data needed for creating an FileSec XElement
    /// </summary>
    /// <remarks>
    /// Example fileGrp XElement:
    /// <code>
    /// &lt;fileSec&gt;
    ///     &lt;fileGrp USE="Image" /&gt;
    /// &lt;/fileSec&gt;
    /// </code>
    /// </remarks>
    // ReSharper disable ClassNeverInstantiated.Global
    public sealed class FileGrpInfo
        // ReSharper restore ClassNeverInstantiated.Global
    {
        /// <summary>
        /// USE XAttribute value.
        /// </summary>
        public string Use { get; set; }
    }
}
