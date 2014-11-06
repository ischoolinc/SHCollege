using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SHCollege.DAO
{
    /// <summary>
    /// log 值用
    /// </summary>
    public class LogValue
    {
        /// <summary>
        /// 名稱
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 舊值
        /// </summary>
        public string OldValue { get; set; }
        /// <summary>
        /// 新值
        /// </summary>
        public string NewValue { get; set; }

        /// <summary>
        /// 值是否相同
        /// </summary>
        /// <returns></returns>
        public bool isSame()
        {
            if (OldValue == NewValue)
                return true;
            else
                return false;
        }

        /// <summary>
        /// 取的新舊值改變字串
        /// </summary>
        /// <returns></returns>
        public string getChangeString1()
        {
            return "[" + Name + "]由「" + OldValue + "」改變為「" + NewValue + "」;";        
        }

        /// <summary>
        /// 取得新增值
        /// </summary>
        /// <returns></returns>
        public string getInsertString1()
        {
            return "[" + Name + "]由「」改變為「" + NewValue + "」;";        
        }

        /// <summary>
        /// 取得刪除值
        /// </summary>
        /// <returns></returns>
        public string getDeleteString1()
        {
            return "[" + Name + "]由「" + OldValue + "」被刪除;";
        }
    }
}
