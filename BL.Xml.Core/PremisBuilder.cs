using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace BL.Xml.Core
{
    public static class PremisBuilder
    {
        #region Premis Related Methods

        /// <summary>
        /// Will create a premis object XElement without a relationship XElement.
        /// </summary>
        /// <param name="premisObjectInfo">Object which holds the the values to be inserted into the premis object XElement.</param>
        /// <returns>The newly created XElement.</returns>
        public static XElement CreatePremisObject(PremisObjectInfo premisObjectInfo)
        {
            if (premisObjectInfo == null)
            {
                throw new ArgumentNullException("premisObjectInfo", "premisObjectInfo cannot be missing");
            }

            var objectElement = CreatePremisObjectOuterElement(premisObjectInfo.PremisObjectType);
            var objectIdentifierElement = CreatePremisObjectIdentifierElement(premisObjectInfo.ObjectIdentifierType, premisObjectInfo.ObjectIdentifierValue);

            var objectCharacteristicsElement = CreateEmptyPremisObjectCharacteristicsElement();

            var compositionLevelElement = CreatePremisCompositionLevelElement(premisObjectInfo.CompositionLevel);
            objectCharacteristicsElement.Add(compositionLevelElement);

            XElement fixityElement = null;
            if (!String.IsNullOrEmpty(premisObjectInfo.MessageDigestAlgorithm) && !String.IsNullOrEmpty(premisObjectInfo.MessageDigest))
            {
                fixityElement = CreatePremisFixityElement(premisObjectInfo.MessageDigestAlgorithm, premisObjectInfo.MessageDigest);
            }
            if (fixityElement != null)
            {
                objectCharacteristicsElement.Add(fixityElement);
            }

            // Metadata only submissions do not have a size defined (it is empty). If so,
            // we should not create the element at all
            if (!String.IsNullOrEmpty(premisObjectInfo.Size))
            {
                var sizeElememt = CreatePremisSizeElement(premisObjectInfo.Size);
                objectCharacteristicsElement.Add(sizeElememt);
            }

            var formatElement = CreatePremisFormatElementWithFormatDesignationChild(premisObjectInfo.FormatName);
            objectCharacteristicsElement.Add(formatElement);

            var originalNameElement = CreatePremisObjectOriginalNameElement(premisObjectInfo.PremisOriginalName);

            objectElement.Add(objectIdentifierElement);
            objectElement.Add(objectCharacteristicsElement);
            objectElement.Add(originalNameElement);

            return objectElement;
        }

        /// <summary>
        /// Will create a premis object XElement with a relationship XElement.
        /// </summary>
        /// <param name="premisObjectInfo">Object which holds the the values to be inserted into the premis object XElement.</param>
        /// <param name="premisRelationshipInfo">Object which holds the the values to be inserted into the premis relationship XElement.</param> 
        /// <returns>The newly created xElement.</returns>
        public static XElement CreatePremisObject(PremisObjectInfo premisObjectInfo, PremisRelationshipInfo premisRelationshipInfo)
        {
            if (premisRelationshipInfo == null)
            {
                throw new ArgumentNullException("premisRelationshipInfo", "premisRelationshipInfo cannot be missing");
            }

            var objectElement = CreatePremisObject(premisObjectInfo);

            var relationshipElement = CreatePremisRelationshipElement(premisRelationshipInfo.RelationshipType, premisRelationshipInfo.RelationshipSubType);
            var relatedObjectIdentificationElement = CreatePremisRelatedObjectIdentificationElement(premisRelationshipInfo.RelatedObjectIdentifierType, premisRelationshipInfo.RelatedObjectIdentifierValue);
            var relatedEventIdentification = CreatePremisRelatedEventIdentificationElement(premisRelationshipInfo.RelatedEventIdentifierType, premisRelationshipInfo.RelatedEventIdentifierValue);
            relationshipElement.Add(relatedObjectIdentificationElement);
            relationshipElement.Add(relatedEventIdentification);

            objectElement.Add(relationshipElement);
            return objectElement;
        }

        /// <summary>
        /// Will create a premis object xElement with its type XAttribute defined by the 'premisType' parameter.
        /// </summary>
        /// <param name="premisType">The premis type that this XElement will describe.</param>
        /// <returns>The newly created XElement.</returns>
        internal static XElement CreatePremisObjectOuterElement(string premisType)
        {
            ValidatorHelperMethods.ValidateString(premisType, "premisType");

            return new XElement(Namespaces.Premis + ("object"), new XAttribute(Namespaces.Xsie + "type", premisType));
        }

        /// <summary>
        /// Will create a blank premis original name xElement.
        /// </summary>
        /// <returns>The newly created xElement.</returns>
        internal static XElement CreatePremisObjectOriginalNameElement()
        {
            return new XElement(Namespaces.Premis + "originalName");
        }

        /// <summary>
        /// Will create a premis original name xElement with contents set by the requested by the originalNameValue parameter.
        /// </summary>
        /// <param name="originalName">The name to insert into this xElement.</param>
        /// <returns>The newly created xElement.</returns>
        internal static XElement CreatePremisObjectOriginalNameElement(string originalName)
        {
            ValidatorHelperMethods.ValidateString(originalName, "originalName");

            return new XElement(Namespaces.Premis + "originalName", originalName);
        }

        /// <summary>
        /// Will create a blank premis Object Identifier name xElement.
        /// </summary>
        /// <returns>The newly created xElement.</returns>
        internal static XElement CreatePremisObjectIdentifierElement()
        {
            return new XElement(Namespaces.Premis + "objectIdentifier",
                                new XElement(Namespaces.Premis + "objectIdentifierType"),
                                new XElement(Namespaces.Premis + "objectIdentifierValue"));
        }

        /// <summary>
        /// Will create a premis Object Identifier name xElement with contents defined by the 'typeValue' & 'indentifierValue' parameters.
        /// </summary>
        /// <param name="type">The value to insert into the 'objectIdentifierType' xElement.</param>
        /// <param name="identifier">The value to insert into the 'objectIdentifierValue' xElement.</param>
        /// <returns>The newly created xElement.</returns>
        internal static XElement CreatePremisObjectIdentifierElement(string type, string identifier)
        {
            ValidatorHelperMethods.ValidateString(type, "type");
            ValidatorHelperMethods.ValidateString(identifier, "identifier");

            return new XElement(Namespaces.Premis + "objectIdentifier",
                                new XElement(Namespaces.Premis + "objectIdentifierType", type),
                                new XElement(Namespaces.Premis + "objectIdentifierValue", identifier));
        }

        /// <summary>
        /// Will create a blank premis object characteristics xElement.
        /// </summary>
        /// <returns>The newly created xElement.</returns>
        internal static XElement CreatePremisObjectCharacteristicsElement(string size)
        {
            // TODO: Can size be empty? (not missing)
            ValidatorHelperMethods.ValidateString(size, "size");

            return new XElement(Namespaces.Premis + "objectCharacteristics",
                                new XElement(Namespaces.Premis + "compositionLevel", "0"),
                                new XElement(Namespaces.Premis + "size", size));
        }

        /// <summary>
        /// Will create a blank premis object characteristics xElement.
        /// </summary>
        internal static XElement CreateEmptyPremisObjectCharacteristicsElement()
        {
            return new XElement(Namespaces.Premis + "objectCharacteristics");
        }

        /// <summary>
        /// Will create a premis object characteristics xElement with its content defined by the 'compositionLevelValue' and 'sizeValue' parameters.
        /// </summary>
        /// <param name="compositionLevel">The value to insert into the 'compositionLevelValue' xElement.</param>
        /// <param name="size">The value to insert into the 'sizeValue' xElement.</param>
        /// <returns>The newly created xElement.</returns>
        internal static XElement CreatePremisObjectCharacteristicsElement(string compositionLevel, string size)
        {
            ValidatorHelperMethods.ValidateString(compositionLevel, "compositionLevel");

            // TODO: Can size be empty? (not missing)
            ValidatorHelperMethods.ValidateString(size, "size");

            return new XElement(Namespaces.Premis + "objectCharacteristics",
                                new XElement(Namespaces.Premis + "compositionLevel", compositionLevel),
                                new XElement(Namespaces.Premis + "size", size));
        }

        /// <summary>
        /// Will create a premis compositionLevel XElement.
        /// </summary>
        /// <param name="compositionLevel">The value to insert into the 'compositionLevelValue' XElement.</param> 
        internal static XElement CreatePremisCompositionLevelElement(string compositionLevel)
        {
            ValidatorHelperMethods.ValidateString(compositionLevel, "compositionLevel");

            return new XElement(Namespaces.Premis + "compositionLevel", compositionLevel);
        }

        /// <summary>
        /// Will create a blank premis fixity XElement.
        /// </summary>
        /// <returns>The newly created XElement.</returns>
        internal static XElement CreatePremisFixityElement()
        {
            return new XElement(Namespaces.Premis + "fixity",
                                new XElement(Namespaces.Premis + "messageDigestAlgorithm"),
                                new XElement(Namespaces.Premis + "messageDigest"));
        }

        /// <summary>
        /// Will create a premis fixity XElement with its contents defined by the 'messageDigestAlgorithmValue' and 'messageDigestValue' parameters.
        /// </summary>
        /// <param name="messageDigestAlgorithmValue">The value to insert into the 'messageDigestAlgorithm' XElement.</param>
        /// <param name="messageDigestValue">The value to insert into the 'messageDigest' XElement.</param>
        /// <returns>The newly created XElement.</returns>
        public static XElement CreatePremisFixityElement(string messageDigestAlgorithmValue, string messageDigestValue)
        {
            return new XElement(Namespaces.Premis + "fixity",
                                new XElement(Namespaces.Premis + "messageDigestAlgorithm", messageDigestAlgorithmValue),
                                new XElement(Namespaces.Premis + "messageDigest", messageDigestValue));
        }

        /// <summary>
        /// Will create a blank premis object characteristics XElement.
        /// </summary>
        /// <param name="size">The value to insert into the size XElement.</param> 
        internal static XElement CreatePremisSizeElement(string size)
        {
            // TODO: Can size be empty or missing?
            return new XElement(Namespaces.Premis + "size", size);
        }

        /// <summary>
        /// Will create a blank premis format XElement.
        /// </summary>
        /// <returns>The newly created XElement.</returns>
        internal static XElement CreatePremisFormatElement()
        {
            return new XElement(Namespaces.Premis + "format",
                                new XElement(Namespaces.Premis + "formatDesignation"),
                                new XElement(Namespaces.Premis + "formatName"));
        }

        /// <summary>
        /// Will create a premis format XElement with its defined by the 'formatDesignation' and 'formatName' parameters.
        /// </summary>
        /// <param name="formatDesignation">The value to insert into the 'formatDesignation' XElement.</param>
        /// <param name="formatName">The value to insert into the 'formatName' XElement.</param>
        /// <returns>The newly created XElement.</returns>
        internal static XElement CreatePremisFormatElement(string formatDesignation, string formatName)
        {
            ValidatorHelperMethods.ValidateString(formatDesignation, "formatDesignation");
            ValidatorHelperMethods.ValidateString(formatName, "formatName");

            return new XElement(Namespaces.Premis + "format",
                                new XElement(Namespaces.Premis + "formatDesignation", formatDesignation),
                                new XElement(Namespaces.Premis + "formatName", formatName));
        }

        /// <summary>
        /// Will create a premis format XElement with a format designtion and format name XElement
        /// </summary>
        /// <param name="formatName">The value to insert into the 'formatName' XElement.</param>
        /// <returns>The newly created XElement.</returns>
        internal static XElement CreatePremisFormatElementWithFormatDesignationChild(string formatName)
        {
            ValidatorHelperMethods.ValidateString(formatName, "formatName");

            return new XElement(Namespaces.Premis + "format",
                                new XElement(Namespaces.Premis + "formatDesignation",
                                             new XElement(Namespaces.Premis + "formatName", formatName)));
        }

        /// <summary>
        /// Will create a premis format XElement with its defined 'formatName' parameter.
        /// </summary>
        /// <param name="formatName">The value to insert into the 'formatName' XElement.</param>
        /// <returns>The newly created XElement.</returns>
        internal static XElement CreatePremisFormatElement(string formatName)
        {
            ValidatorHelperMethods.ValidateString(formatName, "formatName");

            return new XElement(Namespaces.Premis + "format",
                                new XElement(Namespaces.Premis + "formatName", formatName));
        }

        /// <summary>
        /// Will create a blank premis creating application XElement.
        /// </summary>
        /// <returns>The newly created XElement.</returns>
        internal static XElement CreatePremisCreatingAppElement()
        {
            return new XElement(Namespaces.Premis + "creatingApplication",
                                new XElement(Namespaces.Premis + "creatingApplicationName"),
                                new XElement(Namespaces.Premis + "dateCreatedByApplication"));
        }

        /// <summary>
        /// Will create a premis creating application XElement with its content defined by 
        /// the 'creatingApplicationNameValue' and 'dateCreatedByApplicationValue' parameters.
        /// </summary>
        /// <param name="creatingApplicationName">The value to be inserted into the 'creatingApplicationName' XElement.</param>
        /// <param name="dateCreatedByApplication">The value to be inserted into the 'dateCreatedByApplication' XElement.</param>
        /// <param name="version">The value to be inserted into the 'version' XElement.</param>
        /// <returns>The newly created XElement.</returns>
        internal static XElement CreatePremisCreatingAppElement(string creatingApplicationName, string dateCreatedByApplication, string version = "")
        {
            ValidatorHelperMethods.ValidateString(creatingApplicationName, "creatingApplicationName");
            ValidatorHelperMethods.ValidateString(dateCreatedByApplication, "dateCreatedByApplication");

            var xElement = new XElement(Namespaces.Premis + "creatingApplication");
            var xNameElement = new XElement(Namespaces.Premis + "creatingApplicationName", creatingApplicationName);

            xElement.Add(xNameElement);

            if (!String.IsNullOrEmpty(version))
            {
                var versionElement = new XElement(Namespaces.Premis + "creatingApplicationVersion", version);
                xElement.Add(versionElement);
            }

            var secondSubElement = new XElement(Namespaces.Premis + "dateCreatedByApplication", dateCreatedByApplication);
            xElement.Add(secondSubElement);

            return xElement;
        }

        /// <summary>
        /// Will create a blank premis relationship XElement.
        /// </summary>
        /// <returns>The newly created XElement.</returns>
        internal static XElement CreatePremisRelationshipElement()
        {
            return new XElement(Namespaces.Premis + "relationship",
                                new XElement(Namespaces.Premis + "relationshipType"),
                                new XElement(Namespaces.Premis + "relationshipSubType"));
        }

        /// <summary>
        /// Will create a premis relationship XElement with its content defined by 
        /// the 'relationshipTypeValue' and 'relationshipSubTypeValue' parameters.
        /// </summary>
        /// <param name="relationshipType">The value to be inserted into the 'relationshipTypeValue' XElement.</param>
        /// <param name="relationshipSubType">The value to be inserted into the 'relationshipSubTypeValue' XElement.</param>
        /// <returns>The newly created XElement.</returns>
        internal static XElement CreatePremisRelationshipElement(string relationshipType, string relationshipSubType)
        {
            ValidatorHelperMethods.ValidateString(relationshipType, "relationshipType");
            ValidatorHelperMethods.ValidateString(relationshipSubType, "relationshipSubType");

            return new XElement(Namespaces.Premis + "relationship",
                                new XElement(Namespaces.Premis + "relationshipType", relationshipType),
                                new XElement(Namespaces.Premis + "relationshipSubType", relationshipSubType));
        }

        /// <summary>
        /// Will create a blank related object identification XElement.
        /// </summary>
        /// <returns>The newly created XElement.</returns>
        internal static XElement CreatePremisRelatedObjectIdentificationElement()
        {
            return new XElement(Namespaces.Premis + "relatedObjectIdentification",
                                new XElement(Namespaces.Premis + "relatedObjectIdentifierType"),
                                new XElement(Namespaces.Premis + "relatedObjectIdentifierValue"));
        }

        /// <summary>
        /// Will create a related object identification XElement with its contents defined by 
        /// the 'relatedObjectIdentifierTypeValue' and 'relatedObjectIdentifierValue' parameters.
        /// </summary>
        /// <param name="relatedObjectIdentifierType">The value to be inserted into the 'relatedObjectIdentifierTypeValue' XElement.</param>
        /// <param name="relatedObjectIdentifierValue">The value to be inserted into the 'relatedObjectIdentifierValue' XElement.</param>
        /// <returns>The newly created XElement.</returns>
        internal static XElement CreatePremisRelatedObjectIdentificationElement(string relatedObjectIdentifierType, string relatedObjectIdentifierValue)
        {
            ValidatorHelperMethods.ValidateString(relatedObjectIdentifierType, "relatedObjectIdentifierType");
            ValidatorHelperMethods.ValidateString(relatedObjectIdentifierValue, "relatedObjectIdentifierValue");

            return new XElement(Namespaces.Premis + "relatedObjectIdentification",
                                new XElement(Namespaces.Premis + "relatedObjectIdentifierType", relatedObjectIdentifierType),
                                new XElement(Namespaces.Premis + "relatedObjectIdentifierValue", relatedObjectIdentifierValue));
        }

        /// <summary>
        /// Will create a blank related event identification XElement.
        /// </summary>
        /// <returns>The newly created XElement.</returns>
        internal static XElement CreatePremisRelatedEventIdentificationElement()
        {
            return new XElement(Namespaces.Premis + "relatedEventIdentification",
                                new XElement(Namespaces.Premis + "relatedEventIdentifierType"),
                                new XElement(Namespaces.Premis + "relatedEventIdentifierValue"));
        }

        /// <summary>
        /// Will create a related event identification XElement with its content defined by 
        /// the 'relatedEventIdentifierTypeValue' and 'relatedEventIdentifierValue' parameters.
        /// </summary>
        /// <param name="relatedEventIdentifierType">The value to be inserted in to the 'relatedEventIdentifierTypeValue' XElement.</param>
        /// <param name="relatedEventIdentifierValue">The value to be inserted in to the 'relatedEventIdentifierValue' XElement.</param>
        /// <returns>The newly created XElement.</returns>
        internal static XElement CreatePremisRelatedEventIdentificationElement(string relatedEventIdentifierType, string relatedEventIdentifierValue)
        {
            ValidatorHelperMethods.ValidateString(relatedEventIdentifierType, "relatedEventIdentifierType");
            ValidatorHelperMethods.ValidateString(relatedEventIdentifierValue, "relatedEventIdentifierValue");

            return new XElement(Namespaces.Premis + "relatedEventIdentification",
                                new XElement(Namespaces.Premis + "relatedEventIdentifierType", relatedEventIdentifierType),
                                new XElement(Namespaces.Premis + "relatedEventIdentifierValue", relatedEventIdentifierValue));
        }

        #endregion

        #region Premis Event Related Methods

        /// <summary>
        /// Will create a premis event XElement.
        /// </summary>
        /// <param name="premisEventInfo">Object which holds the values to be inserted into the premis event XElement.</param>
        /// <returns>The newly created XElement</returns>
        public static XElement CreatePremisEventElement(PremisEventInfo premisEventInfo)
        {
            if (premisEventInfo == null)
            {
                throw new ArgumentNullException("premisEventInfo", "premisEventInfo cannot be missing");
            }
            var eventElement = CreatePremisEventElement(premisEventInfo.EventType, premisEventInfo.EventDateTime);

            var eventIdentifierElement = CreatePremisEventIdentifierElement(premisEventInfo.EventIdentifierType, premisEventInfo.EventIdentifierValue);
            var eventOutcomeInformationElement = CreatePremisEventOutcomeInformationElement(premisEventInfo.EventOutcome);
            var linkingAgentIdentifierElement = CreatePremisLinkingAgentIdentifierElement(premisEventInfo.LinkingAgentIdentifierType, premisEventInfo.LinkingAgentIdentifierValue);
            eventElement.AddFirst(eventIdentifierElement);
            eventElement.Add(eventOutcomeInformationElement);
            eventElement.Add(linkingAgentIdentifierElement);

            return eventElement;
        }


        /// <summary>
        /// Will create a premis event XElement with an outcome detail but without a linking Agent Identifier.
        /// </summary>
        /// <param name="premisEventInfo">Object which holds the values to be inserted into the premis event XElement.</param>
        /// <param name="premisEventInfoWithOutcomeDetail">Object which holds the values to be inserted into the eventOutcomeDetailNote XElement.</param>
        /// <returns>The newly created XElement</returns>
        public static XElement CreatePremisEventElementWithOutcomeDetailWithoutAgentIdentifier(PremisEventInfoWithOutcomeDetail premisEventInfoWithOutcomeDetail)
        {
            if (premisEventInfoWithOutcomeDetail == null)
            {
                throw new ArgumentNullException("premisEventInfoWithOutcomeDetail", "premisEventInfoWithOutcomeDetail cannot be missing");
            }
            var eventElement = CreatePremisEventElement(premisEventInfoWithOutcomeDetail.EventType, premisEventInfoWithOutcomeDetail.EventDateTime);

            var eventIdentifierElement = CreatePremisEventIdentifierElement(premisEventInfoWithOutcomeDetail.EventIdentifierType, premisEventInfoWithOutcomeDetail.EventIdentifierValue);
            var eventOutcomeInformationElement = CreatePremisEventOutcomeInformationElement(premisEventInfoWithOutcomeDetail.EventOutcome, premisEventInfoWithOutcomeDetail.EventOutcomeDetailNote);

            eventElement.AddFirst(eventIdentifierElement);
            eventElement.Add(eventOutcomeInformationElement);

            return eventElement;
        }

        /// <summary>
        /// Will create a blank premis event XElement.
        /// </summary>
        /// <returns>The newly created XElement.</returns>
        internal static XElement CreatePremisEventElement()
        {
            return new XElement(Namespaces.Premis + "event",
                                new XElement(Namespaces.Premis + "eventType"),
                                new XElement(Namespaces.Premis + "eventDateTime"));
        }

        /// <summary>
        /// Will create a premis event XElement with its contents defined by 
        /// the 'eventTypeValue' and 'eventDateTimeValue' parameters.
        /// </summary>
        /// <param name="eventType">The value to be inserted into the 'eventTypeValue' XElement.</param>
        /// <param name="eventDateTime">The value to be inserted into the 'eventDateTimeValue' XElement.</param>
        /// <returns>The newly created XElement.</returns>
        internal static XElement CreatePremisEventElement(string eventType, string eventDateTime)
        {
            ValidatorHelperMethods.ValidateString(eventType, "eventType");
            ValidatorHelperMethods.ValidateString(eventDateTime, "eventDateTime");

            return new XElement(Namespaces.Premis + "event",
                                new XElement(Namespaces.Premis + "eventType", eventType),
                                new XElement(Namespaces.Premis + "eventDateTime", eventDateTime));
        }

        /// <summary>
        /// Will create a blank premis event identifier XElement.
        /// </summary>
        /// <returns>The newly created XElement.</returns>
        internal static XElement CreatePremisEventIdentifierElement()
        {
            return new XElement(Namespaces.Premis + "eventIdentifier",
                                new XElement(Namespaces.Premis + "eventIdentifierType"),
                                new XElement(Namespaces.Premis + "eventIdentifierValue"));
        }

        /// <summary>
        /// Will create a premis event identifier XElement with its contents defined by 
        /// the 'eventIdentifierTypeValue' and 'eventIdentifierValue' parameters.
        /// </summary>
        /// <param name="eventIdentifierType">The value to be inserted in to the 'eventIdentifierTypeValue' XElement. </param>
        /// <param name="eventIdentifierValue">The value to be inserted in to the 'eventIdentifierValue' XElement.</param>
        /// <returns>The newly created XElement.</returns>
        internal static XElement CreatePremisEventIdentifierElement(string eventIdentifierType, string eventIdentifierValue)
        {
            ValidatorHelperMethods.ValidateString(eventIdentifierType, "eventIdentifierType");
            ValidatorHelperMethods.ValidateString(eventIdentifierValue, "eventIdentifierValue");

            return new XElement(Namespaces.Premis + "eventIdentifier",
                                new XElement(Namespaces.Premis + "eventIdentifierType", eventIdentifierType),
                                new XElement(Namespaces.Premis + "eventIdentifierValue", eventIdentifierValue));
        }

        /// <summary>
        /// Will create a blank premis event outcome information XElement.
        /// </summary>
        /// <returns>The newly created XElement.</returns>
        internal static XElement CreatePremisEventOutcomeInformationElement()
        {
            return new XElement(Namespaces.Premis + "eventOutcomeInformation",
                                new XElement(Namespaces.Premis + "eventOutcome"));
        }

        /// <summary>
        /// Will create a premis event outcome information XElement with its contents defined by 
        /// the 'eventOutcomeValue' parameter.
        /// </summary>
        /// <param name="eventOutcome">The value to be inserted into the 'eventOutcomeValue' XElement.</param>
        /// <returns>The newly created XElement.</returns>
        internal static XElement CreatePremisEventOutcomeInformationElement(string eventOutcome)
        {
            ValidatorHelperMethods.ValidateString(eventOutcome, "eventOutcome");

            return new XElement(Namespaces.Premis + "eventOutcomeInformation",
                                new XElement(Namespaces.Premis + "eventOutcome", eventOutcome));
        }


        /// <summary>
        /// Will create a premis event outcome information XElement with its contents defined by 
        /// the 'eventOutcomeValue' parameter.
        /// </summary>
        /// <param name="eventOutcome">The value to be inserted into the 'eventOutcomeValue' XElement.</param>
        /// <param name="eventOutcomeDetailNote">The value to be inserted into the 'eventOutcomeDetailNote' XElement.</param>
        /// <returns>The newly created XElement.</returns>
        internal static XElement CreatePremisEventOutcomeInformationElement(string eventOutcome, string eventOutcomeDetailNote)
        {
            ValidatorHelperMethods.ValidateString(eventOutcome, "eventOutcome");
            ValidatorHelperMethods.ValidateString(eventOutcomeDetailNote, "eventOutcomeDetailNote");

            return new XElement(Namespaces.Premis + "eventOutcomeInformation",
                                new XElement(Namespaces.Premis + "eventOutcome", eventOutcome),
                                new XElement(Namespaces.Premis + "eventOutcomeDetail",
                                    new XElement(Namespaces.Premis + "eventOutcomeDetailNote", eventOutcomeDetailNote)));
        }

        /// <summary>
        /// Will create a blank premis linking agent identifier XElement.
        /// </summary>
        /// <returns>The newly created XElement.</returns>
        internal static XElement CreatePremisLinkingAgentIdentifierElement()
        {
            return new XElement(Namespaces.Premis + "linkingAgentIdentifier",
                                new XElement(Namespaces.Premis + "linkingAgentIdentifierType"),
                                new XElement(Namespaces.Premis + "linkingAgentIdentifierValue"));
        }

        /// <summary>
        /// Will create a premis linking agent identifier XElement with its contents defined by 
        /// the 'linkingAgentIdentifierTypeValue' and 'linkingAgentIdentifierValue' parameters.
        /// </summary>
        /// <param name="linkingAgentIdentifierType">The value to be inserted into the 'linkingAgentIdentifierTypeValue' XElement.</param>
        /// <param name="linkingAgentIdentifierValue">The value to be inserted into the 'linkingAgentIdentifierValue' XElement.</param>
        /// <returns>The newly created XElement.</returns>
        internal static XElement CreatePremisLinkingAgentIdentifierElement(string linkingAgentIdentifierType, string linkingAgentIdentifierValue)
        {
            ValidatorHelperMethods.ValidateString(linkingAgentIdentifierType, "linkingAgentIdentifierType");
            ValidatorHelperMethods.ValidateString(linkingAgentIdentifierValue, "linkingAgentIdentifierValue");

            return new XElement(Namespaces.Premis + "linkingAgentIdentifier",
                                new XElement(Namespaces.Premis + "linkingAgentIdentifierType", linkingAgentIdentifierType),
                                new XElement(Namespaces.Premis + "linkingAgentIdentifierValue", linkingAgentIdentifierValue));
        }

        #endregion

        #region Premis Agent Related Methods

        /// <summary>
        /// Will create a premis event XElement.
        /// </summary>
        /// <param name="premisAgentInfo">Object which holds the values to be inserted into the premis agent XElement.</param>
        /// <returns>The newly created XElement</returns>
        public static XElement CreatePremisAgentElement(PremisAgentInfo premisAgentInfo)
        {
            if (premisAgentInfo == null)
            {
                throw new ArgumentNullException("premisAgentInfo", "premisAgentInfo cannot be missing");
            }
            var agentElement = CreatePremisAgentElement(premisAgentInfo.AgentName, premisAgentInfo.AgentType);
            var agentIdentifierElement = CreatePremisAgentIdentifierElement(premisAgentInfo.AgentIdentifierType, premisAgentInfo.AgentIdentifierValue);
            agentElement.AddFirst(agentIdentifierElement);
            return agentElement;
        }

        /// <summary>
        /// Will create a blank premis agent XElement.
        /// </summary>
        /// <returns>The newly created XElement.</returns>
        internal static XElement CreatePremisAgentElement()
        {
            return new XElement(Namespaces.Premis + "agent",
                                        new XElement(Namespaces.Premis + "agentName"),
                                        new XElement(Namespaces.Premis + "agentType"));
        }

        /// <summary>
        /// Will create a premis agent XElement with its contents defined by 
        /// the 'agentNameValue' and 'agentTypeValue' parameters.
        /// </summary>
        /// <param name="agentName">The value to be inserted into the 'agentNameValue' XElement.</param>
        /// <param name="agentType">The value to be inserted into the 'agentTypeValue' XElement.</param>
        /// <returns>The newly created XElement.</returns>
        internal static XElement CreatePremisAgentElement(string agentName, string agentType)
        {
            ValidatorHelperMethods.ValidateString(agentName, "agentName");
            ValidatorHelperMethods.ValidateString(agentType, "agentType");

            return new XElement(Namespaces.Premis + "agent",
                                        new XElement(Namespaces.Premis + "agentName", agentName),
                                        new XElement(Namespaces.Premis + "agentType", agentType));
        }

        /// <summary>
        /// Will create a blank premis agent identifier XElement.
        /// </summary>
        /// <returns>The newly created XElement.</returns>
        internal static XElement CreatePremisAgentIdentifierElement()
        {
            return new XElement(Namespaces.Premis + "agentIdentifier",
                                        new XElement(Namespaces.Premis + "agentIdentifierType"),
                                        new XElement(Namespaces.Premis + "agentIdentifierValue"));
        }

        /// <summary>
        /// Will create a premis agent identifier XElement with its contents defined by 
        /// the 'agentIdentifierTypeValue' and 'agentIdentifierValue' parameters.
        /// </summary>
        /// <param name="agentIdentifierType">The value to be inserted into the 'agentIdentifierType' XElement.</param>
        /// <param name="agentIdentifierValue">The value to be inserted into the 'agentIdentifierValue' XElement.</param>
        /// <returns>The newly created XElement.</returns>
        internal static XElement CreatePremisAgentIdentifierElement(string agentIdentifierType, string agentIdentifierValue)
        {
            ValidatorHelperMethods.ValidateString(agentIdentifierType, "agentIdentifierType");
            ValidatorHelperMethods.ValidateString(agentIdentifierValue, "agentIdentifierValue");

            return new XElement(Namespaces.Premis + "agentIdentifier",
                                        new XElement(Namespaces.Premis + "agentIdentifierType", agentIdentifierType),
                                        new XElement(Namespaces.Premis + "agentIdentifierValue", agentIdentifierValue));
        }

        #endregion
    }
}
