using System.Collections.Generic;

namespace Subtext.Framework.Emoticons
{
    public interface IEmoticonsSource
    {
        IEnumerable<Emoticon> GetEmoticons();
    }
}
