using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using System.Xml.Serialization;

namespace BL.Xml.Core
{
    [Serializable]
    public class PremisEvent : ISerializable
    {
        public const string JHoveAgent = "JHOVE";
        public const string ManualAgent = "BLING USER";
        public const string SoftwareRole = "Software";
        public const string ManualRole = "Manual";
        public const string ValidityAcceptedEvent = "Validity Accepted";
        public const string ValidationEvent = "Validation";
        public const string WellformednessEvent = "WellFormedness";
        public const string SuccessOutcome = "Success";
        public const string FailureOutcome = "Failure";
        public const string LocalIdentifierType = "local";
        public const string MigrationEvent = "Migration";

        [XmlIgnore]
        public long Id { get; set; }
        public string Agent { get; set; }
        [XmlIgnore]
        public string Uid { get; set; }
        public string Role { get; set; }
        public string EventType { get; set; }
        [XmlIgnore]
        public string TargetUid { get; set; }
        public string Detail { get; set; }
        public string Outcome { get; set; }
        public string OutcomeDetail { get; set; }
        public string IdentifierType { get { return LocalIdentifierType; } }
        public string EventIdentifier { get; set; }
        public DateTime EventDateTime { get; set; }

        public PremisEvent()
        {
            Uid = string.Empty;
        }

        public PremisEvent(SerializationInfo info, StreamingContext context)
        {
            Agent = (string)info.GetValue("Agent", typeof(string));
            Role = (string)info.GetValue("Role", typeof(string));
            EventType = (string)info.GetValue("EventType", typeof(string));
            Detail = (string)info.GetValue("Detail", typeof(string));
            Outcome = (string)info.GetValue("Outcome", typeof(string));
            OutcomeDetail = (string)info.GetValue("OutcomeDetail", typeof(string));
            EventIdentifier = (string)info.GetValue("EventIdentifier", typeof(string));
            EventDateTime = (DateTime)info.GetValue("EventDateTime", typeof(DateTime));
        }

        public static PremisEvent CreateJhoveValidationPremisEvent(string contentFileId, string outcome)
        {
            return CreateJhovePremisEvent(contentFileId, outcome, ValidationEvent);
        }

        public static PremisEvent CreateJhoveValidationPremisEvent(string contentFileId, string outcome, string outcomeDetail)
        {
            return CreateJhovePremisEvent(contentFileId, outcome, ValidationEvent, outcomeDetail);
        }

        public static PremisEvent CreateAgentValidationPremisEvent(string contentFileId, string outcome, string outcomeDetail, string agent)
        {
            return CreatePremisEvent(contentFileId, agent, ValidationEvent, SoftwareRole, outcome, outcomeDetail);
        }

        public static PremisEvent CreateJhoveWellformednessPremisEvent(string contentFileId, string outcome)
        {
            return CreateJhovePremisEvent(contentFileId, outcome, WellformednessEvent);
        }

        public static PremisEvent CreateJhoveWellformednessPremisEvent(string contentFileId, string outcome, string outcomeDetail)
        {
            return CreateJhovePremisEvent(contentFileId, outcome, WellformednessEvent, outcomeDetail);
        }

        public static PremisEvent CreateAgentWellformednessPremisEvent(string contentFileId, string outcome, string outcomeDetail, string agent)
        {
            return CreatePremisEvent(contentFileId, agent, WellformednessEvent, SoftwareRole, outcome, outcomeDetail);
        }

        private static PremisEvent CreateJhovePremisEvent(string contentFileId, string outcome, string eventType)
        {
            return CreateJhovePremisEvent(contentFileId, outcome, eventType, string.Empty);
        }

        private static PremisEvent CreateJhovePremisEvent(string contentFileId, string outcome, string eventType, string outcomeDetail)
        {
            var premisEvent = new PremisEvent
            {
                Agent = JHoveAgent,
                Role = SoftwareRole,
                EventType = eventType,
                TargetUid = contentFileId,
                Outcome = outcome,
                OutcomeDetail = outcomeDetail
            };

            return premisEvent;
        }

        public static PremisEvent CreateManualApprovalPremisEvent(string contentFileId, string eventType)
        {
            var premisEvent = new PremisEvent
            {
                Agent = ManualAgent,
                Role = ManualRole,
                EventType = eventType,
                TargetUid = contentFileId,
                Outcome = SuccessOutcome
            };

            return premisEvent;
        }

        public static PremisEvent CreatePremisEvent(string contentFileId, string agent, string eventType, string role, string outcome, string outcomeDetail)
        {
            var premisEvent = new PremisEvent
            {
                Agent = agent,
                Role = role,
                EventType = eventType,
                TargetUid = contentFileId,
                Outcome = outcome,
                OutcomeDetail = outcomeDetail
            };

            return premisEvent;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.AppendFormat("Id = {0}\r\n", Id);
            sb.AppendFormat("Agent = {0}\r\n", Agent);
            sb.AppendFormat("Uid = {0}\r\n", Uid);
            sb.AppendFormat("Role = {0}\r\n", Role);
            sb.AppendFormat("EventType = {0}\r\n", EventType);
            sb.AppendFormat("TargetUid = {0}\r\n", TargetUid);
            sb.AppendFormat("Outcome = {0}\r\n", Outcome);
            sb.AppendFormat("OutcomeDetail = {0}\r\n", OutcomeDetail);
            sb.AppendFormat("IdentifierType = {0}\r\n", IdentifierType);
            sb.AppendFormat("EventIdentifier = {0}\r\n", EventIdentifier);
            sb.AppendFormat("EventDateTime = {0}\r\n\n", EventDateTime);

            return sb.ToString();
        }

        #region ISerializable Members

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Agent", Agent);
            info.AddValue("Role", Role);
            info.AddValue("EventType", EventType);
            info.AddValue("Detail", Detail);
            info.AddValue("Outcome", Outcome);
            info.AddValue("OutcomeDetail", OutcomeDetail);
            info.AddValue("EventIdentifier", EventIdentifier);
            info.AddValue("EventDateTime", EventDateTime);
        }

        #endregion
    }
}
