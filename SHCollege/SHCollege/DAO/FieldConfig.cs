using FISCA.UDT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SHCollege.DAO
{
    /// <summary>
    /// 大學推甄欄位對照設定檔
    /// </summary>
    [TableName("sh.college_score_mapping")]
    public class FieldConfig:ActiveRecord
    {
        ///<summary>
        /// 欄位名稱
        ///</summary>
        [Field(Field = "field_name", Indexed = true)]
        public string FieldName { get; set; }

        ///<summary>
        /// 欄位對照
        ///</summary>
        [Field(Field = "field_mapping", Indexed = false)]
        public string FieldMapping { get; set; }

        ///<summary>
        /// 欄位順序
        ///</summary>
        [Field(Field = "field_order", Indexed = false)]
        public int FieldOrder { get; set; }     
    }
}
