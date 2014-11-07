using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using K12.Data;
using SHCollege.DAO;
using Aspose.Cells;

namespace SHCollege.Forms
{
    public partial class SAT_B_Form : FISCA.Presentation.Controls.BaseForm
    {
        List<string> _StudentIDList;
        BackgroundWorker _bgWorker;

        List<UDT_SHSATStudent> _SATStudentList;
        string _RegCode = "";
        string _LocalCode = "";

        ErrorProvider _errorP;

        public SAT_B_Form(List<string> StudentIDList)
        {
            InitializeComponent();
            _bgWorker = new BackgroundWorker();
            _errorP = new ErrorProvider();
            _bgWorker.DoWork += _bgWorker_DoWork;
            _bgWorker.RunWorkerCompleted += _bgWorker_RunWorkerCompleted;
            _bgWorker.ProgressChanged += _bgWorker_ProgressChanged;
            _bgWorker.WorkerReportsProgress = true;
            _StudentIDList = StudentIDList;
        }

        void _bgWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            FISCA.Presentation.MotherForm.SetStatusBarMessage("學科能力測驗報考資料檔產生中..", e.ProgressPercentage);
        }

        void _bgWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            // 產生會出檔
            if (e.Error != null)
            {
                FISCA.Presentation.Controls.MsgBox.Show("產生過程發生錯誤," + e.Error.Message);
            }
            else
            {
                Workbook wb = new Workbook();
                wb.Worksheets[0].Name = "SAT";
                wb.Worksheets[0].Cells.SetColumnWidth(0, 8);
                wb.Worksheets[0].Cells.SetColumnWidth(1, 10);
                wb.Worksheets[0].Cells.SetColumnWidth(2, 3);
                wb.Worksheets[0].Cells.SetColumnWidth(3, 50);
                wb.Worksheets[0].Cells[0,0].PutValue("報名序號");
                wb.Worksheets[0].Cells[0,1].PutValue("身分證");
                wb.Worksheets[0].Cells[0,2].PutValue("學測考試地區");
                wb.Worksheets[0].Cells[0,3].PutValue("學測特殊應考服務需求");

                int rowIdx = 1;
                foreach (UDT_SHSATStudent data in _SATStudentList)
                {
                    wb.Worksheets[0].Cells[rowIdx, 0].PutValue(data.SatSerNo);
                    wb.Worksheets[0].Cells[rowIdx, 1].PutValue(data.IDNumber);
                    wb.Worksheets[0].Cells[rowIdx, 2].PutValue(_LocalCode);
                    
                    rowIdx++;
                }
                Utility.CompletedXlsFName("SAT" + _RegCode, wb);
            }

            btnPrint.Enabled = true;
        }

        void _bgWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                _bgWorker.ReportProgress(1);
                // 取得學生資料
                Dictionary<string, StudentRecord> StudDict = UDTTransfer.GetStudentDictByStudentList(_StudentIDList);

                _bgWorker.ReportProgress(50);
                // 取得 UDT 資料
                _SATStudentList = UDTTransfer.GetSATStudentByStudentIDListList(_StudentIDList);

                _bgWorker.ReportProgress(70);
                foreach (UDT_SHSATStudent data in _SATStudentList)
                {
                    if (StudDict.ContainsKey(data.RefStudentID))
                        data.IDNumber = StudDict[data.RefStudentID].IDNumber;
                }

                // 依照學測編號排序
                _SATStudentList.Sort((x, y) =>
                {
                    return x.SatSerNo.CompareTo(y.SatSerNo);
                });
                _bgWorker.ReportProgress(100);
            }
            catch (Exception ex)
            {
                e.Cancel = true;
                
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void SAT_B_Form_Load(object sender, EventArgs e)
        {
            this.MaximumSize = this.MinimumSize = this.Size;
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (chkData())
            {
                btnPrint.Enabled = false;

                _RegCode = txtRegCode.Text;
                _LocalCode = txtLocalCode.Text;
                _bgWorker.RunWorkerAsync();
            }
        }

        private bool chkData()
        {
            bool pass = true;
            if (string.IsNullOrEmpty(txtRegCode.Text))
            {
                _errorP.SetError(txtRegCode, "報名單位代碼不能空白!");
                pass = false;
            }
            else
            {
                int x;
                if (int.TryParse(txtRegCode.Text, out x) == false)
                {
                    _errorP.SetError(txtRegCode, "報名單位代碼必須數字!");
                    pass = false;
                }

                if (txtRegCode.Text.Length !=3)
                {
                    _errorP.SetError(txtRegCode, "報名單位代碼必須" + 3 + "碼!");
                    pass = false;
                }
            }

            if (string.IsNullOrEmpty(txtLocalCode.Text))
            {
                _errorP.SetError(txtLocalCode, "考試地區代碼不能空白!");
                pass = false;
            }
            else
            {
                int x;
                if (int.TryParse(txtLocalCode.Text, out x) == false)
                {
                    _errorP.SetError(txtLocalCode, "考試地區代碼必須數字!");
                    pass = false;
                }

                if (txtLocalCode.Text.Length != 3)
                {
                    _errorP.SetError(txtLocalCode, "考試地區代碼必須" + 3 + "碼!");
                    pass = false;
                }
            }

            return pass;
        }

        private void txtRegCode_TextChanged(object sender, EventArgs e)
        {
            _errorP.SetError(txtRegCode, "");
        }

        private void txtLocalCode_TextChanged(object sender, EventArgs e)
        {
            _errorP.SetError(txtLocalCode, "");
        }
    }
}
