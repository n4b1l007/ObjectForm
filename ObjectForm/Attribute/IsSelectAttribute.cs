using System;

namespace ObjectForm.Attribute
{
    [AttributeUsage(AttributeTargets.Property)]
    public class IsSelectAttribute : System.Attribute
    {
        private readonly string _url;
        public IsSelectAttribute()
        {
        }
        public IsSelectAttribute(string url)
        {
            _url = url;
        }
        
    }
}