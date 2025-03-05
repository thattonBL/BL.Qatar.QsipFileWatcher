using System;
using System.Collections.Generic;
using System.Text;

namespace BL.Xml.Core
{
    /// <summary>
    /// This class defines the parameters needed for creating an AmdSec XElement.
    /// </summary>
    /// <remarks>
    /// Example amdSec XElement:
    /// <code>
    /// &lt;amdSec ID="amd00000001"&gt;
    ///     &lt;sourceMD ID="amd00000001-source" /&gt;
    /// &lt;/amdSec&gt;
    /// </code>
    /// </remarks>
    // ReSharper disable ClassNeverInstantiated.Global
    public sealed class AmdSecInfo : AmdSecInfoBase
        // ReSharper restore ClassNeverInstantiated.Global
    {
        /// <summary>
        /// The ID XAttribute value.
        /// </summary>
        /// <remarks>A remark on the Id property</remarks>
        public int Id { get; set; }

    }
}
