using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SHCollege.DAO
{
    public class SScore
    {
        /// <summary>
        /// 主要名稱
        /// </summary>
        public string MainName { get; set; }

        private List<string> MapingNameList = new List<string>();

        public void AddScore(string name, decimal Score, decimal credit)
        {
            MapingNameList.Add(name);
            SumScore += Score * credit;
            Credit += credit;
        }

        private decimal SumScore = 0;

        private decimal Credit = 0;

        public decimal GetAverage()
        {
            if (Credit > 0)
            {
                // 取到整數位
                return Math.Round((SumScore / Credit), 0);

            }
            else
                return 0;
        }
    }
}
