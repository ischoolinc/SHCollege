using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FISCA.LogAgent;

namespace SHCollege.DAO
{
    /// <summary>
    /// Log記錄轉換者
    /// </summary>
    public class LogTransfer
    {
        Dictionary<string, LogValue> _LogValueDict;


        Dictionary<string, Dictionary<string, Dictionary<string, string>>> _BatchInsertLog;
        Dictionary<string, Dictionary<string, Dictionary<string, string>>> _BatchUpdateLog;
        Dictionary<string, Dictionary<string, Dictionary<string, string>>> _BatchDeleteLog;

        public LogTransfer()
        {
            _LogValueDict = new Dictionary<string, LogValue>();
            _BatchInsertLog = new Dictionary<string, Dictionary<string, Dictionary<string, string>>>();
            _BatchUpdateLog = new Dictionary<string, Dictionary<string, Dictionary<string, string>>>();
            _BatchDeleteLog = new Dictionary<string, Dictionary<string, Dictionary<string, string>>>();
        }

        /// <summary>
        /// 取得大批新增log
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, Dictionary<string, Dictionary<string, string>>> GetBatchInsertLog()
        {
            return _BatchInsertLog;
        }

        /// <summary>
        /// 取得大批更新 log
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, Dictionary<string, Dictionary<string, string>>> GetBatchUpdateLog()
        {
            return _BatchUpdateLog;
        }

        /// <summary>
        /// 取得大批刪除 log
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, Dictionary<string, Dictionary<string, string>>> GetBatchDeleteLog()
        {
            return _BatchDeleteLog; ;
        }

        /// <summary>
        /// 新增時studentid,uid,fieldname,value
        /// </summary>
        /// <param name="key1"></param>
        /// <param name="key2"></param>
        /// <param name="FieldName"></param>
        /// <param name="Value"></param>
        public void AddBatchInsertLog(string key1, string key2, string FieldName, string Value)
        {
            if (_BatchInsertLog.ContainsKey(key1))
            {
                if (_BatchInsertLog[key1].ContainsKey(key2))
                {
                    if (!_BatchInsertLog.ContainsKey(FieldName))
                        _BatchInsertLog[key1][key2].Add(FieldName, Value);
                }
                else
                    _BatchInsertLog[key1].Add(key2, NewBatchSubLogVal(FieldName, Value));
            }
            else
                _BatchInsertLog.Add(key1, NewBacthLogVal(key2, FieldName, Value));                
            
        }

        private Dictionary<string, string> NewBatchSubLogVal(string FieldName, string value)
        {
            Dictionary<string, string> retVal = new Dictionary<string, string>();
            retVal.Add(FieldName, value);
            return retVal;
        }

        private Dictionary<string,Dictionary<string,string>> NewBacthLogVal(string key,string FieldName,string value)
        {
            Dictionary<string,string> val = new Dictionary<string,string> ();
            val.Add(FieldName,value);
            Dictionary<string, Dictionary<string, string>> retVal = new Dictionary<string, Dictionary<string, string>>();
            retVal.Add(key, val);
            return retVal;
        }

        public void AddBatchDeleteLog(string key1, string key2, string FieldName, string Value)
        {
            if (_BatchDeleteLog.ContainsKey(key1))
            {
                if (_BatchDeleteLog[key1].ContainsKey(key2))
                {
                    if (!_BatchDeleteLog.ContainsKey(FieldName))
                        _BatchDeleteLog[key1][key2].Add(FieldName, Value);
                }
                else
                    _BatchDeleteLog[key1].Add(key2, NewBatchSubLogVal(FieldName, Value));
            }
            else
                _BatchDeleteLog.Add(key1, NewBacthLogVal(key2, FieldName, Value)); 
        }

        public void AddBatchUpdateLog(string key1, string key2, string FieldName, string OldValue,string NewValue)
        {
            // 有差異才記錄
            if (OldValue != NewValue)
            {
                string Value = "由「 " + OldValue + " 」改變為 「 " + NewValue + " 」";
                if (_BatchUpdateLog.ContainsKey(key1))
                {
                    if (_BatchUpdateLog[key1].ContainsKey(key2))
                    {
                        if (!_BatchUpdateLog.ContainsKey(FieldName))
                            _BatchUpdateLog[key1][key2].Add(FieldName, Value);
                    }
                    else
                        _BatchUpdateLog[key1].Add(key2, NewBatchSubLogVal(FieldName, Value));
                }
                else
                    _BatchUpdateLog.Add(key1, NewBacthLogVal(key2, FieldName, Value));             
            }
        }

        /// <summary>
        /// 清除批次catch
        /// </summary>
        public void ClearBatchLog()
        {
            _BatchDeleteLog.Clear();
            _BatchInsertLog.Clear();
            _BatchUpdateLog.Clear();
        }

        /// <summary>
        /// 設定Log值
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="Value"></param>
        public void SetLogValue(string Name, string Value)
        {
            if (!_LogValueDict.ContainsKey(Name))
            {
                LogValue lv = new LogValue();
                lv.Name = Name;
                lv.OldValue = Value;
                lv.NewValue = "";
                _LogValueDict.Add(Name, lv);
            }
            else
            {
                _LogValueDict[Name].NewValue = Value;
            }
        }

        /// <summary>
        /// 取得LogValueList
        /// </summary>
        /// <returns></returns>
        public List<LogValue> getLogValueList()
        {
            return _LogValueDict.Values.ToList();
        }

        /// <summary>
        /// 請除LogValueList值
        /// </summary>
        public void Clear()
        {
            _LogValueDict.Clear();
        }

        /// <summary>
        /// 改變Log
        /// </summary>
        /// <param name="ActionBy"></param>
        /// <param name="Action"></param>
        /// <param name="BeforeString"></param>
        /// <param name="AfterString"></param>
        /// <param name="LogValueList"></param>
        /// <param name="targetCategory"></param>
        /// <param name="targetID"></param>
        public void SaveChangeLog(string ActionBy,string Action,string BeforeString,string AfterString, string targetCategory, string targetID)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(BeforeString);
            foreach (LogValue lv in GetDiffentLogValueList())
                    sb.AppendLine(lv.getChangeString1());
            sb.Append(AfterString);
            FISCA.LogAgent.ApplicationLog.Log(ActionBy, Action, targetCategory, targetID, sb.ToString());
        }

        /// <summary>
        /// 新增 Log
        /// </summary>
        /// <param name="ActionBy"></param>
        /// <param name="Action"></param>
        /// <param name="BeforeString"></param>
        /// <param name="AfterString"></param>
        /// <param name="LogValueList"></param>
        /// <param name="targetCategory"></param>
        /// <param name="targetID"></param>
        public void SaveInsertLog(string ActionBy, string Action, string BeforeString, string AfterString, string targetCategory, string targetID)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(BeforeString);
            foreach (LogValue lv in GetDiffentLogValueList())
                sb.AppendLine(lv.getInsertString1());

            sb.Append(AfterString);
            FISCA.LogAgent.ApplicationLog.Log(ActionBy, Action,targetCategory, targetID, sb.ToString());
        }

        /// <summary>
        /// 刪除Log
        /// </summary>
        /// <param name="ActionBy"></param>
        /// <param name="Action"></param>
        /// <param name="BeforeString"></param>
        /// <param name="AfterString"></param>
        /// <param name="LogValueList"></param>
        /// <param name="targetCategory"></param>
        /// <param name="targetID"></param>
        public void SaveDeleteLog(string ActionBy, string Action, string BeforeString, string AfterString, string targetCategory, string targetID)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(BeforeString);
            foreach (LogValue lv in GetDiffentLogValueList())
                sb.AppendLine(lv.getDeleteString1());

            sb.Append(AfterString);
            FISCA.LogAgent.ApplicationLog.Log(ActionBy, Action, targetCategory, targetID, sb.ToString());
        }

        /// <summary>
        /// 單筆儲存
        /// </summary>
        /// <param name="ActionBy"></param>
        /// <param name="Action"></param>
        /// <param name="targetCategory"></param>
        /// <param name="targetID"></param>
        /// <param name="LogData"></param>
        public void SaveLog(string ActionBy, string Action, string targetCategory, string targetID, StringBuilder LogData)
        { 
            FISCA.LogAgent.ApplicationLog.Log(ActionBy, Action, targetCategory, targetID, LogData.ToString());            
        }

        /// <summary>
        /// 取得差異 log
        /// </summary>
        /// <returns></returns>
        public List<LogValue> GetDiffentLogValueList()
        {
            List<LogValue> retVal = new List<LogValue>();
            foreach (LogValue lv in getLogValueList())
            {
                if (lv.OldValue != lv.NewValue)
                    retVal.Add(lv);            
            }
            return retVal;
        }
    }
}
