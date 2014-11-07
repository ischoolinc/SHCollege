using Aspose.Cells;
using K12.Data;
using SHCollege.DAO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data;

namespace SHCollege.Forms
{
    public partial class BAS_B_Form : FISCA.Presentation.Controls.BaseForm
    {
        List<string> _StudentIDList;
        BackgroundWorker _bgWorker;

        List<UDT_SHSATStudent> _SATStudentList;
        string _RegCode = "";
        string _LocalCode = "";
        List<string> _ColumnList;
        DataTable _dt;
        ErrorProvider _errorP;
        string _ParentNameType = "";
        string _StudTagName1 = "";
        string _StudTagName2 = "";

        public BAS_B_Form(List<string> StudentIDList)
        {
            InitializeComponent();
            _errorP = new ErrorProvider();
            _dt = new System.Data.DataTable();
            _ColumnList = new List<string>();
            _bgWorker = new BackgroundWorker();           
            _bgWorker.DoWork += _bgWorker_DoWork;
            _bgWorker.RunWorkerCompleted += _bgWorker_RunWorkerCompleted;
            _bgWorker.ProgressChanged += _bgWorker_ProgressChanged;
            _bgWorker.WorkerReportsProgress = true;
            _StudentIDList = StudentIDList;
        }

        void _bgWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            FISCA.Presentation.MotherForm.SetStatusBarMessage("各項考試考生基本資料檔產生中..", e.ProgressPercentage);
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
                wb.Worksheets[0].Name = "BAS";
                wb.Worksheets[0].Cells.ImportDataTable(_dt, true, 0, 0);

                wb.Worksheets[0].Cells.SetColumnWidth(0, 8);
                wb.Worksheets[0].Cells.SetColumnWidth(1, 20);
                wb.Worksheets[0].Cells.SetColumnWidth(2, 1);
                wb.Worksheets[0].Cells.SetColumnWidth(3, 10);
                wb.Worksheets[0].Cells.SetColumnWidth(4, 6);
                wb.Worksheets[0].Cells.SetColumnWidth(5, 74);
                wb.Worksheets[0].Cells.SetColumnWidth(6, 12);
                wb.Worksheets[0].Cells.SetColumnWidth(7, 3);
                wb.Worksheets[0].Cells.SetColumnWidth(8, 74);
                wb.Worksheets[0].Cells.SetColumnWidth(9, 10);
                wb.Worksheets[0].Cells.SetColumnWidth(10, 10);
                wb.Worksheets[0].Cells.SetColumnWidth(11, 3);
                wb.Worksheets[0].Cells.SetColumnWidth(12, 3);
                wb.Worksheets[0].Cells.SetColumnWidth(13, 1);              
                                
                
                Utility.CompletedXlsFName("BAS" + _RegCode, wb);
            }

            btnPrint.Enabled = true;
        }

        void _bgWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                _bgWorker.ReportProgress(1);

                _ColumnList.Clear();
                _dt.Clear();
                _dt.Columns.Clear();
                _ColumnList.Add("報名序號");
                _ColumnList.Add("姓名");
                _ColumnList.Add("性別");
                _ColumnList.Add("身分證");
                _ColumnList.Add("生日");
                _ColumnList.Add("戶籍地址");
                _ColumnList.Add("家長");
                _ColumnList.Add("郵區");
                _ColumnList.Add("通訊地址");
                _ColumnList.Add("住宅電話");
                _ColumnList.Add("行動電話");
                _ColumnList.Add("學校代碼");
                _ColumnList.Add("畢業年度");
                _ColumnList.Add("低收入戶");


                foreach (string name in _ColumnList)
                    _dt.Columns.Add(name);


                // 取得學生資料
                Dictionary<string, StudentRecord> StudDict = UDTTransfer.GetStudentDictByStudentList(_StudentIDList);

                _bgWorker.ReportProgress(20);

                // 取得父母姓名
                Dictionary<string, ParentRecord> ParentRecordDict = new Dictionary<string, ParentRecord>();
                List<ParentRecord> ParentList = K12.Data.Parent.SelectByStudentIDs(_StudentIDList);
                foreach (ParentRecord rec in ParentList)
                {
                    if (!ParentRecordDict.ContainsKey(rec.RefStudentID))
                        ParentRecordDict.Add(rec.RefStudentID, rec);
                }

                _bgWorker.ReportProgress(30);

                // 取得電話
                Dictionary<string, PhoneRecord> PhoneRecordDict = new Dictionary<string, PhoneRecord>();
                List<PhoneRecord> PhoneList = K12.Data.Phone.SelectByStudentIDs(_StudentIDList);
                foreach (PhoneRecord rec in PhoneList)
                {
                    if (!PhoneRecordDict.ContainsKey(rec.RefStudentID))
                        PhoneRecordDict.Add(rec.RefStudentID, rec);
                }

                _bgWorker.ReportProgress(40);

                // 取得地址
                Dictionary<string, AddressRecord> AddressRecordDict = new Dictionary<string, AddressRecord>();
                List<AddressRecord> addressList = Address.SelectByStudentIDs(_StudentIDList);
                foreach (AddressRecord rec in addressList)
                {
                    if (!AddressRecordDict.ContainsKey(rec.RefStudentID))
                        AddressRecordDict.Add(rec.RefStudentID, rec);
                }

                _bgWorker.ReportProgress(50);

                // 取得學生類別
                Dictionary<string, string> StudTagRecDict = new Dictionary<string, string>();
                List<StudentTagRecord> StudTagList = StudentTag.SelectByStudentIDs(_StudentIDList);
                foreach (StudentTagRecord rec in StudTagList)
                {
                    if (!StudTagRecDict.ContainsKey(rec.RefStudentID))
                        StudTagRecDict.Add(rec.RefStudentID, rec.FullName);                
                } 

                string GSchoolYear = (int.Parse(School.DefaultSchoolYear) + 1).ToString();
                // 取得 UDT 資料
                _SATStudentList = UDTTransfer.GetSATStudentByStudentIDListList(_StudentIDList);

                _bgWorker.ReportProgress(70);

                // 建立資料
                foreach (UDT_SHSATStudent data in _SATStudentList)
                {
                    if (StudDict.ContainsKey(data.RefStudentID))
                    {
                        DataRow dr = _dt.NewRow();

                        foreach (string colName in _ColumnList)
                        {
                            string Value = "";
                            switch (colName)
                            {
                                case "報名序號": Value = data.SatSerNo; break;
                                case "姓名": Value = StudDict[data.RefStudentID].Name; break;
                                case "性別":
                                    if (StudDict[data.RefStudentID].Gender == "男")
                                        Value = "1";

                                    if (StudDict[data.RefStudentID].Gender == "女")
                                        Value = "2";
                                    break;

                                case "身分證": Value = StudDict[data.RefStudentID].IDNumber; break;

                                case "生日":
                                    if (StudDict[data.RefStudentID].Birthday.HasValue)
                                    {
                                        Value = string.Format("{0:00}", (StudDict[data.RefStudentID].Birthday.Value.Year - 1911)) + string.Format("{0:00}", StudDict[data.RefStudentID].Birthday.Value.Month) + string.Format("{0:00}", StudDict[data.RefStudentID].Birthday.Value.Day);
                                    }
                                    break;
                                case "戶籍地址":
                                    if (AddressRecordDict.ContainsKey(data.RefStudentID))
                                    {
                                        Value = AddressRecordDict[data.RefStudentID].PermanentAddress;
                                    }
                                    break;

                                case "家長":
                                    if (ParentRecordDict.ContainsKey(data.RefStudentID))
                                    {
                                        if (_ParentNameType == "父親")
                                            Value = ParentRecordDict[data.RefStudentID].FatherName;

                                        if (_ParentNameType == "母親")
                                            Value = ParentRecordDict[data.RefStudentID].MotherName;

                                        if (_ParentNameType == "監護人")
                                            Value = ParentRecordDict[data.RefStudentID].CustodianName;

                                    }

                                    break;
                                case "郵區":
                                    if (AddressRecordDict.ContainsKey(data.RefStudentID))
                                    {
                                        Value = AddressRecordDict[data.RefStudentID].MailingArea;
                                    }
                                    break;

                                case "通訊地址":
                                    if (AddressRecordDict.ContainsKey(data.RefStudentID))
                                    {
                                        Value = AddressRecordDict[data.RefStudentID].MailingAddress;
                                        if (AddressRecordDict[data.RefStudentID].MailingArea.Length>0)
                                            Value = Value.Replace(AddressRecordDict[data.RefStudentID].MailingArea, "");
                                    }
                                    break;

                                case "住宅電話":
                                    if (PhoneRecordDict.ContainsKey(data.RefStudentID))
                                    {
                                        Value = PhoneRecordDict[data.RefStudentID].Contact;
                                    }
                                    break;

                                case "行動電話":
                                    if (PhoneRecordDict.ContainsKey(data.RefStudentID))
                                    {
                                        Value = PhoneRecordDict[data.RefStudentID].Cell;
                                    }
                                    break;

                                case "學校代碼":
                                    Value = School.Code;
                                    break;

                                case "畢業年度":
                                    Value = GSchoolYear;
                                    break;

                                case "低收入戶":
                                    Value = "1";
                                    if (StudTagRecDict.ContainsKey(data.RefStudentID))
                                    {
                                        if (_StudTagName1 == StudTagRecDict[data.RefStudentID])
                                            Value = "2";

                                        if (_StudTagName2 == StudTagRecDict[data.RefStudentID])
                                            Value = "3";
                                    }
                                    break;
                            }
                            dr[colName] = Value;
                        }
                        _dt.Rows.Add(dr);
                    }
                }
                _bgWorker.ReportProgress(100);
            }
            catch (Exception ex)
            {
                e.Cancel = true;
            }
        }

        private void BAS_B_Form_Load(object sender, EventArgs e)
        {
            this.MaximumSize = this.MinimumSize = this.Size;
            cbxParentName.DropDownStyle = ComboBoxStyle.DropDownList;
            cbxParentName.Items.Add("父親");
            cbxParentName.Items.Add("母親");
            cbxParentName.Items.Add("監護人");

            cbxStudTag1.DropDownStyle = ComboBoxStyle.DropDownList;
            cbxStudTag2.DropDownStyle = ComboBoxStyle.DropDownList;

            List<string> StudTagNameList = GetStudentTagName();
            cbxStudTag1.Items.AddRange(StudTagNameList.ToArray());
            cbxStudTag2.Items.AddRange(StudTagNameList.ToArray());            
        }

        private List<string> GetStudentTagName()
        {
            List<string> retVal = new List<string>();

            List<StudentTagRecord> ss = StudentTag.SelectAll();
            foreach (StudentTagRecord s in ss)
            {
                if (!retVal.Contains(s.FullName))
                    retVal.Add(s.FullName);
            }
            retVal.Sort();
            return retVal;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (chkData())
            {
                _ParentNameType = cbxParentName.Text;
                _RegCode = txtRegCode.Text;
                _StudTagName1 = cbxStudTag1.Text;
                _StudTagName2 = cbxStudTag2.Text;

                btnPrint.Enabled = false;
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

                if (txtRegCode.Text.Length != 3)
                {
                    _errorP.SetError(txtRegCode, "報名單位代碼必須" + 3 + "碼!");
                    pass = false;
                }
            }            

            return pass;
        }

        private void txtRegCode_TextChanged(object sender, EventArgs e)
        {
            _errorP.SetError(txtRegCode, "");
        }
    }
}
