using System;
using System.Collections.Generic;
using System.Text;

namespace BL.Xml.Core
{
    public sealed class NamespaceDeclarationInfo
    {
        /// <summary>
        /// Dictionary of namespace prefix - identifier pairs.
        /// </summary>
        public Dictionary<string, string> NamespaceDeclarations { get; set; }

        /// <summary>
        /// Creates a new instance of the NamespaceDeclarationsInfo class.
        /// </summary>
        public NamespaceDeclarationInfo()
        {
            NamespaceDeclarations = new Dictionary<string, string>();
        }
    }
}
