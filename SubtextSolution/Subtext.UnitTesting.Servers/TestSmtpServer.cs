using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using LumiSoft.Net.SMTP.Server;

namespace Subtext.UnitTesting.Servers
{
	/// <summary>
	/// Summary description for UnitTestSmtpServer.
	/// </summary>
	public sealed class TestSmtpServer : IDisposable
	{
		public const int DefaultPort = 8082;
		SMTP_Server server;
        IList<ReceivedEmailMessage> inbox = new List<ReceivedEmailMessage>();
		
		/// <summary>
		/// Initializes a new instance of the <see cref="TestSmtpServer"/> class.
		/// </summary>
		public TestSmtpServer()
		{
		}
		
		/// <summary>
		/// Gets the inbox.
		/// </summary>
		/// <value>The inbox.</value>
		public IList<ReceivedEmailMessage> Inbox
		{
			get
			{
				return this.inbox;
			}
		}
		
		/// <summary>
		/// Starts this instance.
		/// </summary>
		public void Start()
		{
			Start(new IPAddress(new byte[] {127, 0, 0, 1}), DefaultPort);
		}
		
		/// <summary>
		/// Starts the specified ip.
		/// </summary>
		/// <param name="ip">The ip.</param>
		/// <param name="port">The port.</param>
		public void Start(string ip, int port)
		{
			Start(IPAddress.Parse(ip), port);
		}
		
		/// <summary>
		/// Starts the specified ip.
		/// </summary>
		/// <param name="ip">The ip.</param>
		/// <param name="port">The port.</param>
		public void Start(IPAddress ip, int port)
		{
			Start(new IPEndPoint(ip, port));
		}
		
		/// <summary>
		/// Starts the specified server end point.
		/// </summary>
		/// <param name="serverEndPoint">The server end point.</param>
		public void Start(IPEndPoint serverEndPoint)
		{
			if(server != null)
			{
				return; //Assume server is started.	
			}
			
			server = new SMTP_Server();
			server.IpAddress = serverEndPoint.Address.ToString();
			server.Port = serverEndPoint.Port;
			server.StoreMessage += OnStoreMessage;
			server.Enabled = true;
		}
		
		private void OnStoreMessage(object sender, NewMail_EventArgs e)
		{
			string rawSmtpMessage;
			
			using(StreamReader reader = new StreamReader(e.MessageStream))
			{
				rawSmtpMessage = reader.ReadToEnd();
			}
			ReceivedEmailMessage message = new ReceivedEmailMessage(rawSmtpMessage);
			
			lock(inbox)
			{
				inbox.Add(message);
			}
		}
		
		/// <summary>
		/// Stops this instance.
		/// </summary>
		public void Stop()
		{
			if(server == null)
				return;
			
			Dispose(true);
		}
		
		#region Dispose Pattern
		/// <summary>
		/// Disposes this instance.
		/// </summary>
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
		
		private void Dispose(bool disposing)
		{
			if(disposing)
			{
				if(this.server != null)
				{
					server.Enabled = false;
					this.server.Dispose();
				}
			}
		}
		#endregion
	}
}
