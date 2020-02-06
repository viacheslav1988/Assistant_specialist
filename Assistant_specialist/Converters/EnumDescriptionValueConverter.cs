using Assistant_specialist.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Assistant_specialist.Converters
{
    class EnumDescriptionValueConverter : IValueConverter
    {
        enum SearchType { MemberInfo, FieldInfo }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Enum current = value as Enum;
            if (current == null) return Binding.DoNothing;
            return GetDescriptionValue(current, parameter as string);
        }

        private object GetDescriptionValue(Enum value, string parameter)
        {
            DescriptionValueAttribute attribute;
            if (parameter == null)
            {
                attribute = SearchAttribute(value, SearchType.MemberInfo);
                if (attribute != null) return attribute.Description;
            }
            else if (parameter == "value")
            {
                attribute = SearchAttribute(value, SearchType.MemberInfo);
                if (attribute != null) return attribute.Value;
            }
            else
            {
                attribute = SearchAttribute(value, SearchType.FieldInfo, parameter);
                if (attribute != null) return attribute.Description;
            }
            return value.ToString();
        }

        private DescriptionValueAttribute SearchAttribute(Enum value, SearchType searchType, string parameter = "")
        {
            DescriptionValueAttribute attribute = null;
            Type type = value.GetType();
            if (searchType == SearchType.MemberInfo)
            {
                MemberInfo[] memberInfo = type.GetMember(value.ToString());
                if (memberInfo != null && memberInfo.Length > 0)
                {
                    object[] customAttributes = memberInfo[0].GetCustomAttributes(typeof(DescriptionValueAttribute), false);
                    if (customAttributes != null && customAttributes.Length > 0)
                    {
                        attribute = customAttributes.FirstOrDefault() as DescriptionValueAttribute;
                    }
                }
            }
            else if (searchType == SearchType.FieldInfo)
            {
                FieldInfo fieldInfo = type.GetField(parameter);
                if (fieldInfo != null)
                {
                    object[] customAttributes = fieldInfo.GetCustomAttributes(typeof(DescriptionValueAttribute), false);
                    if (customAttributes != null && customAttributes.Length > 0)
                    {
                        attribute = customAttributes.FirstOrDefault() as DescriptionValueAttribute;
                    }
                }
            }
            return attribute;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}