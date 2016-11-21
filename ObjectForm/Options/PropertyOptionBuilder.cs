namespace ObjectForm.Options
{
    public class PropertyOptionBuilder
    {
        protected PropertyOption PropertyOption;

        public PropertyOptionBuilder(PropertyOption propertyOption)
        {
            PropertyOption = propertyOption;
        }

        public PropertyOptionBuilder PropertyClass(string className)
        {
            PropertyOption.PropertyClass = className;
            return this;
        }

        public PropertyOptionBuilder WrapArroundWithDiv()
        {
            PropertyOption.DivWrap = true;
            return this;
        }

        public PropertyOptionBuilder WrapArroundWithDiv(string className)
        {
            PropertyOption.DivWrap = true;
            PropertyOption.DivWrapClass = className;
            return this;
        }

        public PropertyOptionBuilder AddInLineStyle(string inLineStyle)
        {
            PropertyOption.InLineStyle = inLineStyle;
            return this;
        }
    }
}