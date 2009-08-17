#region Disclaimer/Info
///////////////////////////////////////////////////////////////////////////////////////////////////
// Subtext WebLog
// 
// Subtext is an open source weblog system that is a fork of the .TEXT
// weblog system.
//
// For updated news and information please visit http://subtextproject.com/
// Subtext is hosted at Google Code at http://code.google.com/p/subtext/
// The development mailing list is at subtext-devs@lists.sourceforge.net 
//
// This project is licensed under the BSD license.  See the License.txt file for more information.
///////////////////////////////////////////////////////////////////////////////////////////////////
#endregion

using System.Collections.ObjectModel;
using System.Linq;

namespace Subtext.Framework.Services
{
    public class CompositeTextTransformation : Collection<ITextTransformation>, ITextTransformation
    {
        public CompositeTextTransformation() : base() { }

        public string Transform(string original)
        {
            return this.Aggregate(original, (resultFromLastTransform, transformation) => transformation.Transform(resultFromLastTransform));
        }

        /// <summary>
        /// Removes the text transformation of the given type.
        /// </summary>
        /// <typeparam name="TTextTransformation"></typeparam>
        public void Remove<TTextTransformation>() where TTextTransformation : ITextTransformation
        {
            foreach (var textTransform in this) 
            {
                if (textTransform.GetType() == typeof(TTextTransformation)) 
                {
                    this.Remove(textTransform);
                    return;
                }
            }
        }
    }
}
