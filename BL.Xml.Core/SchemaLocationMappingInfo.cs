using System;
using System.Collections.Generic;
using System.Text;

namespace BL.Xml.Core
{
    public sealed class SchemaLocationMappingInfo
    {
        /// <summary>
        /// Dictionary of namespace identifier - schema location hint pairs.
        /// </summary>
        public Dictionary<string, string> SchemaLocationMappings { get; set; }

        /// <summary>
        /// Creates a new instance of the SchemaLocationMappingsInfo class.
        /// </summary>
        public SchemaLocationMappingInfo()
        {
            SchemaLocationMappings = new Dictionary<string, string>();
        }

    }
}
