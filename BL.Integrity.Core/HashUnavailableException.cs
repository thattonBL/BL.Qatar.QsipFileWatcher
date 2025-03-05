using System;

namespace BL.Integrity.Core
{
    /// <summary>
    /// Exception that is thrown when a Hash cannot be created
    /// </summary>
    /// <remarks>
    /// No remarks
    /// </remarks>
    ///	<revisionHistory> 
    ///		<revision author="Andy Evans" date="26/11/2005">Initial Version</revision>
    ///	</revisionHistory>
    public class HashUnavailableException : ApplicationException
    {
        #region constructors
        /// <summary>
        /// Construct the object
        /// </summary>
        public HashUnavailableException() : base()
        {
        }

        /// <summary>
        /// Constructor with error message
        /// </summary>
        /// <param name="message"></param>
        public HashUnavailableException(string message) : base(message)
        {
        }

        /// <summary>
        /// Constructor with error message and inner exception
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public HashUnavailableException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        #endregion
    }
}
