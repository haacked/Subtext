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
    public class OptionNameAttribute : Attribute
    {
        /// <summary>
        /// The name of this option
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Defines the name of this option e.g. "user" for Username
        /// </summary>
        /// <param name="optionName"></param>
        public OptionNameAttribute(string optionName)
        {
            Value = optionName;
        }
    }
}