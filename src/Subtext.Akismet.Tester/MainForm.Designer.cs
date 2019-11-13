namespace Subtext.Akismet.Tester
{
	partial class frmMain
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.grpAkismet = new System.Windows.Forms.GroupBox();
			this.btnVerify = new System.Windows.Forms.Button();
			this.txtBlogUrl = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.txtApiKey = new System.Windows.Forms.TextBox();
			this.lblKey = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.txtCommentType = new System.Windows.Forms.TextBox();
			this.lblAuthor = new System.Windows.Forms.Label();
			this.txtAuthor = new System.Windows.Forms.TextBox();
			this.btnInsertSpamAuthor = new System.Windows.Forms.Button();
			this.grpComment = new System.Windows.Forms.GroupBox();
			this.btnHam = new System.Windows.Forms.Button();
			this.btnSpam = new System.Windows.Forms.Button();
			this.btnCheck = new System.Windows.Forms.Button();
			this.txtUserAgent = new System.Windows.Forms.TextBox();
			this.txtContent = new System.Windows.Forms.TextBox();
			this.label5 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.txtIP = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.txtResponse = new System.Windows.Forms.TextBox();
			this.label6 = new System.Windows.Forms.Label();
			this.grpAkismet.SuspendLayout();
			this.grpComment.SuspendLayout();
			this.SuspendLayout();
			// 
			// grpAkismet
			// 
			this.grpAkismet.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.grpAkismet.Controls.Add(this.btnVerify);
			this.grpAkismet.Controls.Add(this.txtBlogUrl);
			this.grpAkismet.Controls.Add(this.label1);
			this.grpAkismet.Controls.Add(this.txtApiKey);
			this.grpAkismet.Controls.Add(this.lblKey);
			this.grpAkismet.Location = new System.Drawing.Point(3, 50);
			this.grpAkismet.Name = "grpAkismet";
			this.grpAkismet.Size = new System.Drawing.Size(404, 82);
			this.grpAkismet.TabIndex = 1;
			this.grpAkismet.TabStop = false;
			this.grpAkismet.Text = "Akismet";
			// 
			// btnVerify
			// 
			this.btnVerify.Location = new System.Drawing.Point(303, 44);
			this.btnVerify.Name = "btnVerify";
			this.btnVerify.Size = new System.Drawing.Size(75, 23);
			this.btnVerify.TabIndex = 4;
			this.btnVerify.Text = "Verify";
			this.btnVerify.UseVisualStyleBackColor = true;
			this.btnVerify.Click += new System.EventHandler(this.btnVerify_Click);
			// 
			// txtBlogUrl
			// 
			this.txtBlogUrl.Location = new System.Drawing.Point(67, 51);
			this.txtBlogUrl.Name = "txtBlogUrl";
			this.txtBlogUrl.Size = new System.Drawing.Size(220, 20);
			this.txtBlogUrl.TabIndex = 3;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(6, 54);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(44, 13);
			this.label1.TabIndex = 2;
			this.label1.Text = "Blog Url";
			// 
			// txtApiKey
			// 
			this.txtApiKey.Location = new System.Drawing.Point(67, 25);
			this.txtApiKey.Name = "txtApiKey";
			this.txtApiKey.Size = new System.Drawing.Size(220, 20);
			this.txtApiKey.TabIndex = 1;
			// 
			// lblKey
			// 
			this.lblKey.AutoSize = true;
			this.lblKey.Location = new System.Drawing.Point(6, 28);
			this.lblKey.Name = "lblKey";
			this.lblKey.Size = new System.Drawing.Size(45, 13);
			this.lblKey.TabIndex = 0;
			this.lblKey.Text = "API Key";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(6, 22);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(31, 13);
			this.label2.TabIndex = 2;
			this.label2.Text = "Type";
			// 
			// txtCommentType
			// 
			this.txtCommentType.Location = new System.Drawing.Point(67, 19);
			this.txtCommentType.Name = "txtCommentType";
			this.txtCommentType.Size = new System.Drawing.Size(210, 20);
			this.txtCommentType.TabIndex = 3;
			this.txtCommentType.Text = "comment";
			// 
			// lblAuthor
			// 
			this.lblAuthor.AutoSize = true;
			this.lblAuthor.Location = new System.Drawing.Point(6, 48);
			this.lblAuthor.Name = "lblAuthor";
			this.lblAuthor.Size = new System.Drawing.Size(38, 13);
			this.lblAuthor.TabIndex = 4;
			this.lblAuthor.Text = "Author";
			// 
			// txtAuthor
			// 
			this.txtAuthor.Location = new System.Drawing.Point(67, 45);
			this.txtAuthor.Name = "txtAuthor";
			this.txtAuthor.Size = new System.Drawing.Size(210, 20);
			this.txtAuthor.TabIndex = 5;
			// 
			// btnInsertSpamAuthor
			// 
			this.btnInsertSpamAuthor.Location = new System.Drawing.Point(283, 41);
			this.btnInsertSpamAuthor.Name = "btnInsertSpamAuthor";
			this.btnInsertSpamAuthor.Size = new System.Drawing.Size(115, 23);
			this.btnInsertSpamAuthor.TabIndex = 6;
			this.btnInsertSpamAuthor.Text = "<< Test Author";
			this.btnInsertSpamAuthor.UseVisualStyleBackColor = true;
			this.btnInsertSpamAuthor.Click += new System.EventHandler(this.btnInsertSpamAuthor_Click);
			// 
			// grpComment
			// 
			this.grpComment.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.grpComment.Controls.Add(this.btnHam);
			this.grpComment.Controls.Add(this.btnSpam);
			this.grpComment.Controls.Add(this.btnCheck);
			this.grpComment.Controls.Add(this.txtUserAgent);
			this.grpComment.Controls.Add(this.txtContent);
			this.grpComment.Controls.Add(this.label5);
			this.grpComment.Controls.Add(this.label3);
			this.grpComment.Controls.Add(this.txtIP);
			this.grpComment.Controls.Add(this.btnInsertSpamAuthor);
			this.grpComment.Controls.Add(this.label4);
			this.grpComment.Controls.Add(this.txtAuthor);
			this.grpComment.Controls.Add(this.lblAuthor);
			this.grpComment.Controls.Add(this.txtCommentType);
			this.grpComment.Controls.Add(this.label2);
			this.grpComment.Location = new System.Drawing.Point(3, 138);
			this.grpComment.Name = "grpComment";
			this.grpComment.Size = new System.Drawing.Size(404, 319);
			this.grpComment.TabIndex = 0;
			this.grpComment.TabStop = false;
			this.grpComment.Text = "Comment";
			// 
			// btnHam
			// 
			this.btnHam.Location = new System.Drawing.Point(323, 289);
			this.btnHam.Name = "btnHam";
			this.btnHam.Size = new System.Drawing.Size(75, 23);
			this.btnHam.TabIndex = 11;
			this.btnHam.Text = "Ham";
			this.btnHam.UseVisualStyleBackColor = true;
			this.btnHam.Click += new System.EventHandler(this.btnHam_Click);
			// 
			// btnSpam
			// 
			this.btnSpam.Location = new System.Drawing.Point(242, 289);
			this.btnSpam.Name = "btnSpam";
			this.btnSpam.Size = new System.Drawing.Size(75, 23);
			this.btnSpam.TabIndex = 10;
			this.btnSpam.Text = "Spam";
			this.btnSpam.UseVisualStyleBackColor = true;
			this.btnSpam.Click += new System.EventHandler(this.btnSpam_Click);
			// 
			// btnCheck
			// 
			this.btnCheck.Location = new System.Drawing.Point(161, 289);
			this.btnCheck.Name = "btnCheck";
			this.btnCheck.Size = new System.Drawing.Size(75, 23);
			this.btnCheck.TabIndex = 9;
			this.btnCheck.Text = "Check";
			this.btnCheck.UseVisualStyleBackColor = true;
			this.btnCheck.Click += new System.EventHandler(this.btnCheck_Click);
			// 
			// txtUserAgent
			// 
			this.txtUserAgent.Location = new System.Drawing.Point(67, 262);
			this.txtUserAgent.Name = "txtUserAgent";
			this.txtUserAgent.Size = new System.Drawing.Size(220, 20);
			this.txtUserAgent.TabIndex = 8;
			this.txtUserAgent.Text = "Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US; rv:1.8.0.7) Gecko/20060909 Firefo" +
				"x/1.5.0.7";
			// 
			// txtContent
			// 
			this.txtContent.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtContent.Location = new System.Drawing.Point(67, 72);
			this.txtContent.Multiline = true;
			this.txtContent.Name = "txtContent";
			this.txtContent.Size = new System.Drawing.Size(331, 158);
			this.txtContent.TabIndex = 8;
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(6, 265);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(60, 13);
			this.label5.TabIndex = 7;
			this.label5.Text = "User Agent";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(6, 75);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(31, 13);
			this.label3.TabIndex = 7;
			this.label3.Text = "Body";
			// 
			// txtIP
			// 
			this.txtIP.Location = new System.Drawing.Point(67, 236);
			this.txtIP.Name = "txtIP";
			this.txtIP.Size = new System.Drawing.Size(220, 20);
			this.txtIP.TabIndex = 6;
			this.txtIP.Text = "24.126.150.127";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(6, 239);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(58, 13);
			this.label4.TabIndex = 5;
			this.label4.Text = "IP Address";
			// 
			// txtResponse
			// 
			this.txtResponse.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtResponse.Location = new System.Drawing.Point(12, 485);
			this.txtResponse.Multiline = true;
			this.txtResponse.Name = "txtResponse";
			this.txtResponse.Size = new System.Drawing.Size(395, 82);
			this.txtResponse.TabIndex = 2;
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(9, 469);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(55, 13);
			this.label6.TabIndex = 3;
			this.label6.Text = "Response";
			// 
			// frmMain
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(414, 581);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.txtResponse);
			this.Controls.Add(this.grpAkismet);
			this.Controls.Add(this.grpComment);
			this.Name = "frmMain";
			this.Text = "Subtext Akismet Tester";
			this.grpAkismet.ResumeLayout(false);
			this.grpAkismet.PerformLayout();
			this.grpComment.ResumeLayout(false);
			this.grpComment.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.GroupBox grpAkismet;
		private System.Windows.Forms.Button btnVerify;
		private System.Windows.Forms.TextBox txtBlogUrl;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox txtApiKey;
		private System.Windows.Forms.Label lblKey;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox txtCommentType;
		private System.Windows.Forms.Label lblAuthor;
		private System.Windows.Forms.TextBox txtAuthor;
		private System.Windows.Forms.Button btnInsertSpamAuthor;
		private System.Windows.Forms.GroupBox grpComment;
		private System.Windows.Forms.TextBox txtContent;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Button btnHam;
		private System.Windows.Forms.Button btnSpam;
		private System.Windows.Forms.Button btnCheck;
		private System.Windows.Forms.TextBox txtUserAgent;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.TextBox txtIP;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TextBox txtResponse;
		private System.Windows.Forms.Label label6;
	}
}

