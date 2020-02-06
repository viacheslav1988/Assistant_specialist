using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Assistant_specialist.Validations
{
    class ValidatingText : ValidationRule
    {
        public string Pattern { get; set; }
        public string ErrorText { get; set; }
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string current = value as string;
            if (current != null)
            {
                if (string.IsNullOrEmpty(Pattern) || string.IsNullOrEmpty(current) || Regex.IsMatch(current, Pattern)) return ValidationResult.ValidResult;
                else return new ValidationResult(false, string.IsNullOrEmpty(ErrorText) ? "Ошибка данных" : ErrorText);
            }
            return ValidationResult.ValidResult;
        }
    }
}