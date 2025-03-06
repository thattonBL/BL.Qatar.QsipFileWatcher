using System.Security;
using System.Xml.Serialization;
using BL.Integrity.Core.HashAlgorithms;

namespace BL.Integrity.Core;

/// <summary>
/// Represents an initial hash of an object
/// </summary>
/// <remarks>
/// Currently only uses the SHA1, SHA256 and SHA512 hash algorithms
/// </remarks>
///	<revisionHistory> 
///		<revision author="Andy Evans, Leigh Willoughby" date="23/11/2005">Initial Version</revision>
///     <revision author="Andy Evans" date="29/11/2005">Added a getter property to allow access to the Sha1Hash algorithm property</revision>
///     <revision author="Andy Evans" date="8/2006">Added support for SHA512</revision>
///     <revision author="Andy Evans" date="24/11/2006">Added support for SHA256</revision>
///     <revision author="Andy Evans" date="26/1/2007">Added support for MD5</revision>
///     <revision author="Andy Evans" date="26/4/2007">Added setter for HashAlgorithm property</revision>
///	</revisionHistory>
[XmlType(Namespace = "http://BL.Dom.Schemas.Integrity.Hash")]
[XmlRootAttribute(Namespace = "http://BL.Dom.Schemas.Integrity.Hash", IsNullable = false)]
public class Hash
{
    #region Protected Variables

    /// <summary>
    /// The hash
    /// </summary>
    protected Sha512Hash _sha512Hash;
    protected Sha1Hash _sha1Hash;
    protected Sha256Hash _sha256Hash;
    protected MD5Hash _md5Hash;

    #endregion

    #region Private Variables

    private HashAlgo _hashAlgorithm;

    #endregion

    #region Properties

    /// <summary>
    /// Get the bytes that represent the hash
    /// </summary>
    [XmlIgnore]
    public byte[] HashBytes
    {
        get
        {
            if (_hashAlgorithm == HashAlgo.SHA1)
                return _sha1Hash.HashBytes;
            else if (_hashAlgorithm == HashAlgo.SHA256)
                return _sha256Hash.HashBytes;
            else if (_hashAlgorithm == HashAlgo.MD5)
                return _md5Hash.HashBytes;
            else
                return _sha512Hash.HashBytes;
        }
        set
        {
            if (_hashAlgorithm == HashAlgo.SHA1)
                _sha1Hash.HashBytes = value;
            else if (_hashAlgorithm == HashAlgo.SHA256)
                _sha256Hash.HashBytes = value;
            else if (_hashAlgorithm == HashAlgo.MD5)
                _md5Hash.HashBytes = value;
            else
                _sha512Hash.HashBytes = value;
        }
    }

    /// <summary>
    /// This get and set has been included for use by the XmlSerialiser.  Do not use the get or set on this 
    /// property
    /// </summary>
    [XmlElementAttribute(ElementName = "Sha512Hash", Namespace = "http://BL.Dom.Schemas.Integrity.Sha512Hash")]
    public Sha512Hash Sha512Hash
    {
        get { return _sha512Hash; }
        set { _sha512Hash = value; }
    }

    /// <summary>
    /// This get and set has been included for use by the XmlSerialiser.  Do not use the get or set on this 
    /// property
    /// </summary>
    [XmlElementAttribute(ElementName = "Sha1Hash", Namespace = "http://BL.Dom.Schemas.Integrity.Sha1Hash")]
    public Sha1Hash Sha1Hash
    {
        get { return _sha1Hash; }
        set { _sha1Hash = value; }
    }

    /// <summary>
    /// This get and set has been included for use by the XmlSerialiser.  Do not use the get or set on this 
    /// property
    /// </summary>
    [XmlElementAttribute(ElementName = "Sha256Hash", Namespace = "http://BL.Dom.Schemas.Integrity.Sha256Hash")]
    public Sha256Hash Sha256Hash
    {
        get { return _sha256Hash; }
        set { _sha256Hash = value; }
    }

    /// <summary>
    /// This get and set has been included for use by the XmlSerialiser.  Do not use the get or set on this 
    /// property
    /// </summary>
    [XmlElementAttribute(ElementName = "MD5Hash", Namespace = "http://BL.Dom.Schemas.Integrity.MD5Hash")]
    public MD5Hash MD5Hash
    {
        get { return _md5Hash; }
        set { _md5Hash = value; }
    }

    /// <summary>
    /// The algorithm used by the underlying hashing algorithm
    /// </summary>
    public string HashAlgorithm
    {
        get
        {
            if (_hashAlgorithm == HashAlgo.SHA1)
                return _sha1Hash.Algorithm;
            else if (_hashAlgorithm == HashAlgo.SHA256)
                return _sha256Hash.Algorithm;
            else if (_hashAlgorithm == HashAlgo.MD5)
                return _md5Hash.Algorithm;
            else
                return _sha512Hash.Algorithm;
        }

        set
        {
            if (_md5Hash != null)
                _hashAlgorithm = HashAlgo.MD5;
            else if (_sha1Hash != null)
                _hashAlgorithm = HashAlgo.SHA1;
            else if (_sha256Hash != null)
                _hashAlgorithm = HashAlgo.SHA256;
            else
                _hashAlgorithm = HashAlgo.SHA512;
        }
    }

    #endregion

    #region Constructors

    /// <summary>
    /// Creates a hash from an unlimited number of byte arrays for a specified hash algorithm
    /// </summary>
    /// <remarks>
    /// The byte arrays are combined into a single byte array and the hash is generated from this single
    /// byte array
    /// </remarks>
    ///	<revisionHistory> 
    ///		<revision author="Andy Evans, Leigh Willoughby" date="23/11/2005">Initial Version</revision>
    ///     <revision author="Andy Evans" date="8/8/2006">Added support for multiple hash algorithms</revision>
    ///	</revisionHistory>
    protected Hash(HashAlgo hashAlgorithm, params byte[][] hashItems)
    {
        _hashAlgorithm = hashAlgorithm;
        //the resulting byte array to be hashed
        byte[] result;

        //only any point in combining if multiple arrays have been passed
        if (hashItems.Length > 1)
        {
            //the length of the destination byte array will be the total of the lengths
            //of all the byte arrays
            int destinationArrayLength = 0;

            //loop all arrays and get total length
            for (int pos = 0; pos < hashItems.Length; pos++)
            {
                destinationArrayLength += hashItems[pos].Length;
            }

            //create the destination array
            byte[] destinationArray = new byte[destinationArrayLength];

            //holds the position at which to start writing to the destination array
            int destinationOffset = 0;

            //loop all arrays and combine into single byte array
            for (int pos = 0; pos < hashItems.Length; pos++)
            {
                //copy to the destination array starting from the end point of the last array
                Buffer.BlockCopy(hashItems[pos], 0, destinationArray, destinationOffset, hashItems[pos].Length);
                //increment the destination offset with the length of the current array
                destinationOffset = +hashItems[pos].Length;
            }
            result = destinationArray;
        }
        //if there is only 1 byte array the array to be hashed is simply the first byte array in the array
        else
        {
            result = hashItems[0];
        }
        //produce the hash from the byte array, depending on selected hash algorithm
        if (_hashAlgorithm == HashAlgo.SHA1)
            _sha1Hash = new Sha1Hash(result);
        else if (_hashAlgorithm == HashAlgo.SHA256)
            _sha256Hash = new Sha256Hash(result);
        else if (_hashAlgorithm == HashAlgo.MD5)
            _md5Hash = new MD5Hash(result);
        else
            _sha512Hash = new Sha512Hash(result);
    }

    protected Hash(params byte[][] hashItems)
    {
        _hashAlgorithm = HashAlgo.SHA1;
        //the resulting byte array to be hashed
        byte[] result;

        //only any point in combining if multiple arrays have been passed
        if (hashItems.Length > 1)
        {
            //the length of the destination byte array will be the total of the lengths
            //of all the byte arrays
            int destinationArrayLength = 0;

            //loop all arrays and get total length
            for (int pos = 0; pos < hashItems.Length; pos++)
            {
                destinationArrayLength += hashItems[pos].Length;
            }

            //create the destination array
            byte[] destinationArray = new byte[destinationArrayLength];

            //holds the position at which to start writing to the destination array
            int destinationOffset = 0;

            //loop all arrays and combine into single byte array
            for (int pos = 0; pos < hashItems.Length; pos++)
            {
                //copy to the destination array starting from the end point of the last array
                Buffer.BlockCopy(hashItems[pos], 0, destinationArray, destinationOffset, hashItems[pos].Length);
                //increment the destination offset with the length of the current array
                destinationOffset = +hashItems[pos].Length;
            }

            result = destinationArray;
        }
        //if there is only 1 byte array the array to be hashed is simply the first byte array in the array
        else
        {
            result = hashItems[0];
        }

        //produce the hash from the byte array, depending on selected hash algorithm
        _sha1Hash = new Sha1Hash(result);
    }

    /// <summary>
    /// Create a Hash using a path to a file for a specified hash algorithm
    /// </summary>
    /// <param name="hashAlgorithm">The hash algorithm.</param>
    /// <param name="path">The path to a file</param>
    /// <revisionHistory>
    /// 	<revision author="Andy Evans, Leigh Willoughby" date="23/11/2005">Initial Version</revision>
    /// 	<revision author="Jackson Pope" date="20/2/2006">Changed to use streams instead of byte arrays</revision>
    /// 	<revision author="Andy Evans" date="8/8/2006">Added support for multiple hash algorithms</revision>
    /// </revisionHistory>
    public Hash(HashAlgo hashAlgorithm, string path)
    {
        //depending on selected hash algorithm
        _hashAlgorithm = hashAlgorithm;

        using (Stream hashStream = GetFileStream(path))
        {
            if (_hashAlgorithm == HashAlgo.SHA1)
                _sha1Hash = new Sha1Hash(hashStream);
            else if (_hashAlgorithm == HashAlgo.SHA256)
                _sha256Hash = new Sha256Hash(hashStream);
            else if (_hashAlgorithm == HashAlgo.MD5)
                _md5Hash = new MD5Hash(hashStream);
            else
                _sha512Hash = new Sha512Hash(hashStream);
        }
    }

    /// <summary>
    /// Create a Hash using a Byte[] hash
    /// </summary>
    /// <param name="hashAlgorithm">The hash algorithm.</param>
    /// <param name="hash">Byte[] hash</param>
    /// <revisionHistory>
    /// </revisionHistory>
    public Hash(HashAlgo hashAlgorithm, Byte[] hash)
    {
        //depending on selected hash algorithm
        _hashAlgorithm = hashAlgorithm;

        if (_hashAlgorithm == HashAlgo.SHA1)
            _sha1Hash = new Sha1Hash(hash);
        else if (_hashAlgorithm == HashAlgo.SHA256)
            _sha256Hash = new Sha256Hash(hash);
        else if (_hashAlgorithm == HashAlgo.MD5)
            _md5Hash = new MD5Hash(hash);
        else
            _sha512Hash = new Sha512Hash(hash);
    }

    public Hash(string path)
    {
        //depending on selected hash algorithm
        _hashAlgorithm = HashAlgo.SHA1;
        using (Stream hashStream = GetFileStream(path))
        {

            _sha1Hash = new Sha1Hash(hashStream);
        }
    }

    /// <summary>
    /// Empty constructor for use by XmlSerialization
    /// </summary>
    public Hash() { }

    #endregion

    #region Private Methods

    /// <summary>
    /// Produce an array of bytes from a given file
    /// </summary>
    /// <param name="path">The path to the file</param>
    /// <returns>An array of bytes from the file</returns>
    ///	<revisionHistory> 
    ///		<revision author="Andy Evans, Leigh Willoughby" date="23/11/2005">Initial Version</revision>
    ///     <revision author="Andy Evans" date="25/11/2005">Added exception checking code to deal with problems with the file path, throws a HashUnavailableException</revision>
    ///	</revisionHistory>
    private static byte[] GetFileBytes(string path)
    {
        if (path == null || path == string.Empty)
        {
            HashUnavailableException hashUnavailableException = new HashUnavailableException("path to file is not set");
            //Log.Write(Severity.Error, hashUnavailableException.Message);
            throw hashUnavailableException;
        }

        byte[] fileBytes = null;
        try
        {
            using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read, 1024, false))
            {
                using (BinaryReader br = new BinaryReader(fs))
                {
                    fileBytes = br.ReadBytes(Convert.ToInt32(fs.Length));
                }
            }
        }
        #region catch
        catch (FileNotFoundException ex)
        {
            string errorMessage = String.Format("file not found at location {0}", path);
            //Log.WriteException(Severity.Error, errorMessage, ex);
            throw new HashUnavailableException(errorMessage, ex);
        }
        catch (DirectoryNotFoundException ex)
        {
            string errorMessage = String.Format("directory not found at location {0}", path);
            //Log.WriteException(Severity.Error, errorMessage, ex);
            throw new HashUnavailableException(errorMessage, ex);
        }
        catch (SecurityException ex)
        {
            string errorMessage = String.Format("the caller does not have the required permission to access file {0}", path);
            //Log.WriteException(Severity.Error, errorMessage, ex);
            throw new HashUnavailableException(errorMessage, ex);
        }
        catch (UnauthorizedAccessException ex)
        {
            string errorMessage = String.Format("the OS does not permit this access to file {0}", path);
            //Log.WriteException(Severity.Error, errorMessage, ex);
            throw new HashUnavailableException(errorMessage, ex);
        }
        catch (PathTooLongException ex)
        {
            string errorMessage = String.Format("the path, {0}, is too long", path);
            //Log.WriteException(Severity.Error, errorMessage, ex);
            throw new HashUnavailableException(errorMessage, ex);
        }
        catch (IOException ex)
        {
            string errorMessage = "general IO error";
            //Log.WriteException(Severity.Error, errorMessage, ex);
            throw new HashUnavailableException(errorMessage, ex);
        }
        #endregion
        return fileBytes;
    }

    private static Stream GetFileStream(string path)
    {
        if (path == null || path == string.Empty)
        {
            HashUnavailableException hashUnavailableException = new HashUnavailableException("path to file is not set");
            //Log.Write(Severity.Error, hashUnavailableException.Message);
            throw hashUnavailableException;
        }

        try
        {
            return new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read, 1024, false);
        }
        #region catch
        catch (FileNotFoundException ex)
        {
            string errorMessage = String.Format("file not found at location {0}", path);
            //Log.WriteException(Severity.Error, errorMessage, ex);
            throw new HashUnavailableException(errorMessage, ex);
        }
        catch (DirectoryNotFoundException ex)
        {
            string errorMessage = String.Format("directory not found at location {0}", path);
            //Log.WriteException(Severity.Error, errorMessage, ex);
            throw new HashUnavailableException(errorMessage, ex);
        }
        catch (SecurityException ex)
        {
            string errorMessage = String.Format("the caller does not have the required permission to access file {0}", path);
            //Log.WriteException(Severity.Error, errorMessage, ex);
            throw new HashUnavailableException(errorMessage, ex);
        }
        catch (UnauthorizedAccessException ex)
        {
            string errorMessage = String.Format("the OS does not permit this access to file {0}", path);
            //Log.WriteException(Severity.Error, errorMessage, ex);
            throw new HashUnavailableException(errorMessage, ex);
        }
        catch (PathTooLongException ex)
        {
            string errorMessage = String.Format("the path, {0}, is too long", path);
            //Log.WriteException(Severity.Error, errorMessage, ex);
            throw new HashUnavailableException(errorMessage, ex);
        }
        catch (IOException ex)
        {
            string errorMessage = "general IO error";
            //Log.WriteException(Severity.Error, errorMessage, ex);
            throw new HashUnavailableException(errorMessage, ex);
        }
        #endregion
    }

    #endregion

    #region Overload Operators

    /// <summary>
    /// Overload == operator 
    /// </summary>
    /// <param name="left">The left hand side of an == statement</param>
    /// <param name="right">The right hand side of an == statement</param>
    /// <returns>True if the two objects match, else false</returns>
    /// <revisionHistory> 
    ///		<revision author="Andy Evans, Leigh Willoughby" date="23/11/2005">Initial Version</revision>
    ///     <revision author="Andy Evans" date="29/11/2005">Added code to check for null parameters</revision>
    ///     <revision author="Morgan Skinner" date="23/01/2006">Recoded to compare the internal objects</revision>
    ///	</revisionHistory>
    public static bool operator ==(Hash left, Hash right)
    {
        bool equal = false;

        if (object.Equals(left, null) || object.Equals(right, null))
        {
            equal = object.Equals(left, null) && object.Equals(right, null);
        }
        else
        {
            equal = left._sha1Hash == right._sha1Hash && left._sha256Hash == right._sha256Hash && left._sha512Hash == right._sha512Hash;
        }
        return equal;
    }

    /// <summary>
    /// Overridden implementation
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public override bool Equals(object obj)
    {
        if (object.Equals(obj, null))
            throw new ArgumentNullException("obj");

        return this == (Hash)obj;
    }

    /// <summary>
    /// Return the hashcode of the object
    /// </summary>
    /// <returns>A hashcode based on the hashed data</returns>
    public override int GetHashCode()
    {
        if (_hashAlgorithm == HashAlgo.SHA1)
            return base.GetHashCode() ^ _sha1Hash.GetHashCode();
        else if (_hashAlgorithm == HashAlgo.SHA256)
            return base.GetHashCode() ^ _sha256Hash.GetHashCode();
        else if (_hashAlgorithm == HashAlgo.MD5)
            return base.GetHashCode() ^ _md5Hash.GetHashCode();
        else
            return base.GetHashCode() ^ _sha512Hash.GetHashCode();
    }

    /// <summary>
    /// Overload != operator 
    /// </summary>
    /// <param name="firstHash"></param>
    /// <param name="secondHash"></param>
    /// <returns>True if the two objects don't match, else false</returns>
    /// <revisionHistory> 
    ///		<revision author="Andy Evans, Leigh Willoughby" date="23/11/2005">Initial Version</revision>
    ///     <revision author="Andy Evans" date="29/11/2005">Added code to check for null parameters</revision>
    ///	</revisionHistory>
    public static bool operator !=(Hash firstHash, Hash secondHash)
    {
        return !(firstHash == secondHash);
    }

    #endregion
}

/// <summary>
/// The Hash Algorithm to be used
/// </summary>
public enum HashAlgo
{
    SHA1 = 0,
    SHA256 = 1,
    SHA512 = 2,
    MD5 = 3
}
