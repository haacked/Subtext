
using System;
using System.Collections.Generic;
using System.Text;

namespace Subtext.Framework
{
	public class BlogAlias
	{
		int id = NullValue.NullInt32;

		int blogId = NullValue.NullInt32;
		string subfolder = "";
		string host = "";
		bool active = true;

		public int Id
		{
			get { return id; }
			set { id = value; }
		}

		public bool IsActive
		{
			get { return active; }
			set { active = value; }
		}

		public string Host
		{
			get { return host; }
			set { host = value; }
		}

		public string Subfolder
		{
			get { return subfolder; }
			set { subfolder = value; }
		}
		public int BlogId
		{
			get { return blogId; }
			set { blogId = value; }
		}

	}
}
