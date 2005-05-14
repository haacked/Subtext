using System;
using System.IO;
using System.Xml;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;

namespace Subtext.Framework.Syndication
{
    /// <summary>
    /// Summary description for BaseSyndicationWriter.
    /// </summary>
    public abstract class BaseSyndicationWriter : XmlTextWriter
    {

        private StringWriter writer  = null;
        protected BlogConfig config;

        protected BaseSyndicationWriter(StringWriter sw): base(sw)
        {
            writer = sw;
            config = Config.CurrentBlog;
        }

        protected BaseSyndicationWriter(): this(new StringWriter())
        {
			
        }

        public StringWriter StringWriter
        {
            get
            {
                Build();
                return writer;
            }
        }

        public string GetXml
        {
            get{return this.StringWriter.ToString();}
        }

        public override string ToString()
        {
            return GetXml;
        }

        private bool _useAggBugs = false;
        public bool UseAggBugs
        {
            get {return this._useAggBugs;}
            set {this._useAggBugs = value;}
        }

        private bool _allowComments = true;
        public bool AllowComments
        {
            get {return this._allowComments;}
            set {this._allowComments = value;}
        }

        private EntryCollection _entries;
        public EntryCollection Entries
        {
            get {return this._entries;}
            set {this._entries = value;}
        }

        protected abstract void Build();
    }
}
