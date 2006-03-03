#region Disclaimer/Info
///////////////////////////////////////////////////////////////////////////////////////////////////
// Subtext WebLog
// 
// Subtext is an open source weblog system that is a fork of the .TEXT
// weblog system.
//
// For updated news and information please visit http://subtextproject.com/
// Subtext is hosted at SourceForge at http://sourceforge.net/projects/subtext
// The development mailing list is at subtext-devs@lists.sourceforge.net 
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
		/// <summary>
		/// Gets a value indicating whether tracking services 
		/// are being used such as Weblogs Ping, Pingbacks, and Trackbacks.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if using tracking services; otherwise, <c>false</c>.
		/// </value>
		public bool UseTrackingServices
		{
			get
			{
				return this.PingWeblogs || this.EnablePingBacks || this.EnableTrackBacks;
			}
		}

		private int _queueStatsCount = 25;
		/// <summary>
		/// Gets or sets the queue stats count. This is the number of 
		/// tracking operations that are currently in the queue.
		/// </summary>
		/// <value></value>
		[XmlAttribute("queueStatsCount")]
		public int QueueStatsCount
		{
			get {return this._queueStatsCount;}
			set {this._queueStatsCount = value;}
		}

		private bool _queueStats = true;
		/// <summary>
		/// Gets or sets a value indicating whether or not to queue stats. 
		/// This simply indicates whether trackbacks and pings are queued 
		/// to occur asynchronously or immediately.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if queueing stats asynchronously; otherwise, <c>false</c>.
		/// </value>
		[XmlAttribute("queueStats")]
		public bool QueueStats
		{
			get {return this._queueStats;}
			set {this._queueStats = value;}
		}

		private bool _enableTrackBacks = true;
		/// <summary>
		/// Gets or sets a value indicating whether trackbaks are enabled.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if track backs are enabled; otherwise, <c>false</c>.
		/// </value>
		[XmlAttribute("enableTrackBacks")]
		public bool EnableTrackBacks
		{
			get {return this._enableTrackBacks;}
			set {this._enableTrackBacks = value;}
		}

		private bool _enablePingBacks = true;
		/// <summary>
		/// Gets or sets a value indicating whether ping backs are enabled.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if ping backs are enabled; otherwise, <c>false</c>.
		/// </value>
		[XmlAttribute("enablePingBacks")]
		public bool EnablePingBacks
		{
			get {return this._enablePingBacks;}
			set {this._enablePingBacks = value;}
		}

		private bool _pingWeblogs = true;
		/// <summary>
		/// Gets or sets a value indicating whether to ping weblogs.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if weblogs are pinged; otherwise, <c>false</c>.
		/// </value>
		[XmlAttribute("pingWeblogs")]
		public bool PingWeblogs
		{
			get {return this._pingWeblogs;}
			set {this._pingWeblogs = value;}
		}

		private bool _enableWebStats = true;
		/// <summary>
		/// Gets or sets a value indicating whether to enable web stats in general.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if web stats are enabled; otherwise, <c>false</c>.
		/// </value>
		[XmlAttribute("enableWebStats")]
		public bool EnableWebStats
		{
			get {return this._enableWebStats;}
			set {this._enableWebStats = value;}
		}

		private bool _enableAggBugs = true;
		/// <summary>
		/// Gets or sets a value indicating whether aggregator bugs are enabled. 
		/// These are 1pixel images used to track the number of users who 
		/// read a particular entry in an aggregator.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if agg bugs are enabled; otherwise, <c>false</c>.
		/// </value>
		[XmlAttribute("enableAggBugs")]
		public bool EnableAggBugs
		{
			get {return this._enableAggBugs;}
			set {this._enableAggBugs = value;}
		}

	}
}
