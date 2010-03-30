namespace SubtextUpgrader {
	partial class UpgradeForm {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing) {
			if (disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UpgradeForm));
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.Backup = new System.Windows.Forms.TextBox();
			this.button3 = new System.Windows.Forms.Button();
			this.button2 = new System.Windows.Forms.Button();
			this.Destination = new System.Windows.Forms.TextBox();
			this.Verbose = new System.Windows.Forms.CheckBox();
			this.button4 = new System.Windows.Forms.Button();
			this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
			this.progressBar1 = new System.Windows.Forms.ProgressBar();
			this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
			this.Cancel = new System.Windows.Forms.Button();
			this.Message = new System.Windows.Forms.TextBox();
			this.tableLayoutPanel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.tableLayoutPanel1.ColumnCount = 2;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 70.31964F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 29.68036F));
			this.tableLayoutPanel1.Controls.Add(this.Backup, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.button3, 1, 1);
			this.tableLayoutPanel1.Controls.Add(this.button2, 1, 0);
			this.tableLayoutPanel1.Controls.Add(this.Destination, 0, 0);
			this.tableLayoutPanel1.Location = new System.Drawing.Point(12, 54);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 2;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(438, 68);
			this.tableLayoutPanel1.TabIndex = 0;
			// 
			// Backup
			// 
			this.Backup.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.Backup.Location = new System.Drawing.Point(3, 37);
			this.Backup.Name = "Backup";
			this.Backup.Size = new System.Drawing.Size(302, 23);
			this.Backup.TabIndex = 4;
			// 
			// button3
			// 
			this.button3.Location = new System.Drawing.Point(311, 37);
			this.button3.Name = "button3";
			this.button3.Size = new System.Drawing.Size(75, 23);
			this.button3.TabIndex = 5;
			this.button3.Text = "Backup...";
			this.button3.UseVisualStyleBackColor = true;
			this.button3.Click += new System.EventHandler(this.button3_Click);
			// 
			// button2
			// 
			this.button2.Location = new System.Drawing.Point(311, 3);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(96, 23);
			this.button2.TabIndex = 3;
			this.button2.Text = "Destination...";
			this.button2.UseVisualStyleBackColor = true;
			this.button2.Click += new System.EventHandler(this.button2_Click);
			// 
			// Destination
			// 
			this.Destination.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.Destination.Location = new System.Drawing.Point(3, 3);
			this.Destination.Name = "Destination";
			this.Destination.Size = new System.Drawing.Size(302, 23);
			this.Destination.TabIndex = 0;
			// 
			// Verbose
			// 
			this.Verbose.AutoSize = true;
			this.Verbose.Location = new System.Drawing.Point(15, 128);
			this.Verbose.Name = "Verbose";
			this.Verbose.Size = new System.Drawing.Size(86, 21);
			this.Verbose.TabIndex = 1;
			this.Verbose.Text = "Verbose?";
			this.Verbose.UseVisualStyleBackColor = true;
			// 
			// button4
			// 
			this.button4.Location = new System.Drawing.Point(119, 155);
			this.button4.Name = "button4";
			this.button4.Size = new System.Drawing.Size(188, 52);
			this.button4.TabIndex = 3;
			this.button4.Text = "Upgrade SubText!";
			this.button4.UseVisualStyleBackColor = true;
			this.button4.Click += new System.EventHandler(this.button4_Click);
			// 
			// progressBar1
			// 
			this.progressBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.progressBar1.Location = new System.Drawing.Point(13, 213);
			this.progressBar1.Name = "progressBar1";
			this.progressBar1.Size = new System.Drawing.Size(428, 23);
			this.progressBar1.TabIndex = 5;
			// 
			// backgroundWorker1
			// 
			this.backgroundWorker1.WorkerReportsProgress = true;
			this.backgroundWorker1.WorkerSupportsCancellation = true;
			this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
			this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
			this.backgroundWorker1.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker1_ProgressChanged);
			// 
			// Cancel
			// 
			this.Cancel.Location = new System.Drawing.Point(323, 155);
			this.Cancel.Name = "Cancel";
			this.Cancel.Size = new System.Drawing.Size(118, 52);
			this.Cancel.TabIndex = 6;
			this.Cancel.Text = "Cancel";
			this.Cancel.UseVisualStyleBackColor = true;
			this.Cancel.Click += new System.EventHandler(this.Cancel_Click);
			// 
			// Message
			// 
			this.Message.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.Message.Location = new System.Drawing.Point(13, 258);
			this.Message.Multiline = true;
			this.Message.Name = "Message";
			this.Message.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.Message.Size = new System.Drawing.Size(428, 217);
			this.Message.TabIndex = 6;
			// 
			// UpgradeForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(453, 487);
			this.Controls.Add(this.Message);
			this.Controls.Add(this.Cancel);
			this.Controls.Add(this.progressBar1);
			this.Controls.Add(this.button4);
			this.Controls.Add(this.Verbose);
			this.Controls.Add(this.tableLayoutPanel1);
			this.Font = new System.Drawing.Font("Tahoma", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "UpgradeForm";
			this.Text = "SubText Upgrader";
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.TextBox Destination;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.CheckBox Verbose;
		private System.Windows.Forms.TextBox Backup;
		private System.Windows.Forms.Button button3;
		private System.Windows.Forms.Button button4;
		private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
		private System.Windows.Forms.ProgressBar progressBar1;
		private System.ComponentModel.BackgroundWorker backgroundWorker1;
		private System.Windows.Forms.Button Cancel;
		private System.Windows.Forms.TextBox Message;
	}
}
