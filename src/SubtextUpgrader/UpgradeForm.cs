using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SubtextUpgrader {
	public partial class UpgradeForm : Form {
		public UpgradeForm() {
			InitializeComponent();
		}

		private void button1_Click(object sender, EventArgs e) {
			if (this.folderBrowserDialog1.ShowDialog() == DialogResult.OK) {
				this.textBox1.Text = this.folderBrowserDialog1.SelectedPath;
			}
		}

		private void button2_Click(object sender, EventArgs e) {
			if (this.folderBrowserDialog1.ShowDialog() == DialogResult.OK) {
				this.textBox2.Text = this.folderBrowserDialog1.SelectedPath;
			}
		}

		private void button3_Click(object sender, EventArgs e) {
			if (this.folderBrowserDialog1.ShowDialog() == DialogResult.OK) {
				this.textBox3.Text = this.folderBrowserDialog1.SelectedPath;

			}
		}

		private void button4_Click(object sender, EventArgs e) {
			if (string.IsNullOrEmpty(this.textBox1.Text)) {
				MessageBox.Show("Source path required.");
				return;
			}

			if (string.IsNullOrEmpty(this.textBox2.Text)) {
				MessageBox.Show("Destination path required.");
				return;
			}

			var backupDirectory = (string.IsNullOrEmpty(this.textBox3.Text)) ? this.textBox3.Text : string.Empty;

			var settings = new Settings(
				this.textBox1.Text,
				this.textBox2.Text,
				true,
				backupDirectory,
				this.checkBox1.Checked);

			var p = new Program(settings);
			p.Run();
		}
	}
}
