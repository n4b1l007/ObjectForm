using System;
using System.Web;
using System.Web.Mvc;
using ObjectForm.Helper;
using ObjectForm.Options;

namespace ObjectForm
{
    public class FormBuilder : IHtmlString
    {
        private FormHtml _formHtml;
        private readonly Type _type;
        private readonly object _model;
        private readonly HtmlHelper _htmlHelper;
        private readonly FormOption _formOption;
        private readonly LabelOption _labelOption;
        private readonly PropertyOption _propertyOption;

        #region lazy singleton
        private static FormBuilder _instance;
        private static readonly object Locker = new object();

        public static FormBuilder GetInstance(HtmlHelper htmlHelper, Type type, object model)
        {
            if (_instance == null)
            {
                lock (Locker)
                {
                    if (_instance == null) // Double-checked locking (works in C#!).
                    {
                        _instance = new FormBuilder(htmlHelper, type, model);
                    }
                }
            }

            return _instance;
        }
        #endregion

        public FormBuilder(HtmlHelper htmlHelper, Type type, object model)
        {
            _model = model;
            _htmlHelper = htmlHelper;
            _type = type;
            _propertyOption = new PropertyOption();
            _formOption = new FormOption();
            _labelOption = new LabelOption();
        }

        public virtual string ToHtmlString()
        {
            _formHtml = new FormHtml(_model, _formOption, _type, _htmlHelper, _labelOption, _propertyOption);

            return _formHtml.ReturnHtml();
        }

        public FormBuilder Options(Action<FormOptionBuilder> buildOptions)
        {
            buildOptions(new FormOptionBuilder(_formOption));
            return this;
        }
        public FormBuilder AddLabelOptions(Action<LabelOptionBuilder> buildOptions)
        {
            buildOptions(new LabelOptionBuilder(_labelOption));
            return this;
        }
        public FormBuilder AddPropertyOptions(Action<PropertyOptionBuilder> buildOptions)
        {
            buildOptions(new PropertyOptionBuilder(_propertyOption));
            return this;
        }







        
    }
}