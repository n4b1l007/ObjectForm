using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectForm.Attribute
{
    [AttributeUsage(AttributeTargets.Property)]
    public class IsSelectAttribute : System.Attribute
    {
        public bool IsSelect { set; get; }

        public IsSelectAttribute()
        {
            IsSelect = true;
        }
        public IsSelectAttribute(bool isSelect)
        {
            IsSelect = isSelect;
        }
    }
}
