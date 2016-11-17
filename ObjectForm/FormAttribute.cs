using System;

namespace ObjectForm
{
    [AttributeUsage(AttributeTargets.Property)]
    public class GridColumnAttribute : Attribute
    {
        public string GridColName { set; get; }

        public bool IsSelect { set; get; }
        public GridColumnAttribute(string name, bool isSelect)
        {
            GridColName = name;
            IsSelect = isSelect;

        }
    }
}
