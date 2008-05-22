﻿using System;
using CookComputing.XmlRpc;

namespace Subtext.Framework.XmlRpc
{
    public struct WordpressCategory
    {
        public string name;
    }

    public interface IWordpressApi
    {
        [XmlRpcMethod("wp.newCategory",
            Description = "Adds a new category to the blog engine.")]
        int newCategory(
          string blogid,
          string username,
          string password,
          WordpressCategory category);
    }
}