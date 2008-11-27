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
            _serverTimeZone = string.Format("{0} ({1})",
                    TimeZone.CurrentTimeZone.StandardName,
                    TimeZone.CurrentTimeZone.GetUtcOffset(DateTime.Now));
            _serverTime = DateTime.Now.ToString("yyyy/MM/dd hh:mm tt");
            _serverUtcTime = DateTime.UtcNow.ToString("yyyy/MM/dd hh:mm tt");
            _currentTime = timeZone.Now.ToString("yyyy/MM/dd hh:mm tt");
        }

        public ServerTimeZoneInfo(string timeZoneText) : this(GetTimeZoneFromText(timeZoneText)) {
        }

        public string ServerTimeZone {
            get {
                return _serverTimeZone; 
            }
            set {
                _serverTimeZone = value;
            }
        }
        string _serverTimeZone;

        public string ServerTime {
            get {
                return _serverTime;
            }
            set {
                _serverTime = value;
            }
        }
        string _serverTime;

        public string ServerUtcTime {
            get {
                return _serverUtcTime;
            }
            set {
                _serverUtcTime = value;
            }
        }
        string _serverUtcTime;

        public string CurrentTime {
            get {
                return _currentTime;
            }
            set {
                _currentTime = value;
            }
        }
        string _currentTime;

        private static WindowsTimeZone GetTimeZoneFromText(string timeZoneText) {
            int timeZoneId = String.IsNullOrEmpty(timeZoneText)
                                     ? TimeZone.CurrentTimeZone.StandardName.GetHashCode()
                                     : int.Parse(timeZoneText);

            return WindowsTimeZone.GetById(timeZoneId);
        }
    }
}
