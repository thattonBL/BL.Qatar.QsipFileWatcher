using System;
using System.Collections.Generic;
using System.Text;

namespace BL.Xml.Core
{
    /// <summary>
    /// This class defines the data needed for creating an FileSec XElement.
    /// </summary>
    /// <remarks>
    /// Example file XElement:
    /// <code>
    /// &lt;file ID="file00000001" ADMID="file00000001-object01"&gt;
    ///     &lt;FLocat LOCTYPE="Url" OTHERLOCTYPE="EXTERNALCONTENT" href="BL_A0017259116_00000001.jp2" /&gt;
    /// &lt;/file&gt;
    /// </code>
    /// </remarks>
    // ReSharper disable ClassNeverInstantiated.Global
    public sealed class FileInfo : FileInfoBase
        // ReSharper restore ClassNeverInstantiated.Global
    {
        /// <summary>
        /// ID XAttribute value (e.g. "file00000001"). Formatted as "file{0:00000000}".
        /// </summary>
        public int Id { get; set; }
    }
}
