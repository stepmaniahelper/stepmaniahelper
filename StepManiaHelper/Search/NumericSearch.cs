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
    public partial class NumericSearch : SearchUserControl
    {
        public const string EQUALS = "==";
        public const string NOTEQUALS = "!=";
        public const string LESSTHAN = "<";
        public const string LESSTHANEQUALS = "<=";
        public const string GREATERTHAN = ">";
        public const string GREATERTHANEQUALS = ">=";

        public static List<string> lstOpcodes = new List<string>
        {
            EQUALS,
            NOTEQUALS,
            LESSTHAN,
            LESSTHANEQUALS,
            GREATERTHAN,
            GREATERTHANEQUALS
        };

        public NumericSearch() : this(null, null, null)
        {

        }

        public NumericSearch(TableLayoutPanel Parent, CSavedSearch Search, CSearchOperand Operand) : base(Parent, Search, Operand)
        {
            InitializeComponent();

            Property = cbxProperty;
            Value = nudValue;
            cbxProperty.DataSource = PropertyNames;
            cbxOperation.DataSource = lstOpcodes;
            cbxOperation.SelectedItem = Operand.OpCode;
            cbxOperation.SelectedIndexChanged += cbxOperation_SelectedIndexChanged;

            Value.Text = Operand.Value;
            cbxProperty.SelectedItem = Operand.Property;
            cbxProperty.SelectedIndexChanged += cbxProperty_SelectedIndexChanged;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            DeleteControl();
        }

        private void cbxProperty_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckReplaceControl(cbxProperty.Text, this);
        }

        private void cbxOperation_SelectedIndexChanged(object sender, EventArgs e)
        {
            Operand.OpCode = cbxOperation.SelectedItem as string;
        }

        private void nudValue_ValueChanged(object sender, EventArgs e)
        {
            Operand.Value = (sender as NumericUpDown).Value.ToString();
        }
    }
}
