using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FISCA.UDT;
using FISCA.Data;
using System.Data;
using FISCA.DSAUtil;
using K12.Data;

namespace SHCollege.DAO
{
    public class UDTTransfer
    {
        /// <summary>
        /// 取得系統內所有一般狀態學生ID
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, string> GetStudentNumIDDictAll()
        {
            Dictionary<string, string> retVal = new Dictionary<string, string>();
            QueryHelper qh = new QueryHelper();
            DataTable dt = qh.Select("select student_number,id from student where status=1 and student_number is not null order by student_number");
            foreach (DataRow dr in dt.Rows)
            {
                string key = dr["student_number"].ToString();
                if (!retVal.ContainsKey(key))
                    retVal.Add(key, dr["id"].ToString());

            }
            return retVal;
        }

        /// <summary>
        /// 取得系統內所有一般狀態學生身分證號,ID
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, string> GetStudentIDNumIDDictAll()
        {
            Dictionary<string, string> retVal = new Dictionary<string, string>();
            QueryHelper qh = new QueryHelper();
            DataTable dt = qh.Select("select id_number,id from student where status=1 and id_number is not null order by id_number");
            foreach (DataRow dr in dt.Rows)
            {
                string key = dr["id_number"].ToString();
                if (!retVal.ContainsKey(key))
                    retVal.Add(key, dr["id"].ToString());

            }
            return retVal;
        }

        /// <summary>
        /// 建立使用到的 UDT Table
        /// </summary>
        public static void CreateUDTTable()
        {
            FISCA.UDT.SchemaManager Manager = new SchemaManager(new DSConnection(FISCA.Authentication.DSAServices.DefaultDataSource));
            Manager.SyncSchema(new UDT_SHSATStudent());
        }


        /// <summary>
        /// 透過學生ID取得大學繁星學測學生
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, UDT_SHSATStudent> GetSATStudentByStudentIDListDict(List<string> StudentIDList)
        {
            Dictionary<string, UDT_SHSATStudent> retVal = new Dictionary<string, UDT_SHSATStudent>();
            if (StudentIDList.Count > 0)
            {
                AccessHelper accessHelper = new AccessHelper();
                string query = "ref_student_id in('" + string.Join("','", StudentIDList.ToArray()) + "')";
                List<UDT_SHSATStudent> SHSATStudentList = accessHelper.Select<UDT_SHSATStudent>(query);
                foreach (UDT_SHSATStudent data in SHSATStudentList)
                {
                    if (!retVal.ContainsKey(data.RefStudentID))
                        retVal.Add(data.RefStudentID, data);
                }
            }
            return retVal;
        }

        /// <summary>
        /// 透過學生ID取得大學繁星學測學生
        /// </summary>
        /// <returns></returns>
        public static List<UDT_SHSATStudent> GetSATStudentByStudentIDListList(List<string> StudentIDList)
        {
            List<UDT_SHSATStudent> retVal = new List<UDT_SHSATStudent>();
            if (StudentIDList.Count > 0)
            {
                AccessHelper accessHelper = new AccessHelper();
                string query = "ref_student_id in('" + string.Join("','", StudentIDList.ToArray()) + "')";
                retVal = accessHelper.Select<UDT_SHSATStudent>(query);
            }
            return retVal;
        }

        /// <summary>
        /// 透過學生ID取得學生資料
        /// </summary>
        /// <param name="StudentIDList"></param>
        /// <returns></returns>
        public static Dictionary<string, StudentRecord> GetStudentDictByStudentList(List<string> StudentIDList)
        {
            Dictionary<string, StudentRecord> retVal = new Dictionary<string, StudentRecord>();

            List<StudentRecord> studList=Student.SelectByIDs(StudentIDList);
            foreach (StudentRecord stud in studList)
                retVal.Add(stud.ID, stud);
                        
            return retVal;
        }

        /// <summary>
        /// 取得系統內所有大學繁星學測學生
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, UDT_SHSATStudent> GetSHSATStudentListDictAll()
        {
            Dictionary<string, UDT_SHSATStudent> retVal = new Dictionary<string, UDT_SHSATStudent>();

            AccessHelper accessHelper = new AccessHelper();
            List<UDT_SHSATStudent> SHSATStudentList = accessHelper.Select<UDT_SHSATStudent>();
            foreach (UDT_SHSATStudent data in SHSATStudentList)
            {
                if (!retVal.ContainsKey(data.RefStudentID))
                    retVal.Add(data.RefStudentID, data);
            }

            return retVal;
        }

        /// <summary>
        /// 刪除學測報名資料
        /// </summary>
        /// <param name="StudentIDList"></param>
        public static void DelSHSATStudentListByStudentIDList(List<string> StudentIDList)
        {
            if (StudentIDList.Count > 0)
            {
                AccessHelper accessHelper = new AccessHelper();
                string query = "ref_student_id in('" + string.Join("','", StudentIDList.ToArray()) + "')";
                List<UDT_SHSATStudent> SHSATStudentList = accessHelper.Select<UDT_SHSATStudent>(query);
                foreach (UDT_SHSATStudent data in SHSATStudentList)
                    data.Deleted = true;

                SHSATStudentList.SaveAll();
            }
        
        }

        /// <summary>
        /// 透過學生ID,科別，取得學生報考學程對照
        /// </summary>
        /// <param name="StudentIDList"></param>
        /// <returns></returns>
        public static Dictionary<string, string> GetStudentSATDeptMappingDict(List<string> StudentIDList)
        {
            Dictionary<string, string> mappingDict = new Dictionary<string, string>();
            mappingDict.Add("普通科", "1");
            mappingDict.Add("普通科科學班", "1");
            mappingDict.Add("普通科資優班", "1");
            mappingDict.Add("普通科音樂班", "2");
            mappingDict.Add("普通科美術班", "3");
            mappingDict.Add("普通科舞蹈班", "4");
            mappingDict.Add("普通科體育班", "5");
            mappingDict.Add("綜合高中學術學程", "6");


            Dictionary<string, string> retVal = new Dictionary<string, string>();
            Dictionary<string, string> DeptDict = new Dictionary<string, string>();

            if (StudentIDList.Count > 0)
            {
                // 取得學生所屬班級科別
                QueryHelper qh1 = new QueryHelper();
                string query1 = "select student.id as sid,dept.name as deptname from student inner join class on student.ref_class_id=class.id inner join dept on class.ref_dept_id=dept.id where student.id in(" + string.Join(",", StudentIDList.ToArray()) + ")";
                DataTable dt1 = qh1.Select(query1);
                foreach (DataRow dr1 in dt1.Rows)
                {
                    string sid = dr1["sid"].ToString();

                    if (!DeptDict.ContainsKey(sid))
                        DeptDict.Add(sid, dr1["deptname"].ToString().Trim().Replace(":","").Replace("：",""));
                }

                // 取得學生本身科別，如果有覆蓋。
                QueryHelper qh2 = new QueryHelper();
                string query2 = "select student.id as sid,dept.name as deptname from student inner join dept on student.ref_dept_id=dept.id where student.id in("+string.Join(",",StudentIDList.ToArray())+")";
                DataTable dt2 = qh2.Select(query2);
                foreach (DataRow dr2 in dt2.Rows)
                {
                    string sid = dr2["sid"].ToString();
                    string deptName = dr2["deptname"].ToString().Trim().Replace(":", "").Replace("：", "");
                    if (!DeptDict.ContainsKey(sid))
                        DeptDict.Add(sid, deptName);
                    else
                        DeptDict[sid] = deptName;
                }

          
                // 比對學程資料並填入，沒有資料填空白
                foreach (string key in DeptDict.Keys)
                {
                    // 預設値
                    string no = "-1";
                    foreach (string k in mappingDict.Keys)
                    {

                        // 使用完整比對
                        if (DeptDict[key] == k)
                        {
                            no = mappingDict[k];
                            break;
                        }

                        //if (DeptDict[key].Contains(k))
                        //{
                        //    no = mappingDict[k];
                        //    break;
                        //}
                    }
                    retVal.Add(key, no);
                }
            }
            return retVal;
        }


        /// <summary>
        /// 取得班級代碼
        /// </summary>
        /// <returns></returns>
        public static List<UDT_SHSATClassCode> GetClassCodeList()
        {
            List<UDT_SHSATClassCode> retVal = new List<UDT_SHSATClassCode>();
            
            // 取得 UDT ClassCode
            AccessHelper accessHelper = new AccessHelper();
            List<UDT_SHSATClassCode> SHSATClassCodeList = accessHelper.Select<UDT_SHSATClassCode>();
            
            // 如果沒有資料，產生預設名稱，讀取系統內班級，依年級、班級名稱排序
            if (SHSATClassCodeList.Count == 0)
            {
                QueryHelper qh = new QueryHelper();
                string query = "select distinct class.id as cid,class_name,class.grade_year from class inner join student on class.id=student.ref_class_id where student.status=1 order by class.grade_year desc,class_name";
                DataTable dt = qh.Select(query);
                foreach (DataRow dr in dt.Rows)
                {
                    UDT_SHSATClassCode code = new UDT_SHSATClassCode();
                    code.ClassName = dr["class_name"].ToString();
                    code.GradeYear = 0;
                    int gg;
                    if (int.TryParse(dr["grade_year"].ToString(), out gg))
                        code.GradeYear = gg;
                    retVal.Add(code);
                }
            }
            else
            {
                Dictionary<string, ClassRecord> cNameDict = new Dictionary<string, ClassRecord>();
                List<ClassRecord> ccList = Class.SelectAll();
                foreach (ClassRecord cr in ccList)
                {
                    cNameDict.Add(cr.Name, cr);
                }

                foreach (UDT_SHSATClassCode code in SHSATClassCodeList)
                {
                    if (cNameDict.ContainsKey(code.ClassName))
                    {
                        code.GradeYear = 0;
                        if (cNameDict[code.ClassName].GradeYear.HasValue)
                            code.GradeYear = cNameDict[code.ClassName].GradeYear.Value;
                    }
                }

                retVal = SHSATClassCodeList;
            }
            return retVal;        
        }
    }
}
