using System;
using System.Xml.Serialization;

namespace Subtext.Framework.Configuration
{
	/// <summary>
	/// Summary description for Tracking.
	/// </summary>
	[Serializable]
	public class Tracking
	{
		public Tracking()
		{

		}

		public bool UseTrackingServices
		{
			get
			{
				return this.PingWeblogs || this.EnablePingBacks || this.EnableTrackBacks;
			}
		}

		private int _queueStatsCount = 25;
		[XmlAttribute("queueStatsCount")]
		public int QueueStatsCount
		{
			get {return this._queueStatsCount;}
			set {this._queueStatsCount = value;}
		}

		private bool _queueStats = true;
		[XmlAttribute("queueStats")]
		public bool QueueStats
		{
			get {return this._queueStats;}
			set {this._queueStats = value;}
		}

		private bool _enableTrackBacks = true;
		[XmlAttribute("enableTrackBacks")]
		public bool EnableTrackBacks
		{
			get {return this._enableTrackBacks;}
			set {this._enableTrackBacks = value;}
		}

		private bool _enablePingBacks = true;
		[XmlAttribute("enablePingBacks")]
		public bool EnablePingBacks
		{
			get {return this._enablePingBacks;}
			set {this._enablePingBacks = value;}
		}

		private bool _pingWeblogs = true;
		[XmlAttribute("pingWeblogs")]
		public bool PingWeblogs
		{
			get {return this._pingWeblogs;}
			set {this._pingWeblogs = value;}
		}

		private bool _enableWebStats = true;
		[XmlAttribute("enableWebStats")]
		public bool EnableWebStats
		{
			get {return this._enableWebStats;}
			set {this._enableWebStats = value;}
		}

		private bool _enableAggBugs = true;
		[XmlAttribute("enableAggBugs")]
		public bool EnableAggBugs
		{
			get {return this._enableAggBugs;}
			set {this._enableAggBugs = value;}
		}

	}
}
