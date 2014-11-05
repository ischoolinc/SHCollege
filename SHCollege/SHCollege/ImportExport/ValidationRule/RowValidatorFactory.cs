using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Campus.DocumentValidator;

namespace SHCollege.ImportExport.ValidationRule
{
    public class RowValidatorFactory : IRowValidatorFactory
    {
        public IRowVaildator CreateRowValidator(string typeName, System.Xml.XmlElement validatorDescription)
        {
            switch (typeName.ToUpper())
            {
              
                default:
                    return null;
            }
        }
    }
}
