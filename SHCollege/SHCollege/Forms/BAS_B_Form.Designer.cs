namespace SHCollege.Forms
{
    partial class BAS_B_Form
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
            this.btnExit = new DevComponents.DotNetBar.ButtonX();
            this.btnPrint = new DevComponents.DotNetBar.ButtonX();
            this.txtRegCode = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.labelX3 = new DevComponents.DotNetBar.LabelX();
            this.labelX4 = new DevComponents.DotNetBar.LabelX();
            this.cbxParentName = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.cbxStudTag1 = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.cbxStudTag2 = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.labelX2 = new DevComponents.DotNetBar.LabelX();
            this.SuspendLayout();
            // 
            // btnExit
            // 
            this.btnExit.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnExit.AutoSize = true;
            this.btnExit.BackColor = System.Drawing.Color.Transparent;
            this.btnExit.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnExit.Location = new System.Drawing.Point(223, 180);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(75, 25);
            this.btnExit.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnExit.TabIndex = 11;
            this.btnExit.Text = "離開";
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnPrint
            // 
            this.btnPrint.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnPrint.AutoSize = true;
            this.btnPrint.BackColor = System.Drawing.Color.Transparent;
            this.btnPrint.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnPrint.Location = new System.Drawing.Point(130, 180);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(75, 25);
            this.btnPrint.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnPrint.TabIndex = 10;
            this.btnPrint.Text = "列印";
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // txtRegCode
            // 
            // 
            // 
            // 
            this.txtRegCode.Border.Class = "TextBoxBorder";
            this.txtRegCode.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.txtRegCode.Location = new System.Drawing.Point(105, 21);
            this.txtRegCode.Name = "txtRegCode";
            this.txtRegCode.Size = new System.Drawing.Size(100, 25);
            this.txtRegCode.TabIndex = 8;
            this.txtRegCode.TextChanged += new System.EventHandler(this.txtRegCode_TextChanged);
            // 
            // labelX1
            // 
            this.labelX1.AutoSize = true;
            this.labelX1.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.labelX1.BackgroundStyle.Class = "";
            this.labelX1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX1.Location = new System.Drawing.Point(12, 23);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(87, 21);
            this.labelX1.TabIndex = 6;
            this.labelX1.Text = "報名單位代碼";
            // 
            // labelX3
            // 
            this.labelX3.AutoSize = true;
            this.labelX3.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.labelX3.BackgroundStyle.Class = "";
            this.labelX3.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX3.Location = new System.Drawing.Point(39, 65);
            this.labelX3.Name = "labelX3";
            this.labelX3.Size = new System.Drawing.Size(60, 21);
            this.labelX3.TabIndex = 12;
            this.labelX3.Text = "家長姓名";
            // 
            // labelX4
            // 
            this.labelX4.AutoSize = true;
            this.labelX4.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.labelX4.BackgroundStyle.Class = "";
            this.labelX4.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX4.Location = new System.Drawing.Point(22, 104);
            this.labelX4.Name = "labelX4";
            this.labelX4.Size = new System.Drawing.Size(77, 21);
            this.labelX4.TabIndex = 13;
            this.labelX4.Text = "低收入戶(2)";
            // 
            // cbxParentName
            // 
            this.cbxParentName.DisplayMember = "Text";
            this.cbxParentName.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cbxParentName.FormattingEnabled = true;
            this.cbxParentName.ItemHeight = 19;
            this.cbxParentName.Location = new System.Drawing.Point(105, 63);
            this.cbxParentName.Name = "cbxParentName";
            this.cbxParentName.Size = new System.Drawing.Size(185, 25);
            this.cbxParentName.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.cbxParentName.TabIndex = 14;
            // 
            // cbxStudTag1
            // 
            this.cbxStudTag1.DisplayMember = "Text";
            this.cbxStudTag1.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cbxStudTag1.FormattingEnabled = true;
            this.cbxStudTag1.ItemHeight = 19;
            this.cbxStudTag1.Location = new System.Drawing.Point(105, 100);
            this.cbxStudTag1.Name = "cbxStudTag1";
            this.cbxStudTag1.Size = new System.Drawing.Size(185, 25);
            this.cbxStudTag1.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.cbxStudTag1.TabIndex = 15;
            // 
            // cbxStudTag2
            // 
            this.cbxStudTag2.DisplayMember = "Text";
            this.cbxStudTag2.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cbxStudTag2.FormattingEnabled = true;
            this.cbxStudTag2.ItemHeight = 19;
            this.cbxStudTag2.Location = new System.Drawing.Point(105, 139);
            this.cbxStudTag2.Name = "cbxStudTag2";
            this.cbxStudTag2.Size = new System.Drawing.Size(185, 25);
            this.cbxStudTag2.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.cbxStudTag2.TabIndex = 17;
            // 
            // labelX2
            // 
            this.labelX2.AutoSize = true;
            this.labelX2.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.labelX2.BackgroundStyle.Class = "";
            this.labelX2.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX2.Location = new System.Drawing.Point(9, 143);
            this.labelX2.Name = "labelX2";
            this.labelX2.Size = new System.Drawing.Size(90, 21);
            this.labelX2.TabIndex = 16;
            this.labelX2.Text = "中低收入戶(3)";
            // 
            // BAS_B_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(323, 215);
            this.Controls.Add(this.cbxStudTag2);
            this.Controls.Add(this.labelX2);
            this.Controls.Add(this.cbxStudTag1);
            this.Controls.Add(this.cbxParentName);
            this.Controls.Add(this.labelX4);
            this.Controls.Add(this.labelX3);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnPrint);
            this.Controls.Add(this.txtRegCode);
            this.Controls.Add(this.labelX1);
            this.DoubleBuffered = true;
            this.Name = "BAS_B_Form";
            this.Text = "各項考試考生基本資料檔";
            this.Load += new System.EventHandler(this.BAS_B_Form_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevComponents.DotNetBar.ButtonX btnExit;
        private DevComponents.DotNetBar.ButtonX btnPrint;
        private DevComponents.DotNetBar.Controls.TextBoxX txtRegCode;
        private DevComponents.DotNetBar.LabelX labelX1;
        private DevComponents.DotNetBar.LabelX labelX3;
        private DevComponents.DotNetBar.LabelX labelX4;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cbxParentName;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cbxStudTag1;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cbxStudTag2;
        private DevComponents.DotNetBar.LabelX labelX2;
    }
}