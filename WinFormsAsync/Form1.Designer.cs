namespace WinFormsAsync
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.button1 = new Button();
            this.lstCustomers = new ListBox();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new Point(39, 48);
            this.button1.Margin = new Padding(5, 6, 5, 6);
            this.button1.Name = "button1";
            this.button1.Size = new Size(369, 82);
            this.button1.TabIndex = 0;
            this.button1.Text = "Customers";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += this.button1_Click;
            // 
            // lstCustomers
            // 
            this.lstCustomers.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            this.lstCustomers.FormattingEnabled = true;
            this.lstCustomers.ItemHeight = 30;
            this.lstCustomers.Location = new Point(558, 48);
            this.lstCustomers.Margin = new Padding(5, 6, 5, 6);
            this.lstCustomers.Name = "lstCustomers";
            this.lstCustomers.Size = new Size(539, 574);
            this.lstCustomers.TabIndex = 1;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new SizeF(12F, 30F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(1120, 676);
            this.Controls.Add(this.lstCustomers);
            this.Controls.Add(this.button1);
            this.Font = new Font("Segoe UI", 15.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.Margin = new Padding(5, 6, 5, 6);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
        }

        #endregion

        private Button button1;
        private ListBox lstCustomers;
    }
}
