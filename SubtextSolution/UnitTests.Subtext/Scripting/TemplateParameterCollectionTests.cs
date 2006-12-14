using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using MbUnit.Framework;
using Subtext.Scripting;

namespace UnitTests.Subtext.Scripting
{
	[TestFixture]
	public class TemplateParameterCollectionTests
	{
		[Test]
		public void CanCopyToArray()
		{
			TemplateParameterCollection parameters = new TemplateParameterCollection();
			parameters.Add(new TemplateParameter("test0", "int", "0"));
			parameters.Add(new TemplateParameter("test1", "int", "1"));
			TemplateParameter[] paramArray = new TemplateParameter[2];
			parameters.CopyTo(paramArray, 0);

			Assert.AreEqual(paramArray[0], parameters[0]);
			Assert.AreEqual(paramArray[1], parameters[1]);
		}

		[Test]
		public void CanClear()
		{
			TemplateParameterCollection parameters = new TemplateParameterCollection();
			((ICollection<TemplateParameter>)parameters).Add(new TemplateParameter("test", "int", "0"));
			Assert.AreEqual(1, parameters.Count);
			parameters.Clear();
			Assert.AreEqual(0, parameters.Count);
		}

		[Test]
		public void CanRemove()
		{
			TemplateParameterCollection parameters = new TemplateParameterCollection();
			TemplateParameter zero = new TemplateParameter("test0", "int", "0");
			TemplateParameter one = new TemplateParameter("test1", "int", "1");
			parameters.Add(zero);
			parameters.Add(one);
			parameters.Remove(zero);
			Assert.AreEqual(1, parameters.Count);
			Assert.AreEqual(one, parameters[0]);
		}

		[Test]
		public void IsReadOnlyIsFalse()
		{
			TemplateParameterCollection parameters = new TemplateParameterCollection();
			Assert.IsFalse(parameters.IsReadOnly);
		}

		[Test]
		public void IndexOfReturnsCorrectIndex()
		{
			TemplateParameterCollection parameters = new TemplateParameterCollection();
			TemplateParameter zero = new TemplateParameter("test0", "int", "0");
			TemplateParameter one = new TemplateParameter("test1", "int", "1");
			parameters.Add(zero);
			parameters.Add(one);
			Assert.AreEqual(2, parameters.Count);
			Assert.AreEqual(0, parameters.IndexOf(zero));
			Assert.AreEqual(1, parameters.IndexOf(one));
		}

		[Test]
		[ExpectedArgumentNullException]
		public void AddThrowsArgumentNullExceptionForNullMatch()
		{
			TemplateParameterCollection parameters = new TemplateParameterCollection();
			Match nullParam = null;
			parameters.Add(nullParam);
		}

		[Test]
		[ExpectedArgumentNullException]
		public void ContainsThrowsArgumentNullExceptionForNullString()
		{
			TemplateParameterCollection parameters = new TemplateParameterCollection();
			string s = null;
			parameters.Contains(s);
		}

		[Test]
		[ExpectedArgumentNullException]
		public void ContainsThrowsArgumentNullExceptionForNullTemplateParameter()
		{
			TemplateParameterCollection parameters = new TemplateParameterCollection();
			TemplateParameter nullParam = null;
			parameters.Contains(nullParam);
		}

		[Test]
		[ExpectedArgumentNullException]
		public void AddThrowsArgumentNullExceptionForNullTemplateParameter()
		{
			TemplateParameterCollection parameters = new TemplateParameterCollection();
			TemplateParameter nullParam = null;
			parameters.Add(nullParam);
		}
	}
}
