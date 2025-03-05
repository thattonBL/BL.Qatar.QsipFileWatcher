using System;
using System.Collections.Generic;
using System.Text;

namespace BL.Xml.Core
{
    /// <summary>
    /// This class defines the parameters which form the base of clasess FileInfo and FileInfoWithStringIdAttribute
    /// </summary>
    public class FileInfoBase
    {
        /// <summary>
        /// ADMID XAttribute value.
        /// </summary>
        public string AdmId { get; set; }

        /// <summary>
        /// LOCTYPE XAttribute value.
        /// </summary>
        public string LocType { get; set; }

        /// <summary>
        /// OTHERLOCTYPE XAttribute value
        /// </summary>
        public string OtherLocType { get; set; }

        /// <summary>
        /// xlink:href XAttribute value.
        /// </summary>
        public string HRef { get; set; }

        public FileInfoBase()
        {
            HRef = "FILE LOCATION GOES HERE";
        }
    }
}
