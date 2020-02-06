using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Assistant_specialist.Tools
{
    static class AssemblyExtension
    {
        public static TOutput GetAssemblyAttributePropertyValue<TInput, TOutput>(this Assembly assembly, string propertyName) where TInput : Attribute
        {
            object[] attributes = assembly.GetCustomAttributes(typeof(TInput), true);
            if (attributes == null || attributes.Length == 0) return default(TOutput);
            try { return (TOutput)attributes[0].GetType().GetProperty(propertyName).GetValue(attributes[0]); }
            catch { return default(TOutput); }
        }
    }
}