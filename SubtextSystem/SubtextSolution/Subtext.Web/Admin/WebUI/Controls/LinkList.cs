#region Disclaimer/Info
///////////////////////////////////////////////////////////////////////////////////////////////////
// .Text WebLog
// 
// .Text is an open source weblog system started by Scott Watermasysk. 
// Blog: http://ScottWater.com/blog 
// RSS: http://scottwater.com/blog/rss.aspx
// Email: Dottext@ScottWater.com
//
// For updated news and information please visit http://scottwater.com/dottext and subscribe to 
// the Rss feed @ http://scottwater.com/dottext/rss.aspx
//
// On its release (on or about August 1, 2003) this application is licensed under the BSD. However, I reserve the 
// right to change or modify this at any time. The most recent and up to date license can always be fount at:
// http://ScottWater.com/License.txt
// 
// Please direct all code related questions to:
// GotDotNet Workspace: http://www.gotdotnet.com/Community/Workspaces/workspace.aspx?id=e99fccb3-1a8c-42b5-90ee-348f6b77c407
// Yahoo Group http://groups.yahoo.com/group/DotText/
// 
///////////////////////////////////////////////////////////////////////////////////////////////////
#endregion

using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Subtext.Web.Admin.WebUI
{
	public class LinkList : Control
	{
		LinkControlCollection _items = new LinkControlCollection();
		
		public LinkControlCollection Items
		{
			get { return _items; }
		}

		protected override void Render(HtmlTextWriter output)
		{	
			if (_items.Count > 0)
			{
			
				output.WriteBeginTag("ul");
				output.WriteAttribute("id", this.ClientID, false);
				output.Write(HtmlTextWriter.TagRightChar);
				foreach(WebControl currentLink in _items)
				{
					output.RenderBeginTag(HtmlTextWriterTag.Li);
					currentLink.RenderControl(output);
					output.RenderEndTag();
				}
				output.WriteEndTag("ul");
			}
		}
	}

	// REFACTOR:
	// This used to be hyperlinks only, I wanted to add LinkButtons. The share
	// WebControl as an ancestor along with 13,000 other things. So this is kind 
	// of bad pool here, need to reevalutate how to get same functionality but
	// in a cleaner way.
	public class LinkControlCollection: System.Collections.CollectionBase
	{
		public LinkControlCollection()
		{
			//
		}

		#region CollectionBase basics
		public LinkControlCollection(WebControl[] items)
		{
			this.AddRange(items);
		}

		public LinkControlCollection(LinkControlCollection items)
		{
			this.AddRange(items);
		}

		public virtual void AddRange(WebControl[] items)
		{
			foreach (WebControl item in items)
			{
				this.List.Add(item);
			}
		}

		public virtual void AddRange(LinkControlCollection items)
		{
			foreach (WebControl item in items)
			{
				this.List.Add(item);
			}
		}

		public virtual void Add(LinkButton value)
		{
			this.List.Add(value);
		}

		public virtual void Add(HyperLink value)
		{
			this.List.Add(value);
		}

		public virtual void Add(string text, string navigateUrl)
		{
			Add(text, navigateUrl, null, null);
		}

		public virtual void Add(string text, string navigateUrl, string cssClass)
		{
			Add(text, navigateUrl, cssClass, null);
		}

		public virtual void Add(string text, string navigateUrl, string cssClass, string target)
		{
			HyperLink adding = new HyperLink();
			adding.Text = text;
			adding.NavigateUrl = navigateUrl;
			if (null != cssClass && cssClass.Length > 0)
				adding.CssClass = cssClass;
			if (null != target && target.Length > 0)
				adding.Target = target;
			
			this.List.Add(adding);
		}

		public virtual LinkButton Add(string text, EventHandler targetHandler)
		{
			return Add(text, null, targetHandler);
		}

		public virtual LinkButton Add(string text, string cssClass, EventHandler targetHandler)
		{
			LinkButton adding = new LinkButton ();
			adding.Text = text;
			adding.Click += new System.EventHandler(targetHandler);
			
			if (null != cssClass && cssClass.Length > 0)
				adding.CssClass = cssClass;
			
			this.List.Add(adding);
			return adding;
		}

		public virtual bool Contains(WebControl value)
		{
			return this.List.Contains(value);
		}

		public virtual int IndexOf(WebControl value)
		{
			return this.List.IndexOf(value);
		}

		public virtual void Insert(int index, WebControl value)
		{
			this.List.Insert(index, value);
		}

		public virtual WebControl this[int index]
		{
			get { return (WebControl)this.List[index]; }
			set	{ this.List[index] = value; }
		}

		public virtual void Remove(WebControl value)
		{
			this.List.Remove(value);
		}

		public new virtual LinkControlCollection.Enumerator GetEnumerator()
		{
			return new LinkControlCollection.Enumerator(this);
		}

		public class Enumerator: System.Collections.IEnumerator
		{
			private System.Collections.IEnumerator wrapped;

			public Enumerator(LinkControlCollection collection)
			{
				this.wrapped = ((System.Collections.CollectionBase)collection).GetEnumerator();
			}

			public WebControl Current
			{
				get { return (WebControl)this.wrapped.Current; }
			}

			object System.Collections.IEnumerator.Current
			{
				get { return (WebControl)this.wrapped.Current; }
			}

			public bool MoveNext()
			{
				return this.wrapped.MoveNext();
			}

			public void Reset()
			{
				this.wrapped.Reset();
			}
		}

		#endregion
	}
}

