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
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Subtext.Framework.Text;

namespace Subtext.Web.Admin.Commands
{
    [Serializable]
    public abstract class ConfirmCommand
    {
        protected const string DEFAULT_CANCEL_FAILURE = "Could not cancel operation. Details: {1}";
        protected const string DEFAULT_CANCEL_SUCCESS = "Operation canceled.";
        protected const string DEFAULT_EXECUTE_FAILURE = "Operation failed. Details: {1}";
        protected const string DEFAULT_EXECUTE_SUCCESS = "Operation succeeded.";
        protected const string DEFAULT_PROMPT = "Are you sure you want to do this?";

        protected string _cancelFailureMessage;
        protected string _cancelSuccessMessage;
        protected string _executeFailureMessage;
        protected string _executeSuccessMessage;
        protected string _promptMessage;

        public virtual bool AutoRedirect { get; set; }

        public virtual string RedirectUrl { get; set; }

        public virtual string PromptMessage
        {
            get { return _promptMessage ?? DEFAULT_PROMPT; }
            set { _promptMessage = value ?? DEFAULT_PROMPT; }
        }

        public virtual string ExecuteSuccessMessage
        {
            get
            {
                if (!Utilities.IsNullorEmpty(_executeSuccessMessage))
                {
                    return _executeSuccessMessage;
                }
                else
                {
                    return DEFAULT_EXECUTE_SUCCESS;
                }
            }
            set { _executeSuccessMessage = value; }
        }

        public virtual string ExecuteFailureMessage
        {
            get
            {
                if (!Utilities.IsNullorEmpty(_executeFailureMessage))
                {
                    return _executeFailureMessage;
                }
                else
                {
                    return DEFAULT_EXECUTE_FAILURE;
                }
            }
            set { _executeFailureMessage = value; }
        }

        public virtual string CancelSuccessMessage
        {
            get
            {
                if (!Utilities.IsNullorEmpty(_cancelSuccessMessage))
                {
                    return _cancelSuccessMessage;
                }
                else
                {
                    return DEFAULT_CANCEL_SUCCESS;
                }
            }
            set { _cancelSuccessMessage = value; }
        }

        public virtual string CancelFailureMessage
        {
            get
            {
                if (!Utilities.IsNullorEmpty(_cancelFailureMessage))
                {
                    return _cancelFailureMessage;
                }
                else
                {
                    return DEFAULT_CANCEL_FAILURE;
                }
            }
            set { _cancelFailureMessage = value; }
        }

        public virtual string FormatMessage(string message)
        {
            return FormatMessage(message, null);
        }

        /// <summary>
        /// Formats the message.
        /// </summary>
        /// <param name="format">Format.</param>
        /// <param name="args">Args.</param>
        /// <returns></returns>
        public virtual string FormatMessage(string format, params object[] args)
        {
            try
            {
                return string.Format(CultureInfo.InvariantCulture, format, args);
            }
            catch (ArgumentNullException)
            {
                return format;
            }
        }

        /// <summary>
        /// Simply concatenates an array of integers for display as text 
        /// using commas.  ex... "1, 2, 3"
        /// </summary>
        /// <param name="integers">Integers.</param>
        /// <returns></returns>
        protected string GetDisplayTextFromIntArray(ICollection<int> integers)
        {
            if (integers == null || integers.Count == 0)
            {
                return string.Empty;
            }

            if (integers.Count == 2)
            {
                return integers.First() + " and " + integers.ElementAt(1);
            }

            string display = string.Empty;
            foreach (int integer in integers)
            {
                display += integer + ", ";
            }
            return display.Left(display.Length - 2);
        }

        public abstract string Cancel();
        public abstract string Execute();
    }
}