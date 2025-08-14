using Aspose.Cells;
using FISCA.Presentation.Controls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data;
using FISCA.Data;
namespace SHCollege
{
    public class Utility
    {
        public static List<string> _tmpSerNoList = new List<string>();


        /// <summary>
        /// 匯出 Excel
        /// </summary>
        /// <param name="inputReportName"></param>
        /// <param name="inputXls"></param>
        public static void CompletedXls(string inputReportName, Workbook inputXls)
        {
            string reportName = inputReportName;

            string path = Path.Combine(Application.StartupPath, "Reports");
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            path = Path.Combine(path, reportName + ".xls");

            Workbook wb = inputXls;

            if (File.Exists(path))
            {
                int i = 1;
                while (true)
                {
                    string newPath = Path.GetDirectoryName(path) + "\\" + Path.GetFileNameWithoutExtension(path) + (i++) + Path.GetExtension(path);
                    if (!File.Exists(newPath))
                    {
                        path = newPath;
                        break;
                    }
                }
            }

            try
            {
                wb.Save(path, SaveFormat.Excel97To2003);
                System.Diagnostics.Process.Start(path);
            }
            catch
            {
                SaveFileDialog sd = new SaveFileDialog();
                sd.Title = "另存新檔";
                sd.FileName = reportName + ".xls";
                sd.Filter = "Excel檔案 (*.xls)|*.xls|所有檔案 (*.*)|*.*";
                if (sd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        wb.Save(sd.FileName, SaveFormat.Excel97To2003);

                    }
                    catch
                    {
                        MsgBox.Show("指定路徑無法存取。", "建立檔案失敗", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
            }
        }


        /// <summary>
        /// 匯出 Excel(固定名稱)
        /// </summary>
        /// <param name="inputReportName"></param>
        /// <param name="inputXls"></param>
        public static void CompletedXlsFName(string inputReportName, Workbook inputXls)
        {
            string reportName = inputReportName;

            string path = Path.Combine(Application.StartupPath, "Reports");
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            path = Path.Combine(path, reportName + ".xls");

            Workbook wb = inputXls;

            try
            {
                wb.Save(path, SaveFormat.Excel97To2003);
                System.Diagnostics.Process.Start(path);
            }
            catch
            {
                SaveFileDialog sd = new SaveFileDialog();
                sd.Title = "另存新檔";
                sd.FileName = reportName + ".xls";
                sd.Filter = "Excel檔案 (*.xls)|*.xls|所有檔案 (*.*)|*.*";
                if (sd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        wb.Save(sd.FileName, SaveFormat.Excel97To2003);

                    }
                    catch
                    {
                        MsgBox.Show("指定路徑無法存取。", "建立檔案失敗", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
            }
        }

        public static void CompletedXls(string inputReportName, DataTable dt)
        {
            string reportName = inputReportName;

            string path = Path.Combine(Application.StartupPath, "Reports");
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            path = Path.Combine(path, reportName + ".xls");

            Workbook wb = new Workbook(new MemoryStream(Properties.Resources.template));
            wb.Settings.Encoding = Encoding.Default;

            wb.Worksheets[0].Cells.ImportDataTable(dt, true, 0, 0);

            if (File.Exists(path))
            {
                int i = 1;
                while (true)
                {
                    string newPath = Path.GetDirectoryName(path) + "\\" + Path.GetFileNameWithoutExtension(path) + (i++) + Path.GetExtension(path);
                    if (!File.Exists(newPath))
                    {
                        path = newPath;
                        break;
                    }
                }
            }

            try
            {
                wb.Save(path, SaveFormat.Excel97To2003);
                System.Diagnostics.Process.Start(path);
            }
            catch
            {
                SaveFileDialog sd = new SaveFileDialog();
                sd.Title = "另存新檔";
                sd.FileName = reportName + ".xls";
                sd.Filter = "Excel檔案 (*.xls)|*.xls|所有檔案 (*.*)|*.*";
                if (sd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        wb.Save(sd.FileName, SaveFormat.Excel97To2003);

                    }
                    catch
                    {
                        MsgBox.Show("指定路徑無法存取。", "建立檔案失敗", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
            }
        }

        /// <summary>
        /// 取得學生基本資料
        /// </summary>
        /// <param name="sids"></param>
        /// <returns></returns>
        public static List<DataRow> GetStudentBaseDataByID(List<string> sids)
        {
            List<DataRow> retVal = new List<DataRow>();
            if (sids.Count > 0)
            {
                QueryHelper qh = new QueryHelper();
                string query = @"select student.id as sid,student.student_number as 學號,name as 姓名,student.id_number as 身分證號碼,class.class_name as 班級名稱 from student inner join class on student.ref_class_id=class.id left join $sh.college.sat.student on student.id=to_number($sh.college.sat.student.ref_student_id,'9999999999') where student.id in(" + string.Join(",", sids.ToArray()) + ") order by $sh.college.sat.student.sat_ser_no asc, student.student_number";
                DataTable dt = qh.Select(query);
                foreach (DataRow dr in dt.Rows)
                    retVal.Add(dr);
            }
            return retVal;
        }

        /// <summary>
        /// 取得學生科目成績
        /// </summary>
        /// <param name="sids"></param>
        /// <returns></returns>
        public static Dictionary<string, List<DataRow>> GetStudentSemsSubjScoreByStudentID(List<string> sids, bool required)
        {
            Dictionary<string, List<DataRow>> retVal = new Dictionary<string, List<DataRow>>();
            DataTable dt = new DataTable();
            QueryHelper qh = new QueryHelper();
            string quWhere = "";
            if (required)
                //quWhere = " WHERE s0.d5 ='必修' AND s0.d6='部訂'";
                quWhere = @"
	                    WHERE 
			                array_to_string(xpath('//Subject/@修課必選修', subj_score_ele), '')::TEXT ='必修'
		                    AND array_to_string(xpath('//Subject/@修課校部訂', subj_score_ele), '')::TEXT ='部訂'
";
            if (sids.Count > 0)
            {
                // 2022-01 Cynthia 增加勾選「只採計部定必修」，並依照恩正建議重寫這段SQL。
                //恩正: (SQL使用了xpath_table，會造成資料庫當機，建議整段重寫。SQL樣板與傳入參數的搭配應該更標準化，並注意sql injection)

                #region 舊SQL
                //                string query = @"select sems_subj_score.id,sems_subj_score.ref_student_id as sid,
                //sems_subj_score.school_year as 學期科目成績學年度,
                //sems_subj_score.semester as 學期科目成績學期,
                //sems_subj_score.grade_year as 學期科目成績年級,
                //s0.d1 as 學期科目名稱,
                //s0.d2 as 學期科目級別,
                //s0.d3 as 學期科目不計學分,
                //s0.d4 as 學期科目不需評分,
                //s0.d5 as 學期科目修課必選修,
                //s0.d6 as 學期科目修課校部訂,
                //s0.d10 as 學期科目是否取得學分,
                //CAST(regexp_replace(s0.d7, '^$', '0') as decimal) as 學期科目原始成績,
                //CAST(regexp_replace(s0.d8, '^$', '0') as decimal) as 學期科目學年調整成績,
                //CAST(regexp_replace(s0.d9, '^$', '0') as decimal) as 學期科目擇優採計成績,
                //CAST(regexp_replace(s0.d11, '^$', '0') as decimal) as 學期科目補考成績,
                //CAST(regexp_replace(s0.d12, '^$', '0') as decimal) as 學期科目重修成績,
                //s0.d13 as 學期科目開課分項類別,
                //CAST(regexp_replace(s0.d14, '^$', '0') as decimal) as 學期科目開課學分數 from sems_subj_score inner join xpath_table('id','score_info','sems_subj_score','/SemesterSubjectScoreInfo/Subject/@科目
                //|/SemesterSubjectScoreInfo/Subject/@科目級別
                //|/SemesterSubjectScoreInfo/Subject/@不計學分
                //|/SemesterSubjectScoreInfo/Subject/@不需評分
                //|/SemesterSubjectScoreInfo/Subject/@修課必選修
                //|/SemesterSubjectScoreInfo/Subject/@修課校部訂
                //|/SemesterSubjectScoreInfo/Subject/@原始成績
                //|/SemesterSubjectScoreInfo/Subject/@學年調整成績
                //|/SemesterSubjectScoreInfo/Subject/@擇優採計成績
                //|/SemesterSubjectScoreInfo/Subject/@是否取得學分
                //|/SemesterSubjectScoreInfo/Subject/@補考成績
                //|/SemesterSubjectScoreInfo/Subject/@重修成績
                //|/SemesterSubjectScoreInfo/Subject/@開課分項類別
                //|/SemesterSubjectScoreInfo/Subject/@開課學分數'
                //,'ref_student_id in(" + string.Join(",", sids.ToArray()) + @")')
                //as s0(id integer,d1 text,d2 text,d3 text,d4 text,d5 text,d6 text,d7 text,d8 text,d9 text,d10 text,d11 text,d12 text,d13 text,d14 text) on sems_subj_score.id=s0.id 
                //{0}
                //order by sid,學期科目成績年級 asc,學期科目成績學年度 desc,學期科目成績學期";
                #endregion

                string query = @"
SELECT
	sems_subj_score_ext.ref_student_id AS sid
	, sems_subj_score_ext.grade_year AS 成績年級
	, sems_subj_score_ext.semester AS 學期
	, sems_subj_score_ext.school_year AS 學年度
	, array_to_string(xpath('//Subject/@開課分項類別', subj_score_ele), '')::text AS 分項類別
	, array_to_string(xpath('//Subject/@科目', subj_score_ele), '')::text AS 科目
	, array_to_string(xpath('//Subject/@科目級別', subj_score_ele), '')::text AS 科目級別
	, array_to_string(xpath('//Subject/@開課學分數', subj_score_ele), '')::text AS 學分數
	, array_to_string(xpath('//Subject/@是否取得學分', subj_score_ele), '')::text AS 取得學分
	, array_to_string(xpath('//Subject/@修課必選修', subj_score_ele), '')::text AS 必選修
	, array_to_string(xpath('//Subject/@修課校部訂', subj_score_ele), '')::text AS 校部訂
	, array_to_string(xpath('//Subject/@是否補修成績', subj_score_ele), '')::text AS 補修成績
	, array_to_string(xpath('//Subject/@原始成績', subj_score_ele), '')::text AS 原始成績
	, array_to_string(xpath('//Subject/@補考成績', subj_score_ele), '')::text AS 補考成績
	, array_to_string(xpath('//Subject/@重修成績', subj_score_ele), '')::text AS 重修成績
	, array_to_string(xpath('//Subject/@重修學年度', subj_score_ele), '')::text AS 重修學年度
	, array_to_string(xpath('//Subject/@重修學期', subj_score_ele), '')::text AS 重修學期
	, array_to_string(xpath('//Subject/@學年調整成績', subj_score_ele), '')::text AS 學年調整成績
	, array_to_string(xpath('//Subject/@擇優採計成績', subj_score_ele), '')::text AS 手動調整成績
	, array_to_string(xpath('//Subject/@不計學分', subj_score_ele), '')::text AS 不計學分
	, array_to_string(xpath('//Subject/@不需評分', subj_score_ele), '')::text AS 不需評分
FROM (
		SELECT 
			sems_subj_score.*
			, 	unnest(xpath('//SemesterSubjectScoreInfo/Subject', xmlparse(content score_info))) as subj_score_ele
		FROM 
			sems_subj_score 
		WHERE ref_student_id IN ({0})
	) AS sems_subj_score_ext
	{1}
ORDER BY grade_year ASC,  school_year DESC, semester  DESC
";


                query = string.Format(query, string.Join(",", sids), quWhere);
                dt = qh.Select(query);

                // 科目重讀判斷
                Dictionary<string, bool> chkSameDict = new Dictionary<string, bool>();

                foreach (DataRow dr in dt.Rows)
                {
                    string sid = dr["sid"].ToString();

                    if (!retVal.ContainsKey(sid))
                        retVal.Add(sid, new List<DataRow>());

                    // 檢查是否有重讀資料sid 年級、學期、科目名稱、科目級別
                    //string kk = sid + dr["成績年級"].ToString() + dr["學期"].ToString() + dr["科目"].ToString() + dr["科目級別"].ToString();

                    //2023-01-06
                    //https://3.basecamp.com/4399967/buckets/15765350/todos/5620389814#__recording_5701708410
                    //重讀後對開課程可能會開在不同學期，因此需要將      原key 成績年級+學期 + 科目 + 級別→改為 成績年級+科目 + 級別
                    //Q: 108 - 1 2 年級 國文III 80分，  108 - 2 2年級 國文IV 90分， 109 - 2 2年級 國文III 100分，這樣高二下國文 要算多少分? (重修高二下，但是 原本的國文III從上學期改到下學期修，而國文IV又沒有重複key，故有兩個成績)
                    //A: 可為和華商確認過不會有這種情境。
                    string kk = sid + dr["成績年級"].ToString() +  dr["科目"].ToString() + dr["科目級別"].ToString();
                    if (!chkSameDict.ContainsKey(kk))
                    {
                        chkSameDict.Add(kk, true);
                        retVal[sid].Add(dr);
                    }
                }

            }
            return retVal;
        }

        public static Dictionary<string, List<DataRow>> GetStudentSemsEntryScoreByStudentID(List<string> sids)
        {
            Dictionary<string, List<DataRow>> retVal = new Dictionary<string, List<DataRow>>();
            if (sids.Count > 0)
            {
                QueryHelper qh = new QueryHelper();
                //string sKey = string.Join(",", sids.ToArray());

                #region 舊SQL
                //                string query = @"select sems_entry_score.id as seid,ref_student_id as sid,school_year 
                //as 學年度,semester as 學期,grade_year as 年級,se1.d1 as 分項, cast(regexp_replace(se1.d2, '^$', '0') as decimal) as 成績 
                //from sems_entry_score inner join xpath_table('id','score_info','sems_entry_score','/SemesterEntryScore/Entry/@分項|/SemesterEntryScore/Entry/@成績',
                //'ref_student_id in(" + sKey + @")') as se1(id integer,d1 text,d2 character varying(10)) on sems_entry_score.id=se1.id where sems_entry_score.ref_student_id in(" + sKey + @") and sems_entry_score.entry_group=1 order by sid,年級 asc,學年度 desc,學期 asc";
                #endregion

                string query = @"
SELECT
	sems_entry_score_ext.ref_student_id AS sid
	, sems_entry_score_ext.grade_year AS 成績年級
	, sems_entry_score_ext.semester AS 學期
	, sems_entry_score_ext.school_year AS 學年度
	, array_to_string(xpath('//Entry/@分項', entry_score_ele), '')::text AS 分項
	, array_to_string(xpath('//Entry/@成績', entry_score_ele), '')::text AS 成績
FROM (
		SELECT 
			sems_entry_score.*
			, 	unnest(xpath('//SemesterEntryScore/Entry', xmlparse(content score_info))) as entry_score_ele
		FROM 
			sems_entry_score 
		WHERE 
			entry_group=1
			AND ref_student_id IN ({0})
	) as sems_entry_score_ext
ORDER BY grade_year ASC,  school_year DESC, semester DESC
";
                query = string.Format(query, string.Join(",", sids));
                DataTable dt = qh.Select(query);

                Dictionary<string, bool> chkSameDict = new Dictionary<string, bool>();

                foreach (DataRow dr in dt.Rows)
                {
                    string sid = dr["sid"].ToString();
                    if (!retVal.ContainsKey(sid))
                        retVal.Add(sid, new List<DataRow>());

                    //檢查是否有重讀資料sid 年級、學期、分項
                    string kk = sid + dr["成績年級"].ToString() + dr["學期"].ToString() + dr["分項"].ToString();
                    if (!chkSameDict.ContainsKey(kk))
                    {
                        chkSameDict.Add(kk, true);
                        retVal[sid].Add(dr);
                    }

                }
            }
            return retVal;
        }

        /// <summary>
        /// 匯出 Excel
        /// </summary>
        /// <param name="inputReportName"></param>
        /// <param name="inputXls"></param>
        public static void CompletedCSV(string inputReportName, DataTable dt)
        {
            string reportName = inputReportName;

            string path = Path.Combine(Application.StartupPath, "Reports");
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            path = Path.Combine(path, reportName + ".csv");

            Workbook wb = new Workbook(new MemoryStream(Properties.Resources.template));
            wb.Settings.Encoding = Encoding.Default;

            wb.Worksheets[0].Cells.ImportDataTable(dt, true, 0, 0);


            //int rowIdx=1;
            //int col=0;
            //foreach (DataColumn dc in dt.Columns)
            //{
            //    wb.Worksheets[0].Cells[0, col].PutValue(dc.ColumnName);
            //    col++;
            //}
            //foreach (DataRow dr in dt.Rows)
            //{
            //    col = 0;
            //    foreach (DataColumn dc in dt.Columns)
            //    {
            //        if (dc.ColumnName.Contains("總平均"))
            //        {
            //            wb.Worksheets[0].Cells[rowIdx, col].Value = string.Format("{0:###.0}", dr[dc.ColumnName]);
            //        }
            //        else
            //            wb.Worksheets[0].Cells[rowIdx, col].PutValue(dr[dc.ColumnName].ToString());
            //        col++;
            //    }
            //    rowIdx++;
            //}

            if (File.Exists(path))
            {
                int i = 1;
                while (true)
                {
                    string newPath = Path.GetDirectoryName(path) + "\\" + Path.GetFileNameWithoutExtension(path) + (i++) + Path.GetExtension(path);
                    if (!File.Exists(newPath))
                    {
                        path = newPath;
                        break;
                    }
                }
            }

            try
            {
                wb.Save(path, SaveFormat.CSV);
                //System.Diagnostics.Process.Start(path);
                System.Diagnostics.Process.Start("notepad.exe", path);
            }
            catch
            {
                SaveFileDialog sd = new SaveFileDialog();
                sd.Title = "另存新檔";
                sd.FileName = reportName + ".csv";
                sd.Filter = "CSV檔案 (*.csv)|*.csv|所有檔案 (*.*)|*.*";
                if (sd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        wb.Save(sd.FileName, SaveFormat.CSV);

                    }
                    catch
                    {
                        MsgBox.Show("指定路徑無法存取。", "建立檔案失敗", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
            }
        }

        public static void CompletedXlsCsv(string inputReportName, DataTable dt)
        {

            #region 儲存檔案
            string reportName = inputReportName;

            string path = Path.Combine(System.Windows.Forms.Application.StartupPath, "Reports");
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            path = Path.Combine(path, reportName + ".csv");

            if (File.Exists(path))
            {
                int i = 1;
                while (true)
                {
                    string newPath = Path.GetDirectoryName(path) + "\\" + Path.GetFileNameWithoutExtension(path) + (i++) + Path.GetExtension(path);
                    if (!File.Exists(newPath))
                    {
                        path = newPath;
                        break;
                    }
                }
            }

            // StreamWriter sw = new StreamWriter(path, false, System.Text.Encoding.Unicode);
            StreamWriter sw = new StreamWriter(path, false, System.Text.Encoding.Default);
            DataTable dataTable = dt;

            List<string> strList = new List<string>();
            foreach (DataColumn dc in dt.Columns)
                strList.Add(dc.ColumnName);

            sw.WriteLine(string.Join(",", strList.ToArray()));

            foreach (DataRow dr in dt.Rows)
            {
                List<string> subList = new List<string>();
                for (int col = 0; col < dt.Columns.Count; col++)
                {
                    subList.Add(dr[col].ToString());
                }
                sw.WriteLine(string.Join(",", subList.ToArray()));
            }

            sw.Close();
            try
            {
                System.Diagnostics.Process.Start("notepad.exe", path);
                //System.Diagnostics.Process.Start(path);
            }
            catch
            {
                try
                {
                    System.Windows.Forms.SaveFileDialog sd = new System.Windows.Forms.SaveFileDialog();
                    sd.Title = "另存新檔";
                    sd.FileName = reportName + ".csv";
                    sd.Filter = "csv檔案 (*.csv)|*.txt|所有檔案 (*.*)|*.*";
                    if (sd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        System.Diagnostics.Process.Start(sd.FileName);
                    }
                }
                catch
                {
                    FISCA.Presentation.Controls.MsgBox.Show("指定路徑無法存取。", "建立檔案失敗", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return;
                }
            }
            #endregion

        }

        /// <summary>
        /// 103學年度入學學生高一在校學業成績檔案規格
        /// </summary>
        /// <returns></returns>
        public static List<string> GetScoreFieldList103_1()
        {
            List<string> Value = new List<string>();
            Value.Add("學號");
            Value.Add("姓名");
            Value.Add("身分證號碼");
            Value.Add("學業總平均(高一上)");
            Value.Add("國文(高一上)");
            Value.Add("英文(高一上)");
            Value.Add("數學(高一上)");
            Value.Add("物理(高一上)");
            Value.Add("化學(高一上)");
            Value.Add("生物(高一上)");
            Value.Add("地球科學(高一上)");
            Value.Add("歷史(高一上)");
            Value.Add("地理(高一上)");
            Value.Add("公民與社會(高一上)");
            Value.Add("音樂(高一上)");
            Value.Add("美術(高一上)");
            Value.Add("舞蹈(高一上)");
            Value.Add("體育(高一上)");
            Value.Add("藝術生活(高一上)");
            Value.Add("生活科技(高一上)");
            Value.Add("家政(高一上)");
            Value.Add("資訊科技概論(高一上)");
            Value.Add("健康與護理(高一上)");
            Value.Add("全民國防教育(高一上)");
            Value.Add("學業總平均(高一下)");
            Value.Add("國文(高一下)");
            Value.Add("英文(高一下)");
            Value.Add("數學(高一下)");
            Value.Add("物理(高一下)");
            Value.Add("化學(高一下)");
            Value.Add("生物(高一下)");
            Value.Add("地球科學(高一下)");
            Value.Add("歷史(高一下)");
            Value.Add("地理(高一下)");
            Value.Add("公民與社會(高一下)");
            Value.Add("音樂(高一下)");
            Value.Add("美術(高一下)");
            Value.Add("舞蹈(高一下)");
            Value.Add("體育(高一下)");
            Value.Add("藝術生活(高一下)");
            Value.Add("生活科技(高一下)");
            Value.Add("家政(高一下)");
            Value.Add("資訊科技概論(高一下)");
            Value.Add("健康與護理(高一下)");
            Value.Add("全民國防教育(高一下)");
            Value.Add("就讀科、學程、班別");
            Value.Add("班級");
            return Value;
        }
    }

    public class RetakeScoreInfo
    {
        public string StudentID { get; set; }
        public string SchoolYear { get; set; }
        public string Semester { get; set; }
        public string Subject { get; set; }
        public string SubjectLevel { get; set; }
        public string RetakeScore { get; set; }
    }

    public static class RetakeScoreHelper
    {
        /// <summary>
        /// 取得多位學生的再次修習成績資料
        /// </summary>
        /// <param name="studentIDs">學生ID清單</param>
        /// <returns>List of 再次修習成績資料</returns>
        public static List<RetakeScoreInfo> GetRetakeScores(List<string> studentIDs)
        {
            List<RetakeScoreInfo> result = new List<RetakeScoreInfo>();
            if (studentIDs == null || studentIDs.Count == 0)
                return result;

            string idList = string.Join(",", studentIDs.Select(id => $"'{id}'"));

            string sql = $@"
            WITH student_score AS (
                SELECT
                    ref_student_id AS student_id,
                    school_year,
                    semester,
                    subject,
                    subj_level,
                    jsonb_array_elements(detail) AS detail
                FROM
                    student_learning_history
                WHERE
                    serial_no = '5.3'
                    AND name = '重讀成績'
                    AND ref_student_id IN ({idList})
            ),
            ranked_scores AS (
                SELECT
                    student_id,
                    school_year,
                    semester,
                    subject,
                    subj_level,
                    detail->>'name' AS name,
                    detail->>'value' AS value,
                    ROW_NUMBER() OVER (
                        PARTITION BY student_id, subject, subj_level 
                        ORDER BY school_year DESC, semester DESC
                    ) AS rn
                FROM student_score
                WHERE detail->>'name' = '再次修習成績'
            )
            SELECT
                student_id,
                school_year,
                semester,
                subject,
                subj_level,
                name,
                value
            FROM ranked_scores
            WHERE rn = 1
            ORDER BY student_id, school_year, semester, subject;
        ";

            FISCA.Data.QueryHelper qh = new FISCA.Data.QueryHelper();
            System.Data.DataTable dt = qh.Select(sql);

            foreach (System.Data.DataRow dr in dt.Rows)
            {
                result.Add(new RetakeScoreInfo
                {
                    StudentID = dr["student_id"] + "",
                    SchoolYear = dr["school_year"] + "",
                    Semester = dr["semester"] + "",
                    Subject = dr["subject"] + "",
                    SubjectLevel = dr["subj_level"] + "",
                    RetakeScore = dr["value"] + ""
                });
            }

            return result;
        }
    }

    public enum RoundMode
    {
        四捨五入,
        無條件捨去,
        無條件進位
    }

    public static class ScoreCalcHelper
    {
        /// <summary>
        /// 計算所有學生各年級學期的分項成績（如學業、體育等），回傳可供比對的結果。
        /// </summary>
        /// <param name="SemsSubjDataDict">學生ID對應的所有科目成績資料</param>
        /// <param name="getScoreCalcRule">取得指定學生的成績計算規則方法</param>
        /// <returns>Dictionary<sid, Dictionary<(年級,學期), Dictionary<分項,分數>>> </returns>
        public static Dictionary<string, Dictionary<(int, int), Dictionary<string, decimal>>>
            CalculateAllStudentSemesterEntryScores(
                Dictionary<string, List<DataRow>> SemsSubjDataDict,
                Func<string, System.Xml.XmlElement> getScoreCalcRule)
        {
            var resultDict = new Dictionary<string, Dictionary<(int, int), Dictionary<string, decimal>>>();

            foreach (var sid in SemsSubjDataDict.Keys)
            {
                var subjRows = SemsSubjDataDict[sid];
                var gradeSemesterGroups = subjRows
                    .GroupBy(dr => new
                    {
                        GradeYear = dr["成績年級"].ToString(),
                        Semester = dr["學期"].ToString()
                    });

                foreach (var group in gradeSemesterGroups)
                {
                    int gradeYear = int.Parse(group.Key.GradeYear);
                    int semester = int.Parse(group.Key.Semester);

                    var entryScores = CalcSemesterEntryScore(subjRows, sid, gradeYear, semester, getScoreCalcRule, true);

                    if (!resultDict.ContainsKey(sid))
                        resultDict[sid] = new Dictionary<(int, int), Dictionary<string, decimal>>();

                    resultDict[sid][(gradeYear, semester)] = entryScores;
                }
            }
            return resultDict;
        }

        /// <summary>
        /// 依據學生成績計算規則，計算該生指定成績年級學期的分項成績（如學業、體育等）。
        /// </summary>
        public static Dictionary<string, decimal> CalcSemesterEntryScore(
            List<DataRow> subjScoreRows,
            string studentID,
            int gradeYear,
            int semester,
            Func<string, System.Xml.XmlElement> getScoreCalcRule,
            bool chkSScore)
        {
                    // Todo 2: 新增 Debug 功能 - 清空 debug.txt 檔案
        // try
        // {
        //     if (System.IO.File.Exists("debug.txt"))
        //         System.IO.File.Delete("debug.txt");
        // }
        // catch (Exception ex)
        // {
        //     // 如果無法刪除檔案，可以選擇忽略或記錄錯誤
        //     Console.WriteLine($"[WARNING] 無法清空 debug.txt: {ex.Message}");
        // }

        // Todo 2: 記錄方法開始資訊
        // try
        // {
        //     System.IO.File.AppendAllText("debug.txt", $"[DEBUG] ===== CalcSemesterEntryScore 開始 =====\n");
        //     System.IO.File.AppendAllText("debug.txt", $"[DEBUG] 學生ID: {studentID}, 年級: {gradeYear}, 學期: {semester}\n");
        //     System.IO.File.AppendAllText("debug.txt", $"[DEBUG] 使用成績類型: {(chkSScore ? "原始成績" : "原始及補考成績擇優")}\n\n");
        // }
        // catch (Exception ex)
        // {
        //     Console.WriteLine($"[ERROR] 無法寫入 debug.txt: {ex.Message}");
        // }

            var ruleElement = getScoreCalcRule(studentID);
            int decimals = 2;
            RoundMode mode = RoundMode.四捨五入; // 預設
            var takeScoreFields = new List<string> { "原始成績" };

            // 解析規則
            if (ruleElement != null)
            {
                var node = ruleElement.SelectSingleNode("各項成績計算位數/學期分項成績計算位數");
                if (node != null && int.TryParse((node as System.Xml.XmlElement)?.GetAttribute("位數"), out int d))
                    decimals = d;
                bool tryParsebool;
                if (bool.TryParse((node as System.Xml.XmlElement)?.GetAttribute("四捨五入"), out tryParsebool) && tryParsebool)
                    mode = RoundMode.四捨五入;
                if (bool.TryParse((node as System.Xml.XmlElement)?.GetAttribute("無條件捨去"), out tryParsebool) && tryParsebool)
                    mode = RoundMode.無條件捨去;
                if (bool.TryParse((node as System.Xml.XmlElement)?.GetAttribute("無條件進位"), out tryParsebool) && tryParsebool)
                    mode = RoundMode.無條件進位;
                var takeNode = ruleElement.SelectSingleNode("分項成績計算採計成績欄位");
                if (takeNode != null)
                {
                    takeScoreFields.Clear();
                    foreach (string field in new[] { "原始成績", "補考成績" })
                    {
                        if (((System.Xml.XmlElement)takeNode).GetAttribute(field) == "True")
                            takeScoreFields.Add(field);
                    }
                }
            }
            // 根據 chkSScore 覆蓋 takeScoreFields
            if (chkSScore)
            {
                takeScoreFields.Clear();
                takeScoreFields.Add("原始成績");
            }
            else
            {
                takeScoreFields.Clear();
                takeScoreFields.AddRange(new[] { "原始成績", "補考成績" });
            }

            var entryCreditCount = new Dictionary<string, decimal>();
            var entryDividend = new Dictionary<string, decimal>();
            // Todo 1: 新增科目名稱記錄字典
            var entrySubjects = new Dictionary<string, List<string>>();

            foreach (var dr in subjScoreRows)
            {
                if (dr["成績年級"].ToString() == gradeYear.ToString() && dr["學期"].ToString() == semester.ToString())
                {
                    if (dr.Table.Columns.Contains("不需評分") && dr["不需評分"].ToString() == "是")
                        continue;
                    if (dr.Table.Columns.Contains("不計學分") && dr["不計學分"].ToString() == "是")
                        continue;
                    if (dr.Table.Columns.Contains("補修成績") && dr["補修成績"].ToString() == "是")
                        continue;

                    string entry = dr["分項類別"].ToString();
                    if (entry == "學業")
                        entry = "學業成績總平均";

                    // Todo 1: 取得科目名稱
                    string subjectName = "";
                    if (dr.Table.Columns.Contains("科目"))
                        subjectName = dr["科目"].ToString();
                    else if (dr.Table.Columns.Contains("subject"))
                        subjectName = dr["subject"].ToString();

                    // Todo 1: 記錄科目名稱到對應的分項
                    if (!entrySubjects.ContainsKey(entry))
                        entrySubjects[entry] = new List<string>();
                    if (!string.IsNullOrEmpty(subjectName) && !entrySubjects[entry].Contains(subjectName))
                        entrySubjects[entry].Add(subjectName);

                                // Todo 2: Debug 記錄科目名稱到檔案
            // try
            // {
            //     System.IO.File.AppendAllText("debug.txt", $"[DEBUG] 科目名稱: {subjectName}\n");
            // }
            // catch (Exception ex)
            // {
            //     Console.WriteLine($"[ERROR] 無法寫入 debug.txt: {ex.Message}");
            // }

                    decimal credit = 0, maxScore = 0, tryScore = 0;
                    decimal.TryParse(dr["學分數"].ToString(), out credit);

                    foreach (var field in takeScoreFields)
                    {
                        if (dr.Table.Columns.Contains(field) && decimal.TryParse(dr[field].ToString(), out tryScore))
                            if (tryScore > maxScore) maxScore = tryScore;
                    }

                                // Todo 2: Debug 記錄所有計算值到檔案
            // try
            // {
            //     System.IO.File.AppendAllText("debug.txt", $"[DEBUG] 最高成績 (maxScore): {maxScore}\n");
            //     System.IO.File.AppendAllText("debug.txt", $"[DEBUG] 學分數 (credit): {maxScore}\n");
            //     System.IO.File.AppendAllText("debug.txt", $"[DEBUG] 加權分數 (maxScore * credit): {maxScore * credit}\n");
            //     System.IO.File.AppendAllText("debug.txt", $"[DEBUG] 科目:{subjectName} | 最高成績:{maxScore} | 學分數:{credit} | 加權分數:{maxScore * credit}\n\n");
            // }
            // catch (Exception ex)
            // {
            //     Console.WriteLine($"[ERROR] 無法寫入 debug.txt: {ex.Message}");
            // }

                    if (!entryCreditCount.ContainsKey(entry))
                        entryCreditCount[entry] = 0;
                    if (!entryDividend.ContainsKey(entry))
                        entryDividend[entry] = 0;

                    entryCreditCount[entry] += credit;
                    entryDividend[entry] += maxScore * credit;
                }
            }

            var result = new Dictionary<string, decimal>();
            foreach (var entry in entryCreditCount.Keys)
            {
                if (entryCreditCount[entry] > 0)
                {
                    decimal avg = entryDividend[entry] / entryCreditCount[entry];
                    decimal pow = (decimal)Math.Pow(10, decimals);
                    decimal rounded;
                    if (mode == RoundMode.無條件捨去)
                        rounded = Math.Floor(avg * pow) / pow;
                    else if (mode == RoundMode.無條件進位)
                        rounded = Math.Ceiling(avg * pow) / pow;
                    else // 四捨五入
                        rounded = Math.Round(avg, decimals, MidpointRounding.AwayFromZero);
                    result[entry] = rounded;
                }
            }

            // Todo 2: 記錄分項成績計算結果
            // try
            // {
            //     System.IO.File.AppendAllText("debug.txt", $"[DEBUG] ===== 分項成績計算結果 =====\n");
            //     foreach (var entry in result.Keys)
            //     {
            //     //     System.IO.File.AppendAllText("debug.txt", $"[DEBUG] {entry}: {result[entry]}\n");
            //     }
            //     System.IO.File.AppendAllText("debug.txt", $"[DEBUG] ===============================\n");
            //     System.IO.File.AppendAllText("debug.txt", $"[DEBUG] ===== CalcSemesterEntryScore 結束 =====\n\n");
            // }
            // catch (Exception ex)
            // {
            //     Console.WriteLine($"[ERROR] 無法寫入 debug.txt: {ex.Message}");
            // }

            return result;
        }
    }
}
