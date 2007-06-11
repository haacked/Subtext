using System;

namespace Subtext.TestLibrary.Servers
{
	/// <summary>
	/// Summary description for SimulatedEmailAddress.
	/// </summary>
	public class TestEmailAddress
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="TestEmailAddress"/> class.
		/// </summary>
		/// <param name="email">The email.</param>
		/// <param name="name">The name.</param>
		public TestEmailAddress(string email, string name)
		{
			this.email = email;
			this.name = name;
		}
		
		/// <summary>
		/// Gets the email.
		/// </summary>
		/// <value>The email.</value>
		public string Email
		{
			get { return this.email; }
		}

		string email;

		/// <summary>
		/// Gets the name.
		/// </summary>
		/// <value>The name.</value>
		public string Name
		{
			get { return this.name; }
		}

		string name;
		
		/// <summary>
		/// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
		/// </summary>
		/// <returns>
		/// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
		/// </returns>
		public override string ToString()
		{
			string emailPortion = "<" + email + ">";
			if(name != null && name.Length > 0)
				return name + " " + emailPortion;
			return emailPortion;
		}

	}
}
