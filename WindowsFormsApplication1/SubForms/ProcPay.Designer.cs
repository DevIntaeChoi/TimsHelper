namespace TimsHelper
{
    partial class ProcPay
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.txtBDateSeq = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnProcPay = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.AutoScroll = true;
            this.panel1.Location = new System.Drawing.Point(12, 40);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1012, 669);
            this.panel1.TabIndex = 0;
            // 
            // txtBDateSeq
            // 
            this.txtBDateSeq.Location = new System.Drawing.Point(116, 11);
            this.txtBDateSeq.Name = "txtBDateSeq";
            this.txtBDateSeq.Size = new System.Drawing.Size(163, 21);
            this.txtBDateSeq.TabIndex = 1;
            this.txtBDateSeq.Text = "20190115\t00002";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(97, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "BDate BDateSeq";
            // 
            // btnProcPay
            // 
            this.btnProcPay.Location = new System.Drawing.Point(288, 9);
            this.btnProcPay.Name = "btnProcPay";
            this.btnProcPay.Size = new System.Drawing.Size(75, 23);
            this.btnProcPay.TabIndex = 3;
            this.btnProcPay.Text = "ProcPay";
            this.btnProcPay.UseVisualStyleBackColor = true;
            this.btnProcPay.Click += new System.EventHandler(this.btnProcPay_Click);
            // 
            // ProcPay
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1036, 721);
            this.Controls.Add(this.btnProcPay);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtBDateSeq);
            this.Controls.Add(this.panel1);
            this.Name = "ProcPay";
            this.Text = "ProcPay";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.ProcPay_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox txtBDateSeq;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnProcPay;
    }
}