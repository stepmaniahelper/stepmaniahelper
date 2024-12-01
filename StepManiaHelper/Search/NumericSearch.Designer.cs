namespace StepManiaHelper.Search
{
    partial class NumericSearch
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.cbxProperty = new System.Windows.Forms.ComboBox();
            this.nudValue = new System.Windows.Forms.NumericUpDown();
            this.cbxOperation = new System.Windows.Forms.ComboBox();
            this.btnDelete = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudValue)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.cbxProperty, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.nudValue, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.cbxOperation, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnDelete, 3, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.MaximumSize = new System.Drawing.Size(0, 22);
            this.tableLayoutPanel1.MinimumSize = new System.Drawing.Size(0, 22);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(379, 22);
            this.tableLayoutPanel1.TabIndex = 3;
            // 
            // cbxProperty
            // 
            this.cbxProperty.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbxProperty.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxProperty.FormattingEnabled = true;
            this.cbxProperty.Location = new System.Drawing.Point(0, 0);
            this.cbxProperty.Margin = new System.Windows.Forms.Padding(0);
            this.cbxProperty.Name = "cbxProperty";
            this.cbxProperty.Size = new System.Drawing.Size(115, 21);
            this.cbxProperty.TabIndex = 0;
            // 
            // nudValue
            // 
            this.nudValue.DecimalPlaces = 1;
            this.nudValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nudValue.Location = new System.Drawing.Point(215, 0);
            this.nudValue.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.nudValue.Maximum = new decimal(new int[] {
            999999,
            0,
            0,
            0});
            this.nudValue.Minimum = new decimal(new int[] {
            999999,
            0,
            0,
            -2147483648});
            this.nudValue.Name = "nudValue";
            this.nudValue.Size = new System.Drawing.Size(112, 20);
            this.nudValue.TabIndex = 2;
            this.nudValue.ValueChanged += new System.EventHandler(this.nudValue_ValueChanged);
            // 
            // cbxOperation
            // 
            this.cbxOperation.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.cbxOperation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxOperation.FormattingEnabled = true;
            this.cbxOperation.Location = new System.Drawing.Point(115, 0);
            this.cbxOperation.Margin = new System.Windows.Forms.Padding(0);
            this.cbxOperation.Name = "cbxOperation";
            this.cbxOperation.Size = new System.Drawing.Size(100, 21);
            this.cbxOperation.TabIndex = 1;
            // 
            // btnDelete
            // 
            this.btnDelete.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnDelete.Location = new System.Drawing.Point(330, 0);
            this.btnDelete.Margin = new System.Windows.Forms.Padding(0);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(48, 21);
            this.btnDelete.TabIndex = 3;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // NumericSearch
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = false;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.MaximumSize = new System.Drawing.Size(0, 21);
            this.MinimumSize = new System.Drawing.Size(379, 21);
            this.Name = "NumericSearch";
            this.Size = new System.Drawing.Size(379, 21);
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.nudValue)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cbxProperty;
        private System.Windows.Forms.ComboBox cbxOperation;
        private System.Windows.Forms.NumericUpDown nudValue;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button btnDelete;
    }
}
