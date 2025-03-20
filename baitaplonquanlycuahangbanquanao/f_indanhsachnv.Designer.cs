
namespace baitaplonquanlycuahangbanquanao
{
    partial class f_indanhsachnv
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
            this.label1 = new System.Windows.Forms.Label();
            this.textBox_NamVaoLam = new System.Windows.Forms.TextBox();
            this.button_indsnv = new System.Windows.Forms.Button();
            this.crystalReportViewer_dsNhanVien = new CrystalDecisions.Windows.Forms.CrystalReportViewer();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(220, 34);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(313, 25);
            this.label1.TabIndex = 9;
            this.label1.Text = "Nhập năm vào làm của nhân viên :";
            // 
            // textBox_NamVaoLam
            // 
            this.textBox_NamVaoLam.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_NamVaoLam.Location = new System.Drawing.Point(539, 29);
            this.textBox_NamVaoLam.Name = "textBox_NamVaoLam";
            this.textBox_NamVaoLam.Size = new System.Drawing.Size(174, 30);
            this.textBox_NamVaoLam.TabIndex = 8;
            // 
            // button_indsnv
            // 
            this.button_indsnv.Location = new System.Drawing.Point(725, 29);
            this.button_indsnv.Name = "button_indsnv";
            this.button_indsnv.Size = new System.Drawing.Size(118, 30);
            this.button_indsnv.TabIndex = 7;
            this.button_indsnv.Text = "IN";
            this.button_indsnv.UseVisualStyleBackColor = true;
            this.button_indsnv.Click += new System.EventHandler(this.button_indsnv_Click);
            // 
            // crystalReportViewer_dsNhanVien
            // 
            this.crystalReportViewer_dsNhanVien.ActiveViewIndex = -1;
            this.crystalReportViewer_dsNhanVien.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.crystalReportViewer_dsNhanVien.Cursor = System.Windows.Forms.Cursors.Default;
            this.crystalReportViewer_dsNhanVien.Location = new System.Drawing.Point(8, 72);
            this.crystalReportViewer_dsNhanVien.Name = "crystalReportViewer_dsNhanVien";
            this.crystalReportViewer_dsNhanVien.Size = new System.Drawing.Size(1333, 542);
            this.crystalReportViewer_dsNhanVien.TabIndex = 6;
            // 
            // f_indanhsachnv
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1354, 640);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox_NamVaoLam);
            this.Controls.Add(this.button_indsnv);
            this.Controls.Add(this.crystalReportViewer_dsNhanVien);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "f_indanhsachnv";
            this.Text = "In Danh Sách Nhân Viên";
            this.Load += new System.EventHandler(this.f_indanhsachnv_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox_NamVaoLam;
        private System.Windows.Forms.Button button_indsnv;
        private CrystalDecisions.Windows.Forms.CrystalReportViewer crystalReportViewer_dsNhanVien;
    }
}