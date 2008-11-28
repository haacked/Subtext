using System;
using System.Collections.Generic;
using System.Text;
using Subtext.Framework.Util;

namespace Subtext.Framework.Components {
    [Serializable]
    public class ServerTimeZoneInfo {
        public ServerTimeZoneInfo() { 
        }
        
        public ServerTimeZoneInfo(WindowsTimeZone timeZone) {
            ServerTimeZone = string.Format("{0} ({1})",
                    TimeZone.CurrentTimeZone.StandardName,
                    TimeZone.CurrentTimeZone.GetUtcOffset(DateTime.Now));
            ServerTime = DateTime.Now.ToString("yyyy/MM/dd hh:mm tt");
            ServerUtcTime = DateTime.UtcNow.ToString("yyyy/MM/dd hh:mm tt");
            CurrentTime = timeZone.Now.ToString("yyyy/MM/dd hh:mm tt");
        }

        public ServerTimeZoneInfo(string timeZoneText) : this(GetTimeZoneFromText(timeZoneText)) {
        }

        public string ServerTimeZone {
            get;
            set;
        }

        public string ServerTime {
            get;
            set;
        }

        public string ServerUtcTime {
            get;
            set;
        }

        public string CurrentTime
        {
            get;
            set;
        }

        private static WindowsTimeZone GetTimeZoneFromText(string timeZoneText) {
            int timeZoneId = String.IsNullOrEmpty(timeZoneText)
                                     ? TimeZone.CurrentTimeZone.StandardName.GetHashCode()
                                     : int.Parse(timeZoneText);

            return WindowsTimeZone.GetById(timeZoneId);
        }
    }
}
