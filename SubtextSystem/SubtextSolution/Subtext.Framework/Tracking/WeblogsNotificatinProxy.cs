#region Disclaimer/Info
///////////////////////////////////////////////////////////////////////////////////////////////////
// .Text WebLog
// 
// .Text is an open source weblog system started by Scott Watermasysk. 
// Blog: http://ScottWater.com/blog 
// RSS: http://scottwater.com/blog/rss.aspx
// Email: Dottext@ScottWater.com
//
// For updated news and information please visit http://scottwater.com/dottext and subscribe to 
// the Rss feed @ http://scottwater.com/dottext/rss.aspx
//
// On its release (on or about August 1, 2003) this application is licensed under the BSD. However, I reserve the 
// right to change or modify this at any time. The most recent and up to date license can always be fount at:
// http://ScottWater.com/License.txt
// 
// Please direct all code related questions to:
// GotDotNet Workspace: http://www.gotdotnet.com/Community/Workspaces/workspace.aspx?id=e99fccb3-1a8c-42b5-90ee-348f6b77c407
// Yahoo Group http://groups.yahoo.com/group/DotText/
// 
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
using CookComputing.XmlRpc;

namespace Subtext.Framework.Tracking
{
	/// <summary>
	/// Summary description for WeblogsNotificatinProxy.
	/// </summary>
	[XmlRpcUrl("http://rpc.weblogs.com/RPC2")] 
	public class WeblogsNotificatinProxy : XmlRpcClientProtocol
	{
		public WeblogsNotificatinProxy()
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
				errormessage = "Error: " + ex.Message;
			}
			return result;

		}

		[XmlRpcMethod("weblogUpdates.ping")]
		public XmlRpcStruct Notifiy(string name, string url)
		{
			return (XmlRpcStruct) Invoke("Notifiy",new object[] {name,url});
		}
	}
}

