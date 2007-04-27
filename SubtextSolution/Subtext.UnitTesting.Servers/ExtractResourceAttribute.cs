using System;
using System.Collections;
using System.IO;
using System.Reflection;
using MbUnit.Core.Framework;
using MbUnit.Core.Invokers;
using MbUnit.Framework;

namespace Subtext.UnitTesting.Servers
{
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
	public class ExtractResourceAttribute : DecoratorPatternAttribute
	{
		/// <summary>
		/// Extracts the resource to a stream. Access the stream like so: <see cref="ExtractResourceAttribute.Stream" />.
		/// </summary>
		/// <param name="resourceName"></param>
		public ExtractResourceAttribute(string resourceName) : this(resourceName, (Type)null)
		{
		}

		/// <summary>
		/// Extracts the resource to a stream.
		/// </summary>
		/// <param name="resourceName"></param>
		/// <param name="type"></param>
		public ExtractResourceAttribute(string resourceName, Type type)
		{
			this.resourceName = resourceName;
			this.type = type;
		}

		/// <summary>
		/// Extracts the specified resource to the destination. 
		/// The destination should be a file name. Will attempt to cleanup resource 
		/// after the test is complete.
		/// </summary>
		/// <param name="resourceName"></param>
		/// <param name="destination"></param>
		public ExtractResourceAttribute(string resourceName, string destination) : this(resourceName, destination, ResourceCleanup.DeleteAfterTest, null)
		{
		}

		/// <summary>
		/// Extracts the specified resource to the destination. 
		/// The destination should be a file name.
		/// </summary>
		/// <param name="resourceName"></param>
		/// <param name="destination"></param>
		/// <param name="cleanupOptions">Whether or not to try and cleanup the resource at the end</param>
		public ExtractResourceAttribute(string resourceName, string destination, ResourceCleanup cleanupOptions) : this(resourceName, destination, cleanupOptions, null)
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="resourceName"></param>
		/// <param name="destination"></param>
		/// <param name="cleanupOptions"></param>
		/// <param name="type"></param>
		public ExtractResourceAttribute(string resourceName, string destination, ResourceCleanup cleanupOptions, Type type)
		{
			this.type = type;
			this.resourceName = resourceName;
			this.destination = destination;
			this.resourceCleanup = cleanupOptions;
		}
		
		/// <summary>
		/// The full name of the resource. Use Reflector to find this out 
		/// if you don't know.
		/// </summary>
		public string ResourceName
		{
			get { return this.resourceName; }
			set { this.resourceName = value; }
		}
		string resourceName;

		/// <summary>
		/// The destination file to write the resource to. 
		/// Should be a path.
		/// </summary>
		public string Destination
		{
			get { return this.destination; }
			set { this.destination = value; }
		}
		string destination;

		/// <summary>
		/// Whether or not to cleanup the resource.
		/// </summary>
		public ResourceCleanup ResourceCleanup
		{
			get { return this.resourceCleanup; }
			set { this.resourceCleanup = value; }
		}

		private ResourceCleanup resourceCleanup;

		/// <summary>
		/// The current resource stream if using the attribute without specifying 
		/// a destination.
		/// </summary>
		public static Stream Stream
		{
			get { return stream; }
		}

		[ThreadStatic]
		private static Stream stream;

		/// <summary>
		/// The type within the assembly that contains the embedded resource.
		/// </summary>
		public Type Type
		{
			get { return this.type; }
			set { this.type = value; }
		}

		private Type type;

		public override IRunInvoker GetInvoker(IRunInvoker invoker)
		{
			return new ExtractResourceRunInvoker(invoker, this);
		}

		private sealed class ExtractResourceRunInvoker : DecoratorRunInvoker
		{
			private ExtractResourceAttribute attribute;

			public ExtractResourceRunInvoker(IRunInvoker invoker, ExtractResourceAttribute attribute) : base(invoker)
			{
				MethodRunInvoker methodInvoker = invoker as MethodRunInvoker;
				if(methodInvoker != null && attribute.Type == null)
				{
					attribute.Type = methodInvoker.Method.DeclaringType;
				}

				this.attribute = attribute;
			}

			public override object Execute(object o, IList args)
			{
				Assembly assembly = attribute.Type.Assembly;
				
				using(Stream stream = assembly.GetManifestResourceStream(attribute.ResourceName))
				{
					if (String.IsNullOrEmpty(attribute.Destination))
					{
						ExtractResourceAttribute.stream = stream;
						return this.Invoker.Execute(o, args);
					}
					else
					{
						WriteResourceToFile(stream);
					}
				}

				try
				{
					return this.Invoker.Execute(o, args);
				}
				finally
				{
					if(attribute.ResourceCleanup == ResourceCleanup.DeleteAfterTest)
						File.Delete(attribute.Destination);
				}
			}

			private void WriteResourceToFile(Stream stream)
			{
				using (StreamWriter outfile = File.CreateText(attribute.Destination))
				{
					using (StreamReader infile = new StreamReader(stream))
					{
						outfile.Write(infile.ReadToEnd());
					}
				}
			}
		}
	}

	public enum ResourceCleanup
	{
		NoCleanup = 0,
		DeleteAfterTest = 1,
	}

	[TestFixture]
	public class ExtractResourceTests
	{
		[Test]
		[ExtractResource("Subtext.UnitTesting.Servers.TestResource.txt", "TestResource.txt")]
		public void CanExtractResource()
		{
			Assert.IsTrue(File.Exists("TestResource.txt"), "Could not find the test resource");
		}

		[Test]
		[ExtractResource("Subtext.UnitTesting.Servers.TestResource.txt")]
		public void CanExtractResourceToStream()
		{
			Stream stream = ExtractResourceAttribute.Stream;
			Assert.IsNotNull(stream, "The Stream is null");
			using(StreamReader reader = new StreamReader(stream))
			{
				Assert.AreEqual("Hello World!", reader.ReadToEnd(), "The contents of the resource is not what we expected.");
			}
		}
	}
}
