using System;
using System.Web.Security;
using MbUnit.Framework;
using Rhino.Mocks;
using Rhino.Mocks.Interfaces;
using Subtext.Web.HostAdmin.PresenterAndViews;
using Subtext.Web.HostAdmin.Presenters;

namespace UnitTests.Subtext.SubtextWeb.HostAdmin
{
	[TestFixture]
	public class UserManagerTests
	{
		[Test]
		public static void SetAllPropertiesToPropertyBehavior()
		{
			MockRepository mocks = new MockRepository();
			IUserManagerView mock = (IUserManagerView)mocks.CreateMock(typeof(IUserManagerView));

			using (mocks.Record())
			{
				UnitTestHelper.SetPropertyBehaviorOnAllProperties(mock);
			}

			using (mocks.Playback())
			{
				mock.PageSize = 25;
				Assert.AreEqual(25, mock.PageSize);
			}
		}

		[Test]
		public void PresenterCanAttachToEvents()
		{
			MockRepository mocks = new MockRepository();
			IUserManagerView viewMock = (IUserManagerView)mocks.CreateMock(typeof(IUserManagerView));
			IEventRaiser initRaiser;
			
			using (mocks.Record())
			{
				//Setup expectations for each event.
				viewMock.Init += null;
				initRaiser = LastCall.IgnoreArguments().GetEventRaiser();
				viewMock.Load += null;
				LastCall.IgnoreArguments();
				viewMock.PreRender += null;
				LastCall.IgnoreArguments();
			}

			using (mocks.Playback())
			{
				new UserManagerPresenter(viewMock, null);
				initRaiser.Raise(viewMock, EventArgs.Empty);
			}
		}

		[Test]
		public void CanSetFilterOnView()
		{
			MockRepository mocks = new MockRepository();
			UserManagerTestContext context = new UserManagerTestContext(mocks);
			context.Presenter.SetFilter("x");
			Assert.AreEqual("x", context.View.CurrentFilter, "Expected the view to be updated");
		}

		[Test]
		public void CanSetFilterToDisplayAllRecords()
		{
			MockRepository mocks = new MockRepository();
			UserManagerTestContext context = new UserManagerTestContext(mocks);
			
			//There are three ways to set the filter to show all records.
			context.Presenter.SetFilter("all");
			Assert.IsNull(context.View.CurrentFilter, "Expected the current filter to be null");
			context.Presenter.SetFilter(string.Empty);
			Assert.IsNull(context.View.CurrentFilter, "Expected the current filter to be null");
			context.Presenter.SetFilter(null);
			Assert.IsNull(context.View.CurrentFilter, "Expected the current filter to be null");
		}

		[Test]
		public void SetFiltersResetsSelectedIndex()
		{
			MockRepository mocks = new MockRepository();
			UserManagerTestContext context = new UserManagerTestContext(mocks);

			//There are three ways to set the filter to show all records.
			context.Presenter.SetFilter("x");
			Assert.AreEqual(-1, context.View.SelectedIndex, "Expected the selected index to be reset");
		}

		[Test]
		public void MakeSureFilterIsHonoredByFindUsers()
		{
			MockRepository mocks = new MockRepository();
			
			//Mock up the membership provider.
			//Make sure we have a collection with a user to return.
			MembershipProvider providerMock = (MembershipProvider) mocks.CreateMock(typeof (MembershipProvider));
			MembershipUserCollection results = new MembershipUserCollection();
			MembershipUser userMock = (MembershipUser)mocks.CreateMock(typeof(MembershipUser));
			SetupResult.For(userMock.ProviderUserKey).Return(Guid.NewGuid());
			SetupResult.For(userMock.UserName).Return("Haacked");
			
			//Make sure that the FindUsersByName is called with the proper filter.
			int totalRecords;
			Expect.Call(providerMock.FindUsersByName("x%", 0, 10, out totalRecords)).Return(results);
			UserManagerTestContext context = new UserManagerTestContext(mocks, providerMock);
			
			//Ok, expectations have been set. we're ready to start testing.
			results.Add(userMock);
			context.Presenter.SetFilter("x");
			context.RaisePreRender();
			
			Assert.AreEqual("x", context.View.CurrentFilter);
			Assert.AreEqual(1, context.View.Users.Count, "Expected the view to be updated with the 'found' user.");
			mocks.VerifyAll();
		}

		[Test]
		public void EnsureDatabindCalledOnPreRender()
		{
			MockRepository mocks = new MockRepository();

			// Setup Users.
			MembershipUserCollection results = new MembershipUserCollection();

			MembershipProvider providerMock = mocks.CreateMock<MembershipProvider>();
		
			MembershipUser userMock = mocks.DynamicMock<MembershipUser>();
			SetupResult.For(userMock.ProviderUserKey).Return(Guid.NewGuid());
			SetupResult.For(userMock.UserName).Return("Haacked");
			
			UserManagerTestContext context = new UserManagerTestContext(mocks
				, delegate
				  	{
						//Make sure provider returns user collection to view.
				  		int totalRecords;
				  		SetupResult.For(providerMock.GetAllUsers(0, 10, out totalRecords)).Return(results);
					}, providerMock);

			//Ok, expectations have been set. we're ready to start testing.
			results.Add(userMock);
			context.RaisePreRender();

			Assert.AreEqual(1, context.View.Users.Count, "User is here and accounted for.");

			mocks.VerifyAll();
		}

		[Test]
		public void EnsureAllUsersIfNoFilterSet()
		{
			MockRepository mocks = new MockRepository();
			IUserManagerView view = (IUserManagerView)mocks.CreateMock(typeof(IUserManagerView));
			UserManagerTestContext context = new UserManagerTestContext(mocks, view
				, delegate(IUserManagerView userManagerView)
					{
						userManagerView.DataBind();
						LastCall.On(userManagerView);
					});

			context.RaisePreRender();
			mocks.VerifyAll();
		}

		[Test]
		public void CanSetCurrentUserKeyOnPresenter()
		{
			MockRepository mocks = new MockRepository();
			UserManagerTestContext context = new UserManagerTestContext(mocks);

			object providerUserKey = Guid.NewGuid();
			MembershipUser userMock = (MembershipUser)mocks.DynamicMock(typeof(MembershipUser));
			SetupResult.For(userMock.ProviderUserKey).Return(providerUserKey);
			SetupResult.For(userMock.UserName).Return("Haacked");

			context.Presenter.SetSelectedUserName("Haacked");
			Assert.AreEqual("Haacked", context.View.SelectedUserName);
		}

		#region UserManagerTestContext
		public class UserManagerTestContext : PresenterTestContext<IUserManagerView, UserManagerPresenter>
		{
			public UserManagerTestContext(MockRepository mocks) : this(mocks, (MembershipProvider)mocks.DynamicMock(typeof(MembershipProvider)))
			{
			}

			public UserManagerTestContext(MockRepository mocks, SetupExpectations expectations) : this(mocks, expectations, (MembershipProvider)mocks.DynamicMock(typeof(MembershipProvider)))
			{
			}

			public UserManagerTestContext(MockRepository mocks, IUserManagerView view, SetupExpectations expectations) : this(mocks, expectations, view, (MembershipProvider)mocks.DynamicMock(typeof(MembershipProvider)))
			{
			}

			public UserManagerTestContext(MockRepository mocks, MembershipProvider provider) : this(mocks, null, provider)
			{
			}

			public UserManagerTestContext(MockRepository mocks, SetupExpectations expectations, MembershipProvider provider) : base(mocks)
			{
				Initialize(mocks, expectations, provider);
			}

			public UserManagerTestContext(MockRepository mocks, SetupExpectations expectations, IUserManagerView view, MembershipProvider provider) : base(mocks, view)
			{
				Initialize(mocks, expectations, provider);
			}

			private void Initialize(MockRepository mockRepository, SetupExpectations expectations, MembershipProvider membershipProvider)
			{
				this.provider = membershipProvider;
				UnitTestHelper.SetPropertyBehaviorOnAllProperties(View);
				View.SelectedIndex = 0;

				if (expectations != null)
				{
					expectations(this.View);
				}

				mockRepository.ReplayAll();
				this.presenter = new UserManagerPresenter(View, this.provider);
				RaiseInitEvent();
				View.PageSize = 10;
			}


			public delegate void SetupExpectations(IUserManagerView view);

			public override UserManagerPresenter Presenter
			{
				get { return this.presenter; }
			}

			private UserManagerPresenter presenter;

			public MembershipProvider MembershipProvider
			{
				get { return this.provider; }
			}

			private MembershipProvider provider;
		}
		#endregion
	}
}
