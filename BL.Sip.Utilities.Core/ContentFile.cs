using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BL.Sip.Utilities.Core
{
    public class ContentFile : ICloneable, IPhysicalFile
    {
        private readonly Lazy<IPhysicalFile> _hashInit;
        private string _hash;
        private string _hashAlgorithm;
        private long? _size;
        private string _format;
        private string _mimeType;

        public ContentFile()
        {
            _hashInit = new Lazy<IPhysicalFile>(() => new DefaultPhysicalFile());
            Filename = string.Empty;
            Location = string.Empty;
            Hash = string.Empty;
            Format = string.Empty;
            FormatVersion = string.Empty;
            MimeType = string.Empty;
            HashAlgorithm = HashHelper.Sha256Hash;
            URI = string.Empty;
        }

        public ContentFile(string id, string metsFile, Func<string, string, IPhysicalFile> f)
        {
            _hashInit = new Lazy<IPhysicalFile>(() => f.Invoke(id, metsFile));
        }

        public string ID { get; set; }
        public string Filename { get; set; }
        public string Location { get; set; }
        public long FileSize { get; set; }
        public long DomID { get; set; }
        public DateTime? DateDomIDAllocated { get; set; }
        public DateTime? DateAvailable { get; set; }
        public DateTime? DatePreserved { get; set; }
        public string Hash
        {
            get { return _hash ?? (_hash = _hashInit.Value.Hash); }
            set { _hash = value; }
        }

        public string HashAlgorithm
        {
            get { return _hashAlgorithm ?? (_hashAlgorithm = _hashInit.Value.HashAlgorithm); }
            set { _hashAlgorithm = value; }
        }

        public long Size
        {
            get { return _size ?? (_hashInit.Value.Size); }
            set { _size = value; }
        }

        public string Format
        {
            get { return _format ?? (_format = _hashInit.Value.Format); }
            set { _format = value; }
        }

        public string FormatVersion { get; set; }
        public string MimeType
        {
            get { return _mimeType ?? (_mimeType = _hashInit.Value.MimeType); }
            set { _mimeType = value; }
        }

        public string URI { get; set; }
        public bool IsValid { get; set; }
        public bool ValidationAttempted { get; set; }
        public string ValidationFailMessage { get; set; }
        public long? ParentID { get; set; }
        public long DatastreamID { get; set; }
        public string FullPath { get { return Location.CombinePath(Filename); } }
        public ContentType Type { get; set; }
        public long WorkFlowEntityId { get; set; }

        public enum ContentType
        {
            Ingestable = 1,
            NonIngestable,
            Metadata,
            External
        }

        #region ICloneable Members

        public object Clone()
        {
            return MemberwiseClone();
        }

        #endregion
    }

    internal class DefaultPhysicalFile : IPhysicalFile
    {
        public string Location { get; set; }
        public string Filename { get; set; }
        public string FullPath { get; private set; }
        public string HashAlgorithm { get; set; }
        public string Hash { get; set; }
        public long Size { get; set; }
        public string Format { get; set; }
        public string MimeType { get; set; }
    }
}
