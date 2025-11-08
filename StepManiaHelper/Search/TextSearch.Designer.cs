namespace StepManiaHelper.Search
{
    partial class TextSearch
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            cbxProperty = new System.Windows.Forms.ComboBox();
            txtValue = new System.Windows.Forms.TextBox();
            label1 = new System.Windows.Forms.Label();
            tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            btnDelete = new System.Windows.Forms.Button();
            tableLayoutPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // cbxProperty
            // 
            cbxProperty.Dock = System.Windows.Forms.DockStyle.Fill;
            cbxProperty.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cbxProperty.FormattingEnabled = true;
            cbxProperty.Location = new System.Drawing.Point(0, 0);
            cbxProperty.Margin = new System.Windows.Forms.Padding(0);
            cbxProperty.Name = "cbxProperty";
            cbxProperty.Size = new System.Drawing.Size(134, 23);
            cbxProperty.TabIndex = 0;
            // 
            // txtValue
            // 
            txtValue.Dock = System.Windows.Forms.DockStyle.Fill;
            txtValue.Location = new System.Drawing.Point(251, 0);
            txtValue.Margin = new System.Windows.Forms.Padding(0, 0, 4, 0);
            txtValue.Name = "txtValue";
            txtValue.Size = new System.Drawing.Size(130, 23);
            txtValue.TabIndex = 1;
            txtValue.Validated += textBox1_Validated;
            // 
            // label1
            // 
            label1.Anchor = System.Windows.Forms.AnchorStyles.None;
            label1.Location = new System.Drawing.Point(134, 4);
            label1.Margin = new System.Windows.Forms.Padding(0);
            label1.MaximumSize = new System.Drawing.Size(0, 17);
            label1.MinimumSize = new System.Drawing.Size(117, 0);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(117, 17);
            label1.TabIndex = 2;
            label1.Text = "Regex Text Match:";
            label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.AutoSize = true;
            tableLayoutPanel1.ColumnCount = 4;
            tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanel1.Controls.Add(cbxProperty, 0, 0);
            tableLayoutPanel1.Controls.Add(txtValue, 2, 0);
            tableLayoutPanel1.Controls.Add(label1, 1, 0);
            tableLayoutPanel1.Controls.Add(btnDelete, 3, 0);
            tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            tableLayoutPanel1.MaximumSize = new System.Drawing.Size(0, 25);
            tableLayoutPanel1.MinimumSize = new System.Drawing.Size(442, 25);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 1;
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanel1.Size = new System.Drawing.Size(442, 25);
            tableLayoutPanel1.TabIndex = 3;
            tableLayoutPanel1.Paint += tableLayoutPanel1_Paint;
            // 
            // btnDelete
            // 
            btnDelete.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            btnDelete.Location = new System.Drawing.Point(385, 0);
            btnDelete.Margin = new System.Windows.Forms.Padding(0);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new System.Drawing.Size(56, 24);
            btnDelete.TabIndex = 3;
            btnDelete.Text = "Delete";
            btnDelete.UseVisualStyleBackColor = true;
            btnDelete.Click += btnDelete_Click;
            // 
            // TextSearch
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            AutoSize = false;
            Controls.Add(tableLayoutPanel1);
            Margin = new System.Windows.Forms.Padding(0);
            MaximumSize = new System.Drawing.Size(0, 24);
            MinimumSize = new System.Drawing.Size(442, 24);
            Name = "TextSearch";
            Size = new System.Drawing.Size(442, 24);
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.ComboBox cbxProperty;
        private System.Windows.Forms.TextBox txtValue;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button btnDelete;
    }
}
