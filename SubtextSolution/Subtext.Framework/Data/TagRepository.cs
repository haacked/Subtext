using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Subtext.Framework.Data
{
    public partial class DatabaseObjectProvider
    {
        public override IDictionary<string, int> GetTopTags(int itemCount)
        {
            using (IDataReader reader = _procedures.GetTopTags(itemCount, BlogId))
            {
                IDictionary<string, int> tags = DataHelper.LoadTags(reader);
                return tags;
            }
        }

    }
}
