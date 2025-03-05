using System;
using System.Collections.Generic;
using System.Text;

namespace BL.Xml.Core
{
    /// <summary>
    /// Contains all of the data needed to create a Div XElement that will ultimately belong in a struct map XElement.
    /// </summary>
    /// <remarks>
    /// Example div XElement:
    /// <code>
    ///     &lt;mets:div ID="log0" CONTENTIDS="ark:/81055/dvdc_100018116168.0x000001" TYPE="Monograph" DMDID="dmd0001" ADMID="amd00000003-rights01"&gt;
    /// </code>
    /// </remarks>
    // ReSharper disable ClassNeverInstantiated.Global
    public sealed class StructMapDivInfo
        // ReSharper restore ClassNeverInstantiated.Global
    {
        /// <summary>
        /// ID XAttribute value.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// CONTENTIDS XAttribute value.
        /// </summary>
        public string ContentIds { get; set; }

        /// <summary>
        /// TYPE XAttribute value.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// DMDID XAttribute value.
        /// </summary>
        public string DmdId { get; set; }

        /// <summary>
        /// ADMID XAttribute value.
        /// </summary>
        public string AdmId { get; set; }
    }
}
