//-------------------------------------------------------------------------------------------------
// CommandLineParser
// Copyright (c) Twenzel. 
// Licensed under the Ms-PL (see license.txt in the CommandLineParser directory)
// Project Homepage: http://cmdlineparser.codeplex.com/
//-------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using SubtextUpgrader.CommandLineParser.Attributes;

namespace SubtextUpgrader.CommandLineParser
{
    /// <summary>
    /// Represents a resolved command line option
    /// </summary>
    public class Option
    {
        /// <summary>
        /// The name of the command line option
        /// </summary>
        public string OptionName { get; private set; }
        /// <summary>
        /// The short name of the command line option
        /// </summary>
        public string ShortOptionName { get; private set; }
        /// <summary>
        /// The Property infos
        /// </summary>
        public PropertyInfo Property { get; private set; }
        /// <summary>
        /// The description of the command line option
        /// </summary>
        public string Description { get; private set; }

        /// <summary>
        /// The default value
        /// </summary>
        public object DefaultValue { get; private set; }

        public Option(PropertyInfo property)
        {
            Property = property;

            GetOptionNames();
            GetDefaultValue();
            GetDescription();
        }

        private void GetDefaultValue()
        {
            object[] defaultValues = Property.GetCustomAttributes(typeof(DefaultValueAttribute), true);
            if (defaultValues != null && defaultValues.Length > 0)
                DefaultValue = ((DefaultValueAttribute)defaultValues[0]).Value;
            else
                if (Property.PropertyType == typeof(string))
                    // default string is empty
                    DefaultValue = "";
                else
                    DefaultValue = Activator.CreateInstance(Property.PropertyType);
        }

        private void GetOptionNames()
        {
            OptionName = Property.Name.ToLower();

            object[] optionNames = Property.GetCustomAttributes(typeof(OptionNameAttribute), true);
            if (optionNames != null && optionNames.Length > 0)
                OptionName = ((OptionNameAttribute)optionNames[0]).Value.ToLower();

            object[] shortOptionNames = Property.GetCustomAttributes(typeof(ShortOptionNameAttribute), true);
            if (shortOptionNames != null && shortOptionNames.Length > 0)
                ShortOptionName = ((ShortOptionNameAttribute)shortOptionNames[0]).Value.ToLower();
        }

        private void GetDescription()
        {
            Description = "No Description available";

            object[] descriptions = Property.GetCustomAttributes(typeof(DescriptionAttribute), true);
            if (descriptions != null && descriptions.Length > 0)
                Description = ((DescriptionAttribute)descriptions[0]).Description;
        }
    }

    /// <summary>
    /// A Option collection
    /// </summary>
    public class Options : List<Option>
    {
        /// <summary>
        /// Returns a Option element which option name equals to argument
        /// </summary>
        /// <param name="argument"></param>
        /// <returns></returns>
        public Option GetByName(string argument)
        {
            Option result = this.SingleOrDefault(o => o.OptionName == argument.ToLower()) ??
                            this.SingleOrDefault(o => o.ShortOptionName == argument.ToLower());

            return result;
        }
    }
}