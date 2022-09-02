using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using StepManiaHelper.Helpers;

namespace StepManiaHelper.Search
{
    public partial class TextSearch : SearchUserControl
    {
        public TextSearch() : this(null, null, null)
        {

        }

        public TextSearch(TableLayoutPanel Parent, CSavedSearch Search, CSearchOperand Operand) : base(Parent, Search, Operand)
        {
            InitializeComponent();

            Property = cbxProperty;
            Value = txtValue;
            cbxProperty.DataSource = PropertyNames;

            txtValue.Text = Operand.Value;
            cbxProperty.SelectedItem = Operand.Property;
            cbxProperty.SelectedIndexChanged += cbxProperty_SelectedIndexChanged;
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            DeleteControl();
        }

        private void cbxProperty_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckReplaceControl(cbxProperty.Text, this);
        }

        private void textBox1_Validated(object sender, EventArgs e)
        {
            Operand.Value = (sender as TextBox).Text;
        }
    }
}
