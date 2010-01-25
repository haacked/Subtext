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
using System.Configuration;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using Subtext.Framework.Configuration;
using Subtext.Framework.Exceptions;
using Subtext.Framework.Properties;
using Subtext.Framework.Providers;
using Subtext.Framework.Security;

namespace Subtext.Framework
{
    /// <summary>
    /// Represents the system, and its settings, that hosts the blogs within this Subtext installation.
    /// </summary>
    public sealed class HostInfo
    {
        private static HostInfo _instance;

        /// <summary>
        /// Returns an instance of <see cref="HostInfo"/> used to 
        /// describe this installation of Subtext.
        /// </summary>
        /// <returns></returns>
        public static HostInfo Instance
        {
            get
            {
                // no lock singleton.
                HostInfo instance = _instance;
                if(instance == null)
                {
                    instance = LoadHost(true);
                    // the next line might overwrite a HostInfo created by another thread,
                    // but if it does, it'll only happen once and it's not so bad. I'll measure it to be sure. 
                    // -phil Jan 18, 2009
                    _instance = instance;
                }
                return _instance;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the HostInfo table exists.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if host info table exists; otherwise, <c>false</c>.
        /// </value>
        public static bool HostInfoTableExists
        {
            get
            {
                try
                {
                    LoadHost(false);
                    return true;
                }
                catch(HostDataDoesNotExistException)
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Gets or sets the name of the host user.
        /// </summary>
        /// <value></value>
        public string HostUserName { get; set; }

        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the host password.
        /// </summary>
        /// <value></value>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the salt.
        /// </summary>
        /// <value></value>
        public string Salt { get; set; }

        /// <summary>
        /// Gets or sets the date this record was created. 
        /// This is essentially the date that Subtext was 
        /// installed.
        /// </summary>
        /// <value></value>
        public DateTime DateCreated { get; set; }

        public bool BlogAggregationEnabled { get; set; }

        public Blog AggregateBlog { get; set; }

        /// <summary>
        /// Loads the host from the Object Provider. This is provided for 
        /// those cases when we really need to hit the data strore. Calling this
        /// method will also reload the HostInfo.Instance from the data store.
        /// </summary>
        /// <param name="suppressException">If true, won't throw an exception.</param>
        /// <returns></returns>
        public static HostInfo LoadHost(bool suppressException)
        {
            try
            {
                _instance = ObjectProvider.Instance().LoadHostInfo(new HostInfo());
                if(_instance != null)
                {
                    _instance.BlogAggregationEnabled =
                        String.Equals(ConfigurationManager.AppSettings["AggregateEnabled"], "true",
                                      StringComparison.OrdinalIgnoreCase);
                    if(_instance.BlogAggregationEnabled)
                    {
                        InitAggregateBlog(_instance);
                    }
                }
                return _instance;
            }
            catch(SqlException e)
            {
                // LoadHostInfo now executes the stored proc subtext_GetHost, instead of checking the table subtext_Host 
                if(e.Message.IndexOf("Invalid object name 'subtext_Host'") >= 0 ||
                   e.Message.IndexOf("Could not find stored procedure 'subtext_GetHost'") >= 0)
                {
                    if(suppressException)
                    {
                        return null;
                    }
                    throw new HostDataDoesNotExistException();
                }
                throw;
            }
        }

        /// <summary>
        /// Updates the host in the persistent store.
        /// </summary>
        /// <param name="host">Host.</param>
        /// <returns></returns>
        public static bool UpdateHost(HostInfo host)
        {
            if(ObjectProvider.Instance().UpdateHost(host))
            {
                _instance = host;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Creates the host in the persistent store.
        /// </summary>
        /// <returns></returns>
        public static bool CreateHost(string hostUserName, string hostPassword, string email)
        {
            if(Instance != null)
            {
                throw new InvalidOperationException(Resources.InvalidOperation_HostRecordAlreadyExists);
            }

            var host = new HostInfo {HostUserName = hostUserName, Email = email};

            SetHostPassword(host, hostPassword);
            _instance = host;
            return UpdateHost(host);
        }

        public static void SetHostPassword(HostInfo host, string newPassword)
        {
            host.Salt = SecurityHelper.CreateRandomSalt();
            if(Config.Settings.UseHashedPasswords)
            {
                string hashedPassword = SecurityHelper.HashPassword(newPassword, host.Salt);
                host.Password = hashedPassword;
            }
            else
            {
                host.Password = newPassword;
            }
        }

        private static void InitAggregateBlog(HostInfo hostInfo)
        {
            string aggregateHost = ConfigurationManager.AppSettings["AggregateUrl"];
            if(aggregateHost == null)
            {
                return;
            }

            // validate host.
            var regex = new Regex(@"^(https?://)?(?<host>.+?)(/.*)?$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            Match match = regex.Match(aggregateHost);

            if(match.Success)
            {
                aggregateHost = match.Groups["host"].Value;
            }

            var blog = new Blog(true /*isAggregateBlog*/)
            {
                Title = ConfigurationManager.AppSettings["AggregateTitle"],
                Skin = SkinConfig.DefaultSkin,
                Host = aggregateHost,
                Subfolder = string.Empty,
                IsActive = true
            };
            //TODO: blog.MobileSkin = ...

            if(hostInfo != null)
            {
                blog.UserName = hostInfo.HostUserName;
                hostInfo.AggregateBlog = blog;
            }
        }
    }
}