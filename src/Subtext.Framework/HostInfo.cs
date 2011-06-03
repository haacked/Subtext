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
using System.Web.Mvc;
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
        private static Lazy<HostInfo> _instance = new Lazy<HostInfo>(EnsureHostInfo);

        private static HostInfo EnsureHostInfo()
        {
            var repository = DependencyResolver.Current.GetService<ObjectProvider>();
            var hostInfo = LoadHostInfoFromDatabase(repository, suppressException: true);
            if (hostInfo != null)
            {
                hostInfo.BlogAggregationEnabled =
                        String.Equals(ConfigurationManager.AppSettings["AggregateEnabled"], "true",
                                      StringComparison.OrdinalIgnoreCase);
                if (hostInfo.BlogAggregationEnabled)
                {
                    hostInfo.Initialize();
                }
            }
            if (hostInfo == null)
            {
                _instance = new Lazy<HostInfo>(EnsureHostInfo);
            }
            return null;
        }

        public static HostInfo LoadHostInfoFromDatabase(ObjectProvider repository, bool suppressException)
        {
            try
            {
                return repository.LoadHostInfo(new HostInfo());
            }
            catch (SqlException e)
            {
                // LoadHostInfo now executes the stored proc subtext_GetHost, instead of checking the table subtext_Host 
                if (e.Message.IndexOf("Invalid object name 'subtext_Host'") >= 0
                    || e.Message.IndexOf("Could not find stored procedure 'subtext_GetHost'") >= 0)
                {
                    if (suppressException)
                    {
                        return null;
                    }
                    throw new HostDataDoesNotExistException();
                }
                throw;
            }
        }

        /// <summary>
        /// Returns an instance of <see cref="HostInfo"/> used to 
        /// describe this installation of Subtext.
        /// </summary>
        /// <returns></returns>
        public static HostInfo Instance
        {
            get
            {
                return _instance.Value;
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
        public DateTime DateCreatedUtc { get; set; }

        public bool BlogAggregationEnabled { get; set; }

        public Blog AggregateBlog { get; set; }

        /// <summary>
        /// Updates the host in the persistent store.
        /// </summary>
        /// <param name="host">Host.</param>
        /// <returns></returns>
        public static bool UpdateHost(ObjectProvider repository, HostInfo host)
        {
            if (repository.UpdateHost(host))
            {
                _instance = new Lazy<HostInfo>(() => host);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Creates the host in the persistent store.
        /// </summary>
        /// <returns></returns>
        public static bool CreateHost(ObjectProvider repository, string hostUserName, string hostPassword, string email)
        {
            if (Instance != null)
            {
                throw new InvalidOperationException(Resources.InvalidOperation_HostRecordAlreadyExists);
            }

            var host = new HostInfo { HostUserName = hostUserName, Email = email };

            SetHostPassword(host, hostPassword);
            _instance = new Lazy<HostInfo>(() => host);
            return repository.UpdateHost(host);
        }

        public static void SetHostPassword(HostInfo host, string newPassword)
        {
            host.Salt = SecurityHelper.CreateRandomSalt();
            if (Config.Settings.UseHashedPasswords)
            {
                string hashedPassword = SecurityHelper.HashPassword(newPassword, host.Salt);
                host.Password = hashedPassword;
            }
            else
            {
                host.Password = newPassword;
            }
        }

        private void Initialize()
        {
            string aggregateHost = ConfigurationManager.AppSettings["AggregateUrl"];
            if (aggregateHost == null)
            {
                return;
            }

            // validate host.
            var regex = new Regex(@"^(https?://)?(?<host>.+?)(/.*)?$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            Match match = regex.Match(aggregateHost);

            if (match.Success)
            {
                aggregateHost = match.Groups["host"].Value;
            }

            var blog = new Blog(true /*isAggregateBlog*/)
            {
                Title = ConfigurationManager.AppSettings["AggregateTitle"],
                Skin = GetAggregateSkin(),
                Host = aggregateHost,
                Subfolder = string.Empty,
                IsActive = true
            };
            //TODO: blog.MobileSkin = ...

            blog.UserName = HostUserName;
            AggregateBlog = blog;
        }

        public static SkinConfig GetAggregateSkin()
        {
            string aggregateSkin = ConfigurationManager.AppSettings["AggregateSkin"] as string;
            if (String.IsNullOrEmpty(aggregateSkin))
            {
                aggregateSkin = "Aggregate/Simple";
            }
            return new SkinConfig { TemplateFolder = aggregateSkin };
        }
    }
}