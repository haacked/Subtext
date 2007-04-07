using System;
using System.IO;
using System.Xml;
using System.Reflection;
using  NUnit.Framework;
using CookComputing.XmlRpc;

// TODO: parse array
// TODO: parse struct
// TODO: parse XmlRpcStruct
// TODO: parse XmlRpcStruct derived
// TODO: array of base64

namespace ntest
{
  [TestFixture]
  public class ParseTest
  {
    struct Struct2
    {
      public int mi;
      public string ms;
      public bool mb;
      public double md;
      public DateTime mdt;
      public byte[] mb64;
      public int[] ma;
      public XmlRpcInt xi;
      public XmlRpcBoolean xb;
      public XmlRpcDouble xd;
      public XmlRpcDateTime xdt;
      public XmlRpcStruct xstr;
    }

    //---------------------- int -------------------------------------------// 
    [Test]
    public void Int_NullType()
    {
      Type parsedType, parsedArrayType;
      string xml = @"<?xml version=""1.0"" ?><value><int>12345</int></value>";
      object obj = Utils.Parse(xml, null, MappingAction.Error, 
        out parsedType, out parsedArrayType);
      Assert.AreEqual(12345, (int)obj);
    }
      
    [Test]
    public void Int_IntType()
    {
      Type parsedType, parsedArrayType;
      string xml = @"<?xml version=""1.0"" ?><value><int>12345</int></value>";
      object obj = Utils.Parse(xml, typeof(int), MappingAction.Error, 
        out parsedType, out parsedArrayType);
      Assert.AreEqual(12345, (int)obj);
    }
      
    [Test]
    public void Int_ObjectType()
    {
      Type parsedType, parsedArrayType;
      string xml = @"<?xml version=""1.0"" ?><value><int>12345</int></value>";
      object obj = Utils.Parse(xml, typeof(object), MappingAction.Error, 
        out parsedType, out parsedArrayType);
      Assert.AreEqual(12345, (int)obj);
    }
      
    //---------------------- string ----------------------------------------// 
    [Test]
    public void String_NullType()
    {
      Type parsedType, parsedArrayType;
      string xml = @"<?xml version=""1.0"" ?>
        <value><string>astring</string></value>";
      object obj = Utils.Parse(xml, null, MappingAction.Error, 
        out parsedType, out parsedArrayType);
      Assert.AreEqual("astring", (string)obj);
    }
      
    [Test]
    public void DefaultString_NullType()
    {
      Type parsedType, parsedArrayType;
      string xml = @"<?xml version=""1.0"" ?>
        <value>astring</value>";
      object obj = Utils.Parse(xml, null, MappingAction.Error, 
        out parsedType, out parsedArrayType);
      Assert.AreEqual("astring", (string)obj);
    }

    [Test]
    public void String_StringType()
    {
      Type parsedType, parsedArrayType;
      string xml = @"<?xml version=""1.0"" ?>
        <value><string>astring</string></value>";
      object obj = Utils.Parse(xml, typeof(string), MappingAction.Error, 
        out parsedType, out parsedArrayType);
      Assert.AreEqual("astring", (string)obj);
    }
      
    [Test]
    public void String_ObjectType()
    {
      Type parsedType, parsedArrayType;
      string xml = @"<?xml version=""1.0"" ?>
        <value><string>astring</string></value>";
      object obj = Utils.Parse(xml, typeof(object), MappingAction.Error, 
        out parsedType, out parsedArrayType);
      Assert.AreEqual("astring", (string)obj);
    }
      
    [Test]
    public void DefaultString_StringType()
    {
      Type parsedType, parsedArrayType;
      string xml = @"<?xml version=""1.0"" ?>
        <value>astring</value>";
      object obj = Utils.Parse(xml, typeof(string), MappingAction.Error, 
        out parsedType, out parsedArrayType);
      Assert.AreEqual("astring", (string)obj);
    }

    [Test]
    public void Empty1String_StringType()
    {
      Type parsedType, parsedArrayType;
      string xml = @"<?xml version=""1.0"" ?>
        <value><string></string></value>";
      object obj = Utils.Parse(xml, typeof(string), MappingAction.Error, 
        out parsedType, out parsedArrayType);
      Assert.AreEqual("", (string)obj);
    }
      
    [Test]
    public void Empty2String_StringType()
    {
      Type parsedType, parsedArrayType;
      string xml = @"<?xml version=""1.0"" ?>
        <value><string/></value>";
      object obj = Utils.Parse(xml, typeof(string), MappingAction.Error, 
        out parsedType, out parsedArrayType);
      Assert.AreEqual("", (string)obj);
    }
      
    [Test]
    public void Default1EmptyString_StringType()
    {
      Type parsedType, parsedArrayType;
      string xml = @"<?xml version=""1.0"" ?>
        <value></value>";
      object obj = Utils.Parse(xml, typeof(string), MappingAction.Error, 
        out parsedType, out parsedArrayType);
      Assert.AreEqual("", (string)obj);
    }

    [Test]
    public void Default2EmptyString_StringType()
    {
      Type parsedType, parsedArrayType;
      string xml = @"<?xml version=""1.0"" ?>
        <value/>";
      object obj = Utils.Parse(xml, typeof(string), MappingAction.Error, 
        out parsedType, out parsedArrayType);
      Assert.AreEqual("", (string)obj);
    }

    //---------------------- boolean ---------------------------------------// 
    [Test]
    public void Boolean_NullType()
    {
      Type parsedType, parsedArrayType;
      string xml = @"<?xml version=""1.0"" ?>
        <value><boolean>1</boolean></value>";
      object obj = Utils.Parse(xml, null, MappingAction.Error, 
        out parsedType, out parsedArrayType);
      Assert.AreEqual(true, (bool)obj);
    }
      
    [Test]
    public void Boolean_BooleanType()
    {
      Type parsedType, parsedArrayType;
      string xml = @"<?xml version=""1.0"" ?>
        <value><boolean>1</boolean></value>";
      object obj = Utils.Parse(xml, typeof(bool), MappingAction.Error, 
        out parsedType, out parsedArrayType);
      Assert.AreEqual(true, (bool)obj);
    }
      
    [Test]
    public void Boolean_ObjectType()
    {
      Type parsedType, parsedArrayType;
      string xml = @"<?xml version=""1.0"" ?>
        <value><boolean>1</boolean></value>";
      object obj = Utils.Parse(xml, typeof(object), MappingAction.Error, 
        out parsedType, out parsedArrayType);
      Assert.AreEqual(true, (bool)obj);
    }
      
    //---------------------- double ----------------------------------------// 
    [Test]
    public void Double_NullType()
    {
      Type parsedType, parsedArrayType;
      string xml = @"<?xml version=""1.0"" ?>
        <value><double>543.21</double></value>";
      object obj = Utils.Parse(xml, null, MappingAction.Error, 
        out parsedType, out parsedArrayType);
      Assert.AreEqual(543.21, (double)obj);
    }
      
    [Test]
    public void Double_DoubleType()
    {
      Type parsedType, parsedArrayType;
      string xml = @"<?xml version=""1.0"" ?>
        <value><double>543.21</double></value>";
      object obj = Utils.Parse(xml, typeof(double), MappingAction.Error, 
        out parsedType, out parsedArrayType);
      Assert.AreEqual(543.21, (double)obj);
    }
      
    [Test]
    public void Double_ObjectType()
    {
      Type parsedType, parsedArrayType;
      string xml = @"<?xml version=""1.0"" ?>
        <value><double>543.21</double></value>";
      object obj = Utils.Parse(xml, typeof(double), MappingAction.Error, 
        out parsedType, out parsedArrayType);
      Assert.AreEqual(543.21, (double)obj);
    }
      
    //---------------------- dateTime ------------------------------------// 
    [Test]
    public void DateTime_NullType()
    {
      Type parsedType, parsedArrayType;
      string xml = @"<?xml version=""1.0"" ?>
        <value><dateTime.iso8601>20020706T11:25:37</dateTime.iso8601></value>";
      object obj = Utils.Parse(xml, null, MappingAction.Error, 
        out parsedType, out parsedArrayType);
      Assert.AreEqual(new DateTime(2002, 7, 6, 11, 25, 37), 
        (DateTime)obj);
    }
      
    [Test]
    public void DateTime_DateTimeType()
    {
      Type parsedType, parsedArrayType;
      string xml = @"<?xml version=""1.0"" ?>
        <value><dateTime.iso8601>20020706T11:25:37</dateTime.iso8601></value>";
      object obj = Utils.Parse(xml, typeof(DateTime), MappingAction.Error, 
        out parsedType, out parsedArrayType);
      Assert.AreEqual(new DateTime(2002, 7, 6, 11, 25, 37), 
        (DateTime)obj);
    }
      
    [Test]
    public void DateTime_ObjectTimeType()
    {
      Type parsedType, parsedArrayType;
      string xml = @"<?xml version=""1.0"" ?>
        <value><dateTime.iso8601>20020706T11:25:37</dateTime.iso8601></value>";
      object obj = Utils.Parse(xml, typeof(object), MappingAction.Error, 
        out parsedType, out parsedArrayType);
      Assert.AreEqual(new DateTime(2002, 7, 6, 11, 25, 37), 
        (DateTime)obj);
    }

    [Test]
    public void DateTime_ROCA()
    {
      Type parsedType, parsedArrayType;
      string xml = @"<?xml version=""1.0"" ?>
        <value><dateTime.iso8601>2002-07-06T11:25:37</dateTime.iso8601></value>";
      XmlRpcSerializer serializer = new XmlRpcSerializer();
      serializer.NonStandard = XmlRpcNonStandard.AllowNonStandardDateTime;
      object obj = Utils.Parse(xml, typeof(DateTime), MappingAction.Error, serializer,
        out parsedType, out parsedArrayType);
      Assert.AreEqual(new DateTime(2002, 7, 6, 11, 25, 37), 
        (DateTime)obj);
    }

    [Test]
    public void DateTime_allZeros1()
    {
      Type parsedType, parsedArrayType;
      string xml = @"<?xml version=""1.0"" ?>
        <value><dateTime.iso8601>0000-00-00T00:00:00</dateTime.iso8601></value>";
      XmlRpcSerializer serializer = new XmlRpcSerializer();
      serializer.NonStandard = XmlRpcNonStandard.MapZerosDateTimeToMinValue;
      object obj = Utils.Parse(xml, typeof(DateTime), MappingAction.Error, serializer,
        out parsedType, out parsedArrayType);
      Assert.AreEqual(DateTime.MinValue, (DateTime)obj);
    }

    [Test]
    public void DateTime_allZeros2()
    {
      Type parsedType, parsedArrayType;
      string xml = @"<?xml version=""1.0"" ?>
        <value><dateTime.iso8601>0000-00-00T00:00:00Z</dateTime.iso8601></value>";
      XmlRpcSerializer serializer = new XmlRpcSerializer();
      serializer.NonStandard = XmlRpcNonStandard.MapZerosDateTimeToMinValue;
      object obj = Utils.Parse(xml, typeof(DateTime), MappingAction.Error, serializer,
        out parsedType, out parsedArrayType);
      Assert.AreEqual(DateTime.MinValue, (DateTime)obj);
    }

    [Test]
    public void DateTime_allZeros3()
    {
      Type parsedType, parsedArrayType;
      string xml = @"<?xml version=""1.0"" ?>
        <value><dateTime.iso8601>00000000T00:00:00Z</dateTime.iso8601></value>";
      XmlRpcSerializer serializer = new XmlRpcSerializer();
      serializer.NonStandard = XmlRpcNonStandard.MapZerosDateTimeToMinValue;
      object obj = Utils.Parse(xml, typeof(DateTime), MappingAction.Error, serializer,
        out parsedType, out parsedArrayType);
      Assert.AreEqual(DateTime.MinValue, (DateTime)obj);
    }

    [Test]
    public void DateTime_allZeros4()
    {
      Type parsedType, parsedArrayType;
      string xml = @"<?xml version=""1.0"" ?>
        <value><dateTime.iso8601>0000-00-00T00:00:00</dateTime.iso8601></value>";
      XmlRpcSerializer serializer = new XmlRpcSerializer();
      serializer.NonStandard = XmlRpcNonStandard.MapZerosDateTimeToMinValue;
      object obj = Utils.Parse(xml, typeof(DateTime), MappingAction.Error, serializer,
        out parsedType, out parsedArrayType);
      Assert.AreEqual(DateTime.MinValue, (DateTime)obj);
    }

    [Test]
    public void DateTime_Empty_Standard()
    {
      Type parsedType, parsedArrayType;
      string xml = @"<?xml version=""1.0"" ?>
        <value><dateTime.iso8601></dateTime.iso8601></value>";
      XmlRpcSerializer serializer = new XmlRpcSerializer();
      serializer.NonStandard = XmlRpcNonStandard.MapEmptyDateTimeToMinValue;
      object obj = Utils.Parse(xml, typeof(DateTime), MappingAction.Error, serializer,
        out parsedType, out parsedArrayType);
      Assert.AreEqual(DateTime.MinValue, (DateTime)obj);
    }

    [Test]
    [ExpectedException(typeof(XmlRpcInvalidXmlRpcException))]
    public void DateTime_Empty_NonStandard()
    {
      Type parsedType, parsedArrayType;
      string xml = @"<?xml version=""1.0"" ?>
        <value><dateTime.iso8601></dateTime.iso8601></value>";
      XmlRpcSerializer serializer = new XmlRpcSerializer();
      object obj = Utils.Parse(xml, typeof(DateTime), MappingAction.Error, serializer,
        out parsedType, out parsedArrayType);
    }

    //---------------------- base64 ----------------------------------------// 
    byte[] testb = new Byte[] 
        {
            121, 111, 117, 32, 99, 97, 110, 39, 116, 32, 114, 101, 97, 100, 
          32, 116, 104, 105, 115, 33 };

    [Test]
    public void Base64_NullType()
    {
      Type parsedType, parsedArrayType;
      string xml = @"<?xml version=""1.0"" ?>
        <value><base64>eW91IGNhbid0IHJlYWQgdGhpcyE=</base64></value>";
      object obj = Utils.Parse(xml, null, MappingAction.Error, 
        out parsedType, out parsedArrayType);
      Assert.IsTrue(obj is byte[], "result is array of byte");
      byte[] ret = obj as byte[];
      Assert.IsTrue(ret.Length == testb.Length);
      for (int i = 0; i < testb.Length; i++)
        Assert.IsTrue(testb[i] == ret[i]);
    }

    [Test]
    public void Base64_Base64Type()
    {
      Type parsedType, parsedArrayType;
      string xml = @"<?xml version=""1.0"" ?>
        <value><base64>eW91IGNhbid0IHJlYWQgdGhpcyE=</base64></value>";
      object obj = Utils.Parse(xml, typeof(byte[]), MappingAction.Error, 
        out parsedType, out parsedArrayType);
      Assert.IsTrue(obj is byte[], "result is array of byte");
      byte[] ret = obj as byte[];
      Assert.IsTrue(ret.Length == testb.Length);
      for (int i = 0; i < testb.Length; i++)
        Assert.IsTrue(testb[i] == ret[i]);
    }

    [Test]
    public void Base64_ObjectType()
    {
      Type parsedType, parsedArrayType;
      string xml = @"<?xml version=""1.0"" ?>
        <value><base64>eW91IGNhbid0IHJlYWQgdGhpcyE=</base64></value>";
      object obj = Utils.Parse(xml, typeof(object), MappingAction.Error, 
        out parsedType, out parsedArrayType);
      Assert.IsTrue(obj is byte[], "result is array of byte");
      byte[] ret = obj as byte[];
      Assert.IsTrue(ret.Length == testb.Length);
      for (int i = 0; i < testb.Length; i++)
        Assert.IsTrue(testb[i] == ret[i]);
    }
  
    //---------------------- array -----------------------------------------// 
    [Test]
    public void MixedArray_NullType()
    {
      Type parsedType, parsedArrayType;
      string xml = @"<?xml version=""1.0"" ?>
<value>
  <array>
    <data>
      <value><i4>12</i4></value>
      <value><string>Egypt</string></value>
      <value><boolean>0</boolean></value>
    </data>
  </array>
</value>";
      object obj = Utils.Parse(xml, null, MappingAction.Error, 
        out parsedType, out parsedArrayType);
      Assert.IsTrue(obj is object[], "result is array of object");
      object[] ret = obj as object[];
      Assert.AreEqual(12, ret[0]);
      Assert.AreEqual("Egypt", ret[1]);
      Assert.AreEqual(false, ret[2]);
    }

    [Test]
    public void MixedArray_ObjectArrayType()
    {
      Type parsedType, parsedArrayType;
      string xml = @"<?xml version=""1.0"" ?>
<value>
  <array>
    <data>
      <value><i4>12</i4></value>
      <value><string>Egypt</string></value>
      <value><boolean>0</boolean></value>
    </data>
  </array>
</value>";
      object obj = Utils.Parse(xml, typeof(object[]), MappingAction.Error, 
        out parsedType, out parsedArrayType);
      Assert.IsTrue(obj is object[], "result is array of object");
      object[] ret = obj as object[];
      Assert.AreEqual(12, ret[0]);
      Assert.AreEqual("Egypt", ret[1]);
      Assert.AreEqual(false, ret[2]);
    }

    [Test]
    public void MixedArray_ObjectType()
    {
      Type parsedType, parsedArrayType;
      string xml = @"<?xml version=""1.0"" ?>
<value>
  <array>
    <data>
      <value><i4>12</i4></value>
      <value><string>Egypt</string></value>
      <value><boolean>0</boolean></value>
    </data>
  </array>
</value>";
      object obj = Utils.Parse(xml, typeof(object), MappingAction.Error, 
        out parsedType, out parsedArrayType);
      Assert.IsTrue(obj is object[], "result is array of object");
      object[] ret = obj as object[];
      Assert.AreEqual(12, ret[0]);
      Assert.AreEqual("Egypt", ret[1]);
      Assert.AreEqual(false, ret[2]);
    }

    [Test]
    public void HomogArray_NullType()
    {
      Type parsedType, parsedArrayType;
      string xml = @"<?xml version=""1.0"" ?>
<value>
  <array>
    <data>
      <value><i4>12</i4></value>
      <value><i4>13</i4></value>
      <value><i4>14</i4></value>
    </data>
  </array>
</value>";
      object obj = Utils.Parse(xml, null, MappingAction.Error, 
        out parsedType, out parsedArrayType);
      Assert.IsTrue(obj is int[], "result is array of int");
      int[] ret = obj as int[];
      Assert.AreEqual(12, ret[0]);
      Assert.AreEqual(13, ret[1]);
      Assert.AreEqual(14, ret[2]);
    }

    [Test]
    public void HomogArray_IntArrayType()
    {
      Type parsedType, parsedArrayType;
      string xml = @"<?xml version=""1.0"" ?>
<value>
  <array>
    <data>
      <value><i4>12</i4></value>
      <value><i4>13</i4></value>
      <value><i4>14</i4></value>
    </data>
  </array>
</value>";
      object obj = Utils.Parse(xml, typeof(int[]), MappingAction.Error, 
        out parsedType, out parsedArrayType);
      Assert.IsTrue(obj is int[], "result is array of int");
      int[] ret = obj as int[];
      Assert.AreEqual(12, ret[0]);
      Assert.AreEqual(13, ret[1]);
      Assert.AreEqual(14, ret[2]);
    }

    [Test]
    public void HomogArray_ObjectArrayType()
    {
      Type parsedType, parsedArrayType;
      string xml = @"<?xml version=""1.0"" ?>
<value>
  <array>
    <data>
      <value><i4>12</i4></value>
      <value><i4>13</i4></value>
      <value><i4>14</i4></value>
    </data>
  </array>
</value>";
      object obj = Utils.Parse(xml, typeof(object[]), MappingAction.Error, 
      out parsedType, out parsedArrayType);
      Assert.IsTrue(obj is object[], "result is array of object");
      object[] ret = obj as object[];
      Assert.AreEqual(12, ret[0]);
      Assert.AreEqual(13, ret[1]);
      Assert.AreEqual(14, ret[2]);
    }

    [Test]
    public void HomogArray_ObjectType()
    {
      Type parsedType, parsedArrayType;
      string xml = @"<?xml version=""1.0"" ?>
<value>
  <array>
    <data>
      <value><i4>12</i4></value>
      <value><i4>13</i4></value>
      <value><i4>14</i4></value>
    </data>
  </array>
</value>";
      object obj = Utils.Parse(xml, typeof(object), MappingAction.Error, 
        out parsedType, out parsedArrayType);
      Assert.IsTrue(obj is int[], "result is array of int");
      int[] ret = obj as int[];
      Assert.AreEqual(12, ret[0]);
      Assert.AreEqual(13, ret[1]);
      Assert.AreEqual(14, ret[2]);
    }
      
    //---------------------- XmlRpcInt -------------------------------------// 
    [Test]
    public void XmlRpcInt_XmlRpcIntType()
    {
      Type parsedType, parsedArrayType;
      string xml = @"<?xml version=""1.0"" ?><value><int>12345</int></value>";
      object obj = Utils.Parse(xml, typeof(XmlRpcInt), MappingAction.Error, 
        out parsedType, out parsedArrayType);
      Assert.IsInstanceOfType(typeof(XmlRpcInt), obj);
      Assert.AreEqual(12345, (XmlRpcInt)obj);
    }

    //---------------------- XmlRpcBoolean ---------------------------------// 
    [Test]
    public void XmlRpcBoolean_XmlRpcBooleanType()
    {
      Type parsedType, parsedArrayType;
      string xml = @"<?xml version=""1.0"" ?>
        <value><boolean>1</boolean></value>";
      object obj = Utils.Parse(xml, typeof(XmlRpcBoolean), MappingAction.Error, 
        out parsedType, out parsedArrayType);
      Assert.AreEqual(new XmlRpcBoolean(true), (XmlRpcBoolean)obj);
    }
            
    //---------------------- XmlRpcDouble ----------------------------------// 
    [Test]
    public void XmlRpcDouble_XmlRpcDoubleType()
    {
      Type parsedType, parsedArrayType;
      string xml = @"<?xml version=""1.0"" ?>
        <value><double>543.21</double></value>";
      object obj = Utils.Parse(xml, typeof(XmlRpcDouble), MappingAction.Error, 
        out parsedType, out parsedArrayType);
      Assert.AreEqual(new XmlRpcDouble(543.21), (XmlRpcDouble)obj);
    }
      
    //---------------------- XmlRpcDateTime --------------------------------// 
    [Test]
    public void XmlRpcDateTime_XmlRpcDateTimeType()
    {
      Type parsedType, parsedArrayType;
      string xml = @"<?xml version=""1.0"" ?>
        <value><dateTime.iso8601>20020706T11:25:37</dateTime.iso8601></value>";
      object obj = Utils.Parse(xml, typeof(XmlRpcDateTime), MappingAction.Error, 
        out parsedType, out parsedArrayType);
      Assert.AreEqual(
        new XmlRpcDateTime(new DateTime(2002, 7, 6, 11, 25, 37)), 
        (XmlRpcDateTime)obj);
    }

#if !FX1_0
    //---------------------- int? -------------------------------------// 
    [Test]
    public void nullableIntType()
    {
      Type parsedType, parsedArrayType;
      string xml = @"<?xml version=""1.0"" ?><value><int>12345</int></value>";
      object obj = Utils.Parse(xml, typeof(int?), MappingAction.Error,
        out parsedType, out parsedArrayType);
      Assert.IsInstanceOfType(typeof(int?), obj);
      Assert.AreEqual(12345, obj);
    }

    //---------------------- bool? ---------------------------------// 
    [Test]
    public void nullableBoolType()
    {
      Type parsedType, parsedArrayType;
      string xml = @"<?xml version=""1.0"" ?>
        <value><boolean>1</boolean></value>";
      object obj = Utils.Parse(xml, typeof(bool?), MappingAction.Error,
        out parsedType, out parsedArrayType);
      Assert.AreEqual(true, obj);
    }

    //---------------------- double? ----------------------------------// 
    [Test]
    public void nullableDoubleType()
    {
      Type parsedType, parsedArrayType;
      string xml = @"<?xml version=""1.0"" ?>
        <value><double>543.21</double></value>";
      object obj = Utils.Parse(xml, typeof(double?), MappingAction.Error,
        out parsedType, out parsedArrayType);
      Assert.AreEqual(543.21, obj);
    }

    //---------------------- DateTime? --------------------------------// 
    [Test]
    public void nullableDateTimeType()
    {
      Type parsedType, parsedArrayType;
      string xml = @"<?xml version=""1.0"" ?>
        <value><dateTime.iso8601>20020706T11:25:37</dateTime.iso8601></value>";
      object obj = Utils.Parse(xml, typeof(DateTime?), MappingAction.Error,
        out parsedType, out parsedArrayType);
      Assert.AreEqual(new DateTime(2002, 7, 6, 11, 25, 37), obj);
    }
#endif
     
  //---------------------- XmlRpcStruct array ----------------------------// 
    [Test]
    public void XmlRpcStructArray()
    {
      Type parsedType, parsedArrayType;
      string xml = @"<?xml version=""1.0"" ?>
<value>
  <array>
    <data>
      <value>
        <struct>
          <member>
            <name>mi</name>
            <value><i4>18</i4></value>
          </member>
        </struct>
      </value>
      <value>
        <struct>
          <member>
            <name>mi</name>
            <value><i4>28</i4></value>
          </member>
        </struct>
      </value>
    </data>
  </array>
</value>";

      object obj = Utils.Parse(xml, null, MappingAction.Error, 
        out parsedType, out parsedArrayType);

      Assert.AreEqual(obj.GetType(), typeof(object[]));
      object[] objarray = (object[])obj;
      Assert.AreEqual(objarray[0].GetType(), typeof(XmlRpcStruct));
      Assert.AreEqual(objarray[1].GetType(), typeof(XmlRpcStruct));
      XmlRpcStruct xstruct1 = objarray[0] as XmlRpcStruct;
      XmlRpcStruct xstruct2 = objarray[1] as XmlRpcStruct;

      Assert.AreEqual(xstruct1["mi"], 18);
      Assert.AreEqual(xstruct2["mi"], 28);
    }


  }
}
