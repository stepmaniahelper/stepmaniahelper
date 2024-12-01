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
    public partial class SearchUserControl : UserControl
    {
        public static List<string> PropertyNames;

        static SearchUserControl()
        {
            PropertyNames = typeof(CSong).GetProperties().Where(x => x.PropertyType != typeof(Image))?.Select(x => x.Name).ToList();
        }

        public TableLayoutPanel Table;
        public ComboBox Property;
        public Control Value;
        public CSavedSearch Search;
        public CSearchOperand Operand;

        public SearchUserControl() : this(null, null, null)
        {

        }

        public SearchUserControl(TableLayoutPanel Parent, CSavedSearch Search, CSearchOperand Operand) : base()
        {
            InitializeComponent();

            this.Table = Parent;
            this.Search = Search;
            this.Operand = Operand;
        }

        public void DeleteControl()
        {
            int Row = Table.GetRow(this);

            // Move the contents of all rows after this one down a row
            for (int index = Row + 1; index < Table.RowCount; index++)
            {
                // Find the control at the current row
                Control control = Table.Controls.OfType<Control>().FirstOrDefault(x => Table.GetRow(x) == index);

                Table.SetRow(control, index - 1);
            }

            // Remove the control from the table
            Table.Controls.Remove(this);

            // Delete the last row
            Table.RowCount -= 1;

            // Resize the table to fit it's contents
            Table.Height -= this.Height;

            // Resize the group to fit its contents
            Table.Parent.Height -= this.Height;

            // Remove the operand from the list of the search
            Search.Operands.Remove(Operand);
        }

        static public SearchUserControl GetControlOfProperType(TableLayoutPanel parent, CSavedSearch search, CSearchOperand operand)
        {
            // Get the type of the selected property
            Type PropertyType = null;
            if (operand.Property != null)
            {
                PropertyType = typeof(CSong).GetProperty(operand.Property)?.PropertyType;
            }

            SearchUserControl NewControl = null;

            // If the type is a string, the new control type is a TestSearch
            if (PropertyType == typeof(string))
            {
                NewControl = new TextSearch(parent, search, operand);
            }
            // If the type is a numeric, the new control type is a NumericSearch
            else if ((PropertyType == typeof(int?))
            || (PropertyType == typeof(double?)))
            {
                NewControl = new NumericSearch(parent, search, operand);
            }
            // The default control type is the text search
            else
            {
                NewControl = new TextSearch(parent, search, operand);
            }

            return NewControl;
        }

        public void CheckReplaceControl(string SelectedProperty, Control CurrentControl)
        {
            // Update the operands property name
            Operand.Property = SelectedProperty;

            // Only perform the below logic if this control is already part of the table
            if (Table != null)
            { 
                // Get a control of the proper type based on the property name
                SearchUserControl NewControl = GetControlOfProperType(Table, Search, Operand);

                // If the current control type isn't the same as the new control type, 
                // we have to replace the current control with the new one
                if (CurrentControl.GetType() != NewControl.GetType())
                {
                    // Match the selected property and width
                    NewControl.Property.SelectedItem = SelectedProperty;
                    NewControl.Width = CurrentControl.Width;

                    // Get the index of the current control in the table
                    int Row = Table.GetRow(CurrentControl);

                    // Remove the current control from the table and place the new on in the same row
                    Table.Controls.Remove(CurrentControl);
                    Table.Controls.Add(NewControl, 0, Row);

                    // Let the control know it's part of the table
                    NewControl.Table = Table;

                    // Let the control know the operand it repesents
                    NewControl.Operand = Operand;

                    // Clear the opcode and value of the opcode
                    Operand.OpCode = null;
                    Operand.Value = null;
                }
            }
        }
    }
}
