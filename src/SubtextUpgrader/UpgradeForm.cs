using System;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using Ionic.Zip;

namespace SubtextUpgrader
{
    public partial class UpgradeForm : Form
    {
        public UpgradeForm()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.SelectedPath = Destination.Text;
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                Destination.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                Backup.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            button4.Enabled = false;
            backgroundWorker1.RunWorkerAsync();
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            if (string.IsNullOrEmpty(Destination.Text))
            {
                MessageBox.Show("Destination path required.");
                return;
            }

            CreateSourceFromResource();

            var backupDirectory = (string.IsNullOrEmpty(Backup.Text)) ? CreateQuietBackupDirectory() : Backup.Text;

            var settings = new Settings(
                Path.Combine(TempSourceDirectory(), "Subtext.Web"),
                Destination.Text,
                true,
                backupDirectory,
                Verbose.Checked);

            try
            {
                //with 2 d's for a double dose o' this pimpin!
                var upgrayedd = new Upgrader(settings, backgroundWorker1);
                upgrayedd.Run();
            }
            catch (Exception ex)
            {
                backgroundWorker1.CancelAsync();
                string message = "Oops! " +
                    Environment.NewLine +
                    ex.Message;

                backgroundWorker1.ReportProgress(100, message);
            }
            finally
            {
                ClearTempSourceDirectory();
            }
        }

        private static string CreateQuietBackupDirectory()
        {
            string tempPath = Path.Combine(Path.GetTempPath(), "SubText-backup");
            Directory.CreateDirectory(tempPath);

            return tempPath;
        }

        void CreateSourceFromResource()
        {
            //* create temporary file
            //* stream embedded zipfile to temporary file
            //* create temporary folder
            //* unzip to temp folder
            //* use result as source directory.
            //* run upgrader
            string fileName = Path.GetTempFileName() + ".zip";
            string resourceName = GetType().Namespace + ".Resources.SubText.zip";
            using (var resx = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName))
            {
                if (resx == null)
                {
                    throw new InvalidOperationException(String.Format("Resource '{0}' does not exist", resourceName));
                }
                using (var fs = File.Create(fileName))
                {
                    const int offset = 0;
                    const int size = 4096;
                    int count;
                    var buffer = new byte[size];
                    while ((count = resx.Read(buffer, offset, size)) > 0)
                    {
                        fs.Write(buffer, offset, count);
                    }
                    fs.Close();
                    resx.Close();
                }
            }

            string extractDirectory = TempSourceDirectory();
            if (Directory.Exists(extractDirectory))
            {
                backgroundWorker1.ReportProgress(3, String.Format("Deleting to '{0}'", extractDirectory));
                ClearTempSourceDirectory();
            }

            using (ZipFile zip = ZipFile.Read(fileName))
            {
                string message = String.Format("Extracting to '{0}'", extractDirectory);
                backgroundWorker1.ReportProgress(5, message);
                foreach (var zipEntry in zip)
                {
                    zipEntry.Extract(extractDirectory, ExtractExistingFileAction.OverwriteSilently);
                }
            }
        }

        static string TempSourceDirectory()
        {
            string extractDirectory = Path.Combine(Path.GetTempPath(), "SubText");
            return extractDirectory;
        }

        private void ClearTempSourceDirectory()
        {
            string extractDirectory = TempSourceDirectory();
            if (Directory.Exists(extractDirectory))
            {
                Directory.Delete(extractDirectory, true);
            }
        }

        void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
            Message.Text += e.UserState + Environment.NewLine;
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            backgroundWorker1.CancelAsync();
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                Message.Text += "Cancelled!";
            }
            button4.Enabled = true;
        }

        private void UpgradeForm_Load(object sender, EventArgs e)
        {
            Destination.Text = Environment.CurrentDirectory;
        }
    }
}
