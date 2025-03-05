using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace BL.Xml.Core
{
    /// <summary>
    /// Contains some simple validation static methods
    /// </summary>
    public static class ValidatorHelperMethods
    {
        /// <summary>
        /// Simple validation of an XDocument
        /// </summary>
        /// <param name="xDocumentToValidate">The XDocument that needs to be validated.</param>
        /// <param name="xDocumentName">The name of the XDocument parameter if an exception is thrown.</param>
        /// <remarks>Throws ArgumentNullException if xDocumentToValidate is null
        /// or xDocumentToValidate.Root is null.</remarks>
        // ReSharper disable UnusedParameter.Global
        public static void ValidateXDocument(XDocument xDocumentToValidate, string xDocumentName)
        // ReSharper restore UnusedParameter.Global
        {
            if (xDocumentToValidate == null)
            {
                throw new ArgumentNullException(String.Format("{0} cannot be missing.", xDocumentName), xDocumentName);
            }

            if (xDocumentToValidate.Root == null)
            {
                throw new ArgumentNullException(String.Format("{0} does not have a root element.", xDocumentName), xDocumentName);
            }
        }

        /// <summary>
        /// Simple validation of an XElement
        /// </summary>
        /// <param name="xElementToValidate">The XElement that needs to be validated.</param>
        /// <param name="xElementName">The name of the XElement parameter if an exception is thrown.</param>
        /// <remarks>Throws ArgumentNullException if xElementToValidate is null.</remarks>
        // ReSharper disable UnusedParameter.Global
        public static void ValidateXElement(XElement xElementToValidate, string xElementName)
        // ReSharper restore UnusedParameter.Global
        {
            if (xElementToValidate == null)
            {
                throw new ArgumentNullException(String.Format("{0} cannot be missing.", xElementName), xElementName);
            }
        }

        /// <summary>
        /// Simple validation of an XNamespace
        /// </summary>
        /// <param name="xNamespaceToValidate">The XNamespace that needs to be validated.</param>
        /// <param name="xNamespaceName">The name of the namespace parameter if an exception is thrown.</param>
        /// <remarks>Throws ArgumentNullException if xNamespaceToValidate is null.</remarks>
        // ReSharper disable UnusedParameter.Global
        public static void ValidateXNamespace(XNamespace xNamespaceToValidate, string xNamespaceName)
        // ReSharper restore UnusedParameter.Global
        {
            if (xNamespaceToValidate == null)
            {
                throw new ArgumentNullException(String.Format("{0} cannot be missing.", xNamespaceName), xNamespaceName);
            }
        }

        /// <summary>
        /// Simple validation of a string parameter
        /// </summary>
        /// <param name="stringToValidate">The string that you want to validate</param>
        /// <param name="stringName">The name of the string parameter that will be used if an exception is thrown.</param>
        /// <remarks>Throws ArgumentException if stringToValidate is empty or missing.</remarks>
        // ReSharper disable UnusedParameter.Global
        public static void ValidateString(string stringToValidate, string stringName)
        // ReSharper restore UnusedParameter.Global
        {
            if (String.IsNullOrEmpty(stringToValidate))
            {
                throw new ArgumentException(String.Format("{0} cannot be empty or missing.", stringName), stringName);
            }
        }

        /// <summary>
        /// Simple validation of an int parameter against an upper limit.
        /// </summary>
        /// <param name="intToValidate">The int that you want to validate.</param>
        /// <param name="intName">The name of the int parameter that will be used if an exception is thrown.</param>
        /// <param name="upperLimit">The Upper limit for the parameter e.g. If upperLimit = 3, intToValidate = 1, 2, 3 are all OK, but 4 is not).</param>
        /// <remarks>Throws ArgumentOutOfRangeException if intToValidate is greater than upperLimit.</remarks>
        public static void ValidateIntUpper(int intToValidate, string intName, int upperLimit)
        {
            // Check is NOT >=
            if (intToValidate > upperLimit)
            {
                throw new ArgumentOutOfRangeException(intName, intToValidate, String.Format("{0} cannot be greater than {1}.", upperLimit));
            }
        }

        /// <summary>
        /// Simple validation of an int parameter against a lower limit.
        /// </summary>
        /// <param name="intToValidate">The int that you want to validate.</param>
        /// <param name="intName">The name of the int parameter that will be used if an exception is thrown.</param>
        /// <param name="lowerLimit">The Lower limit for the parameter e.g. If lowerLimit = 0, intToValidate = 0, 1, are all OK, but -1 is not).</param>
        /// <remarks>Throws ArgumentOutOfRangeException if intToValidate is less than lowerLimit.</remarks>
        public static void ValidateIntLower(int intToValidate, string intName, int lowerLimit)
        {
            // Check is NOT <=
            if (intToValidate < lowerLimit)
            {
                throw new ArgumentOutOfRangeException(intName, intToValidate, String.Format("{0} cannot be less than {1}.", lowerLimit));
            }
        }

        /// <summary>
        /// Simple validation of an int parameter against a lower limit.
        /// </summary>
        /// <param name="intToValidate">The int that you want to validate.</param>
        /// <param name="intName">The name of the int parameter that will be used if an exception is thrown.</param>
        /// <param name="upperLimit">The Upper limit for the parameter e.g. If upperLimit = 3, intToValidate = 1, 2, 3 are all OK, but 4 is not).</param>
        /// <param name="lowerLimit">The Lower limit for the parameter e.g. If lowerLimit = 0, intToValidate = 0, 1, are all OK, but -1 is not).</param>
        /// <remarks>Throws ArgumentOutOfRangeException if intToValidate is less than lowerLimit OR
        /// intToValidate is greater than upperLimit.</remarks>
        public static void ValidateIntUpperAndLower(int intToValidate, string intName, int upperLimit, int lowerLimit)
        {
            // Cheeck is NOT <= or >=
            if (intToValidate > upperLimit || intToValidate < lowerLimit)
            {
                throw new ArgumentOutOfRangeException(intName, intToValidate,
                    String.Format("{0} cannot be greater than {1} or less than {2}.", intName, upperLimit, lowerLimit));
            }
        }
    }
}
