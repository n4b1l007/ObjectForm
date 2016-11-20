using System;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Web.Routing;
using System.Web.WebPages;

namespace ObjectForm
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

        public FormOptionBuilder LabelClass(string className)
        {
            PagerOptions.LabelClass = className;
            return this;
        }
    }
}