namespace baitaplonquanlycuahangbanquanao
{
    partial class f_dangnhap
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
            this.label_dangnhap = new System.Windows.Forms.Label();
            this.label_matkhau = new System.Windows.Forms.Label();
            this.txbtendangnhap = new System.Windows.Forms.TextBox();
            this.txbmatkhau = new System.Windows.Forms.TextBox();
            this.button_dangnhap = new System.Windows.Forms.Button();
            this.checkhienthimk = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // label_dangnhap
            // 
            this.label_dangnhap.AutoSize = true;
            this.label_dangnhap.Location = new System.Drawing.Point(110, 127);
            this.label_dangnhap.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label_dangnhap.Name = "label_dangnhap";
            this.label_dangnhap.Size = new System.Drawing.Size(107, 25);
            this.label_dangnhap.TabIndex = 0;
            this.label_dangnhap.Text = "Tài khoản";
            // 
            // label_matkhau
            // 
            this.label_matkhau.AutoSize = true;
            this.label_matkhau.Location = new System.Drawing.Point(110, 202);
            this.label_matkhau.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label_matkhau.Name = "label_matkhau";
            this.label_matkhau.Size = new System.Drawing.Size(101, 25);
            this.label_matkhau.TabIndex = 1;
            this.label_matkhau.Text = "Mật khẩu";
            // 
            // txbtendangnhap
            // 
            this.txbtendangnhap.Location = new System.Drawing.Point(260, 127);
            this.txbtendangnhap.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.txbtendangnhap.Name = "txbtendangnhap";
            this.txbtendangnhap.Size = new System.Drawing.Size(196, 31);
            this.txbtendangnhap.TabIndex = 2;
            // 
            // txbmatkhau
            // 
            this.txbmatkhau.Location = new System.Drawing.Point(260, 202);
            this.txbmatkhau.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.txbmatkhau.Name = "txbmatkhau";
            this.txbmatkhau.Size = new System.Drawing.Size(196, 31);
            this.txbmatkhau.TabIndex = 3;
            // 
            // button_dangnhap
            // 
            this.button_dangnhap.Location = new System.Drawing.Point(200, 311);
            this.button_dangnhap.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.button_dangnhap.Name = "button_dangnhap";
            this.button_dangnhap.Size = new System.Drawing.Size(168, 67);
            this.button_dangnhap.TabIndex = 4;
            this.button_dangnhap.Text = "Đăng nhập";
            this.button_dangnhap.UseVisualStyleBackColor = true;
            this.button_dangnhap.Click += new System.EventHandler(this.button_dangnhap_Click);
            // 
            // checkhienthimk
            // 
            this.checkhienthimk.AutoSize = true;
            this.checkhienthimk.Location = new System.Drawing.Point(260, 262);
            this.checkhienthimk.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.checkhienthimk.Name = "checkhienthimk";
            this.checkhienthimk.Size = new System.Drawing.Size(198, 29);
            this.checkhienthimk.TabIndex = 5;
            this.checkhienthimk.Text = "Hiển thị mật khẩu";
            this.checkhienthimk.UseVisualStyleBackColor = true;
            this.checkhienthimk.CheckedChanged += new System.EventHandler(this.checkhienthimk_CheckedChanged);
            // 
            // f_dangnhap
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(681, 500);
            this.Controls.Add(this.checkhienthimk);
            this.Controls.Add(this.button_dangnhap);
            this.Controls.Add(this.txbmatkhau);
            this.Controls.Add(this.txbtendangnhap);
            this.Controls.Add(this.label_matkhau);
            this.Controls.Add(this.label_dangnhap);
            this.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.Name = "f_dangnhap";
            this.Text = "Đăng nhập";
            this.Load += new System.EventHandler(this.f_dangnhap_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label_dangnhap;
        private System.Windows.Forms.Label label_matkhau;
        private System.Windows.Forms.TextBox txbtendangnhap;
        private System.Windows.Forms.TextBox txbmatkhau;
        private System.Windows.Forms.Button button_dangnhap;
        private System.Windows.Forms.CheckBox checkhienthimk;
    }
}