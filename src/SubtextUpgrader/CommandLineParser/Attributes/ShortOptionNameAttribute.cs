//-------------------------------------------------------------------------------------------------
// CommandLineParser
// Copyright (c) Twenzel. 
// Licensed under the Ms-PL (see license.txt in the CommandLineParser directory)
// Project Homepage: http://cmdlineparser.codeplex.com/
//-------------------------------------------------------------------------------------------------

using System;

namespace SubtextUpgrader.CommandLineParser.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ShortOptionNameAttribute : Attribute
    {
        /// <summary>
        /// The short name of this option
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Defines the short name of this option e.g. "u" for Username
        /// </summary>
        /// <param name="optionName"></param>
        public ShortOptionNameAttribute(string optionName)
        {
            Value = optionName;
        }
    }
}