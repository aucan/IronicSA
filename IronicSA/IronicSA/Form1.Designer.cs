namespace IronicSA
{
    partial class Form1
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnRegression = new System.Windows.Forms.Button();
            this.chClassification = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // btnRegression
            // 
            this.btnRegression.Location = new System.Drawing.Point(33, 61);
            this.btnRegression.Name = "btnRegression";
            this.btnRegression.Size = new System.Drawing.Size(125, 47);
            this.btnRegression.TabIndex = 0;
            this.btnRegression.Text = "Regression";
            this.btnRegression.UseVisualStyleBackColor = true;
            this.btnRegression.Click += new System.EventHandler(this.btnRegression_Click);
            // 
            // chClassification
            // 
            this.chClassification.AutoSize = true;
            this.chClassification.Location = new System.Drawing.Point(33, 12);
            this.chClassification.Name = "chClassification";
            this.chClassification.Size = new System.Drawing.Size(112, 21);
            this.chClassification.TabIndex = 1;
            this.chClassification.Text = "Classification";
            this.chClassification.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(282, 253);
            this.Controls.Add(this.chClassification);
            this.Controls.Add(this.btnRegression);
            this.Name = "Form1";
            this.Text = "IronicSA";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnRegression;
        private System.Windows.Forms.CheckBox chClassification;
    }
}

