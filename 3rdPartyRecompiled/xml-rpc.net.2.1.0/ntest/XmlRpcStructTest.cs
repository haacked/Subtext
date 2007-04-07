using System;
using System.IO;
using System.Reflection;
using  NUnit.Framework;
using CookComputing.XmlRpc;

namespace ntest
{
  [TestFixture]
  public class XmlRpcStructTest
  {
    [Test]
    public void Set()
    {
      XmlRpcStruct xps = new XmlRpcStruct();
      xps["foo"] = "abcdef";
      Assert.AreEqual("abcdef", xps["foo"]);
    }

    [Test]
    [ExpectedException(typeof(ArgumentException))]
    public void SetInvalidKey()
    {
      XmlRpcStruct xps = new XmlRpcStruct();
      xps[1] = "abcdef";
    }

    [Test]
    public void DoubleSet()
    {
      XmlRpcStruct xps = new XmlRpcStruct();
      xps["foo"] = "12345";
      xps["foo"] = "abcdef";
      Assert.AreEqual("abcdef", xps["foo"]);
    }

    [Test]
    [ExpectedException(typeof(ArgumentException))]
    public void AddInvalidKey()
    {
      XmlRpcStruct xps = new XmlRpcStruct();
      xps.Add(1, "abcdef");
    }

    [Test]
    public void Add()
    {
      XmlRpcStruct xps = new XmlRpcStruct();
      xps.Add("foo", "abcdef");
      Assert.AreEqual("abcdef", xps["foo"]);
    }

    [Test]
    [ExpectedException(typeof(ArgumentException))]
    public void DoubleAdd()
    {
      XmlRpcStruct xps = new XmlRpcStruct();
      xps.Add("foo", "123456");
      xps.Add("foo", "abcdef");
      Assert.Fail("Test should throw ArgumentException");
    }

  }
}
