using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assistant_specialist.Models
{
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true, Inherited = true)]
    public class DescriptionValueAttribute : Attribute
    {
        private string description;
        private string value;
        public DescriptionValueAttribute(string description)
        {
            this.description = description;
        }
        public virtual string Description
        {
            get { return description; }
        }
        public virtual string Value
        {
            get { return value; }
            set { this.value = value; }
        }
    }
}