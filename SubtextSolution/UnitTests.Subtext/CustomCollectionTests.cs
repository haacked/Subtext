using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using MbUnit.Framework;
using Subtext.Extensibility;
using Subtext.Extensibility.Providers;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Logging;
using Subtext.Scripting;

namespace UnitTests.Subtext
{
	/// <summary>
	/// Series of tests for the multitude of custom collections.
	/// </summary>
	[TypeFixture(typeof(CollectionHolder))]
	[ProviderFactory(typeof(CollectionProvider), typeof(CollectionHolder))]
	public class CustomCollectionTests
	{
		[Test]
		public void CanAddSingleObject(CollectionHolder holder)
		{
			SetupHolderCollectionWithTwoItems(holder);
			Assert.AreEqual(2, holder.Collection.Count, "expected two items after invoking Add twice.");
		}

		private static void SetupHolderCollectionWithTwoItems(CollectionHolder holder)
		{
			ICollection collection = holder.Collection;
			object instanceToAdd = holder.ObjectToAdd;
			
			MethodInfo addMethod = collection.GetType().GetMethod("Add", new Type[] {instanceToAdd.GetType()});
			
			Assert.IsNotNull(addMethod, "Could not find the method.");
			
			addMethod.Invoke(collection, new object[] {instanceToAdd});
			Assert.AreEqual(1, holder.Collection.Count, "expected one item after invoking Add.");
			
			addMethod.Invoke(collection, new object[] {holder.ObjectsToAdd[0]});
		}

		[Test]
		public void AddRangeCanAddCollection(CollectionHolder holder)
		{
			SetupHolderCollectionWithTwoItems(holder);
			
			//Instantiate new collection.
			ConstructorInfo constructor = holder.Collection.GetType().GetConstructor(new Type[0]);
			ICollection collection = holder.CreateCollectionInstance();
			
			MethodInfo addRangeMethod = collection.GetType().GetMethod("AddRange", new Type[] {collection.GetType()});
			addRangeMethod.Invoke(collection, new object[] {holder.Collection});
			
			Assert.AreEqual(2, collection.Count, "expected two items after invoking AddRange.");
		}
		
		[Test]
		public void AddRangeCanAddArray(CollectionHolder holder)
		{
			//Instantiate new collection.
			object[] objectsToAdd = holder.ObjectsToAdd;
			
			Console.WriteLine("ObjectsToAdd: " + objectsToAdd.GetType().FullName);
			Console.WriteLine("ObjectsToAdd: " + holder.ObjectsToAdd.GetType().FullName);
			MethodInfo addRangeMethod = holder.Collection.GetType().GetMethod("AddRange", new Type[] {objectsToAdd.GetType()});
			addRangeMethod.Invoke(holder.Collection, new object[] {objectsToAdd});
			
			Assert.AreEqual(2, holder.Collection.Count, "expected two items after invoking AddRange.");
		}
	}
	
	public class CollectionProvider
	{
		[Factory]
		public CollectionHolder ProviderCollection
		{
			get
			{
				return new ProviderCollectionHolder(typeof(ProviderCollection), typeof(ProviderInfo), typeof(ProviderInfo[]));
			}
		}
		
		[Factory]
		public CollectionHolder ArchiveCountCollection
		{
			get
			{
				return new CollectionHolder(typeof(ArchiveCountCollection), typeof(ArchiveCount), typeof(ArchiveCount[]));
			}
		}
		
		[Factory]
		public CollectionHolder BlogInfoCollection
		{
			get
			{
				return new CollectionHolder(typeof(BlogInfoCollection), typeof(BlogInfo), typeof(BlogInfo[]));
			}
		}
		
		[Factory]
		public CollectionHolder EntryCollection
		{
			get
			{
				return new EntryCollectionHolder(typeof(EntryCollection), typeof(Entry), typeof(Entry[]));
			}
		}
		
		[Factory]
		public CollectionHolder EntryDayCollection
		{
			get
			{
				return new CollectionHolder(typeof(List<EntryDay>), typeof(EntryDay), typeof(EntryDay[]));
			}
		}
		
		[Factory]
		public CollectionHolder EntryViewCollection
		{
			get
			{
				return new CollectionHolder(typeof(EntryViewCollection), typeof(EntryView), typeof(EntryView[]));
			}
		}
		
		[Factory]
		public CollectionHolder ImageCollection
		{
			get
			{
				return new CollectionHolder(typeof(ImageCollection), typeof(Image), typeof(Image[]));
			}
		}
		
		[Factory]
		public CollectionHolder KeyWordCollection
		{
			get
			{
				return new CollectionHolder(typeof(KeyWordCollection), typeof(KeyWord), typeof(KeyWord[]));
			}
		}
		
		[Factory]
		public CollectionHolder LinkCategoryCollection
		{
			get
			{
				return new CollectionHolder(typeof(LinkCategoryCollection), typeof(LinkCategory), typeof(LinkCategory[]));
			}
		}
		
		[Factory]
		public CollectionHolder LinkCollection
		{
			get
			{
				return new CollectionHolder(typeof(LinkCollection), typeof(Link), typeof(Link[]));
			}
		}
		
		[Factory]
		public CollectionHolder ReferrerCollection
		{
			get
			{
				return new CollectionHolder(typeof(ReferrerCollection), typeof(Referrer), typeof(Referrer[]));
			}
		}
		
		[Factory]
		public CollectionHolder ViewStatCollection
		{
			get
			{
				return new CollectionHolder(typeof(ViewStatCollection), typeof(ViewStat), typeof(ViewStat[]));
			}
		}
		
		[Factory]
		public CollectionHolder LogEntryCollection
		{
			get
			{
				return new CollectionHolder(typeof(LogEntryCollection), typeof(LogEntry), typeof(LogEntry[]));
			}
		}
		
		[Factory]
		public CollectionHolder ScriptCollection
		{
			get
			{
				return new ScriptCollectionHolder(typeof(ScriptCollection), typeof(Script), typeof(Script[]));
			}
		}
		
		[Factory]
		public CollectionHolder TemplateParameterCollection
		{
			get
			{
				return new TemplateParameterCollectionHolder(typeof(TemplateParameterCollection), typeof(TemplateParameter), typeof(TemplateParameter[]));
			}
		}
	}
	
	public class CollectionHolder
	{
		object objectToAdd;
		object[] objectsToAdd;
		
		public CollectionHolder(Type collectionType, Type instanceType, Type instanceArrayType)
		{
			this.CollectionType = collectionType;
			this.InstanceType = instanceType;
			this.InstanceArrayType = instanceArrayType;
		}
		
		/// <summary>
		/// Creates the collection instance.
		/// </summary>
		/// <returns></returns>
		public virtual ICollection CreateCollectionInstance()
		{
			ConstructorInfo constructor = CollectionType.GetConstructor(new Type[0]);
			return constructor.Invoke(null) as ICollection;
		}
		
		/// <summary>
		/// Creates the instance.
		/// </summary>
		/// <returns></returns>
		public virtual object CreateInstance()
		{
			ConstructorInfo constructor = this.InstanceType.GetConstructor(new Type[0]);
			return constructor.Invoke(null);
		}
		
		/// <summary>
		/// Creates the instance array.
		/// </summary>
		/// <returns></returns>
		public virtual object[] CreateInstanceArray()
		{
			ConstructorInfo constructor = this.InstanceArrayType.GetConstructor(new Type[] {typeof(int)});
			object[] instances = (object[])constructor.Invoke(new object[] {2});
			instances[0] = CreateInstance();
			instances[1] = CreateInstance();
			return instances;
		}
	
		Type CollectionType;
		Type InstanceType;
		Type InstanceArrayType;
		ICollection collection;
		
		public ICollection Collection
		{
			get
			{
				if(this.collection == null)
					this.collection = this.CreateCollectionInstance();
				return this.collection;
			}
		}
		
		public object ObjectToAdd
		{
			get
			{
				if(objectToAdd == null)
					objectToAdd = CreateInstance();
				
				return objectToAdd;
			}
		}
		
		public object[] ObjectsToAdd
		{
			get
			{
				if(objectsToAdd == null)
					objectsToAdd = CreateInstanceArray();
				
				return objectsToAdd;
			}
		}
	}
	
	public class ProviderCollectionHolder : CollectionHolder
	{
		public ProviderCollectionHolder(Type collectionType, Type instanceType, Type instanceArrayType) : base(collectionType, instanceType, instanceArrayType)
		{
		}
				
		public override object CreateInstance()
		{
			return UnitTestHelper.CreateProviderInfoInstance("test", "test");
		}
	}
	
	public class EntryCollectionHolder : CollectionHolder
	{
		public EntryCollectionHolder(Type collectionType, Type instanceType, Type instanceArrayType) : base(collectionType, instanceType, instanceArrayType)
		{
		}
		
		public override object CreateInstance()
		{
			return new Entry(PostType.Story);
		}
	}
	
	public class ScriptCollectionHolder : CollectionHolder
	{
		public ScriptCollectionHolder(Type collectionType, Type instanceType, Type instanceArrayType) : base(collectionType, instanceType, instanceArrayType)
		{
		}
		
		public override object CreateInstance()
		{
			return new Script(UnitTestHelper.GenerateRandomHostname());
		}
		
		public override ICollection CreateCollectionInstance()
		{
			return new ScriptCollection(UnitTestHelper.GenerateRandomHostname());
		}
	}
	
	public class TemplateParameterCollectionHolder : CollectionHolder
	{
		public TemplateParameterCollectionHolder(Type collectionType, Type instanceType, Type instanceArrayType) : base(collectionType, instanceType, instanceArrayType)
		{
		}
		
		public override object CreateInstance()
		{
			return new TemplateParameter(UnitTestHelper.GenerateRandomHostname(), "varchar", "testing");
		}
	}
}
