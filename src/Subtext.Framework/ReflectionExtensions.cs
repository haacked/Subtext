using System;
using System.Reflection;

namespace Subtext.Framework
{
    public static class ReflectionExtensions
    {
        public static Version GetVersion(this Type type)
        {
            return new AssemblyName(type.Assembly.FullName).Version;
        }
    }
}
