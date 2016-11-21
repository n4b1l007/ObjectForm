namespace ObjectForm.Options
{
    /// <summary>
    ///     Html Form options builder class. Enables a fluent interface for adding options to the html form.
    /// </summary>
    public class FormOptionBuilder
    {
        protected FormOption PagerOptions;

        public FormOptionBuilder(FormOption pagerOptions)
        {
            PagerOptions = pagerOptions;
        }

        /// <summary>
        ///     Set the action name of the form
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public FormOptionBuilder Action(string action)
        {
            PagerOptions.Action = action;
            return this;
        }

        public FormOptionBuilder ApplyBootstrap()
        {
            PagerOptions.IsBootstrap = true;
            return this;
        }
    }
}