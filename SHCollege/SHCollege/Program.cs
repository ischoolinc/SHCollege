using Campus.DocumentValidator;
using FISCA.Permission;
using FISCA.Presentation;
using SHCollege.DAO;
using SHCollege.Forms;
using SHCollege.ImportExport.ValidationRule;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace SHCollege
{
    public class Program
    {

        static BackgroundWorker _bgLLoadUDT = new BackgroundWorker();

        [FISCA.MainMethod()]
        public static void Main()
        {

            _bgLLoadUDT.DoWork += _bgLLoadUDT_DoWork;
            _bgLLoadUDT.RunWorkerCompleted += _bgLLoadUDT_RunWorkerCompleted;
            _bgLLoadUDT.RunWorkerAsync();
                       
        }

        static void _bgLLoadUDT_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            // 列印綜合紀錄表未輸入完整名單
            Catalog catalog1 = RoleAclSource.Instance["學生"]["功能按鈕"];
            catalog1.Add(new RibbonFeature("SH_College_ImportSATStudent", "匯入大學繁星學測報名序號"));

            Catalog catalog02 = RoleAclSource.Instance["學生"]["資料項目"];
            catalog02.Add(new DetailItemFeature(typeof(DetailContent.SatStudentContent)));

            Catalog catalog3 = RoleAclSource.Instance["學生"]["功能按鈕"];
            catalog3.Add(new RibbonFeature("SH_College_ExportSATStudent", "匯出大學繁星學測報名序號"));

            RibbonBarItem rbExport = MotherForm.RibbonBarItems["學生", "大學繁星"];
            rbExport["匯出"].Image = Properties.Resources.匯出;
            rbExport["匯出"].Size = RibbonBarButton.MenuButtonSize.Large;
            rbExport["匯出"]["匯出學測報名序號"].Enable = UserAcl.Current["SH_College_ExportSATStudent"].Executable;
            rbExport["匯出"]["匯出學測報名序號"].Click += delegate
            {
                SmartSchool.API.PlugIn.Export.Exporter exporter = new ImportExport.ExportSATStudent();
                ExportStudentV2 wizard = new ExportStudentV2(exporter.Text, exporter.Image);
                exporter.InitializeExport(wizard);
                wizard.ShowDialog();
            }; 

            RibbonBarItem rbImport = MotherForm.RibbonBarItems["學生", "大學繁星"];
            rbImport["匯入"].Image = Properties.Resources.Import_Image;
            rbImport["匯入"].Size = RibbonBarButton.MenuButtonSize.Large;
            rbImport["匯入"]["匯入學測報名序號"].Enable = UserAcl.Current["SH_College_ImportSATStudent"].Executable;
            rbImport["匯入"]["匯入學測報名序號"].Click += delegate
            {
                Utility._tmpSerNoList.Clear();
                new ImportExport.ImportSATStudent().Execute();
            };

            // 產生繁星成績
            Catalog catalog01 = RoleAclSource.Instance["學生"]["大學繁星"];
            catalog01.Add(new RibbonFeature("SH_College_ScoreForm", "大學繁星推甄成績檔"));

            RibbonBarItem item01 = K12.Presentation.NLDPanels.Student.RibbonBarItems["大學繁星"];
            item01["報表"].Image = Properties.Resources.Report;
            item01["報表"].Size = RibbonBarButton.MenuButtonSize.Large;
            item01["報表"]["大學繁星推甄成績檔"].Enable = UserAcl.Current["SH_College_ScoreForm"].Executable;
            item01["報表"]["大學繁星推甄成績檔"].Click += delegate
            {
                if (K12.Presentation.NLDPanels.Student.SelectedSource.Count > 0)
                {
                    ScoreForm sf = new ScoreForm();
                    sf.ShowDialog();
                }
                else
                {
                    FISCA.Presentation.Controls.MsgBox.Show("請選擇學生!");
                }
            };

            Catalog catalog05 = RoleAclSource.Instance["學生"]["功能按鈕"];
            catalog05.Add(new RibbonFeature("SH_College_SATStudentDelete", "刪除學測報名序號"));

            K12.Presentation.NLDPanels.Student.ListPaneContexMenu["刪除學測報名序號"].Enable = UserAcl.Current["SH_College_SATStudentDelete"].Executable;

            K12.Presentation.NLDPanels.Student.ListPaneContexMenu["刪除學測報名序號"].Click += delegate
            {
                if (K12.Presentation.NLDPanels.Student.SelectedSource.Count > 0)
                {
                    if (FISCA.Presentation.Controls.MsgBox.Show("請問要刪除學測報名序號?", "刪除學測報名序號", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Question, System.Windows.Forms.MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.Yes)
                    {
                        EventHandler eh;
                        string EventCode = "SH_College_SATStudentContent";
                        //啟動更新事件
                        eh = FISCA.InteractionService.PublishEvent(EventCode);
                        DAO.UDTTransfer.DelSHSATStudentListByStudentIDList(K12.Presentation.NLDPanels.Student.SelectedSource);
                        FISCA.Presentation.Controls.MsgBox.Show("刪除學測報名序號完成.");
                        
                        eh(e, EventArgs.Empty);
                    }
                }
                else
                {
                    FISCA.Presentation.Controls.MsgBox.Show("請選擇學生!");
                }
            };

            FeatureAce UserPermission = FISCA.Permission.UserAcl.Current["SH_College_SATStudentContent"];

            if (UserPermission.Editable)
                K12.Presentation.NLDPanels.Student.AddDetailBulider(new DetailBulider<DetailContent.SatStudentContent>());
        }

        static void _bgLLoadUDT_DoWork(object sender, DoWorkEventArgs e)
        {
            UDTTransfer.CreateUDTTable();

            #region 自訂驗證規則
            FactoryProvider.FieldFactory.Add(new FieldValidatorFactory());
            #endregion
        }

    }
}
