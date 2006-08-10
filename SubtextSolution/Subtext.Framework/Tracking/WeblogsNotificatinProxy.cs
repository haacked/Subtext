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

#region Notes
 ///////////////////////////////////////////////////////////////////////////////////////////////////
 // The code in this file is freely distributable.
 // 
 // ASPNetWeblog isnot responsible for, shall have no liability for 
 // and disclaims all warranties whatsoever, expressed or implied, related to this code,
 // including without limitation any warranties related to performance, security, stability,
 // or non-infringement of title of the control.
 // 
 // If you have any questions, comments or concerns, please contact
 // Scott Watermasysk, Scott@TripleASP.Net.
 // 
 // For more information on this code, updates, and other tools to integrate blogging 
 // into your existing applications, please visit, http://aspnetweblog.com
 // 
 ///////////////////////////////////////////////////////////////////////////////////////////////////
#endregion
using System;
using Subtext.Framework.Logging;
using CookComputing.XmlRpc;

namespace Subtext.Framework.Tracking
{
	/// <summary>
	/// Summary description for WeblogsNotificatinProxy.
	/// </summary>
	[XmlRpcUrl("http://rpc.weblogs.com/RPC2")] 
	public class WeblogsNotificatinProxy : XmlRpcClientProtocol
	{
		static Log Log = new Log();
		
		public WeblogsNotificatinProxy() : base()
		{
		}

		private string errormessage = "No Error";
		public string ErrorMessage
		{
			get{return errormessage;}
		}

		public bool Ping(string name, string url)
		{
			bool result = false;
			try
			{
				XmlRpcStruct rpcstruct = Notifiy(name,url);
				if(rpcstruct.ContainsKey("flerror"))
				{
					//Weblogs.com return false if there is no error
					//I want to return true, indicating a successful notification
					result = !(bool)rpcstruct["flerror"];
					if(!result)
					{
						if(rpcstruct.ContainsKey("message"))
						{
							errormessage = (string)rpcstruct["message"];
						}
						else
						{
							errormessage = "Unknown Error";
						}
					}
					
				}
			}
			catch(Exception ex)
			{
				Log.Warn("Error while Ping: " + ex.Message);
				errormessage = "Error: " + ex.Message;
			}
			return result;

		}

		[XmlRpcMethod("weblogUpdates.ping")]
		public XmlRpcStruct Notifiy(string name, string url)
		{
			//TODO: IS this really supposed to be misspelled?
			return (XmlRpcStruct) Invoke("Notifiy", new object[] {name,url});
		}
	}
}

