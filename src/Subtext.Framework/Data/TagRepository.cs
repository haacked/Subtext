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

using System.Collections.Generic;
using System.Data;

namespace Subtext.Framework.Data
{
    public partial class DatabaseObjectProvider
    {
        public override IDictionary<string, int> GetTopTags(int itemCount)
        {
            using (IDataReader reader = _procedures.GetTopTags(itemCount, BlogId))
            {
                IDictionary<string, int> tags = DataHelper.ReadTags(reader);
                return tags;
            }
        }

    }
}
