﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SHCollege.DAO;
using Aspose.Cells;
using FISCA.UDT;


namespace SHCollege.Forms
{
    public partial class ScoreForm103_1 : FISCA.Presentation.Controls.BaseForm
    {
                // 讀取資料
        BackgroundWorker _bgLoadMapping;
        // 產生資料
        BackgroundWorker _bgExporData;

        // 存在 UDT Mapping 資料
        List<FieldConfig> _FieldConfigList;

        // 儲存 UDT Mapping 資料用
        List<FieldConfig> _SaveFieldConfigList;

        // 使用原始成績
        bool _chkSScore = true;

        public ScoreForm103_1()
        {
            InitializeComponent();
            _FieldConfigList = new List<FieldConfig> ();
            _SaveFieldConfigList = new List<FieldConfig>();

            _bgLoadMapping = new BackgroundWorker();
            _bgLoadMapping.DoWork += _bgLoadMapping_DoWork;
            _bgLoadMapping.RunWorkerCompleted += _bgLoadMapping_RunWorkerCompleted;

            _bgExporData = new BackgroundWorker();
            _bgExporData.DoWork += _bgExporData_DoWork;
            _bgExporData.RunWorkerCompleted += _bgExporData_RunWorkerCompleted;
            _bgExporData.WorkerReportsProgress = true;
            _bgExporData.ProgressChanged += _bgExporData_ProgressChanged;
        }

        void _bgExporData_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            FISCA.Presentation.MotherForm.SetStatusBarMessage("大學繁星(高一在校學業成績)產生中..", e.ProgressPercentage);
        }

        void _bgLoadMapping_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            // 清除畫面並將資料填入
            dgData.Rows.Clear();

            // 檢查是否用預設欄位
            if (_FieldConfigList.Count > 3)
            {
                foreach (FieldConfig fc in _FieldConfigList)
                {
                    int rowIdx = dgData.Rows.Add();

                    dgData.Rows[rowIdx].Tag = fc;
                    dgData.Rows[rowIdx].Cells[colFieldName.Index].Value = fc.FieldName;
                    dgData.Rows[rowIdx].Cells[colFieldMapping.Index].Value = fc.FieldMapping;
                }
            }
            else
            { 
                // 使用預設
                List<string> filedList = GetDefaultFieldName();
                List<string> ddList = new List<string>();

                foreach (string str in filedList)
                {

                    int rowIdx = dgData.Rows.Add();
                    FieldConfig fc = new FieldConfig();
                    fc.FieldName = str;
                    dgData.Rows[rowIdx].Tag = fc;
                    dgData.Rows[rowIdx].Cells[colFieldName.Index].Value = fc.FieldName;
                    string ffName = fc.FieldName.Replace("(高一上)", "").Replace("(高一下)", "").Replace("(高二上)", "").Replace("(高二下)", "");
                    dgData.Rows[rowIdx].Cells[colFieldMapping.Index].Value = ffName;
                }

            }
            btnEnable(true);
        }

        void _bgLoadMapping_DoWork(object sender, DoWorkEventArgs e)
        {
            // 讀取 UDT 資料
            AccessHelper ah = new AccessHelper();
            List<FieldConfig> FieldConfigList = ah.Select<FieldConfig>();

            // 排序
            _FieldConfigList = (from data in FieldConfigList orderby data.FieldOrder ascending select data).ToList();
     
        }

        void _bgExporData_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            // 匯出至csv

            try
            {
                DataTable dt = e.Result as DataTable;

                if (dt != null)
                {
                    // 需要產生xls與csv                  

                    Utility.CompletedXls("大學繁星(高一在校學業成績)", dt);
                    Utility.CompletedXlsCsv("大學繁星(高一在校學業成績)", dt);                                    
                }
            }
            catch (Exception ex)
            {
                SmartSchool.ErrorReporting.ErrorMessgae errMsg = new SmartSchool.ErrorReporting.ErrorMessgae(ex);
                FISCA.Presentation.Controls.MsgBox.Show("產生 csv 檔案發生錯誤:"+ex.Message);                
            }
            _bgLoadMapping.RunWorkerAsync();            
        }

        void _bgExporData_DoWork(object sender, DoWorkEventArgs e)
        {
            _bgExporData.ReportProgress(1);
            if (_SaveFieldConfigList.Count > 0)
            {
                
                // 取得所選學生ID
                List<string> StudentIDList = K12.Presentation.NLDPanels.Student.SelectedSource;

                // 取得學生基本資料,已依學號排序
                List<DataRow> StudBaseList = Utility.GetStudentBaseDataByID(StudentIDList);
                _bgExporData.ReportProgress(30);
                // 取得學生科目成績資料 key:studentID
                Dictionary<string,List<DataRow>> SemsSubjDataDict = Utility.GetStudentSemsSubjScoreByStudentID(StudentIDList, ckReq.Checked);

                // 取得學生學期總成績
                Dictionary<string, List<DataRow>> SemsEntryDataDict = Utility.GetStudentSemsEntryScoreByStudentID(StudentIDList);

                // 取得學測報名資料
                Dictionary<string, UDT_SHSATStudent> SHSATStudentDict = DAO.UDTTransfer.GetSATStudentByStudentIDListDict(StudentIDList);

                // 取得科別對照
                Dictionary<string, string> studDeptDict = DAO.UDTTransfer.GetStudentSATDeptMappingDict(StudentIDList);

                // 取得班級代碼對照
                Dictionary<string, string> classCodeDict = new Dictionary<string, string>();
                List<UDT_SHSATClassCode> classcodeList = UDTTransfer.GetClassCodeList();
                foreach (UDT_SHSATClassCode code in classcodeList)
                {
                    if (!classCodeDict.ContainsKey(code.ClassName))
                    {
                        classCodeDict.Add(code.ClassName, code.ClassCode);
                    }
                }

                _bgExporData.ReportProgress(60);

                // 輸出用 
                DataTable exportDT = new DataTable();
                //DataTable exportDT103_1 = new DataTable();

                //// 取得一年級學生規格檔
                //List<string> scoreField103List = Utility.GetScoreFieldList103_1();

                //exportDT.Columns.Add("學號");
                //exportDT.Columns.Add("班級");

                // 填入 columns
                foreach (FieldConfig fc in _SaveFieldConfigList)
                {                    
                    DataColumn column = new DataColumn();
                    column.DataType = Type.GetType("System.String");
                    column.ColumnName = fc.FieldName;
                    if(!exportDT.Columns.Contains(fc.FieldName))
                        exportDT.Columns.Add(column);
                }

                //// 103 規格
                //foreach (string name in scoreField103List)
                //{
                //    DataColumn column = new DataColumn();
                //    column.DataType = Type.GetType("System.String");
                //    column.ColumnName = name;
                //    exportDT103_1.Columns.Add(column);                
                //}

                Dictionary<string, List<string>> _ScoreNameMappingDict = new Dictionary<string, List<string>>();

                Dictionary<string, List<string>> _CalScoreNameMappingDict = new Dictionary<string, List<string>>();

                // 建立成績對照表,
                foreach (FieldConfig fc in _SaveFieldConfigList)
                { 
                    string[] strS=fc.FieldMapping.Split(',');

                    if (!_ScoreNameMappingDict.ContainsKey(fc.FieldName))
                        _ScoreNameMappingDict.Add(fc.FieldName, new List<string>());

                    foreach (string ss in strS)
                        _ScoreNameMappingDict[fc.FieldName].Add(ss.Trim());
                
                }

                // 建立成績計算對照+
                foreach (FieldConfig fc in _SaveFieldConfigList)
                {
                    if(fc.FieldMapping.IndexOf('+')>0)
                    {
                        string[] strS = fc.FieldMapping.Split('+');

                        if (!_CalScoreNameMappingDict.ContainsKey(fc.FieldName))
                            _CalScoreNameMappingDict.Add(fc.FieldName, new List<string>());

                        foreach (string ss in strS)
                            _CalScoreNameMappingDict[fc.FieldName].Add(ss.Trim());
                    }
                }

                Dictionary<string, SScore> ssScoreDict = new Dictionary<string, SScore>();

                _bgExporData.ReportProgress(75);
                // 輸出資料
                foreach (DataRow dr in StudBaseList)
                {
                    ssScoreDict.Clear();

                    string sid = dr["sid"].ToString();
                    DataRow newRow = exportDT.NewRow();

                    // 填入成績初始值
                    for (int col = 0; col < exportDT.Columns.Count; col++)
                    {
                        newRow[col] = "-1";
                    }

                    if (exportDT.Columns.Contains("學號"))
                        newRow["學號"] = dr["學號"];

                    if (exportDT.Columns.Contains("身分證號碼"))
                        newRow["身分證號碼"] = dr["身分證號碼"];

                    if (exportDT.Columns.Contains("姓名"))
                        newRow["姓名"] = dr["姓名"];

                    // 就讀科、學程、班別
                    if (exportDT.Columns.Contains("就讀科、學程、班別"))
                    {
                        // 預設値
                        newRow["就讀科、學程、班別"] = "";
                        if (studDeptDict.ContainsKey(sid))
                            newRow["就讀科、學程、班別"] = studDeptDict[sid];
                    }

                    // 班級代碼
                    if (dr["班級名稱"]!=null)
                    if (exportDT.Columns.Contains("班級"))
                    {
                        string className = dr["班級名稱"].ToString();
                        if (classCodeDict.ContainsKey(className))
                            newRow["班級"] = classCodeDict[className];
                    }

                    
                    // 預設報考1
                    if (exportDT.Columns.Contains("報名學測或術科考試情形"))
                        newRow["報名學測或術科考試情形"] = "1";  // 預設値

                    if (SHSATStudentDict.ContainsKey(sid))
                    {
                        if (exportDT.Columns.Contains("班級座號"))
                            newRow["班級座號"] = SHSATStudentDict[sid].SatClassSeatNo;

                        if (exportDT.Columns.Contains("學測報名序號"))
                            newRow["學測報名序號"] = SHSATStudentDict[sid].SatSerNo;
                    }

                        #region 比對學期科目成績
                        // 比對學期科目成績
                        if (SemsSubjDataDict.ContainsKey(sid))
                        {
                            List<DataRow> dd = SemsSubjDataDict[sid];
                            foreach (DataRow dr1 in dd)
                            {
                                if ((dr1["成績年級"].ToString() == "1" || dr1["成績年級"].ToString() == "4") && dr1["學期"].ToString() == "1")
                                {
                                    string subjName = dr1["科目"].ToString().Trim();

                                    foreach (string strKey in _ScoreNameMappingDict.Keys)
                                    {
                                        if (strKey.Contains("一上"))
                                        {
                                            foreach (string strSKey in _ScoreNameMappingDict[strKey])
                                            {
                                                if (strSKey == subjName)
                                                {
                                                    if (newRow[strKey] == null)
                                                        newRow[strKey] = "-1";

                                                    if (newRow[strKey].ToString() != "-1")
                                                    {
                                                        newRow[strKey] += "_" + ParseSubjScore(dr1);
                                                        break;
                                                    }
                                                    else
                                                    {
                                                        newRow[strKey] = ParseSubjScore(dr1);
                                                        break;
                                                    }
                                                }
                                            }
                                        }                                    
                                    }

                                    // 即時計算
                                    foreach (string strKey in _CalScoreNameMappingDict.Keys)
                                    {
                                        if (strKey.Contains("一上"))
                                        {
                                            foreach (string strSKey in _CalScoreNameMappingDict[strKey])
                                            {
                                                if (strSKey == subjName)
                                                {
                                                    if (!ssScoreDict.ContainsKey(strKey))
                                                        ssScoreDict.Add(strKey, new SScore());

                                                    ssScoreDict[strKey].MainName = strKey;

                                                    decimal ss=0;
                                                    decimal cc=0;
                                                    decimal.TryParse(dr1["原始成績"].ToString(), out ss);
                                                    decimal.TryParse(dr1["學分數"].ToString(), out cc);
                                                    ssScoreDict[strKey].AddScore(subjName, ss, cc);
                                                        break;
                                                 
                                                }
                                            }
                                        }
                                    }
                                }

                                if ((dr1["成績年級"].ToString() == "1" || dr1["成績年級"].ToString() == "4") && dr1["學期"].ToString() == "2")
                                {
                                    string subjName = dr1["科目"].ToString().Trim();

                                    foreach (string strKey in _ScoreNameMappingDict.Keys)
                                    {
                                        if (strKey.Contains("一下"))
                                        {
                                            foreach (string strSKey in _ScoreNameMappingDict[strKey])
                                            {
                                                if (strSKey == subjName)
                                                {
                                                    if (newRow[strKey] == null)
                                                        newRow[strKey] = "-1";

                                                    if (newRow[strKey].ToString() != "-1")
                                                    {
                                                        newRow[strKey] +="_" + ParseSubjScore(dr1);
                                                        break;
                                                    }
                                                    else
                                                    {
                                                        newRow[strKey] = ParseSubjScore(dr1);
                                                        break;
                                                    }

                                                }
                                            }
                                        }
                                    }

                                    // 即時計算
                                    foreach (string strKey in _CalScoreNameMappingDict.Keys)
                                    {
                                        if (strKey.Contains("一下"))
                                        {
                                            foreach (string strSKey in _CalScoreNameMappingDict[strKey])
                                            {
                                                if (strSKey == subjName)
                                                {
                                                    if (!ssScoreDict.ContainsKey(strKey))
                                                        ssScoreDict.Add(strKey, new SScore());

                                                    ssScoreDict[strKey].MainName = strKey;

                                                    decimal ss = 0;
                                                    decimal cc = 0;
                                                    decimal.TryParse(dr1["原始成績"].ToString(), out ss);
                                                    decimal.TryParse(dr1["學分數"].ToString(), out cc);
                                                    ssScoreDict[strKey].AddScore(subjName, ss, cc);
                                                    break;

                                                }
                                            }
                                        }
                                    }
                                }

                                if ((dr1["成績年級"].ToString() == "2" || dr1["成績年級"].ToString() == "5") && dr1["學期"].ToString() == "1")
                                {
                                    string subjName = dr1["科目"].ToString().Trim();

                                    foreach (string strKey in _ScoreNameMappingDict.Keys)
                                    {
                                        if (strKey.Contains("二上"))
                                        {
                                            foreach (string strSKey in _ScoreNameMappingDict[strKey])
                                            {
                                                if (strSKey == subjName)
                                                {
                                                    if (newRow[strKey] == null)
                                                        newRow[strKey] = "-1";

                                                    if (newRow[strKey].ToString() != "-1")
                                                    {
                                                        newRow[strKey] +=  "_" + ParseSubjScore(dr1);
                                                        break;
                                                    }
                                                    else
                                                    {
                                                        newRow[strKey] = ParseSubjScore(dr1);
                                                        break;
                                                    }

                                                }
                                            }
                                        }
                                    }

                                    // 即時計算
                                    foreach (string strKey in _CalScoreNameMappingDict.Keys)
                                    {
                                        if (strKey.Contains("二上"))
                                        {
                                            foreach (string strSKey in _CalScoreNameMappingDict[strKey])
                                            {
                                                if (strSKey == subjName)
                                                {
                                                    if (!ssScoreDict.ContainsKey(strKey))
                                                        ssScoreDict.Add(strKey, new SScore());

                                                    ssScoreDict[strKey].MainName = strKey;

                                                    decimal ss = 0;
                                                    decimal cc = 0;
                                                    decimal.TryParse(dr1["原始成績"].ToString(), out ss);
                                                    decimal.TryParse(dr1["學分數"].ToString(), out cc);
                                                    ssScoreDict[strKey].AddScore(subjName, ss, cc);
                                                    break;

                                                }
                                            }
                                        }
                                    }
                                }

                                if ((dr1["成績年級"].ToString() == "2" || dr1["成績年級"].ToString() == "5") && dr1["學期"].ToString() == "2")
                                {
                                    string subjName = dr1["科目"].ToString().Trim();
                                    foreach (string strKey in _ScoreNameMappingDict.Keys)
                                    {
                                        if (strKey.Contains("二下"))
                                        {
                                            foreach (string strSKey in _ScoreNameMappingDict[strKey])
                                            {
                                                if (strSKey == subjName)
                                                {
                                                    if (newRow[strKey] == null)
                                                        newRow[strKey] = "-1";
                                                    if (newRow[strKey].ToString() != "-1")
                                                    {
                                                        newRow[strKey] +=  "_" + ParseSubjScore(dr1);
                                                        break;
                                                    }
                                                    else
                                                    {
                                                        newRow[strKey] = ParseSubjScore(dr1);
                                                        break;
                                                    }

                                                }
                                            }
                                        }
                                    }

                                    // 即時計算
                                    foreach (string strKey in _CalScoreNameMappingDict.Keys)
                                    {
                                        if (strKey.Contains("二下"))
                                        {
                                            foreach (string strSKey in _CalScoreNameMappingDict[strKey])
                                            {
                                                if (strSKey == subjName)
                                                {
                                                    if (!ssScoreDict.ContainsKey(strKey))
                                                        ssScoreDict.Add(strKey, new SScore());

                                                    ssScoreDict[strKey].MainName = strKey;

                                                    decimal ss = 0;
                                                    decimal cc = 0;
                                                    decimal.TryParse(dr1["原始成績"].ToString(), out ss);
                                                    decimal.TryParse(dr1["學分數"].ToString(), out cc);
                                                    ssScoreDict[strKey].AddScore(subjName, ss, cc);
                                                    break;

                                                }
                                            }
                                        }
                                    }
                                }


                                if ((dr1["成績年級"].ToString() == "3" || dr1["成績年級"].ToString() == "6") && dr1["學期"].ToString() == "1")
                                {
                                    string subjName = dr1["科目"].ToString().Trim();

                                    foreach (string strKey in _ScoreNameMappingDict.Keys)
                                    {
                                        if (strKey.Contains("三上"))
                                        {
                                            foreach (string strSKey in _ScoreNameMappingDict[strKey])
                                            {
                                                if (strSKey == subjName)
                                                {
                                                    if (newRow[strKey] == null)
                                                        newRow[strKey] = "-1";

                                                    if (newRow[strKey].ToString() != "-1")
                                                    {
                                                        newRow[strKey] += "_" + ParseSubjScore(dr1);
                                                        break;
                                                    }
                                                    else
                                                    {
                                                        newRow[strKey] = ParseSubjScore(dr1);
                                                        break;
                                                    }

                                                }
                                            }
                                        }
                                    }

                                    // 即時計算
                                    foreach (string strKey in _CalScoreNameMappingDict.Keys)
                                    {
                                        if (strKey.Contains("三上"))
                                        {
                                            foreach (string strSKey in _CalScoreNameMappingDict[strKey])
                                            {
                                                if (strSKey == subjName)
                                                {
                                                    if (!ssScoreDict.ContainsKey(strKey))
                                                        ssScoreDict.Add(strKey, new SScore());

                                                    ssScoreDict[strKey].MainName = strKey;

                                                    decimal ss = 0;
                                                    decimal cc = 0;
                                                    decimal.TryParse(dr1["原始成績"].ToString(), out ss);
                                                    decimal.TryParse(dr1["學分數"].ToString(), out cc);
                                                    ssScoreDict[strKey].AddScore(subjName, ss, cc);
                                                    break;

                                                }
                                            }
                                        }
                                    }

                                }

                                if ((dr1["成績年級"].ToString() == "3" || dr1["成績年級"].ToString() == "6") && dr1["學期"].ToString() == "2")
                                {
                                    string subjName = dr1["科目"].ToString().Trim();

                                    foreach (string strKey in _ScoreNameMappingDict.Keys)
                                    {
                                        if (strKey.Contains("三下"))
                                        {
                                            foreach (string strSKey in _ScoreNameMappingDict[strKey])
                                            {
                                                if (strSKey == subjName)
                                                {
                                                    if (newRow[strKey] == null)
                                                        newRow[strKey] = "-1";

                                                    if (newRow[strKey].ToString() != "-1")
                                                    {
                                                        newRow[strKey] +=  "_" + ParseSubjScore(dr1);
                                                        break;
                                                    }
                                                    else
                                                    {
                                                        newRow[strKey] = ParseSubjScore(dr1);
                                                        break;
                                                    }

                                                }
                                            }
                                        }
                                    }
                                    // 即時計算
                                    foreach (string strKey in _CalScoreNameMappingDict.Keys)
                                    {
                                        if (strKey.Contains("三下"))
                                        {
                                            foreach (string strSKey in _CalScoreNameMappingDict[strKey])
                                            {
                                                if (strSKey == subjName)
                                                {
                                                    if (!ssScoreDict.ContainsKey(strKey))
                                                        ssScoreDict.Add(strKey, new SScore());

                                                    ssScoreDict[strKey].MainName = strKey;

                                                    decimal ss = 0;
                                                    decimal cc = 0;
                                                    decimal.TryParse(dr1["原始成績"].ToString(), out ss);
                                                    decimal.TryParse(dr1["學分數"].ToString(), out cc);
                                                    ssScoreDict[strKey].AddScore(subjName, ss, cc);
                                                    break;

                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    #endregion

                    #region 比對學期分項成績
                    if (SemsEntryDataDict.ContainsKey(sid))
                    {
                        foreach (DataRow dr2 in SemsEntryDataDict[sid])
                        {

                            if (_chkSScore)
                            {
                                #region 學業原始
                                if ((dr2["成績年級"].ToString() == "1" || dr2["成績年級"].ToString() == "4") && dr2["學期"].ToString() == "1" && dr2["分項"].ToString() == "學業(原始)")
                                {
                                    foreach (FieldConfig fc in _SaveFieldConfigList)
                                    {
                                        if (fc.FieldName == "學業總平均(高一上)")
                                        {
                                            newRow[fc.FieldName] = ParseEntryScore(dr2);
                                            break;
                                        }
                                    }
                                }
                                if ((dr2["成績年級"].ToString() == "1" || dr2["成績年級"].ToString() == "4") && dr2["學期"].ToString() == "2" && dr2["分項"].ToString() == "學業(原始)")
                                {
                                    foreach (FieldConfig fc in _SaveFieldConfigList)
                                    {
                                        if (fc.FieldName == "學業總平均(高一下)")
                                        {
                                            newRow[fc.FieldName] = ParseEntryScore(dr2);
                                            break;
                                        }
                                    }
                                }

                                if ((dr2["成績年級"].ToString() == "2" || dr2["成績年級"].ToString() == "5") && dr2["學期"].ToString() == "1" && dr2["分項"].ToString() == "學業(原始)")
                                {
                                    foreach (FieldConfig fc in _SaveFieldConfigList)
                                    {
                                        if (fc.FieldName == "學業總平均(高二上)")
                                        {
                                            newRow[fc.FieldName] = ParseEntryScore(dr2);
                                            break;
                                        }
                                    }
                                }

                                if ((dr2["成績年級"].ToString() == "2" || dr2["成績年級"].ToString() == "5") && dr2["學期"].ToString() == "2" && dr2["分項"].ToString() == "學業(原始)")
                                {
                                    foreach (FieldConfig fc in _SaveFieldConfigList)
                                    {
                                        if (fc.FieldName == "學業總平均(高二下)")
                                        {
                                            newRow[fc.FieldName] = ParseEntryScore(dr2);
                                            break;
                                        }
                                    }
                                }


                                if ((dr2["成績年級"].ToString() == "3" || dr2["成績年級"].ToString() == "6") && dr2["學期"].ToString() == "1" && dr2["分項"].ToString() == "學業(原始)")
                                {
                                    foreach (FieldConfig fc in _SaveFieldConfigList)
                                    {
                                        if (fc.FieldName == "學業總平均(高三上)")
                                        {
                                            newRow[fc.FieldName] = ParseEntryScore(dr2);
                                            break;
                                        }
                                    }
                                }

                                if ((dr2["成績年級"].ToString() == "3" || dr2["成績年級"].ToString() == "6") && dr2["學期"].ToString() == "2" && dr2["分項"].ToString() == "學業(原始)")
                                {
                                    foreach (FieldConfig fc in _SaveFieldConfigList)
                                    {
                                        if (fc.FieldName == "學業總平均(高三下)")
                                        {
                                            newRow[fc.FieldName] = ParseEntryScore(dr2);
                                            break;
                                        }
                                    }
                                }
                                #endregion
                            }
                            else
                            {
                                #region 學業
                                if ((dr2["成績年級"].ToString() == "1" || dr2["成績年級"].ToString() == "4") && dr2["學期"].ToString() == "1" && dr2["分項"].ToString() == "學業")
                                {
                                    foreach (FieldConfig fc in _SaveFieldConfigList)
                                    {
                                        if (fc.FieldName == "學業總平均(高一上)")
                                        {
                                            newRow[fc.FieldName] = ParseEntryScore(dr2);
                                            break;
                                        }
                                    }
                                }

                                if ((dr2["成績年級"].ToString() == "1" || dr2["成績年級"].ToString() == "4") && dr2["學期"].ToString() == "2" && dr2["分項"].ToString() == "學業")
                                {
                                    foreach (FieldConfig fc in _SaveFieldConfigList)
                                    {
                                        if (fc.FieldName == "學業總平均(高一下)")
                                        {
                                            newRow[fc.FieldName] = ParseEntryScore(dr2);
                                            break;
                                        }
                                    }
                                }

                                if ((dr2["成績年級"].ToString() == "2" || dr2["成績年級"].ToString() == "5") && dr2["學期"].ToString() == "1" && dr2["分項"].ToString() == "學業")
                                {
                                    foreach (FieldConfig fc in _SaveFieldConfigList)
                                    {
                                        if (fc.FieldName == "學業總平均(高二上)")
                                        {
                                            newRow[fc.FieldName] = ParseEntryScore(dr2);
                                            break;
                                        }
                                    }
                                }

                                if ((dr2["成績年級"].ToString() == "2" || dr2["成績年級"].ToString() == "5") && dr2["學期"].ToString() == "2" && dr2["分項"].ToString() == "學業")
                                {
                                    foreach (FieldConfig fc in _SaveFieldConfigList)
                                    {
                                        if (fc.FieldName == "學業總平均(高二下)")
                                        {
                                            newRow[fc.FieldName] = ParseEntryScore(dr2);
                                            break;
                                        }
                                    }
                                }

                                if ((dr2["成績年級"].ToString() == "3" || dr2["成績年級"].ToString() == "6") && dr2["學期"].ToString() == "1" && dr2["分項"].ToString() == "學業")
                                {
                                    foreach (FieldConfig fc in _SaveFieldConfigList)
                                    {
                                        if (fc.FieldName == "學業總平均(高三上)")
                                        {
                                            newRow[fc.FieldName] = ParseEntryScore(dr2);
                                            break;
                                        }
                                    }
                                }

                                if ((dr2["成績年級"].ToString() == "3" || dr2["成績年級"].ToString() == "6") && dr2["學期"].ToString() == "2" && dr2["分項"].ToString() == "學業")
                                {
                                    foreach (FieldConfig fc in _SaveFieldConfigList)
                                    {
                                        if (fc.FieldName == "學業總平均(高三下)")
                                        {
                                            newRow[fc.FieldName] = ParseEntryScore(dr2);
                                            break;
                                        }
                                    }
                                }
                                #endregion
                            }
                        }
                    }
                    #endregion

                    // 當+ 科目成績附蓋
                    foreach (string key in ssScoreDict.Keys)
                    {
                        if (exportDT.Columns.Contains(key))
                        {
                            newRow[key] = ssScoreDict[key].GetAverage();
                        }
                    }

                    exportDT.Rows.Add(newRow);
                }
                _bgExporData.ReportProgress(100);

                //// 轉換資料
                //foreach (DataRow dr in exportDT.Rows)
                //{
                //    DataRow newRow = exportDT103_1.NewRow();

                //    foreach (string name in scoreField103List)
                //    {
                //        if (exportDT.Columns.Contains(name))
                //        {
                //            newRow[name] = dr[name];
                //        }
                //    }

                //    exportDT103_1.Rows.Add(newRow);
                //}
                e.Result = exportDT;
            }
        }

        private decimal ParseSubjScore(DataRow dr)
        {
            decimal d1, d2;
            if (_chkSScore)
            {
                decimal.TryParse(dr["原始成績"].ToString(), out d1);
                return d1;
            }
            else
            {
                    decimal.TryParse(dr["原始成績"].ToString(), out d1);
                    decimal.TryParse(dr["補考成績"].ToString(), out d2);

                    if (d1 >= d2)
                        return d1;
                    else
                        return d2;
            }        
        }

        private string ParseEntryScore(DataRow dr)
        {
            string retVal = "";
            decimal dd;
            if (_chkSScore)
            {
                if (dr["分項"].ToString() == "學業(原始)")
                {
                    retVal = dr["成績"].ToString();
                
                }         
            }
            else
            {
                if (dr["分項"].ToString() == "學業")
                    retVal = dr["成績"].ToString();                
            }
            return retVal;
        }
        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ScoreForm_Load(object sender, EventArgs e)
        {
            cboSubjectScoreType.Items.Add("原始成績");
            cboSubjectScoreType.Items.Add("原始及補考成績擇優");
            cboSubjectScoreType.SelectedIndex = 0;
            btnEnable(false);
            _bgLoadMapping.RunWorkerAsync();
        }

        private void btnEnable(bool bo)
        {
            btnExportMaping.Enabled = bo;
            btnImportMapping.Enabled = bo;
            btnExportCSV.Enabled = bo;
            btnLoadDefaultField.Enabled = bo;
        }

        private void btnExportMaping_Click(object sender, EventArgs e)
        {
            // 匯出對照表
            Workbook wb = new Workbook();
            wb.Worksheets[0].Cells[0, 0].PutValue(dgData.Columns[colFieldName.Index].HeaderText);
            wb.Worksheets[0].Cells[0, 1].PutValue(dgData.Columns[colFieldMapping.Index].HeaderText);

            int rowIdx = 1;
            foreach (DataGridViewRow dgvr in dgData.Rows)
            {
                if (dgvr.IsNewRow)
                    continue;

                int colIdx = 0;
                foreach (DataGridViewCell dgvc in dgvr.Cells)
                {
                    if (dgvc.Value != null)
                        wb.Worksheets[0].Cells[rowIdx, colIdx].PutValue(dgvc.Value.ToString());

                    colIdx++;
                }
                rowIdx++;
            }
            Utility.CompletedXls("甄選對應表", wb);
        
        }

        private void btnImportMapping_Click(object sender, EventArgs e)
        {
            // 匯入對照表
               OpenFileDialog od = new OpenFileDialog ();
                od.Title="讀取匯入檔案";
                od.Filter="Excel檔案 (*.xls)|*.xls|所有檔案 (*.*)|*.*";

                if (od.ShowDialog() == DialogResult.OK)
                {
                    Workbook iwb = new Workbook(@od.FileName);

                    bool chkRead = true;

                    if (iwb.Worksheets[0].Cells[0, 0].StringValue != "甄選欄位名稱" || iwb.Worksheets[0].Cells[0, 1].StringValue != "名稱對應系統內")
                    {
                        chkRead = false;
                        FISCA.Presentation.Controls.MsgBox.Show("欄位名稱錯誤無法開啟檔案。");
                    }
                    if (chkRead)
                    {
                        dgData.Rows.Clear();
                        for (int row = 1; row <= iwb.Worksheets[0].Cells.MaxDataRow; row++)
                        {
                            int rowIdx = dgData.Rows.Add();
                            dgData.Rows[rowIdx].Cells[colFieldName.Index].Value = iwb.Worksheets[0].Cells[row, 0].StringValue;
                            dgData.Rows[rowIdx].Cells[colFieldMapping.Index].Value = iwb.Worksheets[0].Cells[row, 1].StringValue;
                        }

                        FISCA.Presentation.Controls.MsgBox.Show("匯入完成");
                        SaveConfig();
                        _bgLoadMapping.RunWorkerAsync();
                    }
                }            
        }

        /// <summary>
        /// 預設資料欄位
        /// </summary>
        /// <returns></returns>
        private List<string> GetDefaultFieldName()
        {
            List<string> retVal = new List<string>();
            retVal.Add("學號");
            retVal.Add("身分證號碼");
            retVal.Add("學業總平均(高一上)");
            retVal.Add("國語文(高一上)");
            retVal.Add("英語文(高一上)");
            retVal.Add("數學(高一上)");
            retVal.Add("物理(高一上)");
            retVal.Add("化學(高一上)");
            retVal.Add("生物(高一上)");
            retVal.Add("地球科學(高一上)");
            retVal.Add("歷史(高一上)");
            retVal.Add("地理(高一上)");
            retVal.Add("公民與社會(高一上)");
            retVal.Add("音樂(高一上)");
            retVal.Add("美術(高一上)");
            retVal.Add("舞蹈(高一上)");
            retVal.Add("體育(高一上)");
            retVal.Add("生活科技(高一上)");
            retVal.Add("資訊科技(高一上)");
            retVal.Add("學業總平均(高一下)");
            retVal.Add("國語文(高一下)");
            retVal.Add("英語文(高一下)");
            retVal.Add("數學(高一下)");
            retVal.Add("物理(高一下)");
            retVal.Add("化學(高一下)");
            retVal.Add("生物(高一下)");
            retVal.Add("地球科學(高一下)");
            retVal.Add("歷史(高一下)");
            retVal.Add("地理(高一下)");
            retVal.Add("公民與社會(高一下)");
            retVal.Add("音樂(高一下)");
            retVal.Add("美術(高一下)");
            retVal.Add("舞蹈(高一下)");
            retVal.Add("體育(高一下)");
            retVal.Add("生活科技(高一下)");
            retVal.Add("資訊科技(高一下)");
            retVal.Add("就讀科、學程、班別");
            retVal.Add("班級座號");
            retVal.Add("姓名");

            return retVal;
        
        }

        private void btnExportCSV_Click(object sender, EventArgs e)
        {

            if (ChkData())
            {
                btnEnable(false);

                if (cboSubjectScoreType.Text == "原始成績")
                    _chkSScore = true;
                else
                    _chkSScore = false;

                if (SaveConfig())
                {
                    // 產生資料
                    _bgExporData.RunWorkerAsync();
                }
            }
            else
            {
                FISCA.Presentation.Controls.MsgBox.Show("名稱對應有問題請檢查!");
            }
        }

        private bool ChkData()
        {
            bool pass = true;

            foreach (DataGridViewRow dgvr in dgData.Rows)
            {
                if (dgvr.IsNewRow)
                    continue;

                foreach (DataGridViewCell cell in dgvr.Cells)
                {
                    if (cell.ErrorText != "")
                        pass = false;

                    if (cell.Value != null)
                    {
                        string value = cell.Value.ToString();
                        if (value.IndexOf(',') > 0 && value.IndexOf('+') > 0)
                        {
                            cell.ErrorText = "不能同時使用,+";
                            pass = false;
                        }
                    }
                }
            }

            return pass;
        }

        /// <summary>
        /// 儲存設定值
        /// </summary>
        private bool SaveConfig()
        {
            bool pass = true;
            // 檢查畫面資料是否重複
            List<string> chkStr = new List<string>();
            bool hasSame = false;
            foreach (DataGridViewRow dgvr in dgData.Rows)
            {
                if (dgvr.IsNewRow)
                    continue;

                foreach (DataGridViewCell dgvc in dgvr.Cells)
                {
                    if (dgvc.Value != null)
                        if (dgvc.ColumnIndex == colFieldName.Index)
                        {
                            string key = dgvc.Value.ToString();
                            if (!chkStr.Contains(key))
                                chkStr.Add(key);
                            else
                            {
                                hasSame = true;
                                break;
                            }
                        }
                }

                if (hasSame)
                    break;
            }

            if (hasSame)
            {
                FISCA.Presentation.Controls.MsgBox.Show("有相同欄位名稱無法產生，請檢查!");
                pass = false;
            }
            else
            {
 
               // 刪除舊資料
                foreach (FieldConfig fc in _FieldConfigList)
                    fc.Deleted = true;

                _FieldConfigList.SaveAll();

                // 儲存畫面值到 UDT                
                List<string> fiedNameList = new List<string>();
                _SaveFieldConfigList.Clear();
                int fieldOrder = 0;
                foreach (DataGridViewRow dgvr in dgData.Rows)
                {
                    if (dgvr.IsNewRow)
                        continue;

                    FieldConfig fc = new FieldConfig();
                    

                    if (dgvr.Cells[colFieldName.Index].Value != null)
                        fc.FieldName = dgvr.Cells[colFieldName.Index].Value.ToString();

                    if (dgvr.Cells[colFieldMapping.Index].Value != null)
                        fc.FieldMapping = dgvr.Cells[colFieldMapping.Index].Value.ToString();

                    // 欄位順序重設
                    fc.FieldOrder = fieldOrder;
                    fieldOrder++;

                    fiedNameList.Add(fc.FieldName);
                    _SaveFieldConfigList.Add(fc);
                }
            
                // 儲存資料
                _SaveFieldConfigList.SaveAll();
            }
            return pass;
        }

        private void dgData_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            dgData.EndEdit();
            // 檢查對照
            if (dgData.CurrentCell.ColumnIndex == colFieldMapping.Index)
            {
                dgData.CurrentCell.ErrorText="";
                if (dgData.CurrentCell.Value != null)
                {
                    string value = dgData.CurrentCell.Value.ToString();
                    // 同時包含 , +
                    if (value.IndexOf(",") > 0 && value.IndexOf("+") > 0)
                    {
                        dgData.CurrentCell.ErrorText = "不能同時使用,+";
                    }
                }
            }
            dgData.BeginEdit(false);
        }

        private void btnLoadDefaultField_Click(object sender, EventArgs e)
        {
            btnLoadDefaultField.Enabled = false;
            dgData.Rows.Clear();
            List<string> filedList = GetDefaultFieldName();

            foreach (string str in filedList)
            {
                int rowIdx = dgData.Rows.Add();
                FieldConfig fc = new FieldConfig();
                fc.FieldName = str;
                dgData.Rows[rowIdx].Tag = fc;
                dgData.Rows[rowIdx].Cells[colFieldName.Index].Value = fc.FieldName;
                string ffName = fc.FieldName.Replace("(高一上)", "").Replace("(高一下)", "").Replace("(高二上)", "").Replace("(高二下)", "");
                dgData.Rows[rowIdx].Cells[colFieldMapping.Index].Value = ffName;
            }

            btnLoadDefaultField.Enabled = true;
        }
    }
}
