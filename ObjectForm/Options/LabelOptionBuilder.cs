namespace ObjectForm.Options
{
    public class LabelOptionBuilder
    {
        protected LabelOption LabelOption;

        public LabelOptionBuilder(LabelOption labelOption)
        {
            LabelOption = labelOption;
        }

        public LabelOptionBuilder RemoveLabels()
        {
            LabelOption.RemoveLabel = true;
            return this;
        }

        public LabelOptionBuilder AddLabelClass(string className)
        {
            LabelOption.LabelClass = className;
            return this;
        }

        public LabelOptionBuilder WrapArroundWithDiv()
        {
            LabelOption.DivWrap = true;
            return this;
        }

        public LabelOptionBuilder WrapArroundWithDiv(string className)
        {
            LabelOption.DivWrap = true;
            LabelOption.DivWrapClass = className;
            return this;
        }

        public LabelOptionBuilder AddInLineStyle(string inLineStyle)
        {
            LabelOption.InLineStyle = inLineStyle;
            return this;
        }
    }
}