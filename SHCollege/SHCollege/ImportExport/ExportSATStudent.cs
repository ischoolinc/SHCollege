using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SmartSchool.API.PlugIn;
using SHCollege.DAO;

namespace SHCollege.ImportExport
{
    /// <summary>
    /// 匯出學測學生
    /// </summary>
    public class ExportSATStudent : SmartSchool.API.PlugIn.Export.Exporter
    {
        // 可勾選選項
        List<string> ExportItemList;

        public ExportSATStudent()
        {
            this.Image = null;
            this.Text = "匯出學生學測報名序號";
            ExportItemList = new List<string>();            
            ExportItemList.Add("身分證號");
            ExportItemList.Add("報名序號");
            ExportItemList.Add("班級座號");
        }

        public override void InitializeExport(SmartSchool.API.PlugIn.Export.ExportWizard wizard)
        {
             wizard.ExportableFields.AddRange(ExportItemList);
             wizard.ExportPackage += delegate(object sender, SmartSchool.API.PlugIn.Export.ExportPackageEventArgs e)
            {
                Dictionary<string, UDT_SHSATStudent> SHSATStudentDict = UDTTransfer.GetSATStudentByStudentIDListDict(e.List);

                foreach (UDT_SHSATStudent data in SHSATStudentDict.Values)
                {
                    RowData row = new RowData();
                    row.ID = data.RefStudentID;
                        foreach (string field in e.ExportFields)
                        {
                            if (wizard.ExportableFields.Contains(field))
                            {
                                switch (field)
                                {                                  
                                    case "身分證號":
                                        row.Add(field,data.IDNumber);
                                        break;
                                    case "報名序號":
                                        row.Add(field, data.SatSerNo);
                                        break;
                                    case "班級座號":
                                        row.Add(field,data.SatClassSeatNo);
                                        break;                
                                }
                            }
                        }
                        e.Items.Add(row);
                }           
            
            };
        }
    }
}
