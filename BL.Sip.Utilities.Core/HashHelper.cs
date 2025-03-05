using BL.Integrity.Core;

namespace BL.Sip.Utilities.Core
{
    public class HashHelper
    {
        public const string Sha1Hash = "SHA1";
        public const string Sha256Hash = "SHA256";
        public const string Sha512Hash = "SHA512";
        public const string Md5Hash = "MD5";

        public static HashAlgo SelectHashAlgo(string hashAlgorithm)
        {
            if (String.IsNullOrEmpty(hashAlgorithm))
                return HashAlgo.SHA256;

            HashAlgo hashAlgo;
            switch (hashAlgorithm.ToUpper().Trim())
            {
                case Sha1Hash:
                    hashAlgo = HashAlgo.SHA1;
                    break;
                case Sha256Hash:
                    hashAlgo = HashAlgo.SHA256;
                    break;
                case Sha512Hash:
                    hashAlgo = HashAlgo.SHA512;
                    break;
                case Md5Hash:
                    hashAlgo = HashAlgo.MD5;
                    break;
                default:
                    hashAlgo = HashAlgo.SHA256;
                    break;
            }
            return hashAlgo;
        }

        public static string HashAlgorithmToPremisFormat(string hashAlgorithm)
        {
            switch (hashAlgorithm.ToUpper().Trim())
            {
                case Sha1Hash:
                    return "SHA-1";
                case Sha256Hash:
                    return "SHA-256";
                case Sha512Hash:
                    return "SHA-512";
            }
            return hashAlgorithm;
        }

        public static byte[] GetFileBytes(string pathToFile)
        {
            FileStream fs = new FileStream(pathToFile, FileMode.Open, FileAccess.Read, FileShare.Read);
            int filesize = Convert.ToInt32(CalculateFileSize(pathToFile));
            byte[] data = new byte[filesize];
            int countBytesRead = fs.Read(data, 0, filesize);
            return data;
        }

        public static long CalculateFileSize(string path)
        {
            long fileSize = 0;
            FileInfo fileInfo = new FileInfo(path);

            if (fileInfo.Exists)
            {
                fileSize = fileInfo.Length;
            }
            return fileSize;
        }


        public static byte[] GetHashBytes(Hash hd, string hashAlgorithm)
        {
            byte[] hashByteArray;
            switch (hashAlgorithm.ToUpper().Trim())
            {
                case Sha1Hash:
                    hashByteArray = hd.Sha1Hash.HashBytes;
                    break;
                case Sha256Hash:
                    hashByteArray = hd.Sha256Hash.HashBytes;
                    break;
                case Sha512Hash:
                    hashByteArray = hd.Sha512Hash.HashBytes;
                    break;
                case Md5Hash:
                    hashByteArray = hd.MD5Hash.HashBytes;
                    break;
                default:
                    hashByteArray = hd.Sha256Hash.HashBytes;
                    break;
            }
            return hashByteArray;
        }

        public static string NormaliseHashBytes(byte[] hashByteArray)
        {
            return NormaliseHashString(BitConverter.ToString(hashByteArray));
        }

        public static byte[] DenormaliseHashString(string hashString)
        {
            return Enumerable.Range(0, hashString.Length / 2)
                             .Select(i => hashString.Substring(i * 2, 2))
                             .Select(i => Convert.ToByte(i, 16))
                             .ToArray();
        }

        private static string NormaliseHashString(string hashString)
        {
            return hashString.Replace("-", "").ToLower().Trim();
        }
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
}
