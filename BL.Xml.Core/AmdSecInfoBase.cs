using System;
using System.Collections.Generic;
using System.Text;

namespace BL.Xml.Core
{
    /// <summary>
    /// This class defines the parameters which form the base of clasess AmdSecInfo and AmdSecInfoWithStringIdAttribute
    /// </summary>
    public class AmdSecInfoBase
    {
        /// <summary>
        /// The name of the mets:XXXXXMD XElement (e.g. SourceMD -> mets:SourceMD).
        /// </summary>
        public string Md { get; set; }

        /// <summary>
        /// The ID XAttribute of the XXXXXMd XElement.
        /// </summary>
        public string MdId { get; set; }

    }
}
