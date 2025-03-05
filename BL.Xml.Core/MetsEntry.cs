using System;
using System.Collections.Generic;
using System.Text;

namespace BL.Xml.Core
{
    /// <summary>
    /// Contains the pertinent data for creating a Content file
    /// </summary>
    public sealed class MetsEntry
    {
        /// <summary>
        /// e.g 'mets:file ID='
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// e.g 'mets:file ... xlink:href='
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// e.g ark:/81055/dvdc_100000058781.0x000003
        /// </summary>
        public string ArkId { get; set; }

        /// <summary>
        /// e.g e0d123e5f316bef78bfdf5a008837577
        /// </summary>
        public string ChecksumValue { get; set; }

        /// <summary>
        /// e.g MD5
        /// </summary>
        public string CheckSumType { get; set; }

        /// <summary>
        /// e.g 525
        /// </summary>
        public long Size { get; set; }

        /// <summary>
        /// e.g amd0005
        /// </summary>
        public string AmdId { get; set; }

        /// <summary>
        /// e.g Image, Image-folded
        /// </summary>
        public string FileGroup { get; set; }

        /// <summary>
        /// e.g 'text/plain', 'text/html'
        /// </summary>
        public string MimeType { get; set; }
    }

}
