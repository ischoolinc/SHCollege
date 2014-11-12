namespace SHCollege.Forms
{
    partial class CreateSerNoForm
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
            this.btnSave = new DevComponents.DotNetBar.ButtonX();
            this.txtRegCode = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.labelX2 = new DevComponents.DotNetBar.LabelX();
            this.cboSerNoType = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.labelX3 = new DevComponents.DotNetBar.LabelX();
            this.iptSerNo = new DevComponents.Editors.IntegerInput();
            ((System.ComponentModel.ISupportInitialize)(this.iptSerNo)).BeginInit();
            this.SuspendLayout();
            // 
            // btnExit
            // 
            this.btnExit.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnExit.AutoSize = true;
            this.btnExit.BackColor = System.Drawing.Color.Transparent;
            this.btnExit.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnExit.Location = new System.Drawing.Point(194, 149);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(75, 25);
            this.btnExit.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnExit.TabIndex = 11;
            this.btnExit.Text = "離開";
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnSave
            // 
            this.btnSave.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnSave.AutoSize = true;
            this.btnSave.BackColor = System.Drawing.Color.Transparent;
            this.btnSave.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnSave.Location = new System.Drawing.Point(101, 149);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 25);
            this.btnSave.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnSave.TabIndex = 10;
            this.btnSave.Text = "儲存";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // txtRegCode
            // 
            // 
            // 
            // 
            this.txtRegCode.Border.Class = "TextBoxBorder";
            this.txtRegCode.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.txtRegCode.Location = new System.Drawing.Point(128, 17);
            this.txtRegCode.Name = "txtRegCode";
            this.txtRegCode.Size = new System.Drawing.Size(141, 25);
            this.txtRegCode.TabIndex = 8;
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
            this.labelX1.Location = new System.Drawing.Point(35, 19);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(87, 21);
            this.labelX1.TabIndex = 6;
            this.labelX1.Text = "報名單位代碼";
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
            this.labelX2.Location = new System.Drawing.Point(35, 63);
            this.labelX2.Name = "labelX2";
            this.labelX2.Size = new System.Drawing.Size(87, 21);
            this.labelX2.TabIndex = 12;
            this.labelX2.Text = "序號產生方式";
            // 
            // cboSerNoType
            // 
            this.cboSerNoType.DisplayMember = "Text";
            this.cboSerNoType.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cboSerNoType.DropDownWidth = 200;
            this.cboSerNoType.FormattingEnabled = true;
            this.cboSerNoType.ItemHeight = 19;
            this.cboSerNoType.Location = new System.Drawing.Point(128, 61);
            this.cboSerNoType.Name = "cboSerNoType";
            this.cboSerNoType.Size = new System.Drawing.Size(141, 25);
            this.cboSerNoType.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.cboSerNoType.TabIndex = 13;
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
            this.labelX3.Location = new System.Drawing.Point(35, 102);
            this.labelX3.Name = "labelX3";
            this.labelX3.Size = new System.Drawing.Size(87, 21);
            this.labelX3.TabIndex = 14;
            this.labelX3.Text = "流水號起始値";
            // 
            // iptSerNo
            // 
            this.iptSerNo.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.iptSerNo.BackgroundStyle.Class = "DateTimeInputBackground";
            this.iptSerNo.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.iptSerNo.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            this.iptSerNo.Location = new System.Drawing.Point(128, 100);
            this.iptSerNo.MaxValue = 99999;
            this.iptSerNo.MinValue = 1;
            this.iptSerNo.Name = "iptSerNo";
            this.iptSerNo.ShowUpDown = true;
            this.iptSerNo.Size = new System.Drawing.Size(141, 25);
            this.iptSerNo.TabIndex = 15;
            this.iptSerNo.Value = 1;
            // 
            // CreateSerNoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(305, 189);
            this.Controls.Add(this.iptSerNo);
            this.Controls.Add(this.labelX3);
            this.Controls.Add(this.cboSerNoType);
            this.Controls.Add(this.labelX2);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.txtRegCode);
            this.Controls.Add(this.labelX1);
            this.DoubleBuffered = true;
            this.Name = "CreateSerNoForm";
            this.Text = "設定學測報名序號";
            this.Load += new System.EventHandler(this.CreateSerNoForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.iptSerNo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevComponents.DotNetBar.ButtonX btnExit;
        private DevComponents.DotNetBar.ButtonX btnSave;
        private DevComponents.DotNetBar.Controls.TextBoxX txtRegCode;
        private DevComponents.DotNetBar.LabelX labelX1;
        private DevComponents.DotNetBar.LabelX labelX2;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cboSerNoType;
        private DevComponents.DotNetBar.LabelX labelX3;
        private DevComponents.Editors.IntegerInput iptSerNo;
    }
}