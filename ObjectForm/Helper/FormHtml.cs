using System;
using System.Web.Mvc;
using ObjectForm.Options;

namespace ObjectForm.Helper
{
    public class FormHtml
    {
        private readonly FormOption _formOption;
        private readonly HtmlHelper _htmlHelper;
        private readonly LabelOption _labelOption;
        private readonly PropertyOption _propertyOption;
        private readonly Type _type;

        public FormHtml(FormOption formOption, Type type, HtmlHelper htmlHelper, LabelOption labelOption,
            PropertyOption propertyOption)
        {
            _formOption = formOption;
            _type = type;
            _htmlHelper = htmlHelper;
            _labelOption = labelOption;
            _propertyOption = propertyOption;
        }

        public string RetuenHtml()
        {
            var modelForm = new TagBuilder("form");
            var formProperty = new FormProperty(_htmlHelper, _labelOption, _formOption, _propertyOption);

            if (_formOption.Action != string.Empty)
                modelForm.Attributes.Add("action", _formOption.Action);
            if (_formOption.AttributeId != string.Empty)
                modelForm.Attributes.Add("id", _formOption.AttributeId);
            if (_formOption.AttributeClass != string.Empty)
                modelForm.Attributes.Add("class", _formOption.AttributeClass);

            #region Property Html

            var properties = _type.GetProperties();

            foreach (var property in properties)
            {
                modelForm.InnerHtml += formProperty.Generator(property);
            }

            #endregion

            const string button = "<br /><input type = \"submit\" value=\"Create\" class=\"btn btn-success\" />";

            modelForm.InnerHtml += button;
            return modelForm.ToString();
        }
    }
}