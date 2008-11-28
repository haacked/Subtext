
using System;
using System.Collections.Generic;
using System.Text;

namespace Subtext.Framework
{
	public class BlogAlias
	{
        public BlogAlias()
        {
            Id = NullValue.NullInt32;

            BlogId = NullValue.NullInt32;
            Subfolder = string.Empty;
            Host = string.Empty;
            IsActive = true;
        }

		public int Id
		{
			get;
			set;
		}

		public bool IsActive
		{
			get;
			set;
		}

		public string Host
		{
			get;
			set;
		}

		public string Subfolder
		{
			get;
			set;
		}

		public int BlogId
		{
			get;
			set;
		}
	}
}
