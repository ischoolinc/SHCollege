using Campus.DocumentValidator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FISCA.Data;
using System.Data;

namespace SHCollege.ImportExport.ValidationRule
{
    public class StudentNumberCheck : IFieldValidator
    {
        List<string> SNumList;

        public StudentNumberCheck()
        {
            SNumList = new List<string>();
            QueryHelper qh = new QueryHelper();
            string query = "select student_number from student where status=1 and student_number is not null";
            DataTable dt = qh.Select(query);
            foreach (DataRow dr in dt.Rows)
                SNumList.Add(dr["student_number"].ToString());
        }

        public string Correct(string Value)
        {
            return string.Empty;
        }

        public string ToString(string template)
        {
            return template;
        }

        public bool Validate(string Value)
        {
            return SNumList.Contains(Value);
        }
    }
}
