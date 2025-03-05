using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BL.Sip.Utilities.Core
{
    public interface IPhysicalFile
    {
        string Location { get; set; }
        string Filename { get; set; }
        string FullPath { get; }
        string HashAlgorithm { get; set; }
        string Hash { get; set; }
        long Size { get; set; }
        string Format { get; set; }
        string MimeType { get; set; }
    }
}
