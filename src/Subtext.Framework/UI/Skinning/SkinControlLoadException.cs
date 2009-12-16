using System;

namespace Subtext.Framework.UI.Skinning
{
    public class SkinControlLoadException : Exception
    {
        public SkinControlLoadException(string message, string controlPath, Exception exception) : base(message, exception)
        {
            ControlPath = controlPath;
        }

        public string ControlPath { get; private set; }
    }
}
