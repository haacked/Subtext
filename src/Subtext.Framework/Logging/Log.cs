#region Disclaimer/Info

///////////////////////////////////////////////////////////////////////////////////////////////////
// Subtext WebLog
// 
// Subtext is an open source weblog system that is a fork of the .TEXT
// weblog system.
//
// For updated news and information please visit http://subtextproject.com/
// Subtext is hosted at Google Code at http://code.google.com/p/subtext/
// The development mailing list is at subtext@googlegroups.com 
//
// This project is licensed under the BSD license.  See the License.txt file for more information.
///////////////////////////////////////////////////////////////////////////////////////////////////

#endregion

using System;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Web;
using log4net;
using log4net.Core;
using Subtext.Framework.Web.HttpModules;

namespace Subtext.Framework.Logging
{
    /// <summary>
    /// Provides logging for the Subtext framework. This class is typically instantiated as a 
    /// <c>private static readonly</c> member of a class in order to handle logging inside of 
    /// the class. This class is a specialized wrapper for the log4net framework.
    /// </summary>
    /// <para>
    /// The class provides methods to log messages at the following levels:
    /// </para>
    /// <remarks>
    /// <list type="definition">
    ///	<item>
    ///	<term>DEBUG</term>
    ///	<description>
    ///	The <see cref="M:Subtext.Framework.Logging.Log.Debug(System.Object)"/> and 
    ///	<see cref="M:Subtext.Framework.Logging.Log.DebugFormat(System.String,System.Object[])"/> methods log messages
    ///	at the <c>DEBUG</c> level. That is the level with that name defined in the log4net
    ///	repositories. The <see cref="P:Subtext.Framework.Logging.Log.IsDebugEnabled"/>
    ///	property tests if this level is enabled for logging.
    ///	</description>
    ///	</item>
    ///	<item>
    ///	<term>INFO</term>
    ///	<description>
    ///	The <see cref="M:Subtext.Framework.Logging.Log.Info(System.Object)"/> and 
    ///	<see cref="M:Subtext.Framework.Logging.Log.InfoFormat(System.String,System.Object[])"/> methods log messages
    ///	at the <c>INFO</c> level. That is the level with that name defined in the log4net
    ///	repositories. The <see cref="P:Subtext.Framework.Logging.Log.IsInfoEnabled"/>
    ///	property tests if this level is enabled for logging.
    ///	</description>
    ///	</item>
    ///	<item>
    ///	<term>WARN</term>
    ///	<description>
    ///	The <see cref="M:Subtext.Framework.Logging.Log.Warn(System.Object)"/> and 
    ///	<see cref="M:Subtext.Framework.Logging.Log.WarnFormat(System.String,System.Object[])"/> methods log messages
    ///	at the <c>WARN</c> level. That is the level with that name defined in the log4net
    ///	repositories. The <see cref="P:Subtext.Framework.Logging.Log.IsWarnEnabled"/>
    ///	property tests if this level is enabled for logging.
    ///	</description>
    ///	</item>
    ///	<item>
    ///	<term>ERROR</term>
    ///	<description>
    ///	The <see cref="M:Subtext.Framework.Logging.Log.Error(System.Object)"/> and 
    ///	<see cref="M:Subtext.Framework.Logging.Log.ErrorFormat(System.String,System.Object[])"/> methods log messages
    ///	at the <c>ERROR</c> level. That is the level with that name defined in the log4net
    ///	repositories. The <see cref="P:Subtext.Framework.Logging.Log.IsErrorEnabled"/>
    ///	property tests if this level is enabled for logging.
    ///	</description>
    ///	</item>
    ///	<item>
    ///	<term>FATAL</term>
    ///	<description>
    ///	The <see cref="M:Subtext.Framework.Logging.Log.Fatal(System.Object)"/> and 
    ///	<see cref="M:Subtext.Framework.Logging.Log.FatalFormat(System.String,System.Object[])"/> methods log messages
    ///	at the <c>FATAL</c> level. That is the level with that name defined in the log4net
    ///	repositories. The <see cref="P:Subtext.Framework.Logging.Log.IsFatalEnabled"/>
    ///	property tests if this level is enabled for logging.
    ///	</description>
    ///	</item>
    ///	</list>
    /// </remarks>
    /// <example>
    /// An example of using the Log class
    /// <code lang="C#">
    /// public class Subtext.Framework.Wigdetry
    /// {
    ///		<b>private static readonly Log __log = new Log();</b>
    ///		...
    ///		public void DoSomething()
    ///		{
    ///			try
    ///			{
    ///			}
    ///			catch(Exception e)
    ///			{
    ///				<b>__log.Error("Something had gone terribly wrong", e);</b>
    ///			}
    ///		}
    /// }
    /// </code>
    /// </example>
    public class Log : ILog
    {
        private static readonly ILog __nullLog = new NullLog();
        private readonly ILog _log;

        /// <summary>
        /// Default constructor. Uses <see cref="T:System.Diagnostics.StackFrame"/> to discover the class it is being called from 
        /// and automatically establishes log name as the <see cref="P:System.Type.FullName"/> of the class type.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public Log() : this(GetCallerType())
        {
        }

        /// <summary>
        /// Instantiates a log using the <see cref="P:System.Type.FullName"/> of the suppled type of the class as the name.
        /// </summary>
        /// <param name="type"><see cref="T:System.Type"/> of the class to create a log for</param>
        public Log(Type type)
        {
            _log = CreateInnerLogger(type);
        }

        /// <summary>
        /// Instantiates a log which wraps the specified inner logger.
        /// </summary>
        /// <param name="innerLogger"><see cref="T:System.Type"/> of the class to create a log for</param>
        public Log(ILog innerLogger)
        {
            _log = innerLogger;
        }

        #region ILog Members

        /// <summary>
        /// Checks if the log is enabled for the <c>ERROR</c> level.
        /// </summary>
        /// <value>
        /// <c>true</c> if this log is enabled for the <c>ERROR</c> events, <c>false</c> otherwise
        /// </value>
        /// <remarks>
        /// <para>
        /// This function is intended to lessen the computational cost of disabled log debug statements.
        /// </para>
        /// </remarks>
        public bool IsErrorEnabled
        {
            get { return _log.IsErrorEnabled; }
        }

        /// <summary>
        /// Checks if the log is enabled for the <c>FATAL</c> level.
        /// </summary>
        /// <value>
        /// <c>true</c> if this log is enabled for the <c>FATAL</c> events, <c>false</c> otherwise
        /// </value>
        /// <remarks>
        /// <para>
        /// This function is intended to lessen the computational cost of disabled log debug statements.
        /// </para>
        /// </remarks>
        public bool IsFatalEnabled
        {
            get { return _log.IsFatalEnabled; }
        }

        /// <summary>
        /// Checks if the log is enabled for the <c>WARN</c> level.
        /// </summary>
        /// <value>
        /// <c>true</c> if this log is enabled for the <c>WARN</c> events, <c>false</c> otherwise
        /// </value>
        /// <remarks>
        /// <para>
        /// This function is intended to lessen the computational cost of disabled log debug statements.
        /// </para>
        /// </remarks>
        public bool IsWarnEnabled
        {
            get { return _log.IsWarnEnabled; }
        }

        /// <summary>
        /// Checks if the log is enabled for the <c>INFO</c> level.
        /// </summary>
        /// <value>
        /// <c>true</c> if this log is enabled for the <c>INFO</c> events, <c>false</c> otherwise
        /// </value>
        /// <remarks>
        /// <para>
        /// This function is intended to lessen the computational cost of disabled log debug statements.
        /// </para>
        /// </remarks>
        public bool IsInfoEnabled
        {
            get { return _log.IsInfoEnabled; }
        }

        /// <summary>
        /// Checks if the log is enabled for the <c>DEBUG</c> level.
        /// </summary>
        /// <value>
        /// <c>true</c> if this log is enabled for the <c>DEBUG</c> events, <c>false</c> otherwise
        /// </value>
        /// <remarks>
        /// <para>
        /// This function is intended to lessen the computational cost of disabled log debug statements.
        /// </para>
        /// </remarks>
        public bool IsDebugEnabled
        {
            get { return _log.IsDebugEnabled; }
        }

        /// <summary>
        /// Logs a message with the <c>ERROR</c> level.
        /// </summary>
        /// <param name="provider">An <see cref="T:System.IFormatProvider"/> that supplies culture-specific formatting information</param>
        /// <param name="format">A <see cref="T:System.String"/> containing zero or more format items</param>
        /// <param name="args">An <see cref="T:System.Array"/> containing zero or more objects to format</param>
        /// <remarks>
        /// <para>
        /// The message is formatted using the <c>String.Format</c> method. 
        /// See <see cref="M:System.String.Format(System.String,System.Object)"/> for details of the syntax 
        /// of the format string and the behavior of the formatting.
        /// </para>
        /// <para>
        /// This method does not take an <see cref="T:System.Exception"/> object to include in the log event. 
        /// To pass an <see cref="T:System.Exception"/> use
        /// one of the <see cref="M:Subtext.Framework.Logging.Log.Error(System.Object)"/> methods instead.
        /// </para>
        /// </remarks>
        public void ErrorFormat(IFormatProvider provider, string format, params object[] args)
        {
            SetBlogRequestContext();
            _log.ErrorFormat(provider, format, args);
        }

        /// <summary>
        /// Logs a message with the <c>ERROR</c> level.
        /// </summary>
        /// <param name="format">A <see cref="T:System.String"/> containing zero or more format items</param>
        /// <param name="args">An <see cref="T:System.Array"/> containing zero or more objects to format</param>
        /// <remarks>
        /// <para>
        /// The message is formatted using the <c>String.Format</c> method. 
        /// See <see cref="M:System.String.Format(System.String,System.Object)"/> for details of the syntax 
        /// of the format string and the behavior of the formatting.
        /// </para>
        /// <para>
        /// This method does not take an <see cref="T:System.Exception"/> object to include in the log event. 
        /// To pass an <see cref="T:System.Exception"/> use
        /// one of the <see cref="M:Subtext.Framework.Logging.Log.Error(System.Object)"/> methods instead.
        /// </para>
        /// </remarks>
        public void ErrorFormat(string format, params object[] args)
        {
            SetBlogRequestContext();
            _log.ErrorFormat(CultureInfo.InvariantCulture, format, args);
        }

        /// <summary>
        /// Logs a message object with the <c>INFO</c> level.
        /// </summary>
        /// <param name="message">The message object to log</param>
        /// <param name="exception">The exception to log, including its stack trace</param>
        /// <remarks>
        /// <para>
        ///  This method first checks if this logger is <c>INFO</c> enabled. If so, 
        ///  it converts the message object (passed as parameter) to a string 
        ///  by invoking the appropriate <see cref="T:log4net.ObjectRenderer.IObjectRenderer"/>. 
        ///  It then proceeds to call all the registered appenders in this logger and also 
        ///  higher in the hierarchy depending on the value of the additivity flag.
        /// </para>
        /// </remarks>
        public void Info(object message, Exception exception)
        {
            SetBlogRequestContext();
            _log.Info(message, exception);
        }

        /// <summary>
        /// Logs a message object with the <c>INFO</c> level.
        /// </summary>
        /// <param name="message">The message object to log</param>
        /// <remarks>
        /// <para>
        ///  This method first checks if this logger is <c>INFO</c> enabled. If so, 
        ///  it converts the message object (passed as parameter) to a string 
        ///  by invoking the appropriate <see cref="T:log4net.ObjectRenderer.IObjectRenderer"/>. 
        ///  It then proceeds to call all the registered appenders in this logger and also 
        ///  higher in the hierarchy depending on the value of the additivity flag.
        /// </para>
        /// <para>
        /// <b>WARNING</b> Note that passing an <see cref="T:System.Exception"/> to this method 
        /// will print the name of the <see cref="T:System.Exception"/> but no stack trace. 
        /// To print a stack trace use the <see cref="M:Subtext.Framework.Logging.Log.Info(System.Object,System.Exception)"/> form 
        /// instead.
        /// </para>
        /// </remarks>
        public void Info(object message)
        {
            SetBlogRequestContext();
            _log.Info(message);
        }

        /// <summary>
        /// Logs a message object with the <c>DEBUG</c> level.
        /// </summary>
        /// <param name="message">The message object to log</param>
        /// <param name="exception">The exception to log, including its stack trace</param>
        /// <remarks>
        /// <para>
        ///  This method first checks if this logger is <c>DEBUG</c> enabled. If so, 
        ///  it converts the message object (passed as parameter) to a string 
        ///  by invoking the appropriate <see cref="T:log4net.ObjectRenderer.IObjectRenderer"/>. 
        ///  It then proceeds to call all the registered appenders in this logger and also 
        ///  higher in the hierarchy depending on the value of the additivity flag.
        /// </para>
        /// <para>
        /// This method is compiled to nothing if DEBUG compilation constant is not set (production build).
        /// </para>
        /// </remarks>
        public void Debug(object message, Exception exception)
        {
            SetBlogRequestContext();
            _log.Debug(message, exception);
        }

        /// <summary>
        /// Logs a message object with the <c>DEBUG</c> level.
        /// </summary>
        /// <param name="message">The message object to log</param>
        /// <remarks>
        /// <para>
        ///  This method first checks if this logger is <c>DEBUG</c> enabled. If so, 
        ///  it converts the message object (passed as parameter) to a string 
        ///  by invoking the appropriate <see cref="T:log4net.ObjectRenderer.IObjectRenderer"/>. 
        ///  It then proceeds to call all the registered appenders in this logger and also 
        ///  higher in the hierarchy depending on the value of the additivity flag.
        /// </para>
        /// <para>
        /// <b>WARNING</b> Note that passing an <see cref="T:System.Exception"/> to this method 
        /// will print the name of the <see cref="T:System.Exception"/> but no stack trace. 
        /// To print a stack trace use the <see cref="M:Subtext.Framework.Logging.Log.Debug(System.Object,System.Exception)"/> form 
        /// instead.
        /// </para>
        /// <para>
        /// This method is compiled to nothing if DEBUG compilation constant is not set (production build).
        /// </para>
        /// </remarks>
        public void Debug(object message)
        {
            SetBlogRequestContext();
            _log.Debug(message);
        }

        /// <summary>
        /// Logs a message object with the <c>WARN</c> level.
        /// </summary>
        /// <param name="message">The message object to log</param>
        /// <param name="exception">The exception to log, including its stack trace</param>
        /// <remarks>
        /// <para>
        ///  This method first checks if this logger is <c>WARN</c> enabled. If so, 
        ///  it converts the message object (passed as parameter) to a string 
        ///  by invoking the appropriate <see cref="T:log4net.ObjectRenderer.IObjectRenderer"/>. 
        ///  It then proceeds to call all the registered appenders in this logger and also 
        ///  higher in the hierarchy depending on the value of the additivity flag.
        /// </para>
        /// </remarks>
        public void Warn(object message, Exception exception)
        {
            SetBlogRequestContext();
            _log.Warn(message, exception);
        }

        /// <summary>
        /// Logs a message object with the <c>WARN</c> level.
        /// </summary>
        /// <param name="message">The message object to log</param>
        /// <remarks>
        /// <para>
        ///  This method first checks if this logger is <c>WARN</c> enabled. If so, 
        ///  it converts the message object (passed as parameter) to a string 
        ///  by invoking the appropriate <see cref="T:log4net.ObjectRenderer.IObjectRenderer"/>. 
        ///  It then proceeds to call all the registered appenders in this logger and also 
        ///  higher in the hierarchy depending on the value of the additivity flag.
        /// </para>
        /// <para>
        /// <b>WARNING</b> Note that passing an <see cref="T:System.Exception"/> to this method 
        /// will print the name of the <see cref="T:System.Exception"/> but no stack trace. 
        /// To print a stack trace use the <see cref="M:Subtext.Framework.Logging.Log.Warn(System.Object,System.Exception)"/> form 
        /// instead.
        /// </para>
        /// </remarks>
        public void Warn(object message)
        {
            SetBlogRequestContext();
            _log.Warn(message);
        }

        /// <summary>
        /// Logs a message with the <c>WARN</c> level.
        /// </summary>
        /// <param name="provider">An <see cref="T:System.IFormatProvider"/> that supplies culture-specific formatting information</param>
        /// <param name="format">A <see cref="T:System.String"/> containing zero or more format items</param>
        /// <param name="args">An <see cref="T:System.Array"/> containing zero or more objects to format</param>
        /// <remarks>
        /// <para>
        /// The message is formatted using the <c>String.Format</c> method. 
        /// See <see cref="M:System.String.Format(System.String,System.Object)"/> for details of the syntax 
        /// of the format string and the behavior of the formatting.
        /// </para>
        /// <para>
        /// This method does not take an <see cref="T:System.Exception"/> object to include in the log event. 
        /// To pass an <see cref="T:System.Exception"/> use
        /// one of the <see cref="M:Subtext.Framework.Logging.Log.Warn(System.Object)"/> methods instead.
        /// </para>
        /// </remarks>
        public void WarnFormat(IFormatProvider provider, string format, params object[] args)
        {
            SetBlogRequestContext();
            _log.WarnFormat(provider, format, args);
        }

        /// <summary>
        /// Logs a message with the <c>WARN</c> level.
        /// </summary>
        /// <param name="format">A <see cref="T:System.String"/> containing zero or more format items</param>
        /// <param name="args">An <see cref="T:System.Array"/> containing zero or more objects to format</param>
        /// <remarks>
        /// <para>
        /// The message is formatted using the <c>String.Format</c> method. 
        /// See <see cref="M:System.String.Format(System.String,System.Object)"/> for details of the syntax 
        /// of the format string and the behavior of the formatting.
        /// </para>
        /// <para>
        /// This method does not take an <see cref="T:System.Exception"/> object to include in the log event. 
        /// To pass an <see cref="T:System.Exception"/> use
        /// one of the <see cref="M:Subtext.Framework.Logging.Log.Warn(System.Object)"/> methods instead.
        /// </para>
        /// </remarks>
        public void WarnFormat(string format, params object[] args)
        {
            SetBlogRequestContext();
            _log.WarnFormat(CultureInfo.InvariantCulture, format, args);
        }

        /// <summary>
        /// Logs a message object with the <c>FATAL</c> level.
        /// </summary>
        /// <param name="message">The message object to log</param>
        /// <param name="exception">The exception to log, including its stack trace</param>
        /// <remarks>
        /// <para>
        ///  This method first checks if this logger is <c>FATAL</c> enabled. If so, 
        ///  it converts the message object (passed as parameter) to a string 
        ///  by invoking the appropriate <see cref="T:log4net.ObjectRenderer.IObjectRenderer"/>. 
        ///  It then proceeds to call all the registered appenders in this logger and also 
        ///  higher in the hierarchy depending on the value of the additivity flag.
        /// </para>
        /// </remarks>
        public void Fatal(object message, Exception exception)
        {
            SetBlogRequestContext();
            _log.Fatal(message, exception);
        }

        /// <summary>
        /// Logs a message object with the <c>FATAL</c> level.
        /// </summary>
        /// <param name="message">The message object to log</param>
        /// <remarks>
        /// <para>
        ///  This method first checks if this logger is <c>FATAL</c> enabled. If so, 
        ///  it converts the message object (passed as parameter) to a string 
        ///  by invoking the appropriate <see cref="T:log4net.ObjectRenderer.IObjectRenderer"/>. 
        ///  It then proceeds to call all the registered appenders in this logger and also 
        ///  higher in the hierarchy depending on the value of the additivity flag.
        /// </para>
        /// <para>
        /// <b>WARNING</b> Note that passing an <see cref="T:System.Exception"/> to this method 
        /// will print the name of the <see cref="T:System.Exception"/> but no stack trace. 
        /// To print a stack trace use the <see cref="M:Subtext.Framework.Logging.Log.Fatal(System.Object,System.Exception)"/> form 
        /// instead.
        /// </para>
        /// </remarks>
        public void Fatal(object message)
        {
            SetBlogRequestContext();
            _log.Fatal(message);
        }

        /// <summary>
        /// Logs a message object with the <c>ERROR</c> level.
        /// </summary>
        /// <param name="message">The message object to log</param>
        /// <param name="exception">The exception to log, including its stack trace</param>
        /// <remarks>
        /// <para>
        ///  This method first checks if this logger is <c>ERROR</c> enabled. If so, 
        ///  it converts the message object (passed as parameter) to a string 
        ///  by invoking the appropriate <see cref="T:log4net.ObjectRenderer.IObjectRenderer"/>. 
        ///  It then proceeds to call all the registered appenders in this logger and also 
        ///  higher in the hierarchy depending on the value of the additivity flag.
        /// </para>
        /// </remarks>
        public void Error(object message, Exception exception)
        {
            SetBlogRequestContext();
            _log.Error(message, exception);
        }

        /// <summary>
        /// Logs a message object with the <c>ERROR</c> level.
        /// </summary>
        /// <param name="message">The message object to log</param>
        /// <remarks>
        /// <para>
        ///  This method first checks if this logger is <c>ERROR</c> enabled. If so, 
        ///  it converts the message object (passed as parameter) to a string 
        ///  by invoking the appropriate <see cref="T:log4net.ObjectRenderer.IObjectRenderer"/>. 
        ///  It then proceeds to call all the registered appenders in this logger and also 
        ///  higher in the hierarchy depending on the value of the additivity flag.
        /// </para>
        /// <para>
        /// <b>WARNING</b> Note that passing an <see cref="T:System.Exception"/> to this method 
        /// will print the name of the <see cref="T:System.Exception"/> but no stack trace. 
        /// To print a stack trace use the <see cref="M:Subtext.Framework.Logging.Log.Error(System.Object,System.Exception)"/> form 
        /// instead.
        /// </para>
        /// </remarks>
        public void Error(object message)
        {
            SetBlogRequestContext();
            _log.Error(message);
        }

        /// <summary>
        /// Logs a message with the <c>INFO</c> level.
        /// </summary>
        /// <param name="provider">An <see cref="T:System.IFormatProvider"/> that supplies culture-specific formatting information</param>
        /// <param name="format">A <see cref="T:System.String"/> containing zero or more format items</param>
        /// <param name="args">An <see cref="T:System.Array"/> containing zero or more objects to format</param>
        /// <remarks>
        /// <para>
        /// The message is formatted using the <c>String.Format</c> method. 
        /// See <see cref="M:System.String.Format(System.String,System.Object)"/> for details of the syntax 
        /// of the format string and the behavior of the formatting.
        /// </para>
        /// <para>
        /// This method does not take an <see cref="T:System.Exception"/> object to include in the log event. 
        /// To pass an <see cref="T:System.Exception"/> use
        /// one of the <see cref="M:Subtext.Framework.Logging.Log.Info(System.Object)"/> methods instead.
        /// </para>
        /// </remarks>
        public void InfoFormat(IFormatProvider provider, string format, params object[] args)
        {
            SetBlogRequestContext();
            _log.InfoFormat(provider, format, args);
        }

        /// <summary>
        /// Logs a message with the <c>INFO</c> level.
        /// </summary>
        /// <param name="format">A <see cref="T:System.String"/> containing zero or more format items</param>
        /// <param name="args">An <see cref="T:System.Array"/> containing zero or more objects to format</param>
        /// <remarks>
        /// <para>
        /// The message is formatted using the <c>String.Format</c> method. 
        /// of the format string and the behavior of the formatting.
        /// </para>
        /// <para>
        /// This method does not take an <see cref="T:System.Exception"/> object to include in the log event. 
        /// To pass an <see cref="T:System.Exception"/> use
        /// one of the <see cref="M:Subtext.Framework.Logging.Log.Info(System.Object)"/> methods instead.
        /// </para>
        /// </remarks>
        public void InfoFormat(string format, params object[] args)
        {
            SetBlogRequestContext();
            _log.InfoFormat(CultureInfo.InvariantCulture, format, args);
        }

        /// <summary>
        /// Logs a message with the <c>FATAL</c> level.
        /// </summary>
        /// <param name="provider">An <see cref="T:System.IFormatProvider"/> that supplies culture-specific formatting information</param>
        /// <param name="format">A <see cref="T:System.String"/> containing zero or more format items</param>
        /// <param name="args">An <see cref="T:System.Array"/> containing zero or more objects to format</param>
        /// <remarks>
        /// <para>
        /// The message is formatted using the <c>String.Format</c> method. 
        /// See <see cref="M:System.String.Format(System.String,System.Object)"/> for details of the syntax 
        /// of the format string and the behavior of the formatting.
        /// </para>
        /// <para>
        /// This method does not take an <see cref="T:System.Exception"/> object to include in the log event. 
        /// To pass an <see cref="T:System.Exception"/> use
        /// one of the <see cref="M:Subtext.Framework.Logging.Log.Fatal(System.Object)"/> methods instead.
        /// </para>
        /// </remarks>
        public void FatalFormat(IFormatProvider provider, string format, params object[] args)
        {
            SetBlogRequestContext();
            _log.FatalFormat(provider, format, args);
        }

        /// <summary>
        /// Logs a message with the <c>FATAL</c> level.
        /// </summary>
        /// <param name="format">A <see cref="T:System.String"/> containing zero or more format items</param>
        /// <param name="args">An <see cref="T:System.Array"/> containing zero or more objects to format</param>
        /// <remarks>
        /// <para>
        /// The message is formatted using the <c>String.Format</c> method. 
        /// of the format string and the behavior of the formatting.
        /// </para>
        /// <para>
        /// This method does not take an <see cref="T:System.Exception"/> object to include in the log event. 
        /// To pass an <see cref="T:System.Exception"/> use
        /// one of the <see cref="M:Subtext.Framework.Logging.Log.Fatal(System.Object)"/> methods instead.
        /// </para>
        /// </remarks>
        public void FatalFormat(string format, params object[] args)
        {
            SetBlogRequestContext();
            _log.FatalFormat(CultureInfo.InvariantCulture, format, args);
        }

        /// <summary>
        /// Logs a message with the <c>DEBUG</c> level.
        /// </summary>
        /// <param name="provider">An <see cref="T:System.IFormatProvider"/> that supplies culture-specific formatting information</param>
        /// <param name="format">A <see cref="T:System.String"/> containing zero or more format items</param>
        /// <param name="args">An <see cref="T:System.Array"/> containing zero or more objects to format</param>
        /// <remarks>
        /// <para>
        /// The message is formatted using the <c>String.Format</c> method. 
        /// See <see cref="M:System.String.Format(System.String,System.Object)"/> for details of the syntax 
        /// of the format string and the behavior of the formatting.
        /// </para>
        /// <para>
        /// This method does not take an <see cref="T:System.Exception"/> object to include in the log event. 
        /// To pass an <see cref="T:System.Exception"/> use
        /// one of the <see cref="M:Subtext.Framework.Logging.Log.Debug(System.Object)"/> methods instead.
        /// </para>
        /// <para>
        /// This method is compiled to nothing if DEBUG compilation constant is not set (production build).
        /// </para>
        /// </remarks>
        public void DebugFormat(IFormatProvider provider, string format, params object[] args)
        {
            SetBlogRequestContext();
            _log.DebugFormat(provider, format, args);
        }

        /// <summary>
        /// Logs a message with the <c>DEBUG</c> level.
        /// </summary>
        /// <param name="format">A <see cref="T:System.String"/> containing zero or more format items</param>
        /// <param name="args">An <see cref="T:System.Array"/> containing zero or more objects to format</param>
        /// <remarks>
        /// <para>
        /// The message is formatted using the <c>String.Format</c> method. 
        /// See <see cref="M:System.String.Format(System.String,System.Object)"/> for details of the syntax 
        /// of the format string and the behavior of the formatting.
        /// </para>
        /// <para>
        /// This method does not take an <see cref="T:System.Exception"/> object to include in the log event. 
        /// To pass an <see cref="T:System.Exception"/> use
        /// one of the <see cref="M:Subtext.Framework.Logging.Log.Debug(System.Object)"/> methods instead.
        /// </para>
        /// </remarks>
        public void DebugFormat(string format, params object[] args)
        {
            SetBlogRequestContext();
            _log.DebugFormat(CultureInfo.InvariantCulture, format, args);
        }

        public ILogger Logger
        {
            get { return _log.Logger; }
        }

        public void DebugFormat(string format, object arg0, object arg1, object arg2)
        {
            SetBlogRequestContext();
            _log.DebugFormat(format, arg0, arg1, arg2);
        }

        public void DebugFormat(string format, object arg0, object arg1)
        {
            SetBlogRequestContext();
            _log.DebugFormat(format, arg0, arg1);
        }

        public void DebugFormat(string format, object arg0)
        {
            SetBlogRequestContext();
            _log.DebugFormat(format, arg0);
        }

        public void ErrorFormat(string format, object arg0, object arg1, object arg2)
        {
            SetBlogRequestContext();
            _log.ErrorFormat(format, arg0, arg1, arg2);
        }

        public void ErrorFormat(string format, object arg0, object arg1)
        {
            SetBlogRequestContext();
            _log.ErrorFormat(format, arg0, arg1);
        }

        public void ErrorFormat(string format, object arg0)
        {
            SetBlogRequestContext();
            _log.ErrorFormat(format, arg0);
        }

        public void FatalFormat(string format, object arg0, object arg1, object arg2)
        {
            SetBlogRequestContext();
            _log.FatalFormat(format, arg0, arg1, arg2);
        }

        public void FatalFormat(string format, object arg0, object arg1)
        {
            SetBlogRequestContext();
            _log.FatalFormat(format, arg0, arg1);
        }

        public void FatalFormat(string format, object arg0)
        {
            SetBlogRequestContext();
            _log.FatalFormat(format, arg0);
        }

        public void InfoFormat(string format, object arg0, object arg1, object arg2)
        {
            SetBlogRequestContext();
            _log.InfoFormat(format, arg0, arg1, arg2);
        }

        public void InfoFormat(string format, object arg0, object arg1)
        {
            SetBlogRequestContext();
            _log.InfoFormat(format, arg0, arg1);
        }

        public void InfoFormat(string format, object arg0)
        {
            SetBlogRequestContext();
            _log.InfoFormat(format, arg0);
        }

        public void WarnFormat(string format, object arg0, object arg1, object arg2)
        {
            SetBlogRequestContext();
            _log.WarnFormat(format, arg0, arg1, arg2);
        }

        public void WarnFormat(string format, object arg0, object arg1)
        {
            SetBlogRequestContext();
            _log.WarnFormat(format, arg0, arg1);
        }

        public void WarnFormat(string format, object arg0)
        {
            SetBlogRequestContext();
            _log.WarnFormat(format, arg0);
        }

        #endregion

        #region private class NulLog : ILog

        private class NullLog : ILog
        {
            #region ILog Members

            public void ErrorFormat(IFormatProvider provider, string format, params object[] args)
            {
            }

            void ILog.ErrorFormat(string format, params object[] args)
            {
            }

            public void Info(object message, Exception exception)
            {
            }

            void ILog.Info(object message)
            {
            }

            public void Debug(object message, Exception exception)
            {
            }

            void ILog.Debug(object message)
            {
            }

            public bool IsErrorEnabled
            {
                get { return false; }
            }

            public bool IsFatalEnabled
            {
                get { return false; }
            }

            public bool IsWarnEnabled
            {
                get { return false; }
            }

            public void Warn(object message, Exception exception)
            {
            }

            void ILog.Warn(object message)
            {
            }

            public bool IsInfoEnabled
            {
                get { return false; }
            }

            public bool IsDebugEnabled
            {
                get { return false; }
            }

            public void WarnFormat(IFormatProvider provider, string format, params object[] args)
            {
            }

            void ILog.WarnFormat(string format, params object[] args)
            {
            }

            public void Fatal(object message, Exception exception)
            {
            }

            void ILog.Fatal(object message)
            {
            }

            public void Error(object message, Exception exception)
            {
            }

            void ILog.Error(object message)
            {
            }

            public void InfoFormat(IFormatProvider provider, string format, params object[] args)
            {
            }

            void ILog.InfoFormat(string format, params object[] args)
            {
            }

            public void FatalFormat(IFormatProvider provider, string format, params object[] args)
            {
            }

            void ILog.FatalFormat(string format, params object[] args)
            {
            }

            public void DebugFormat(IFormatProvider provider, string format, params object[] args)
            {
            }

            void ILog.DebugFormat(string format, params object[] args)
            {
            }

            public ILogger Logger
            {
                get { return null; }
            }

            public void DebugFormat(string format, object arg0, object arg1, object arg2)
            {
            }

            public void DebugFormat(string format, object arg0, object arg1)
            {
            }

            public void DebugFormat(string format, object arg0)
            {
            }

            public void ErrorFormat(string format, object arg0, object arg1, object arg2)
            {
            }

            public void ErrorFormat(string format, object arg0, object arg1)
            {
            }

            public void ErrorFormat(string format, object arg0)
            {
            }

            public void FatalFormat(string format, object arg0, object arg1, object arg2)
            {
            }

            public void FatalFormat(string format, object arg0, object arg1)
            {
            }

            public void FatalFormat(string format, object arg0)
            {
            }

            public void InfoFormat(string format, object arg0, object arg1, object arg2)
            {
            }

            public void InfoFormat(string format, object arg0, object arg1)
            {
            }

            public void InfoFormat(string format, object arg0)
            {
            }

            public void WarnFormat(string format, object arg0, object arg1, object arg2)
            {
            }

            public void WarnFormat(string format, object arg0, object arg1)
            {
            }

            public void WarnFormat(string format, object arg0)
            {
            }

            #endregion
        }

        #endregion

        private static ILog CreateInnerLogger(Type type)
        {
            ILog log = LogManager.GetLogger(type);
            return log ?? __nullLog;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static Type GetCallerType()
        {
            return new StackFrame(2, false).GetMethod().DeclaringType;
        }

        /// <summary>
        /// Sets the blog id context in the Log4net ThreadContext.
        /// </summary>
        /// <param name="blogId">Blog id.</param>
        static void SetBlogIdContext(int blogId)
        {
            if(blogId == NullValue.NullInt32 && ThreadContext.Properties["BlogId"] != null)
            {
                return;
            }

            ThreadContext.Properties["BlogId"] = blogId;
        }

        static void SetBlogRequestContext()
        {
            if(HttpContext.Current != null)
            {
                try
                {
                    Uri url = HttpContext.Current.Request.Url;
                    if (url != null && ThreadContext.Properties != null)
                    {
                        ThreadContext.Properties["Url"] = url.ToString();
                    }
                }
                catch (HttpException) { 
                
                }

                if(HttpContext.Current.Items != null && BlogRequest.Current != null)
                {
                    var blog = BlogRequest.Current.Blog;
                    if(blog != null)
                    {
                        SetBlogIdContext(blog.Id);
                    }
                }
            }
        }

        /// <summary>
        /// Resets blog id context in the Log4net ThreadContext.
        /// </summary>
        public static void ResetBlogIdContext()
        {
            ThreadContext.Properties["BlogId"] = NullValue.NullInt32;
        }
    }
}