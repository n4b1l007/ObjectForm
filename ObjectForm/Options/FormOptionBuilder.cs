namespace ObjectForm.Options
{
    public class FormOptionBuilder
    {
        protected FormOption PagerOptions;

        public FormOptionBuilder(FormOption pagerOptions)
        {
            PagerOptions = pagerOptions;
        }

        public FormOptionBuilder Action(string action)
        {
            PagerOptions.Action = action;
            return this;
        }
        public FormOptionBuilder Class(string className)
        {
            PagerOptions.AttributeClass = className;
            return this;
        }
        public FormOptionBuilder ApplyBootstrap()
        {
            PagerOptions.IsBootstrap = true;
            return this;
        }


        public FormOptionBuilder WrapArroundWithDiv()
        {
            PagerOptions.DivWrap = true;
            return this;
        }

        public FormOptionBuilder WrapArroundWithDiv(string className)
        {
            PagerOptions.DivWrap = true;
            PagerOptions.DivWrapClass = className;
            return this;
        }
    }
}