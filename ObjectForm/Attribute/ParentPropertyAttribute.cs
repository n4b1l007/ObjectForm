using System;

namespace ObjectForm.Attribute
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ParentPropertyesAttribute : System.Attribute
    {
        private readonly string _parents;

        public ParentPropertyesAttribute(string parents)
        {
            _parents = parents;
        }
    }
}