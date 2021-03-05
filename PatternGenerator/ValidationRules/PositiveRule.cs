using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace PatternGenerator.ValidationRules
{
    public class PositiveRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (!int.TryParse((string)value, out int size))
                return new ValidationResult(false, "Please enter an integer value");
            if (size <= 0)
                return new ValidationResult(false, "Value must be greater than zero");
            return ValidationResult.ValidResult;
        }
    }
}
