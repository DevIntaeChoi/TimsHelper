namespace TimsHelper.SubForms
{
    partial class SQLView
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
            this.cboQuery = new System.Windows.Forms.ComboBox();
            this.btnInquiry = new System.Windows.Forms.Button();
            this.btnEditQuery = new System.Windows.Forms.Button();
            this.btnAddQuery = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnExcel = new System.Windows.Forms.Button();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.AutoScroll = true;
            this.panel1.Location = new System.Drawing.Point(12, 44);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1115, 689);
            this.panel1.TabIndex = 1;
            // 
            // cboQuery
            // 
            this.cboQuery.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboQuery.FormattingEnabled = true;
            this.cboQuery.Location = new System.Drawing.Point(12, 12);
            this.cboQuery.Name = "cboQuery";
            this.cboQuery.Size = new System.Drawing.Size(390, 20);
            this.cboQuery.TabIndex = 0;
            // 
            // btnInquiry
            // 
            this.btnInquiry.Location = new System.Drawing.Point(653, 9);
            this.btnInquiry.Name = "btnInquiry";
            this.btnInquiry.Size = new System.Drawing.Size(75, 23);
            this.btnInquiry.TabIndex = 2;
            this.btnInquiry.Text = "조회";
            this.btnInquiry.UseVisualStyleBackColor = true;
            this.btnInquiry.Click += new System.EventHandler(this.btnInquiry_Click);
            // 
            // btnEditQuery
            // 
            this.btnEditQuery.Location = new System.Drawing.Point(408, 10);
            this.btnEditQuery.Name = "btnEditQuery";
            this.btnEditQuery.Size = new System.Drawing.Size(75, 23);
            this.btnEditQuery.TabIndex = 3;
            this.btnEditQuery.Text = "쿼리편집";
            this.btnEditQuery.UseVisualStyleBackColor = true;
            this.btnEditQuery.Click += new System.EventHandler(this.btnEditQuery_Click);
            // 
            // btnAddQuery
            // 
            this.btnAddQuery.Location = new System.Drawing.Point(489, 10);
            this.btnAddQuery.Name = "btnAddQuery";
            this.btnAddQuery.Size = new System.Drawing.Size(75, 23);
            this.btnAddQuery.TabIndex = 4;
            this.btnAddQuery.Text = "쿼리추가";
            this.btnAddQuery.UseVisualStyleBackColor = true;
            this.btnAddQuery.Click += new System.EventHandler(this.btnAddQuery_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Location = new System.Drawing.Point(568, 10);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(75, 23);
            this.btnRefresh.TabIndex = 5;
            this.btnRefresh.Text = "새로고침";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // btnExcel
            // 
            this.btnExcel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExcel.Location = new System.Drawing.Point(1052, 9);
            this.btnExcel.Name = "btnExcel";
            this.btnExcel.Size = new System.Drawing.Size(75, 23);
            this.btnExcel.TabIndex = 2;
            this.btnExcel.Text = "엑셀저장";
            this.btnExcel.UseVisualStyleBackColor = true;
            this.btnExcel.Click += new System.EventHandler(this.btnExcel_Click);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(751, 12);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(178, 16);
            this.checkBox1.TabIndex = 6;
            this.checkBox1.Text = "그리드안보이게(엑셀저장용)";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // SQLView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1139, 784);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.btnAddQuery);
            this.Controls.Add(this.btnEditQuery);
            this.Controls.Add(this.btnExcel);
            this.Controls.Add(this.btnInquiry);
            this.Controls.Add(this.cboQuery);
            this.Controls.Add(this.panel1);
            this.Name = "SQLView";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "SQLView";
            this.Load += new System.EventHandler(this.SQLView_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ComboBox cboQuery;
        private System.Windows.Forms.Button btnInquiry;
        private System.Windows.Forms.Button btnEditQuery;
        private System.Windows.Forms.Button btnAddQuery;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnExcel;
        private System.Windows.Forms.CheckBox checkBox1;
    }
}