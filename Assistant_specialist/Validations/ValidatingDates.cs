using Assistant_specialist.ViewModels.PositiveResponseVM;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;

namespace Assistant_specialist.Validations
{
    class ValidatingDates : ValidationRule
    {
        public double DateRangeFrom { get; set; }
        public double DateRangeTo { get; set; }
        public string ErrorText1 { get; set; }
        public string ErrorText2 { get; set; }
        public string ErrorText3 { get; set; }

        private T GetPropertyValue<T>(object source, string propertyName)
        {
            try
            {
                return (T)source.GetType().GetProperty(propertyName).GetValue(source);
            }
            catch
            {
                return default(T);
            }
        }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            BindingGroup bindingGroup = value as BindingGroup;
            if (bindingGroup != null)
            {
                PositiveResponseViewModel viewModel = bindingGroup.Items[0] as PositiveResponseViewModel;
                if (viewModel != null)
                {
                    DateTime? startDate = GetPropertyValue<DateTime?>(viewModel, "ContractDateUnchecked");
                    DateTime? startRangeDate = GetPropertyValue<DateTime?>(viewModel, "StartDateDiscountUnchecked");
                    DateTime? endRangeDate = GetPropertyValue<DateTime?>(viewModel, "EndDateDiscountUnchecked");
                    if (startDate == null & startRangeDate != null & endRangeDate != null)
                    {
                        return new ValidationResult(false, ErrorText1 == null ? "Начальная дата не определена" : ErrorText1);
                    }
                    if (startDate != null & startRangeDate != null & endRangeDate != null)
                    {
                        if (((DateTime)startRangeDate - (DateTime)startDate).Days < 0 ||
                            ((DateTime)endRangeDate - (DateTime)startDate).Days < 0)
                        {
                            return new ValidationResult(false, ErrorText2 == null ? "Выбранные даты меньше начальной даты" : ErrorText2);
                        }
                        if (DateRangeFrom >= ((DateTime)endRangeDate - (DateTime)startRangeDate).Days ||
                            ((DateTime)endRangeDate - (DateTime)startRangeDate).Days >= DateRangeTo)
                        {
                            return new ValidationResult(false, ErrorText3 == null ? "Некорректный диапазон дат" : ErrorText3);
                        }
                    }
                }
            }
            return ValidationResult.ValidResult;
        }
    }
}