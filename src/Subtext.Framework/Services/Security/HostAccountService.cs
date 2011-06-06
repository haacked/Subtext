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

using Subtext.Framework.Configuration;
using Subtext.Framework.Providers;

namespace Subtext.Framework.Security
{
    public class HostAccountService : AccountService
    {
        ObjectRepository _repository;
        HostInfo _hostInfo;

        public HostAccountService(HostInfo hostInfo, ISubtextContext context)
            : base(context)
        {
            _repository = context.Repository;
            _hostInfo = hostInfo;
        }

        public override void UpdatePassword(string password)
        {
            _hostInfo.Password = Config.Settings.UseHashedPasswords ? SecurityHelper.HashPassword(password, _hostInfo.Salt) : password;
            HostInfo.UpdateHost(_repository, _hostInfo);
        }

        public bool IsValidPassword(string password)
        {
            return IsValidPassword(password, _hostInfo.Password, Config.Settings.UseHashedPasswords, _hostInfo.Salt);
        }
    }
}
