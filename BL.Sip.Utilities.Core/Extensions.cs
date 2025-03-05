using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BL.Sip.Utilities.Core
{
    public static class Extensions
    {
        public static string CombinePath(this string directory, string fileName)
        {
            try
            {
                var uri = new Uri(directory);
                return "file" == uri.Scheme
                    ? Path.Combine(directory, fileName)
                    : new Uri(new Uri(directory.EndsWith(@"/") ? directory : directory + @"/"), fileName)
                        .AbsoluteUri;
            }
            catch (UriFormatException)
            {
                return Path.Combine(directory, fileName);
            }
        }

        /// <summary>
        /// Prepends the static string "ark:/81055/" to a unique identifier to create a British Library ARK.
        /// </summary>
        /// <param name="uid">A unique identifier, e.g. 123_abc</param>
        /// <returns>A British Library ARK, e.g. ark:/81055/123_abc</returns>
        public static string ToArk(this string uid)
        {
            if (String.IsNullOrEmpty(uid))
                return uid;

            uid = uid.Trim().TrimStart('/'); // trim whitespace and leading forward slash, if present
            return String.Format("ark:/81055/{0}", uid);
        }

        /// <summary>
        /// Removes the static part of a British Library ARK, returning the variable part.
        /// </summary>
        /// <param name="ark">A British Library ARK, e.g. ark:/81055/123_abc</param>
        /// <returns>The dynamic part of the ARK, e.g. 123_abc</returns>
        public static string ToUid(this string ark)
        {
            var start = ark.LastIndexOf('/');
            return -1 == start ? ark : ark.Substring(start + 1);
        }
    }
}
