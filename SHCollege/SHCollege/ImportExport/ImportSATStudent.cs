using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Campus.DocumentValidator;
using Campus.Import;
using K12.Data;
using System.Windows.Forms;
using SHCollege.DAO;

namespace SHCollege.ImportExport
{
    public class ImportSATStudent : ImportWizard
    {

        private ImportOption _Option;
        Dictionary<string, UDT_SHSATStudent> _SHSATStudentListDict = new Dictionary<string, UDT_SHSATStudent>();
        Dictionary<string, string> _StudentNumIDDict = new Dictionary<string, string>();
        Dictionary<string, StudentRecord> _StudentRecDict = new Dictionary<string, StudentRecord>();

        EventHandler eh;
        string EventCode = "SH_College_SATStudentContent";

        public ImportSATStudent()
        {
            this.IsSplit = false;
            this.IsLog = false;
            //啟動更新事件
            eh = FISCA.InteractionService.PublishEvent(EventCode);
        }

        public override ImportAction GetSupportActions()
        {
            return ImportAction.InsertOrUpdate;
        }

        public override string GetValidateRule()
        {
            return Properties.Resources.ImportSATStudXML;
        }

        public override string Import(List<IRowStream> Rows)
        {
            FISCA.Presentation.Controls.MsgBox.Show("以匯入檔內容為主，新增與更新系統內報名序號、學測班級、學測座號。");

            if (_Option.Action == ImportAction.InsertOrUpdate)
            {
                List<UDT_SHSATStudent> SHSATStudentList = new List<UDT_SHSATStudent>();
                int pIdx = 0;
                foreach (IRowStream row in Rows)
                {
                    pIdx++;
                    this.ImportProgress = pIdx;

                    string StudentNumber = "", IDNumber = "", SATSerNo = "", SATClassName = "", SATSeatNo = "";
                    IDNumber = row.GetValue("身分證號");                    
                    SATSerNo = row.GetValue("報名序號");
                    SATClassName = row.GetValue("學測班級");
                    SATSeatNo = row.GetValue("學測座號");

                    if (SATSeatNo.Length == 1)
                        SATSeatNo = "0" + SATSeatNo;

                    if (_StudentNumIDDict.ContainsKey(IDNumber))
                    {
                        string sid = _StudentNumIDDict[IDNumber];

                        StudentRecord rec = _StudentRecDict[sid];
                        IDNumber = rec.IDNumber;


                        if (_SHSATStudentListDict.ContainsKey(sid))
                        {
                            // 更新
                            _SHSATStudentListDict[sid].SatClassName = SATClassName;
                            _SHSATStudentListDict[sid].SatSeatNo = SATSeatNo;
                            _SHSATStudentListDict[sid].SatSerNo = SATSerNo;
                            _SHSATStudentListDict[sid].IDNumber = IDNumber;
                            _SHSATStudentListDict[sid].StudentNumber = StudentNumber;
                            SHSATStudentList.Add(_SHSATStudentListDict[sid]);
                        }
                        else
                        {
                            // 新增
                            UDT_SHSATStudent newData = new UDT_SHSATStudent();
                            newData.SatClassName = SATClassName;
                            newData.SatSeatNo = SATSeatNo;
                            newData.SatSerNo = SATSerNo;
                            newData.IDNumber = IDNumber;
                            newData.StudentNumber = StudentNumber;
                            newData.RefStudentID = sid;
                  
                            SHSATStudentList.Add(newData);
                        }
                    }

                }
                SHSATStudentList.SaveAll();
                eh(this, EventArgs.Empty);
            }
            return "";
        }

        public override void Prepare(ImportOption Option)
        {
            _Option = Option;
            _SHSATStudentListDict = UDTTransfer.GetSHSATStudentListDictAll();
            // 身分證號
            _StudentNumIDDict = UDTTransfer.GetStudentIDNumIDDictAll();
            // 取得學生資料
            List<string> studentIDList = _StudentNumIDDict.Values.ToList();
            List<StudentRecord> recList = Student.SelectByIDs(studentIDList);
            foreach (StudentRecord rec in recList)
                _StudentRecDict.Add(rec.ID, rec);
        }
    }
}
