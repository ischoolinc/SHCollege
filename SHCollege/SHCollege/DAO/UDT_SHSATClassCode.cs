using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FISCA.UDT;


namespace SHCollege.DAO
{
    /// <summary>
    /// 大學繁星-班級代碼
    /// </summary>
    [TableName("sh.college.sat.class_code")]
    public class UDT_SHSATClassCode:ActiveRecord
    {
      
        ///<summary>
        /// 班級名稱
        ///</summary>
        [Field(Field = "class_name", Indexed = false)]
        public string ClassName { get; set; }

        ///<summary>
        /// 班級代碼
        ///</summary>
        [Field(Field = "class_code", Indexed = false)]
        public string ClassCode { get; set; }        
    }
}
