using Aspose.Cells;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SHCollege.DAO;

namespace SHCollege.Forms
{
    public partial class SetClassCodeForm : FISCA.Presentation.Controls.BaseForm
    {
        // 讀取資料
        BackgroundWorker _bgLoadMapping;

        // UDT Mapping 資料
        List<UDT_SHSATClassCode> _ClassCodeList;
                

        public SetClassCodeForm()
        {
            _ClassCodeList = new List<UDT_SHSATClassCode>();           
            _bgLoadMapping = new BackgroundWorker();
            _bgLoadMapping.DoWork += _bgLoadMapping_DoWork;
            _bgLoadMapping.RunWorkerCompleted += _bgLoadMapping_RunWorkerCompleted;

            InitializeComponent();
            
        }

        void _bgLoadMapping_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            btnSave.Enabled = true;
            dgData.Rows.Clear();
            // 將UDT資料填入畫面                       
            _ClassCodeList = (from data in _ClassCodeList orderby data.ClassName select data).ToList();
            foreach (UDT_SHSATClassCode code in _ClassCodeList)
            {
                int rowIdx = dgData.Rows.Add();
                dgData.Rows[rowIdx].Cells[colClassName.Index].Value = code.ClassName;
                dgData.Rows[rowIdx].Cells[colClassCode.Index].Value = code.ClassCode;
                dgData.Rows[rowIdx].Tag = code;
            }
        }

        void _bgLoadMapping_DoWork(object sender, DoWorkEventArgs e)
        {
            _ClassCodeList = UDTTransfer.GetClassCodeList();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
           

        /// <summary>
        /// 儲存設定值
        /// </summary>
        private bool SaveConfig()
        {
            bool pass = true;
            // 檢查畫面資料是否重複
            List<string> chkStr = new List<string>();
            bool error = false;

            // 檢查有相同資料
            foreach (DataGridViewRow dgvr in dgData.Rows)
            {
                foreach (DataGridViewCell dgvc in dgvr.Cells)
                {              
                    
                        if (dgvc.ColumnIndex == colClassCode.Index)
                        {
                            if (dgvc.Value == null)
                                continue;

                            if (dgvc.Value.ToString().Trim() == "")
                                continue;

                            string key = dgvc.Value.ToString();
                            if (!chkStr.Contains(key))
                                chkStr.Add(key);
                            else
                            {
                                dgvc.ErrorText = "班級代碼重覆!";
                            }
                        }                
                }

            }

            // 檢查是否有錯誤
            foreach (DataGridViewRow dgvr in dgData.Rows)
            {
               
                foreach (DataGridViewCell dgvc in dgvr.Cells)
                {
                    if (dgvc.ErrorText != "")
                    {
                        error = true;
                        break;                        
                    }
                }
            }

            if (error)
            {
                FISCA.Presentation.Controls.MsgBox.Show("班級代碼有誤無法儲存，請檢查!");
                pass = false;
            }
            else
            {
                List<UDT_SHSATClassCode> codeList = new List<UDT_SHSATClassCode>();
                // 儲存畫面資料至 UDT
                foreach (DataGridViewRow dgvr in dgData.Rows)
                {
                    UDT_SHSATClassCode code = dgvr.Tag as UDT_SHSATClassCode;
                    if (code != null)
                    {
                        if (dgvr.Cells[colClassCode.Index].Value != null)
                            code.ClassCode = dgvr.Cells[colClassCode.Index].Value.ToString();
                        else

                            code.ClassCode = "";
                        codeList.Add(code);
                    }
                
                }
                codeList.SaveAll();
            }
            return pass;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            // 儲存資料
            btnSave.Enabled = false;
            if(SaveConfig())
            {
                FISCA.Presentation.Controls.MsgBox.Show("儲存成功.");
                this.Close();
            }
            btnSave.Enabled = true;
        }

        private void dgData_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            dgData.EndEdit();
            if (dgData.CurrentCell.ColumnIndex == colClassCode.Index)
            {
                dgData.CurrentCell.ErrorText="";
                if (dgData.CurrentCell.Value != null)
                {
                    string value = dgData.CurrentCell.Value.ToString();

                    // 內容長度檢查
                    if (value.Length != 3)
                        dgData.CurrentCell.ErrorText = "班級代碼必須3碼";

                    // 內容數字檢查
                    int dd;
                    if (int.TryParse(value, out dd) == false)
                    {
                        dgData.CurrentCell.ErrorText = "班級代碼必須數字";
                    }
                
                }            
            }            
            dgData.BeginEdit(false);
        }

        private void SetClassCodeForm_Load(object sender, EventArgs e)
        {
            // 載入資料
            btnSave.Enabled = false;
            _bgLoadMapping.RunWorkerAsync();
        }        

    }
}
