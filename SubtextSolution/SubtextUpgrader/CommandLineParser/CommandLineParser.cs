//-------------------------------------------------------------------------------------------------
// CommandLineParser
// Copyright (c) Twenzel. 
// Licensed under the Ms-PL (see license.txt in the CommandLineParser directory)
// Project Homepage: http://cmdlineparser.codeplex.com/
//-------------------------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Reflection;
using System.Text;

namespace SubtextUpgrader.CommandLineParser
{
    public static class CommandLineParser
    {
        /// <summary>
        /// Parse the arguments and returns a "filled" object
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="args"></param>
        /// <returns></returns>
        public static TResult Parse<TResult>(string[] args) where TResult : new()
        {
            var result = new TResult();

            if (args != null)
                ParseArgs(result, args);

            return result;
        }

        /// <summary>
        /// Returns the "help" infos
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <returns></returns>
        public static string GetInfo<TResult>()
        {
            const int columnWidth = 10;

            // get available options
            Options options = GetOptions(typeof(TResult));

            var sb = new StringBuilder();
            foreach (Option option in options)
            {
                sb.AppendFormat(string.Format("/{0} {1}\r\n", option.OptionName.PadRight(columnWidth - 1, ' '), option.Description));

                if (!String.IsNullOrEmpty(Convert.ToString(option.DefaultValue)))
                    sb.AppendFormat(string.Format("{0} default value: {1}\r\n", "".PadRight(columnWidth, ' '), option.DefaultValue));

                if (!string.IsNullOrEmpty(option.ShortOptionName))
                    sb.AppendFormat(string.Format("/{0} see {1}\r\n", option.ShortOptionName.PadRight(columnWidth - 1, ' '), option.OptionName));
            }


            return sb.ToString();
        }

        private static void ParseArgs<T>(T result, string[] args)
        {
            // get available options
            Options options = GetOptions(result.GetType());

            // iterate thru arguments
            for (int i = 0; i < args.Length; i++)
            {
                string argument = args[i];
                string value = null;

                // remove "/";
                if (argument.StartsWith("/"))
                    argument = argument.Substring(1);

                // check if argument has a value
                if (argument.Contains(":"))
                {
                    value = argument.Substring(argument.IndexOf(":") + 1);
                    argument = argument.Substring(0, argument.IndexOf(":"));
                }

                // get matching option
                Option option = options.GetByName(argument);

                if (option != null)
                {
                    // set property
                    SetProperty(result, option, value);
                }
            }
        }

        private static Options GetOptions(Type resultType)
        {
            var result = new Options();

            // get all public properties as "Options"
            PropertyInfo[] properties = resultType.GetProperties();

            foreach (PropertyInfo property in properties)
            {
                result.Add(new Option(property));
            }

            return result;
        }

        private static void SetProperty(object result, Option option, string value)
        {
            // get correct value
            object val = GetValue(option, value);

            // set value to property
            option.Property.SetValue(result, val, null);
        }

        private static object GetValue(Option option, string value)
        {
            try
            {
                // get default value
                object result;
                if (value == null)
                {
                    // if Otpion is bool, the default value is true so that /enabled is equal to /enabled:true
                    if (option.Property.PropertyType == typeof(bool))
                        result = true;
                    else
                        result = option.DefaultValue;
                }
                else
                {
                    // support lists
                    if (option.Property.PropertyType is IEnumerable)
                        result = Convert.ChangeType(value.Split(';'), option.Property.PropertyType);
                    else
                        result = Convert.ChangeType(value, option.Property.PropertyType);
                }

                return result;
            }
            catch (Exception e)
            {
                throw new InvalidValueException(option, e);
            }
        }
    }
}