using System;
using System.Text;

namespace BL.Sip.Utilities.Core
{
	public class WorkflowException : ApplicationException
	{
		#region Declarations

		public string ErrorCode { get; set; }

		public string AdditionalMessage { get; set; }

		public string SuggestedAction { get; set; }

		public string RecipientGroup { get; set; }

		#endregion

		#region Constructors

		public WorkflowException(string message)
			: base(message)
		{
		}

		public WorkflowException(string message, Exception exception)
			: base(message, exception)
		{
		}

		public WorkflowException(string message, string suggestedAction)
			: base(message)
		{
			SuggestedAction = suggestedAction;
		}

		public WorkflowException(string message, string suggestedAction, string additionalMessage)
			: base(message)
		{
			SuggestedAction = suggestedAction;
			AdditionalMessage = additionalMessage;
		}

		public WorkflowException(string message, string suggestedAction, string additionalMessage, string recipientGroup)
			: base(message)
		{
			SuggestedAction = suggestedAction;
			AdditionalMessage = additionalMessage;
			RecipientGroup = recipientGroup;
		}

		public WorkflowException(string message, string suggestedAction, string additionalMessage, string recipientGroup, Exception exception)
			: base(message, exception)
		{
			SuggestedAction = suggestedAction;
			AdditionalMessage = additionalMessage;
			RecipientGroup = recipientGroup;
		}

		#endregion

		#region Methods

		public override string ToString()
		{
			var sb = new StringBuilder();

			sb.AppendFormat("{0} Message: {1}",
				GetType().FullName,
				Message);

			if (!String.IsNullOrWhiteSpace(StackTrace))
				sb.AppendFormat("\r\nat {0}", StackTrace);

			if (null != InnerException)
				sb.AppendFormat("\r\nInner Exception: {0}", InnerException);

			if (!String.IsNullOrWhiteSpace(ErrorCode))
				sb.AppendFormat("\r\nError Code: {0}", ErrorCode);

			if (!String.IsNullOrWhiteSpace(SuggestedAction))
				sb.AppendFormat("\r\nSuggested Action: {0}", SuggestedAction);

			if (!String.IsNullOrWhiteSpace(AdditionalMessage))
				sb.AppendFormat("\r\nAdditional Message: {0}", AdditionalMessage);

			if (!String.IsNullOrWhiteSpace(RecipientGroup))
				sb.AppendFormat("\r\nRecipient Group: {0}", RecipientGroup);

			return sb.ToString();
		}

		#endregion
	}
}
