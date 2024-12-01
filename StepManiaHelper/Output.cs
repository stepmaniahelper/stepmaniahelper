using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace StepManiaHelper
{
    public partial class Output : Form
    {
        public Options OptionsForm;

        public Output(Options OptionsForm)
        {
            InitializeComponent();
            this.OptionsForm = OptionsForm;
        }

        public void UpdateTitleAndText(string strText)
        {
            this.ChangeFormTitle(strText);
            this.AddText("\n\n\n" + strText + "\n");
        }

        public void AddText(string strText)
        {
            try
            {
                this.Invoke((MethodInvoker)delegate
                {
                    this.richTextBox1.AppendText(strText);
                });
            }
            catch (Exception e)
            {
                
            }
        }

        public void ChangeFormTitle(string strText)
        {
            try
            {
                this.Invoke((MethodInvoker)delegate
                {
                    this.Text = strText;
                });
            }
            catch (Exception e)
            {
                
            }
        }

        public void SetError(string strExceptionMessage)
        {
            ChangeFormTitle("ERROR :(");
            AddText("\nERROR: " + strExceptionMessage + "\n");
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.OptionsForm.Close();
        }

        private void btnRetry_Click(object sender, EventArgs e)
        {
            this.OptionsForm.Retry();
        }
    }
}
