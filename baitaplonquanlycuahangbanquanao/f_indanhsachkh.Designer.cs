
namespace baitaplonquanlycuahangbanquanao
{
    partial class f_indanhsachkh
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
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox_NamDatHang = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox_thangdathang = new System.Windows.Forms.TextBox();
            this.button_indskh = new System.Windows.Forms.Button();
            this.crystalReportViewer_dsKhachHang = new CrystalDecisions.Windows.Forms.CrystalReportViewer();
            this.SuspendLayout();
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(407, 60);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(64, 25);
            this.label3.TabIndex = 21;
            this.label3.Text = "Năm :";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(179, 60);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(80, 25);
            this.label2.TabIndex = 20;
            this.label2.Text = "Tháng :";
            // 
            // textBox_NamDatHang
            // 
            this.textBox_NamDatHang.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_NamDatHang.Location = new System.Drawing.Point(477, 57);
            this.textBox_NamDatHang.Name = "textBox_NamDatHang";
            this.textBox_NamDatHang.Size = new System.Drawing.Size(113, 30);
            this.textBox_NamDatHang.TabIndex = 19;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(179, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(337, 25);
            this.label1.TabIndex = 17;
            this.label1.Text = "Nhập ngày đặt hàng của khách hàng ";
            // 
            // textBox_thangdathang
            // 
            this.textBox_thangdathang.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_thangdathang.Location = new System.Drawing.Point(265, 57);
            this.textBox_thangdathang.Name = "textBox_thangdathang";
            this.textBox_thangdathang.Size = new System.Drawing.Size(90, 30);
            this.textBox_thangdathang.TabIndex = 16;
            // 
            // button_indskh
            // 
            this.button_indskh.Location = new System.Drawing.Point(644, 57);
            this.button_indskh.Name = "button_indskh";
            this.button_indskh.Size = new System.Drawing.Size(125, 30);
            this.button_indskh.TabIndex = 15;
            this.button_indskh.Text = "IN";
            this.button_indskh.UseVisualStyleBackColor = true;
            this.button_indskh.Click += new System.EventHandler(this.button_indskh_Click);
            // 
            // crystalReportViewer_dsKhachHang
            // 
            this.crystalReportViewer_dsKhachHang.ActiveViewIndex = -1;
            this.crystalReportViewer_dsKhachHang.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.crystalReportViewer_dsKhachHang.Cursor = System.Windows.Forms.Cursors.Default;
            this.crystalReportViewer_dsKhachHang.Location = new System.Drawing.Point(4, 111);
            this.crystalReportViewer_dsKhachHang.Name = "crystalReportViewer_dsKhachHang";
            this.crystalReportViewer_dsKhachHang.Size = new System.Drawing.Size(1351, 490);
            this.crystalReportViewer_dsKhachHang.TabIndex = 14;
            // 
            // f_indanhsachkh
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1361, 605);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBox_NamDatHang);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox_thangdathang);
            this.Controls.Add(this.button_indskh);
            this.Controls.Add(this.crystalReportViewer_dsKhachHang);
            this.Name = "f_indanhsachkh";
            this.Text = "In danh sách khách hàng";
            this.Load += new System.EventHandler(this.f_indanhsachkh_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox_NamDatHang;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox_thangdathang;
        private System.Windows.Forms.Button button_indskh;
        private CrystalDecisions.Windows.Forms.CrystalReportViewer crystalReportViewer_dsKhachHang;
    }
}