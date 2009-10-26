#region Disclaimer/Info

///////////////////////////////////////////////////////////////////////////////////////////////////
// Subtext WebLog
// 
// Subtext is an open source weblog system that is a fork of the .TEXT
// weblog system.
//
// For updated news and information please visit http://subtextproject.com/
// Subtext is hosted at Google Code at http://code.google.com/p/subtext/
// The development mailing list is at subtext@googlegroups.com 
//
// This project is licensed under the BSD license.  See the License.txt file for more information.
///////////////////////////////////////////////////////////////////////////////////////////////////

#endregion

using System;
using System.Xml.Serialization;

namespace Subtext.Framework.Configuration
{
    /// <summary>
    /// Class used to manage the settings for various tracking systems 
    /// such as TrackBacks, PingBacks and Weblogs Pings.
    /// </summary>
    [Serializable]
    public class Tracking
    {
        public Tracking()
        {
            QueueStatsCount = 25;
            QueueStats = true;
        }

        /// <summary>
        /// Gets a value indicating whether tracking services 
        /// are being used such as Weblogs Ping, Pingbacks, and Trackbacks.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if using tracking services; otherwise, <c>false</c>.
        /// </value>
        public bool UseTrackingServices
        {
            get { return PingWeblogs || EnablePingBacks || EnableTrackBacks; }
        }

        /// <summary>
        /// Gets or sets the queue stats count. This is the number of 
        /// tracking operations that are currently in the queue.
        /// </summary>
        /// <value></value>
        [XmlAttribute("queueStatsCount")]
        public int QueueStatsCount { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether or not to queue stats. 
        /// This simply indicates whether trackbacks and pings are queued 
        /// to occur asynchronously or immediately.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if queueing stats asynchronously; otherwise, <c>false</c>.
        /// </value>
        [XmlAttribute("queueStats")]
        public bool QueueStats { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether trackbaks are enabled.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if track backs are enabled; otherwise, <c>false</c>.
        /// </value>
        [XmlAttribute("enableTrackBacks")]
        public bool EnableTrackBacks { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether ping backs are enabled.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if ping backs are enabled; otherwise, <c>false</c>.
        /// </value>
        [XmlAttribute("enablePingBacks")]
        public bool EnablePingBacks { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to ping weblogs.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if weblogs are pinged; otherwise, <c>false</c>.
        /// </value>
        [XmlAttribute("pingWeblogs")]
        public bool PingWeblogs { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to enable web stats in general.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if web stats are enabled; otherwise, <c>false</c>.
        /// </value>
        [XmlAttribute("enableWebStats")]
        public bool EnableWebStats { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether aggregator bugs are enabled. 
        /// These are 1pixel images used to track the number of users who 
        /// read a particular entry in an aggregator.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if agg bugs are enabled; otherwise, <c>false</c>.
        /// </value>
        [XmlAttribute("enableAggBugs")]
        public bool EnableAggBugs { get; set; }
    }
}