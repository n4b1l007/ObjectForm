using System;
using System.Web;
using System.Web.Mvc;
using ObjectForm.Helper;
using ObjectForm.Options;

namespace ObjectForm
{
    public class ObjectForm : IHtmlString
    {
        private FormHtml _formHtml;
        private readonly Type _type;
        private readonly HtmlHelper _htmlHelper;
        private readonly FormOption _formOption;
        private readonly LabelOption _labelOption;
        private readonly PropertyOption _propertyOption;

        public ObjectForm(HtmlHelper htmlHelper, Type type)
        {
            _htmlHelper = htmlHelper;
            _type = type;
            _propertyOption = new PropertyOption();
            _formOption = new FormOption();
            _labelOption = new LabelOption();
        }

        public virtual string ToHtmlString()
        {
            _formHtml = new FormHtml(_formOption, _type, _htmlHelper, _labelOption, _propertyOption);

            return _formHtml.ReturnHtml();
        }

        public ObjectForm Options(Action<FormOptionBuilder> buildOptions)
        {
            buildOptions(new FormOptionBuilder(_formOption));
            return this;
        }
        public ObjectForm AddLabelOptions(Action<LabelOptionBuilder> buildOptions)
        {
            buildOptions(new LabelOptionBuilder(_labelOption));
            return this;
        }
        public ObjectForm AddPropertyOptions(Action<PropertyOptionBuilder> buildOptions)
        {
            buildOptions(new PropertyOptionBuilder(_propertyOption));
            return this;
        }
    }
}