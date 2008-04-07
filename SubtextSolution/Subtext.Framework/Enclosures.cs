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
using Subtext.Framework.Components;
using Subtext.Framework.Providers;

namespace Subtext.Framework
{
    public static class Enclosures
    {
        public static int Create(Enclosure enclosure)
        {
            if (enclosure == null)
                throw new ArgumentNullException("enclosure", "The enclosure cannot be NULL");

            if(!enclosure.IsValid)
                throw new ArgumentException(enclosure.ValidationMessage);

            enclosure.Id = ObjectProvider.Instance().Create(enclosure);

            return enclosure.Id;
        }

        public static bool Update(Enclosure enclosure)
        {
            if (enclosure == null)
                throw new ArgumentNullException("enclosure", "The enclosure can not be NULL.");

            if (!enclosure.IsValid)
                throw new ArgumentException(enclosure.ValidationMessage);

            return ObjectProvider.Instance().Update(enclosure);
        }

        public static bool Delete(int enclosureId)
        {
            return ObjectProvider.Instance().DeleteMetaTag(enclosureId);
        }
    }
}
