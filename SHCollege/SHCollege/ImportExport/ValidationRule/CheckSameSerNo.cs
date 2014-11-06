using Campus.DocumentValidator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SHCollege.ImportExport.ValidationRule
{
    public class CheckSameSerNo : IFieldValidator
    {

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
            bool retVal = true;
            if (Utility._tmpSerNoList.Contains(Value))
                    retVal = false;

                Utility._tmpSerNoList.Add(Value);

            return retVal;
        }
    }
}
