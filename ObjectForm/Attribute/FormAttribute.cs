using System;

namespace ObjectForm.Attribute
{
    [AttributeUsage(AttributeTargets.Property)]
    public class GridColumnAttribute : System.Attribute
    {
        public string GridColName { set; get; }
        
        public GridColumnAttribute(string name)
        {
            GridColName = name;

        }
    }
}
