//-------------------------------------------------------------------------------------------------
// CommandLineParser
// Copyright (c) Twenzel. 
// Licensed under the Ms-PL (see license.txt in the CommandLineParser directory)
// Project Homepage: http://cmdlineparser.codeplex.com/
//-------------------------------------------------------------------------------------------------

using System;

namespace SubtextUpgrader.CommandLineParser
{
    public class InvalidValueException : Exception
    {

        public InvalidValueException(Option option, Exception e)
            : base(string.Format("{0} has a invalid value!", option.OptionName), e)
        {
        }
    }
}