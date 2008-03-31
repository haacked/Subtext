using System;
using System.Collections.Generic;
using System.Text;

namespace Subtext.Framework.Services
{
    public class BrowserInfo
    {
        public BrowserInfo(bool mobile)
        {
            this.mobile = mobile;
        }

        public bool Mobile
        {
            get
            {
                return mobile;
            }
        }
        bool mobile;
    }
}
