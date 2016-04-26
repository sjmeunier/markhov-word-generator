using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using MarkovChains;

namespace WordGenerator
{
    public partial class Form1 : Form
    {
        private bool LoadedData;

        public Form1()
        {
            InitializeComponent();
            LoadedData = false;
        }
        WordGenPhrase oWordGen = new WordGenPhrase();
        private void butLoad_Click(object sender, EventArgs e)
        {

        }

        private void butGenerate_Click(object sender, EventArgs e)
        {
            lblWord.Text = oWordGen.GenerateWord();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void loadDataFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openfileDialog1 = new OpenFileDialog();
            openfileDialog1.Filter = "Text files|*.txt|All files|*.*";
            openfileDialog1.ShowDialog();
            bool RetValue = false;
            if (openfileDialog1.FileName != "")
            {
                if (File.Exists(openfileDialog1.FileName))
                {
                    RetValue = oWordGen.LoadFromFile(openfileDialog1.FileName);
                    if (RetValue == false)
                    {
                        MessageBox.Show("Unable to load data from file", "Error", MessageBoxButtons.OK);
                        butGenerate.Enabled = false;
                    }
                    else
                    {
                        butGenerate.Enabled = true;
                        LoadedData = true;
                    }
                }
                else
                {
                    MessageBox.Show("File does not exist", "Error", MessageBoxButtons.OK);
                    butGenerate.Enabled = false;
                }
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox aboutBox = new AboutBox();
            aboutBox.ShowDialog();
        }
    }
}