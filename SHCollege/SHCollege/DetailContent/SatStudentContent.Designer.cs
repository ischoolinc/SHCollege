namespace SHCollege.DetailContent
{
    partial class SatStudentContent
    {
        /// <summary> 
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置 Managed 資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 元件設計工具產生的程式碼

        /// <summary> 
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器
        /// 修改這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.txtSATSerNo = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX3 = new DevComponents.DotNetBar.LabelX();
            this.txtSatClassName = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.txtSatSeatNo = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX2 = new DevComponents.DotNetBar.LabelX();
            this.SuspendLayout();
            // 
            // txtSATSerNo
            // 
            // 
            // 
            // 
            this.txtSATSerNo.Border.Class = "TextBoxBorder";
            this.txtSATSerNo.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.txtSATSerNo.Location = new System.Drawing.Point(90, 23);
            this.txtSATSerNo.Name = "txtSATSerNo";
            this.txtSATSerNo.Size = new System.Drawing.Size(146, 25);
            this.txtSATSerNo.TabIndex = 9;
            this.txtSATSerNo.TextChanged += new System.EventHandler(this.txtSATSerNo_TextChanged);
            // 
            // labelX3
            // 
            this.labelX3.AutoSize = true;
            // 
            // 
            // 
            this.labelX3.BackgroundStyle.Class = "";
            this.labelX3.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX3.Location = new System.Drawing.Point(24, 25);
            this.labelX3.Name = "labelX3";
            this.labelX3.Size = new System.Drawing.Size(60, 21);
            this.labelX3.TabIndex = 8;
            this.labelX3.Text = "報名序號";
            // 
            // txtSatClassName
            // 
            // 
            // 
            // 
            this.txtSatClassName.Border.Class = "TextBoxBorder";
            this.txtSatClassName.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.txtSatClassName.Location = new System.Drawing.Point(323, 23);
            this.txtSatClassName.Name = "txtSatClassName";
            this.txtSatClassName.Size = new System.Drawing.Size(78, 25);
            this.txtSatClassName.TabIndex = 11;
            this.txtSatClassName.TextChanged += new System.EventHandler(this.txtSatClassName_TextChanged);
            // 
            // labelX1
            // 
            this.labelX1.AutoSize = true;
            // 
            // 
            // 
            this.labelX1.BackgroundStyle.Class = "";
            this.labelX1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX1.Location = new System.Drawing.Point(257, 25);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(60, 21);
            this.labelX1.TabIndex = 10;
            this.labelX1.Text = "學測班級";
            // 
            // txtSatSeatNo
            // 
            // 
            // 
            // 
            this.txtSatSeatNo.Border.Class = "TextBoxBorder";
            this.txtSatSeatNo.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.txtSatSeatNo.Location = new System.Drawing.Point(482, 23);
            this.txtSatSeatNo.Name = "txtSatSeatNo";
            this.txtSatSeatNo.Size = new System.Drawing.Size(48, 25);
            this.txtSatSeatNo.TabIndex = 13;
            this.txtSatSeatNo.TextChanged += new System.EventHandler(this.txtSatSeatNo_TextChanged);
            // 
            // labelX2
            // 
            this.labelX2.AutoSize = true;
            // 
            // 
            // 
            this.labelX2.BackgroundStyle.Class = "";
            this.labelX2.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX2.Location = new System.Drawing.Point(416, 25);
            this.labelX2.Name = "labelX2";
            this.labelX2.Size = new System.Drawing.Size(60, 21);
            this.labelX2.TabIndex = 12;
            this.labelX2.Text = "學測座號";
            // 
            // SatStudentContent
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.txtSatSeatNo);
            this.Controls.Add(this.labelX2);
            this.Controls.Add(this.txtSatClassName);
            this.Controls.Add(this.labelX1);
            this.Controls.Add(this.txtSATSerNo);
            this.Controls.Add(this.labelX3);
            this.Name = "SatStudentContent";
            this.Size = new System.Drawing.Size(550, 75);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevComponents.DotNetBar.Controls.TextBoxX txtSATSerNo;
        private DevComponents.DotNetBar.LabelX labelX3;
        private DevComponents.DotNetBar.Controls.TextBoxX txtSatClassName;
        private DevComponents.DotNetBar.LabelX labelX1;
        private DevComponents.DotNetBar.Controls.TextBoxX txtSatSeatNo;
        private DevComponents.DotNetBar.LabelX labelX2;
    }
}
