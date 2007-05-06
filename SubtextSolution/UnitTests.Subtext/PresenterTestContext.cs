using System;
using Rhino.Mocks;
using Rhino.Mocks.Interfaces;
using Subtext.Web.UI;

namespace UnitTests.Subtext
{
	public abstract class PresenterTestContext<TView, TPresenter>
		where TView : IView
		where TPresenter : PresenterBase<TView>
	{
		protected MockRepository mocks;
		private IEventRaiser initRaiser;
		private IEventRaiser loadRaiser;
		private IEventRaiser preRenderRaiser;

		public PresenterTestContext(MockRepository mocks) : this(mocks, (TView)mocks.DynamicMock(typeof(TView)))
		{
		}

		public PresenterTestContext(MockRepository mocks, TView view)
		{
			this.mocks = mocks;
			this.view = view;
			this.view.Init += null;
			this.initRaiser = LastCall.IgnoreArguments().GetEventRaiser();
			this.view.Load += null;
			this.loadRaiser = LastCall.IgnoreArguments().GetEventRaiser();
			this.view.PreRender += null;
			this.preRenderRaiser = LastCall.IgnoreArguments().GetEventRaiser();
		}

		public TView View
		{
			get { return this.view; }
		}

		private TView view;

		public abstract TPresenter Presenter { get;}

		public void RaiseInitEvent()
		{
			RaiseInitEvent(EventArgs.Empty);
		}

		public void RaiseInitEvent(EventArgs args)
		{
			initRaiser.Raise(View, args);
		}

		public void RaiseLoadEvent()
		{
			RaiseLoadEvent(EventArgs.Empty);
		}

		public void RaiseLoadEvent(EventArgs args)
		{
			this.loadRaiser.Raise(View, args);
		}

		public void RaisePreRender()
		{
			RaisePreRender(EventArgs.Empty);
		}

		public void RaisePreRender(EventArgs args)
		{
			this.preRenderRaiser.Raise(View, args);
		}
	}
}
