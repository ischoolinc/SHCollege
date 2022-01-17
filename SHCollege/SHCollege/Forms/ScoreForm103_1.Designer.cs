namespace SHCollege.Forms
{
    partial class ScoreForm103_1
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dgData = new DevComponents.DotNetBar.Controls.DataGridViewX();
            this.colFieldName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colFieldMapping = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.cboSubjectScoreType = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.btnExportMaping = new DevComponents.DotNetBar.ButtonX();
            this.btnImportMapping = new DevComponents.DotNetBar.ButtonX();
            this.btnExit = new DevComponents.DotNetBar.ButtonX();
            this.btnExportCSV = new DevComponents.DotNetBar.ButtonX();
            this.labelX2 = new DevComponents.DotNetBar.LabelX();
            this.btnLoadDefaultField = new DevComponents.DotNetBar.ButtonX();
            this.ckReq = new DevComponents.DotNetBar.Controls.CheckBoxX();
            ((System.ComponentModel.ISupportInitialize)(this.dgData)).BeginInit();
            this.SuspendLayout();
            // 
            // dgData
            // 
            this.dgData.AllowUserToAddRows = false;
            this.dgData.AllowUserToDeleteRows = false;
            this.dgData.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgData.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgData.BackgroundColor = System.Drawing.Color.White;
            this.dgData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgData.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colFieldName,
            this.colFieldMapping});
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgData.DefaultCellStyle = dataGridViewCellStyle1;
            this.dgData.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(215)))), ((int)(((byte)(229)))));
            this.dgData.Location = new System.Drawing.Point(12, 39);
            this.dgData.Name = "dgData";
            this.dgData.RowHeadersWidth = 51;
            this.dgData.RowTemplate.Height = 24;
            this.dgData.Size = new System.Drawing.Size(481, 362);
            this.dgData.TabIndex = 0;
            this.dgData.CurrentCellDirtyStateChanged += new System.EventHandler(this.dgData_CurrentCellDirtyStateChanged);
            // 
            // colFieldName
            // 
            this.colFieldName.HeaderText = "甄選欄位名稱";
            this.colFieldName.MinimumWidth = 6;
            this.colFieldName.Name = "colFieldName";
            this.colFieldName.ReadOnly = true;
            // 
            // colFieldMapping
            // 
            this.colFieldMapping.HeaderText = "名稱對應系統內";
            this.colFieldMapping.MinimumWidth = 6;
            this.colFieldMapping.Name = "colFieldMapping";
            // 
            // labelX1
            // 
            this.labelX1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelX1.AutoSize = true;
            this.labelX1.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.labelX1.BackgroundStyle.Class = "";
            this.labelX1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX1.Location = new System.Drawing.Point(12, 423);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(75, 26);
            this.labelX1.TabIndex = 1;
            this.labelX1.Text = "成績採用";
            // 
            // cboSubjectScoreType
            // 
            this.cboSubjectScoreType.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cboSubjectScoreType.DisplayMember = "Text";
            this.cboSubjectScoreType.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cboSubjectScoreType.FormattingEnabled = true;
            this.cboSubjectScoreType.ItemHeight = 23;
            this.cboSubjectScoreType.Location = new System.Drawing.Point(72, 422);
            this.cboSubjectScoreType.Name = "cboSubjectScoreType";
            this.cboSubjectScoreType.Size = new System.Drawing.Size(155, 29);
            this.cboSubjectScoreType.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.cboSubjectScoreType.TabIndex = 0;
            // 
            // btnExportMaping
            // 
            this.btnExportMaping.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnExportMaping.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnExportMaping.BackColor = System.Drawing.Color.Transparent;
            this.btnExportMaping.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnExportMaping.Location = new System.Drawing.Point(12, 459);
            this.btnExportMaping.Name = "btnExportMaping";
            this.btnExportMaping.Size = new System.Drawing.Size(79, 25);
            this.btnExportMaping.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnExportMaping.TabIndex = 1;
            this.btnExportMaping.Text = "匯出對照";
            this.btnExportMaping.Click += new System.EventHandler(this.btnExportMaping_Click);
            // 
            // btnImportMapping
            // 
            this.btnImportMapping.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnImportMapping.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnImportMapping.BackColor = System.Drawing.Color.Transparent;
            this.btnImportMapping.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnImportMapping.Location = new System.Drawing.Point(93, 459);
            this.btnImportMapping.Name = "btnImportMapping";
            this.btnImportMapping.Size = new System.Drawing.Size(79, 25);
            this.btnImportMapping.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnImportMapping.TabIndex = 2;
            this.btnImportMapping.Text = "匯入對照";
            this.btnImportMapping.Click += new System.EventHandler(this.btnImportMapping_Click);
            // 
            // btnExit
            // 
            this.btnExit.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExit.BackColor = System.Drawing.Color.Transparent;
            this.btnExit.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnExit.Location = new System.Drawing.Point(419, 459);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(75, 25);
            this.btnExit.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnExit.TabIndex = 5;
            this.btnExit.Text = "離開";
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnExportCSV
            // 
            this.btnExportCSV.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnExportCSV.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExportCSV.BackColor = System.Drawing.Color.Transparent;
            this.btnExportCSV.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnExportCSV.Location = new System.Drawing.Point(340, 459);
            this.btnExportCSV.Name = "btnExportCSV";
            this.btnExportCSV.Size = new System.Drawing.Size(75, 25);
            this.btnExportCSV.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnExportCSV.TabIndex = 4;
            this.btnExportCSV.Text = "列印";
            this.btnExportCSV.Click += new System.EventHandler(this.btnExportCSV_Click);
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
            this.labelX2.Location = new System.Drawing.Point(12, 12);
            this.labelX2.Name = "labelX2";
            this.labelX2.Size = new System.Drawing.Size(92, 26);
            this.labelX2.TabIndex = 7;
            this.labelX2.Text = "欄位對照表";
            // 
            // btnLoadDefaultField
            // 
            this.btnLoadDefaultField.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnLoadDefaultField.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnLoadDefaultField.BackColor = System.Drawing.Color.Transparent;
            this.btnLoadDefaultField.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnLoadDefaultField.Location = new System.Drawing.Point(174, 459);
            this.btnLoadDefaultField.Name = "btnLoadDefaultField";
            this.btnLoadDefaultField.Size = new System.Drawing.Size(112, 25);
            this.btnLoadDefaultField.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnLoadDefaultField.TabIndex = 3;
            this.btnLoadDefaultField.Text = "恢復預設欄位";
            this.btnLoadDefaultField.Click += new System.EventHandler(this.btnLoadDefaultField_Click);
            // 
            // ckReq
            // 
            this.ckReq.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ckReq.AutoSize = true;
            this.ckReq.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.ckReq.BackgroundStyle.Class = "";
            this.ckReq.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.ckReq.Checked = true;
            this.ckReq.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ckReq.CheckValue = "Y";
            this.ckReq.Location = new System.Drawing.Point(340, 423);
            this.ckReq.Name = "ckReq";
            this.ckReq.Size = new System.Drawing.Size(179, 26);
            this.ckReq.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.ckReq.TabIndex = 11;
            this.ckReq.Text = "只採計部定必修科目";
            // 
            // ScoreForm103_1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(511, 496);
            this.Controls.Add(this.ckReq);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnExportCSV);
            this.Controls.Add(this.btnLoadDefaultField);
            this.Controls.Add(this.labelX2);
            this.Controls.Add(this.btnImportMapping);
            this.Controls.Add(this.btnExportMaping);
            this.Controls.Add(this.cboSubjectScoreType);
            this.Controls.Add(this.labelX1);
            this.Controls.Add(this.dgData);
            this.DoubleBuffered = true;
            this.Name = "ScoreForm103_1";
            this.Text = "大學繁星(高一在校學業成績)";
            this.Load += new System.EventHandler(this.ScoreForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgData)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevComponents.DotNetBar.Controls.DataGridViewX dgData;
        private DevComponents.DotNetBar.LabelX labelX1;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cboSubjectScoreType;
        private DevComponents.DotNetBar.ButtonX btnExportMaping;
        private DevComponents.DotNetBar.ButtonX btnImportMapping;
        private DevComponents.DotNetBar.ButtonX btnExit;
        private DevComponents.DotNetBar.ButtonX btnExportCSV;
        private DevComponents.DotNetBar.LabelX labelX2;
        private DevComponents.DotNetBar.ButtonX btnLoadDefaultField;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFieldName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFieldMapping;
        private DevComponents.DotNetBar.Controls.CheckBoxX ckReq;
    }
}