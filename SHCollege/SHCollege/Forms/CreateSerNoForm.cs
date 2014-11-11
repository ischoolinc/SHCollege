using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SHCollege.DAO;
using K12.Data;

namespace SHCollege.Forms
{
    public partial class CreateSerNoForm : FISCA.Presentation.Controls.BaseForm
    {
        BackgroundWorker _bgSetData;
        List<UDT_SHSATStudent> _SATStudent;
        List<string> _StudentIDList;

        string _SelType1 = "學校代碼+班座";
        string _SelType2 = "學校代碼+流水序號";
        string _UserSel = "";
        string _SchoolCode = "";
        int _SeNoStart = 1;

        public CreateSerNoForm(List<string> StudentIDList)
        {
            InitializeComponent();
            _StudentIDList = StudentIDList;
            _SATStudent = new List<UDT_SHSATStudent>();
            _bgSetData = new BackgroundWorker();
            _bgSetData.DoWork += _bgSetData_DoWork;
            _bgSetData.WorkerReportsProgress = true;
            _bgSetData.ProgressChanged += _bgSetData_ProgressChanged;
            _bgSetData.RunWorkerCompleted += _bgSetData_RunWorkerCompleted;
        }

        void _bgSetData_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            FISCA.Presentation.MotherForm.SetStatusBarMessage("學測報名序號產生中..", e.ProgressPercentage);
        }

        void _bgSetData_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            btnSave.Enabled = true;
            FISCA.Presentation.Controls.MsgBox.Show("設定學測報名序號完成.");
            this.Close();
        }

        void _bgSetData_DoWork(object sender, DoWorkEventArgs e)
        {
            _bgSetData.ReportProgress(1);
            // 刪除學生學生學測報名序號
            UDTTransfer.DelSHSATStudentListByStudentIDList(_StudentIDList);            

            // 取得班級代碼
            Dictionary<string, string> classCodeDict = new Dictionary<string, string>();
            List<UDT_SHSATClassCode> classCodeList = UDTTransfer.GetClassCodeList();
            foreach (UDT_SHSATClassCode data in classCodeList)
            {
                if (!string.IsNullOrEmpty(data.ClassCode))
                    classCodeDict.Add(data.ClassName, data.ClassCode);            
            }

            _bgSetData.ReportProgress(30);

            // 取得班級資料
            Dictionary<string, string> classNameDict = new Dictionary<string, string>();
            List<ClassRecord> crList = Class.SelectAll();
            foreach (ClassRecord cr in crList)
                classNameDict.Add(cr.ID, cr.Name);

            // 取得學生資料
            List<StudentRecord> studList = Student.SelectByIDs(_StudentIDList);

            _bgSetData.ReportProgress(50);
            List<StudData> sdList = new List<StudData>();
            foreach (StudentRecord rec in studList)
            {
                StudData sd = new StudData();
                sd.StudentID = rec.ID;
                // 學號
                sd.StudentNumber = rec.StudentNumber;

                sd.IDNumber = rec.IDNumber;

                // 使用學生班級比對班級代碼
                if (classNameDict.ContainsKey(rec.RefClassID))
                {
                    sd.ClassName = classNameDict[rec.RefClassID];
                    if (classCodeDict.ContainsKey(classNameDict[rec.RefClassID]))
                        sd.ClassCode = classCodeDict[classNameDict[rec.RefClassID]];
                }
                // 座號
                sd.SeatNo = "99";
                if (rec.SeatNo.HasValue)
                    sd.SeatNo = string.Format("{0:00}", rec.SeatNo.Value);

                sdList.Add(sd);
            }

            
            _bgSetData.ReportProgress(70);
            // 新增所選學生學測報名序號
            // 使用班級座號
            if (_UserSel == _SelType1)
            { 
                // 排序:班級代碼+座號
                List<StudData> dataList = (from data in sdList orderby data.ClassCode ascending, data.SeatNo ascending select data).ToList();

                List<UDT_SHSATStudent> udt_studList = new List<UDT_SHSATStudent>();                
                foreach (StudData sd in dataList)
                {
                    UDT_SHSATStudent udata = new UDT_SHSATStudent();
                    udata.IDNumber = sd.IDNumber;
                    udata.RefStudentID = sd.StudentID;
                    udata.SatClassName = sd.ClassName;
                    udata.SatSeatNo = sd.SeatNo;
                    udata.SatClassSeatNo = sd.ClassCode + sd.SeatNo;
                    // 學校代碼+班級代碼+座號
                    udata.SatSerNo = _SchoolCode+ sd.ClassCode + sd.SeatNo;
                    udt_studList.Add(udata);
                }

                // 儲存資料
                udt_studList.SaveAll();                
            }

            
            // 使用流水號
            if (_UserSel == _SelType2)
            {
                int sno = 1;

                // 排序:依學號
                List<StudData> dataList = (from data in sdList orderby data.StudentNumber ascending select data).ToList();
                List<UDT_SHSATStudent> udt_studList = new List<UDT_SHSATStudent>();
                foreach (StudData sd in dataList)
                {
                    UDT_SHSATStudent udata = new UDT_SHSATStudent();
                    string srStr = string.Format("{0:00000}", sno);
                    udata.IDNumber = sd.IDNumber;
                    udata.RefStudentID = sd.StudentID;
                    udata.SatClassName = srStr.Substring(0, 3);
                    udata.SatSeatNo = srStr.Substring(3, 2);
                    udata.SatClassSeatNo = srStr;
                    // 學校代碼+班級代碼+座號
                    udata.SatSerNo = _SchoolCode + srStr;
                    udt_studList.Add(udata);
                    sno++;
                }

                // 儲存資料
                udt_studList.SaveAll();               
            }
            _bgSetData.ReportProgress(100);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (CheckData())
            {
                if (FISCA.Presentation.Controls.MsgBox.Show("按「是」將取代學生原有學測報名序號、班級代碼、座號!", "設定學測報名序號", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.Yes)
                {
                    _UserSel = cboSerNoType.Text;
                    _SchoolCode = txtRegCode.Text;
                    _SeNoStart = iptSerNo.Value;
                    btnSave.Enabled = false;
                    _bgSetData.RunWorkerAsync();
                }
            }
        }

        /// <summary>
        /// 檢查資料
        /// </summary>
        /// <returns></returns>
        private bool CheckData()
        {
            bool pass = true;
            List<string> errMsg = new List<string>();
            // 檢查學校代碼是否輸入與輸入三位數字
            if (txtRegCode.Text.Length != 3)
            {
                errMsg.Add("學校代碼必須3碼");
                pass = false;
            }
            else
            {
                int dd;
                if (int.TryParse(txtRegCode.Text, out dd) == false)
                {
                    errMsg.Add("學校代碼必須數字");
                    pass = false;
                }            
            }

            if (cboSerNoType.Text == "")
            {
                errMsg.Add("請選擇序號產生方式!");
                pass = false;
            }
            if (errMsg.Count > 0)
            {
                FISCA.Presentation.Controls.MsgBox.Show(string.Join("! ",errMsg.ToArray()));
            }

            return pass;
        }

        private void CreateSerNoForm_Load(object sender, EventArgs e)
        {
            this.MaximumSize = this.MinimumSize = this.Size;
            // 序號排序可選模式
            cboSerNoType.Items.Add(_SelType1);
            cboSerNoType.Items.Add(_SelType2);
        }
    }
}
