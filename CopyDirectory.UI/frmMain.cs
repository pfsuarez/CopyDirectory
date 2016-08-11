using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CopyDirectory.BL;

namespace CopyDirectory.UI
{
    public partial class frmMain : Form
    {
        IO_Functions ioFunctions;

        public frmMain()
        {
            InitializeComponent();
            ioFunctions = new IO_Functions();
            Configure();
        }

        private void Configure()
        {
            btnPickSource.Click += BtnPick_Click;
            btnPickTarget.Click += BtnPick_Click;
            btnProcess.Click += BtnProcess_Click;

            ioFunctions.SuccesfullCopy += IoFunctions_SuccesfullCopy;
        }

        private void IoFunctions_SuccesfullCopy(object sender, IOFunctionsEventArgs e)
        {
            lbxResult.Items.Add(string.Format("New file copied: {0}", e.FileNameCreated));
            Application.DoEvents();
        }

        private void BtnProcess_Click(object sender, EventArgs e)
        {
            lbxResult.Items.Clear();

            try
            {
                ioFunctions.Copy(txtSource.Text, txtTarget.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Copy Directory Application");
            }
        }

        private void BtnPick_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            SelectFolder(btn.Tag.ToString());
        }

        private void SelectFolder(string destination)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            DialogResult result = fbd.ShowDialog();

            if (!string.IsNullOrWhiteSpace(fbd.SelectedPath))
            {
                if (destination.Equals("Source"))
                     txtSource.Text = fbd.SelectedPath;
                else
                     txtTarget.Text = fbd.SelectedPath;
            }
        }
    }
}
