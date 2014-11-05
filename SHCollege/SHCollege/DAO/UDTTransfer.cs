using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FISCA.UDT;
using FISCA.Data;
using System.Data;
using FISCA.DSAUtil;

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
        public static Dictionary<string, UDT_SHSATStudent> GetSATStudentByStudentIDList(List<string> StudentIDList)
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
    }
}
