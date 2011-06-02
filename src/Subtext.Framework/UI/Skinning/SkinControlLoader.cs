#region Disclaimer/Info

///////////////////////////////////////////////////////////////////////////////////////////////////
// Subtext WebLog
// 
// Subtext is an open source weblog system that is a fork of the .TEXT
// weblog system.
//
// For updated news and information please visit http://subtextproject.com/
// Subtext is hosted at Google Code at http://code.google.com/p/subtext/
// The development mailing list is at subtext@googlegroups.com 
//
// This project is licensed under the BSD license.  See the License.txt file for more information.
///////////////////////////////////////////////////////////////////////////////////////////////////

#endregion

using System;
using System.Diagnostics;
using System.Web;
using System.Web.UI;
using Subtext.Framework.Configuration;
using Subtext.Framework.Properties;

namespace Subtext.Framework.UI.Skinning
{
    public class SkinControlLoader : ISkinControlLoader
    {
        const string ControlLocationFormat = "~/Skins/{0}/Controls/{1}.ascx";
        const string SystemFolderName = "_System";
        const string ErrorControlName = "Error";

        public SkinControlLoader(IContainerControl container, SkinConfig skin)
        {
            Container = container;
            Skin = skin;
        }

        protected IContainerControl Container
        {
            get;
            private set;
        }

        protected SkinConfig Skin
        {
            get;
            private set;
        }

        public Control LoadControl(string controlName)
        {
            var control = LoadControlFromSkin(controlName);
            if (control != null && control.ID != null)
            {
                control.ID = control.ID.Replace('.', '_');
            }
            var skinControl = control as ISkinControlContainer;
            if (skinControl != null)
            {
                skinControl.SkinControlLoader = this;
            }
            return control;
        }

        Control LoadControlFromSkin(string controlName)
        {
            var result = GetLoadControlResult(Skin.TemplateFolder, controlName);
            if (result.SkinControl != null)
            {
                return result.SkinControl;
            }

            // short circuit for this specific exception.
            if (result.Exception is HttpParseException)
            {
                return GetErrorControl(new SkinControlLoadException(Resources.SkinControlLoadException_Message, result.ControlPath, result.Exception));
            }

            // Fallback
            var fallBack = GetLoadControlResult(SystemFolderName, controlName);
            if (fallBack.SkinControl != null)
            {
                return fallBack.SkinControl;
            }

            // Need to return the original control path and exception.
            return GetErrorControl(new SkinControlLoadException(Resources.SkinControlLoadException_Message, result.ControlPath, result.Exception));
        }

        private Control GetErrorControl(SkinControlLoadException exception)
        {
            var result = GetLoadControlResult(SystemFolderName, ErrorControlName);
            Debug.Assert(result != null, "The result should never be null");
            var control = result.SkinControl;
            if (control == null)
            {
                throw new InvalidOperationException("The system Error skin control is missing. Did you delete it by mistake? It should be located at '" + result.ControlPath + "'");
            }

            var errorControl = result.SkinControl as IErrorControl;
            if (errorControl != null)
            {
                errorControl.Exception = exception;
            }
            return control;
        }

        private SkinControlLoadResult GetLoadControlResult(string folderName, string controlName)
        {
            string controlPath = string.Format(ControlLocationFormat, folderName, controlName);
            try
            {
                var control = Container.LoadControl(controlPath);
                return new SkinControlLoadResult(controlPath, control, null);
            }
            catch (HttpParseException hpe)
            {
                var control = GetErrorControl(new SkinControlLoadException(Resources.SkinControlLoadException_Message, controlPath, hpe));
                return new SkinControlLoadResult(controlPath, control, null);
            }
            catch (HttpCompileException hce)
            {
                var control = GetErrorControl(new SkinControlLoadException(Resources.SkinControlLoadException_Message, controlPath, hce));
                return new SkinControlLoadResult(controlPath, control, null);
            }
            catch (Exception e)
            {
                return new SkinControlLoadResult(controlPath, null, e);
            }
        }
    }
}
