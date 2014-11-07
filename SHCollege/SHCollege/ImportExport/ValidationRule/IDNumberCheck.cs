using Campus.DocumentValidator;
using FISCA.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SHCollege.ImportExport.ValidationRule
{
    public class IDNumberCheck : IFieldValidator
    {
        List<string> IDNumList;

        public IDNumberCheck() 
        {
            IDNumList = new List<string>();
            QueryHelper qh = new QueryHelper();
            string query = "select id_number from student where status=1 and id_number is not null";
            DataTable dt = qh.Select(query);
            foreach (DataRow dr in dt.Rows)
                IDNumList.Add(dr["id_number"].ToString());
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
            return IDNumList.Contains(Value);
        }
    }
}
