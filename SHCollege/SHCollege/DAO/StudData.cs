using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SHCollege.DAO
{
    /// <summary>
    /// 學生學測資料排序用
    /// </summary>
    public class StudData
    {
        public string StudentID { get; set; }

        /// <summary>
        /// 班級代碼
        /// </summary>
        public string ClassCode { get; set; }

        /// <summary>
        /// 座號
        /// </summary>
        public string SeatNo { get; set; }

    
        /// <summary>
        /// 學號
        /// </summary>
        public string StudentNumber { get; set; }

        /// <summary>
        /// 身分證號
        /// </summary>
        public string IDNumber { get; set; }
    }
}
