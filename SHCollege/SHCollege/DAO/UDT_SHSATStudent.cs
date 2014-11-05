using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FISCA.UDT;

namespace SHCollege.DAO
{

    /// <summary>
    /// 大學繁星-學生學測報名序號
    /// </summary>
    [TableName("sh.college.sat.student")]
    public class UDT_SHSATStudent:ActiveRecord
    {
        ///<summary>
        /// 學生系統編號
        ///</summary>
        [Field(Field = "ref_student_id", Indexed = false)]
        public string RefStudentID { get; set; }

        ///<summary>
        /// 學號
        ///</summary>
        [Field(Field = "student_number", Indexed = false)]
        public string StudentNumber { get; set; }

        ///<summary>
        /// 身分證號
        ///</summary>
        [Field(Field = "id_number", Indexed = false)]
        public string IDNumber { get; set; }

        ///<summary>
        /// 學測報名序號
        ///</summary>
        [Field(Field = "sat_ser_no", Indexed = false)]
        public string SatSerNo { get; set; }

        ///<summary>
        /// 學測報名時班級
        ///</summary>
        [Field(Field = "sat_class_name", Indexed = false)]
        public string SatClassName { get; set; }

        ///<summary>
        /// 學測報名時座號
        ///</summary>
        [Field(Field = "sat_seat_no", Indexed = false)]
        public string SatSeatNo { get; set; }

    }
}
