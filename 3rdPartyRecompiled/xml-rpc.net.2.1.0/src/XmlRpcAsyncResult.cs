/* 
XML-RPC.NET library
Copyright (c) 2001-2006, Charles Cook <charlescook@cookcomputing.com>

Permission is hereby granted, free of charge, to any person 
obtaining a copy of this software and associated documentation 
files (the "Software"), to deal in the Software without restriction, 
including without limitation the rights to use, copy, modify, merge, 
publish, distribute, sublicense, and/or sell copies of the Software, 
and to permit persons to whom the Software is furnished to do so, 
subject to the following conditions:

The above copyright notice and this permission notice shall be 
included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, 
EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES 
OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND 
NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT 
HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, 
WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, 
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
DEALINGS IN THE SOFTWARE.
*/

namespace CookComputing.XmlRpc
{
  using System;
  using System.IO;
  using System.Net;
  using System.Reflection;
  using System.Text;
  using System.Threading;

  public class XmlRpcAsyncResult : IAsyncResult
  {
    // IAsyncResult members
    public object AsyncState 
    { 
      get { return userAsyncState; } 
    }

    public WaitHandle AsyncWaitHandle 
    {
      get
      {
        bool completed = isCompleted;
        if (manualResetEvent == null)
        {
          lock(this)
          {
            if (manualResetEvent == null)
              manualResetEvent = new ManualResetEvent(completed);
          }
        }
        if (!completed && isCompleted)
          manualResetEvent.Set();
        return manualResetEvent;
      }
    }

    public bool CompletedSynchronously 
    { 
      get { return completedSynchronously; } 
      set 
      { 
        if (completedSynchronously)
          completedSynchronously = value;
      }
    }

    public bool IsCompleted 
    {
      get { return isCompleted; } 
    }

    public bool UseIndentation 
    {
      get { return _useIndentation; } 
    }

    public int Indentation 
    {
      get { return _indentation; } 
    }

    public bool UseIntTag
    {
      get { return _useIntTag; }
    }

    // public members
    public void Abort()
    {
      if (request != null)
        request.Abort();
    }

    public Exception Exception 
    {
      get { return exception; } 
    }

    public XmlRpcClientProtocol ClientProtocol 
    { 
      get { return clientProtocol; } 
    }

    //internal members
    internal XmlRpcAsyncResult(
      XmlRpcClientProtocol ClientProtocol, 
      XmlRpcRequest XmlRpcReq, 
      Encoding XmlEncoding,
      bool useIndentation,
      int indentation,
      bool UseIntTag,
      WebRequest Request, 
      AsyncCallback UserCallback, 
      object UserAsyncState, 
      int retryNumber)
    {
      xmlRpcRequest = XmlRpcReq;
      clientProtocol = ClientProtocol;
      request = Request;
      userAsyncState = UserAsyncState;
      userCallback = UserCallback;
      completedSynchronously = true;
      xmlEncoding = XmlEncoding;
      _useIndentation = useIndentation;
      _indentation = indentation;
      _useIntTag = UseIntTag;
    }
  
    internal void Complete(
      Exception ex)
    {
      exception = ex;
      Complete();
    }

    internal void Complete()
    {
      try
      {
        if (responseStream != null)
        {
          responseStream.Close();
          responseStream = null;
        }
        if (responseBufferedStream != null)
          responseBufferedStream.Position = 0;
      }
      catch(Exception ex)
      {
        if (exception == null)
          exception = ex;
      }
      isCompleted = true;
      try
      {
        if (manualResetEvent != null)
          manualResetEvent.Set();
      }
      catch(Exception ex)
      {
        if (exception == null)
          exception = ex;
      }
      if (userCallback != null)
        userCallback(this);
    }

    internal WebResponse WaitForResponse()
    {
      if (!isCompleted)
        AsyncWaitHandle.WaitOne();
      if (exception != null)
        throw exception;
      return response;
    }

    internal bool EndSendCalled 
    { 
      get { return endSendCalled; } 
      set { endSendCalled = value; }
    }

    internal byte[] Buffer 
    { 
      get { return buffer; } 
      set { buffer = value; }
    }

    internal WebRequest Request { get { return request; } 
    }

    internal WebResponse Response 
    { 
      get { return response; } 
      set { response = value; }
    }

    internal Stream ResponseStream
    { 
      get { return responseStream; } 
      set { responseStream = value; }
    }

    internal XmlRpcRequest XmlRpcRequest
    { 
      get { return xmlRpcRequest; } 
      set { xmlRpcRequest = value; }
    }

    internal Stream ResponseBufferedStream
    { 
      get { return responseBufferedStream; } 
      set { responseBufferedStream = value; }
    }

    internal Encoding XmlEncoding
    {
      get { return xmlEncoding; } 
    }

    XmlRpcClientProtocol clientProtocol;
    WebRequest request;
    AsyncCallback userCallback;
    object userAsyncState;
    bool completedSynchronously;
    bool isCompleted;
    bool endSendCalled = false;
    ManualResetEvent manualResetEvent;
    Exception exception;
    WebResponse response;
    Stream responseStream;
    Stream responseBufferedStream;
    byte[] buffer;
    XmlRpcRequest xmlRpcRequest;
    Encoding xmlEncoding;
    bool _useIndentation;
    int _indentation;
    bool _useIntTag;
  }
}