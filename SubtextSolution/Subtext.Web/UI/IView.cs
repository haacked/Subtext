using System;

namespace Subtext.Web.UI
{
	/// <summary>
	/// Represents the "View" in the Supervising Controller pattern 
	/// (a flavor of Model View Presenter).
	/// </summary>
	public interface IView
	{
		/// <summary>
		/// Called when the view initializes.
		/// </summary>
		event EventHandler Init;

		/// <summary>
		/// Called when the view is loaded.
		/// </summary>
		event EventHandler Load;

		/// <summary>
		/// Called when the view is ready to render.
		/// </summary>
		event EventHandler PreRender;
		
		/// <summary>
		/// Returns true if the current request is a postback. 
		/// This view is specific to ASP.NET.
		/// </summary>
		bool IsPostBack { get;}
		
		/// <summary>
		/// Binds the data to the view.
		/// </summary>
		void DataBind();

		/// <summary>
		/// True if the validation controls all return valid.
		/// </summary>
		bool IsValid { get;}
	}
}
