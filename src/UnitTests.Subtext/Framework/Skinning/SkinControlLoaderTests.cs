using System.Web;
using System.Web.UI;
using MbUnit.Framework;
using Moq;
using Moq.Stub;
using Subtext.Framework.Configuration;
using Subtext.Framework.UI.Skinning;
using Subtext.Web.Skins._System.Controls;

namespace UnitTests.Subtext.Framework.Skinning
{
    [TestFixture]
    public class SkinControlLoaderTests
    {
        [Test]
        public void LoadControl_WithControlName_LoadsTheControlFromTheSkinFolder()
        {
            // arrange
            var containerControl = new Mock<IContainerControl>();
            var loadedControl = new UserControl { ID = "Foo.Bar" };
            containerControl.Setup(tc => tc.LoadControl("~/Skins/OfMyChinnyChinChin/Controls/ViewPost.ascx")).Returns(loadedControl);
            var skin = new SkinConfig {TemplateFolder = "OfMyChinnyChinChin"};
            var skinControlLoader = new SkinControlLoader(containerControl.Object, skin);
            
            // act
            var control = skinControlLoader.LoadControl("ViewPost");

            // assert
            Assert.AreSame(loadedControl, control);
        }

        [Test]
        public void LoadControl_WithControlName_ReplacesDotWithUnderscoreInId()
        {
            // arrange
            var containerControl = new Mock<IContainerControl>();
            var loadedControl = new UserControl {ID = "Foo.Bar"};
            containerControl.Setup(tc => tc.LoadControl("~/Skins/OfMyChinnyChinChin/Controls/ViewPost.ascx")).Returns(loadedControl);
            var skin = new SkinConfig { TemplateFolder = "OfMyChinnyChinChin" };
            var skinControlLoader = new SkinControlLoader(containerControl.Object, skin);

            // act
            var control = skinControlLoader.LoadControl("ViewPost");

            // assert
            Assert.AreEqual("Foo_Bar", control.ID);
        }


        [Test]
        public void LoadControl_WithControlThrowingHttpException_LoadsFallbackControl()
        {
            // arrange
            var containerControl = new Mock<IContainerControl>();
            var fallbackControl = new UserControl { ID = "Foo.Bar" };
            containerControl.Setup(tc => tc.LoadControl("~/Skins/OfMyChinnyChinChin/Controls/ViewPost.ascx")).Throws(new HttpException());
            containerControl.Setup(tc => tc.LoadControl("~/Skins/_System/Controls/ViewPost.ascx")).Returns(fallbackControl);
            var skin = new SkinConfig { TemplateFolder = "OfMyChinnyChinChin" };
            var skinControlLoader = new SkinControlLoader(containerControl.Object, skin);

            // act
            var control = skinControlLoader.LoadControl("ViewPost");

            // assert
            Assert.AreSame(fallbackControl, control);
        }

        [Test]
        public void LoadControl_WithControlThrowingHttpParseException_LoadsErrorControlWithExceptionProperty()
        {
            // arrange
            var containerControl = new Mock<IContainerControl>();
            var exception = new HttpParseException();
            var userControl = new Mock<UserControl>();
            userControl.Stub(c => c.ID);
            var errorControl = userControl.As<IErrorControl>();
            userControl.Object.ID = "Foo.Bar";
            errorControl.Stub(c => c.Exception);
            containerControl.Setup(tc => tc.LoadControl("~/Skins/VsShirts/Controls/ViewPost.ascx")).Throws(exception);
            containerControl.Setup(tc => tc.LoadControl("~/Skins/_System/Controls/Error.ascx")).Returns((UserControl)errorControl.Object);
            var skin = new SkinConfig { TemplateFolder = "VsShirts" };
            var skinControlLoader = new SkinControlLoader(containerControl.Object, skin);

            // act
            var control = skinControlLoader.LoadControl("ViewPost") as IErrorControl;

            // assert
            Assert.AreEqual(exception, control.Exception.InnerException);
        }

        [Test]
        public void LoadControl_WithControlThrowingHttpParseException_LoadsErrorControlWithExceptionHavingControlPath()
        {
            // arrange
            var containerControl = new Mock<IContainerControl>();
            var exception = new HttpParseException();
            var errorControl = new Error();
            containerControl.Setup(tc => tc.LoadControl("~/Skins/OfMyChinnyChinChin/Controls/ViewPost.ascx")).Throws(exception);
            containerControl.Setup(tc => tc.LoadControl("~/Skins/_System/Controls/Error.ascx")).Returns(errorControl);
            var skin = new SkinConfig { TemplateFolder = "OfMyChinnyChinChin" };
            var skinControlLoader = new SkinControlLoader(containerControl.Object, skin);

            // act
            var control = skinControlLoader.LoadControl("ViewPost") as IErrorControl;

            // assert
            Assert.AreEqual("~/Skins/OfMyChinnyChinChin/Controls/ViewPost.ascx", control.Exception.ControlPath);
        }

        [Test]
        public void LoadControl_WithControlAndFallbackThrowingHttpException_LoadsErrorControl()
        {
            // arrange
            var containerControl = new Mock<IContainerControl>();
            var errorControl = new Error();
            containerControl.Setup(tc => tc.LoadControl("~/Skins/OfMyChinnyChinChin/Controls/ViewPost.ascx")).Throws(new HttpException());
            containerControl.Setup(tc => tc.LoadControl("~/Skins/_System/Controls/ViewPost.ascx")).Throws(new HttpException());
            containerControl.Setup(tc => tc.LoadControl("~/Skins/_System/Controls/Error.ascx")).Returns(errorControl);
            var skin = new SkinConfig { TemplateFolder = "OfMyChinnyChinChin" };
            var skinControlLoader = new SkinControlLoader(containerControl.Object, skin);

            // act
            var control = skinControlLoader.LoadControl("ViewPost") as Error;

            // assert
            Assert.AreSame(errorControl, control);
            Assert.AreEqual("~/Skins/OfMyChinnyChinChin/Controls/ViewPost.ascx", control.Exception.ControlPath);
        }
    }
}