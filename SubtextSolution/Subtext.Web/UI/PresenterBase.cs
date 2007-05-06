using System;

namespace Subtext.Web.UI
{
	/// <summary>
	/// Abstract base class for the presenter.
	/// </summary>
	/// <remarks>
	/// The reason for the private OnInternalEvent methods is to 
	/// allow us to have pre and post processing of the event 
	/// handlers. 
	/// </remarks>
	/// <typeparam name="T"></typeparam>
	public abstract class PresenterBase<T> where T:IView
	{
		private T view;

		/// <summary>
		/// Constructor for the presenter.
		/// </summary>
		/// <param name="view"></param>
		public PresenterBase(T view)
		{
			this.view = view;

			SubscribeToEvents();
		}

		/// <summary>
		/// Returns the view this is a presenter for.
		/// </summary>
		protected T View
		{
			get { return this.view; }
		}


		/// <summary>
		/// Called by the Init method. Presenters that implement this 
		/// base class should subscribe to their view specific 
		/// events, if any, when overriding this method.
		/// </summary>
		protected abstract void SubscribeToViewEvents();

		void SubscribeToEvents()
		{
			view.Init += OnInternalInit;
			view.Load += OnInternalLoad;
			view.PreRender += OnInternalPreRender;
		}

		private void OnInternalInit(object sender, EventArgs e)
		{
			SubscribeToViewEvents();
			OnInit(sender, e);
		}

		/// <summary>
		/// When overridden in a base class, handles the init event 
		/// of the view.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected virtual void OnInit(object sender, EventArgs e)
		{
		}

		/// <summary>
		/// When overridden in a base class, 
		/// handles the load event of the view.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected virtual void OnLoad(object sender, EventArgs e)
		{
		}

		/// <summary>
		/// When overridden in a base class, 
		/// handles the load event of the view.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnInternalLoad(object sender, EventArgs e)
		{
			OnLoad(sender, e);
		}

		void OnInternalPreRender(object sender, EventArgs e)
		{
			OnPreRender(sender, e);
		}

		/// <summary>
		/// When overridden in a base class, 
		/// handles the PreRender event of the view.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected virtual void OnPreRender(object sender, EventArgs e)
		{
		}
	}
}
