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
using Subtext.Framework.Logging;

namespace Subtext.Framework.Tracking
{
    /// <summary>
    /// Summary description for WeblogsNotificatinProxy.
    /// </summary>
    [XmlRpcUrl("http://rpc.weblogs.com/RPC2")]
    public class WeblogsNotificatinProxy : XmlRpcClientProtocol
    {
        static readonly Log Log = new Log();

        private string _errorMessage = "No Error";

        public string ErrorMessage
        {
            get { return _errorMessage; }
        }

        public bool Ping(string name, string url)
        {
            bool result = false;
            try
            {
                XmlRpcStruct rpcstruct = Notifiy(name, url);
                if(rpcstruct.ContainsKey("flerror"))
                {
                    //Weblogs.com return false if there is no error
                    //I want to return true, indicating a successful notification
                    result = !(bool)rpcstruct["flerror"];
                    if(!result)
                    {
                        if(rpcstruct.ContainsKey("message"))
                        {
                            _errorMessage = (string)rpcstruct["message"];
                        }
                        else
                        {
                            _errorMessage = "Unknown Error";
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                Log.Warn("Error while Ping: " + ex.Message);
                _errorMessage = "Error: " + ex.Message;
            }
            return result;
        }

        [XmlRpcMethod("weblogUpdates.ping")]
        public XmlRpcStruct Notifiy(string name, string url)
        {
            //TODO: IS this really supposed to be misspelled?
            return (XmlRpcStruct)Invoke("Notifiy", new object[] {name, url});
        }
    }
}