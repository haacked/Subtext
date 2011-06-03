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
using Subtext.Framework.Components;
using Subtext.Framework.Providers;

namespace Subtext.Framework
{
    public static class Enclosures
    {
        public static int Create(this ObjectProvider repository, Enclosure enclosure)
        {
            if (enclosure == null)
            {
                throw new ArgumentNullException("enclosure");
            }

            if (!enclosure.IsValid)
            {
                throw new ArgumentException(enclosure.ValidationMessage);
            }

            enclosure.Id = repository.Create(enclosure);

            return enclosure.Id;
        }

        public static bool Update(Enclosure enclosure)
        {
            if (enclosure == null)
            {
                throw new ArgumentNullException("enclosure");
            }

            if (!enclosure.IsValid)
            {
                throw new ArgumentException(enclosure.ValidationMessage);
            }

            return ObjectProvider.Instance().Update(enclosure);
        }

        public static bool Delete(int enclosureId)
        {
            return ObjectProvider.Instance().DeleteEnclosure(enclosureId);
        }
    }
}