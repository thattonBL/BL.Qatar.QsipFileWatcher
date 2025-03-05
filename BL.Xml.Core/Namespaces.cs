using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace BL.Xml.Core
{
    public static class Namespaces
    {
        /// <summary>
        /// The mets namespace
        /// </summary>
        public static readonly XNamespace Mets = "http://www.loc.gov/METS/";

        /// <summary>
        /// The mods namespace
        /// </summary>
        public static readonly XNamespace Mods = "http://www.loc.gov/mods/v3";

        /// <summary>
        /// The xlink namespace
        /// </summary>
        public static readonly XNamespace Xlink = "http://www.w3.org/1999/xlink";

        /// <summary>
        /// The xsie namespace
        /// </summary>
        public static readonly XNamespace Xsie = "http://www.w3.org/2001/XMLSchema-instance";

        /// <summary>
        /// The mediamd namespace
        /// </summary>
        public static readonly XNamespace MediaMd = "http://bl.uk/namespace/mediaMD";

        /// <summary>
        /// The premis namespace
        /// </summary>
        public static readonly XNamespace Premis = "info:lc/xmlns/premis-v2";

        /// <summary>
        /// The processmd namespace
        /// </summary>
        public static readonly XNamespace ProcessMd = "http://bl.uk/namespace/processMD";

        /// <summary>
        /// The blprocess namespace
        /// </summary>
        public static readonly XNamespace BlProcess = "http://bl.uk/namespaces/blprocess";

        /// <summary>
        /// The rtc namespace
        /// </summary>
        public static readonly XNamespace Rights = "http://cosimo.stanford.edu/sdr/metsrights/";

        /// <summary>
        /// The mets bldigitized
        /// </summary>
        public static readonly XNamespace BlDigitized = "http://bl.uk/namespaces/bldigitized";

        /// <summary>
        /// The marc namespace
        /// </summary>
        public static readonly XNamespace Marc = "http://www.loc.gov/MARC21/slim";
        
    }
}
