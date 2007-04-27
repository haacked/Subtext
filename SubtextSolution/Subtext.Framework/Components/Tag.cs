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
using System.Collections.Generic;

namespace Subtext.Framework.Components
{
    [Serializable]
    public class Tag
    {
        public Tag(KeyValuePair<string,int> tag)
        {
            tagName = tag.Key;
            tagCount = tag.Value;
        }

        private string tagName;
        public string TagName
        {
            get { return tagName; }
            set { tagName = value; }
        }

        private int tagCount;
        public int Count
        {
            get { return tagCount; }
            set { tagCount = value; }
        }

        private int tagWeight;
        public int Weight
        {
            get { return tagWeight; }
            set { tagWeight = value; }
        }

        private double factor;
        public double Factor
        {
            get { return factor; }
            set { factor = value; }
        }
    }
}
