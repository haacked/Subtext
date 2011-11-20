using System;

namespace Subtext.Framework.Services
{
    public class LazyNotNull<T>
    {
        Lazy<T> _lazyValue = null;
        Func<T> _valueFactory = null;

        public LazyNotNull(Func<T> valueFactory)
        {
            _lazyValue = new Lazy<T>(valueFactory);
            _valueFactory = valueFactory;
        }

        public T Value
        {
            get
            {
                var lazyValue = _lazyValue;
                if (lazyValue.Value != null)
                {
                    return lazyValue.Value;
                }
                _lazyValue = new Lazy<T>(_valueFactory);
                return default(T);
            }
        }
    }
}
