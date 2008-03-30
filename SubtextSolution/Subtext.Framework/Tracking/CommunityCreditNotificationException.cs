using System;
using System.Collections.Generic;
using System.Text;

namespace Subtext.Framework.Tracking
{
   public class CommunityCreditNotificationException: ApplicationException
   {
      public CommunityCreditNotificationException() : base() { }

      public CommunityCreditNotificationException(string message) : base(message) { }

      public CommunityCreditNotificationException(string message, Exception innerException) : base(message, innerException) { }
   }
}
