using Subtext.Extensibility.Providers;

namespace UnitTests.Subtext
{
    public class UnitTestEmailProvider : EmailProvider
    {
        public string From;
        EmailProvider internalProvider;
        public string Message;
        public string ReplyTo;
        public string Subject;
        public string To;

        /// <summary>
        /// Sends an email.
        /// </summary>
        /// <param name="to"></param>
        /// <param name="from"></param>
        /// <param name="subject"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public override void Send(string to, string from, string subject, string message)
        {
            To = to;
            From = from;
            Subject = subject;
            Message = message;

            if(internalProvider == null)
            {
                return;
            }

            internalProvider.Send(to, from, subject, message);
        }

        public void SetInternalProvider(EmailProvider provider)
        {
            internalProvider = provider;
        }
    }
}