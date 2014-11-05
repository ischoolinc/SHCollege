using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Campus.DocumentValidator;

namespace SHCollege.ImportExport.ValidationRule
{
    public class FieldValidatorFactory : IFieldValidatorFactory
    {
        public IFieldValidator CreateFieldValidator(string typeName, System.Xml.XmlElement validatorDescription)
        {
            switch (typeName.ToUpper())
            {
                case "IMPORTSATSTUDSTUDENTNUMBERCHECK":
                    return new StudentNumberCheck();
                default:
                    return null;
            }
        }
    }
}
