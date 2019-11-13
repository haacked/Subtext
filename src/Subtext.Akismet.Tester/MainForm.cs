using System;
using System.Net;
using System.Windows.Forms;

namespace Subtext.Akismet.Tester
{
	public partial class frmMain : Form
	{
		AkismetClient client;
		public frmMain()
		{
			InitializeComponent();
		}

		private void btnInsertSpamAuthor_Click(object sender, EventArgs e)
		{
			txtAuthor.Text = "viagra-test-123";
		}

		private void btnVerify_Click(object sender, EventArgs e)
		{
			try
			{
				client = new AkismetClient(this.txtApiKey.Text, new Uri(this.txtBlogUrl.Text));
				this.txtResponse.Text = client.VerifyApiKey().ToString();
			}
			catch(Exception exc)
			{
				this.txtResponse.Text = exc.Message;
			}
		}

		private void btnCheck_Click(object sender, EventArgs e)
		{
			try
			{
				if (CheckVerification())
				{
					this.txtResponse.Text = client.CheckCommentForSpam(GetComment()).ToString();
				}
			}
			catch (Exception exc)
			{
				this.txtResponse.Text = exc.Message;
			}
		}

		private void btnSpam_Click(object sender, EventArgs e)
		{
			try
			{
				client.SubmitSpam(GetComment());
			}
			catch (Exception exc)
			{
				this.txtResponse.Text = exc.Message;
			}
		}

		private void btnHam_Click(object sender, EventArgs e)
		{
			try
			{
				client.SubmitHam(GetComment());
			}
			catch (Exception exc)
			{
				this.txtResponse.Text = exc.Message;
			}
		}

		private bool CheckVerification()
		{
			if (this.client == null)
			{
				this.txtResponse.Text = "Please verify Akismet first.";
				return false;
			}
			return true;
		}
		
		private IComment GetComment()
		{
			Comment comment = new Comment(IPAddress.Parse(this.txtIP.Text), txtUserAgent.Text);

			comment.CommentType = txtCommentType.Text;
			comment.Author = txtAuthor.Text;
			comment.Content = txtContent.Text;
			
			return comment;
		}
	}
}