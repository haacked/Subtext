using System.IO;
using System.Reflection;
using System.Text;

namespace Subtext.Framework.Resource
{
    public static class ResourceHelper
    {
        /// <summary>
        /// Unpacks an embedded resource into a Stream.  The resource name should 
        /// be everything after 'UnitTests.Subtext.Resources.'.
        /// </summary>
        /// <remarks>Omit the UnitTests.Subtext.Resources. part of the 
        /// resource name.</remarks>
        /// <param name="resourceName">Name of the resource.</param>
        public static Stream UnpackEmbeddedResource(string resourceName)
        {
            Assembly assembly = typeof(ResourceHelper).Assembly;
            return assembly.GetManifestResourceStream(resourceName);
        }

        /// <summary>
        /// Unpacks an embedded resource as a string. The resource name should 
        /// be everything after 'UnitTests.Subtext.Resources.'.
        /// </summary>
        /// <remarks>Omit the UnitTests.Subtext.Resources. part of the 
        /// resource name.</remarks>
        /// <param name="resourceName"></param>
        /// <param name="encoding">The path to write the file as.</param>
        public static string UnpackEmbeddedResource(string resourceName, Encoding encoding)
        {
            Stream stream = UnpackEmbeddedResource(resourceName);
            using (StreamReader reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }
    }
}
