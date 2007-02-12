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
using System.Reflection;

namespace Subtext.Reflection
{
	/// <summary>
	/// Helper class to simplify common reflection tasks.
	/// </summary>
	public static class ReflectionHelper
	{
		/// <summary>
		/// Returns the value of the private member specified.
		/// </summary>
		/// <param name="fieldName">Name of the member.</param>
		/// <param name="source">The object that contains the member.</param>
		public static object GetStaticField(string fieldName, object source)
		{
			return GetStaticField(fieldName, source, source.GetType());
		}

		/// <summary>
		/// Returns the value of the private member specified.
		/// </summary>
		/// <param name="fieldName">Name of the member.</param>
        /// <param name="source">The object that contains the member.</param>
		/// <param name="type"></param>
		public static object GetStaticField(string fieldName, object source, Type type)
		{
			FieldInfo field = type.GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Static);
			if(field != null)
			{
				return field.GetValue(type);
			}
			return null;
		}

		/// <summary>
		/// Returns the value of the private member specified.
		/// </summary>
		/// <param name="memberName">Name of the member.</param>
		/// <param name="source">The object that contains the member.</param>
		public static object GetPrivateInstanceField(string memberName, object source)
		{
			FieldInfo field = source.GetType().GetField(memberName, BindingFlags.GetField | BindingFlags.NonPublic | BindingFlags.Instance);
			if(field != null)
			{
				return field.GetValue(source);
			}
			return null;
		}

        public static object RunStaticMethod(Type t, string strMethod, object[] objParams)
        {
            BindingFlags eFlags = BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
            return RunMethod(t, strMethod, null, objParams, eFlags);
        }

        public static object RunInstanceMethod(Type t, string strMethod, object objInstance, object[] aobjParams)
        {
            BindingFlags eFlags = BindingFlags.Instance | BindingFlags.Public |
                                  BindingFlags.NonPublic;
            return RunMethod(t, strMethod, objInstance, aobjParams, eFlags);
        }

        private static object RunMethod(Type t, string strMethod, object objInstance, object[] objParams,
                                        BindingFlags eFlags)
        {
            MethodInfo m = t.GetMethod(strMethod, eFlags);
            if (m == null)
            {
                throw new ArgumentException("There is no method '" + strMethod + "' for type '" + t + "'.");
            }

            object objRet = m.Invoke(objInstance, objParams);
            return objRet;
        }
	}
}