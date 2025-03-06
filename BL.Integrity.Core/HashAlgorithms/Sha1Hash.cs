﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Serialization;

namespace BL.Integrity.Core.HashAlgorithms
{
    /// <summary>
    /// The Sha1Hash class encapsulates the DOM System use of hashes generated by the SHA1 Hash algorithm.
	/// </summary>
    /// <remarks>
	/// The SHA1 hash algorithm generates a 160 bit hash from an input byte sequence.
	/// The generated hashes are employed in the Timestamping and Verification processes of the DOM System.
	/// The 160 bit hash can be represented as a sequence of 20 bytes or 40 hexadecimal characters.  The
	/// DOM System utilises both of these forms.
    /// </remarks>
    ///	<revisionHistory> 
    ///		<revision author="Andy Evans, Leigh Willoughby" date="23/11/2005">Initial Version</revision>
    ///	</revisionHistory>
    [XmlType(Namespace = "http://BL.Schemas.Integrity.Sha1Hash")]
    [XmlRootAttribute(Namespace = "http://BL.Schemas.Integrity.Sha1Hash", IsNullable = false)]
    public class Sha1Hash
    {
        #region Constants
        // Integer constant which defines the size of a SHA1 hash in bytes
        private const int HASH_LENGTH_BYTES = 20;
        // The type of hashing algorithm used
        private const string HASH_IMPLEMENTATION = "SHA1";
        #endregion

        #region Private variables
        // The hash in bytes
        private byte[] _hashBytes = new byte[HASH_LENGTH_BYTES];
        // String constant contains the object identifier for the SHA1 hash algorithm
        private string _algorithm = @"1.3.14.3.2.26";
        #endregion

        #region Properties
        /// <summary>
        /// The hash in bytes
        /// </summary>
        /// <remarks>
        /// The set has been included for use by the XmlSerialiser.  Do not use the set on this property
        /// </remarks>
        ///	<revisionHistory> 
        ///		<revision author="Andy Evans, Leigh Willoughby" date="23/11/2005">Initial Version</revision>
        ///	</revisionHistory>
        [XmlAttributeAttribute(AttributeName = "Value", DataType = "hexBinary")]
        public byte[] HashBytes
        {
            get { return _hashBytes; }
            set { _hashBytes = value; }
        }
        /// <summary>
        /// The object identifier for the SHA1 Hash Algorithm
        /// </summary>
        /// <remarks>
        /// The set has been included for use by the XmlSerialiser.  Do not use the set on this property
        /// </remarks>
        ///	<revisionHistory> 
        ///		<revision author="Andy Evans, Leigh Willoughby" date="23/11/2005">Initial Version</revision>
        ///	</revisionHistory>
        [XmlAttributeAttribute]
        public string Algorithm
        {
            get { return _algorithm; }
            set { _algorithm = value; }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// This will construct a hash object from a byte array
        /// </summary>
        /// <param name="bytesToHash">An array of bytes to be hashed</param>
        ///	<revisionHistory> 
        ///		<revision author="Andy Evans, Leigh Willoughby" date="23/11/2005">Initial Version</revision>
        ///	</revisionHistory>
        public Sha1Hash(byte[] bytesToHash)
        {
            if (HASH_LENGTH_BYTES == bytesToHash.Length)
                _hashBytes = bytesToHash;
            else
                this.CreateHashFromBytes(bytesToHash);
        }

        /// <summary>
        /// This will construct a hash object from a file stream
        /// </summary>
        /// <param name="fileStreamToHash">A stream on the file to be hashed</param>
        ///	<revisionHistory> 
        ///     <revision author="Jackson Pope" date="20/2/2006">Initial version</revision>
        ///	</revisionHistory>
        public Sha1Hash(Stream fileStreamToHash)
        {
            this.CreateHashFromStream(fileStreamToHash);
        }


        /// <summary>
        /// Empty constructor for use by the XmlSerialiser.  Do not use this constructor
        /// </summary>
        public Sha1Hash() { }
        #endregion

        #region overload operators
        /// <summary>
        /// Overload == operator to compare the byte contents of two Sha1Hash objects
        /// </summary>
        /// <param name="left">The left hand side of an == statement</param>
        /// <param name="right">The right hand side of an == statement</param>
        /// <returns>True if the two objects match, else false</returns>
        /// <revisionHistory> 
        ///		<revision author="Andy Evans, Leigh Willoughby" date="23/11/2005">Initial Version</revision>
        ///	</revisionHistory>
        public static bool operator ==(Sha1Hash left, Sha1Hash right)
        {
            bool equal = false;

            if (object.Equals(left, null) || object.Equals(right, null))
                equal = object.Equals(left, null) && object.Equals(right, null);
            else
            {
                equal = left._hashBytes.Length == right.HashBytes.Length;

                if (equal)
                {
                    // byte by byte comparison, will compare until out of bytes or two don't match
                    for (int pos = 0; pos < left._hashBytes.Length; pos++)
                    {
                        // If two bytes don't match bail out and return false
                        equal = (left._hashBytes[pos] == right._hashBytes[pos]);
                        if (!equal)
                            break;
                    }
                }
            }

            return equal;
        }

        /// <summary>
        /// Override to keep the compiler happy
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (object.Equals(obj, null))
                throw new ArgumentNullException("obj");
            if (!(obj is Sha1Hash))
                throw new ArgumentException("The passed object is not a Sha1Hash", "obj");

            return this == (Sha1Hash)obj;
        }

        /// <summary>
        /// Overriden implementation to generate a hashcode from the bytes in the hash, rather than the
        /// standard implementation that produces a hash based on the memory address of the object.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            int hashCode = 0;
            int shift = 24;

            for (int pos = 0; pos < _hashBytes.Length; pos++)
            {
                hashCode ^= _hashBytes[pos] << shift;

                shift = (shift == 0) ? 24 : shift - 8;
            }

            return hashCode ^ this._algorithm.GetHashCode();
        }

        /// <summary>
        /// Overload != operator to compare the byte contents of two Sha1Hash objects
        /// </summary>
        /// <param name="firstHash"></param>
        /// <param name="secondHash"></param>
        /// <returns>True if the two objects don't match, else false</returns>
        /// <revisionHistory> 
        ///		<revision author="Andy Evans, Leigh Willoughby" date="23/11/2005">Initial Version</revision>
        ///	</revisionHistory>
        public static bool operator !=(Sha1Hash firstHash, Sha1Hash secondHash)
        {
            return !(firstHash == secondHash);
        }
        #endregion

        #region PrivateMethods
        /// <summary>
        /// This will construct a hash object from a byte array
        /// </summary>
        /// <param name="bytesToHash">An array of bytes to be hashed</param>
        ///	<revisionHistory> 
        ///		<revision author="Andy Evans, Leigh Willoughby" date="23/11/2005">Initial Version</revision>
        ///	</revisionHistory>        
        private void CreateHashFromBytes(byte[] bytesToHash)
        {
            // Generate the SHA1 Hash from the byte array
            using (HashAlgorithm hashAlgorithm = HashAlgorithm.Create(HASH_IMPLEMENTATION))
            {
                _hashBytes = hashAlgorithm.ComputeHash(bytesToHash);
            }
        }

        private void CreateHashFromStream(Stream fileStreamToHash)
        {
            // Generate the SHA1 Hash from the byte array
            using (HashAlgorithm hashAlgorithm = HashAlgorithm.Create(HASH_IMPLEMENTATION))
            {
                _hashBytes = hashAlgorithm.ComputeHash(fileStreamToHash);
            }
        }

        #endregion
    }
}
