using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Subtext.Framework.Exceptions
{
    public class DeprecatedPhysicalPathsException : Exception
    {
        string message;
        public DeprecatedPhysicalPathsException(ReadOnlyCollection<string> physicalPaths)
        {
            InvalidPhysicalPaths = physicalPaths;
            message = "In order to complete the upgrade, please delete the following directories/files." + Environment.NewLine;
            foreach (var path in physicalPaths) {
                message += " " + path + Environment.NewLine;
            }
        }

        public DeprecatedPhysicalPathsException(IEnumerable<string> physicalPaths) : this(new ReadOnlyCollection<string>(physicalPaths.ToList()))
        {
        }

        public override string Message
        {
            get
            {
                return message;
            }
        }

        public ReadOnlyCollection<string> InvalidPhysicalPaths
        {
            get;
            private set;
        }
    }
}
