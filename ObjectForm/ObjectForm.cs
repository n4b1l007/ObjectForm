using System;
using System.Web;
using System.Web.Mvc;
using ObjectForm.Helper;

namespace ObjectForm
{
    public class ObjectForm : IHtmlString
    {
        private readonly HtmlHelper _htmlHelper;
        private readonly Type _type;
        private FormHtml _formHtml;
        public FormOption FormOption;

        public ObjectForm(HtmlHelper htmlHelper, Type type)
        {
            _htmlHelper = htmlHelper;
            _type = type;
            FormOption = new FormOption();
        }

        public virtual string ToHtmlString()
        {
            _formHtml = new FormHtml(FormOption, _type, _htmlHelper);

            return _formHtml.RetuenHtml();
        }

        public ObjectForm Options(Action<FormOptionBuilder> buildOptions)
        {
            buildOptions(new FormOptionBuilder(FormOption));
            return this;
        }
    }
}