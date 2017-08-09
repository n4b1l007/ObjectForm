using System;
using System.Collections;
using System.Collections.Generic;
using System.Web.Mvc;
using ObjectForm.Options;
using ObjectForm.Settings;

namespace ObjectForm.Helper
{
    public class FormHtml
    {
        private readonly FormOption _formOption;
        private readonly HtmlHelper _htmlHelper;
        private readonly LabelOption _labelOption;
        private readonly PropertyOption _propertyOption;
        private readonly Type _type;

        private readonly object _model;

        public FormHtml(object model,
                        FormOption formOption,
                        Type type,
                        HtmlHelper htmlHelper,
                        LabelOption labelOption,
                        PropertyOption propertyOption)
        {
            _model = model;
            _formOption = formOption;
            _type = type;
            _htmlHelper = htmlHelper;
            _labelOption = labelOption;
            _propertyOption = propertyOption;
        }

        public string ReturnHtml()
        {
            var modelForm = new TagBuilder(HtmlTags.Form);
            var formProperty = new FormProperty(_htmlHelper, _model, _labelOption, _formOption, _propertyOption);

            if (_formOption.Action != string.Empty)
                modelForm.Attributes.Add(HtmlTags.Action, _formOption.Action);

            if (_formOption.AttributeId != string.Empty)
                modelForm.Attributes.Add(HtmlTags.Id, _formOption.AttributeId);

            if (_formOption.AttributeClass != string.Empty)
                modelForm.Attributes.Add(HtmlTags.Class, _formOption.AttributeClass);

            #region Property Html

            var properties = _type.GetProperties();

            var modelFormInnerHtml = new TagBuilder(HtmlTags.Div);
            modelFormInnerHtml.AddCssClass("row");

            var table = string.Empty;
            foreach (var property in properties)
            {
                var propValue = property.GetValue(_model, null);
                //if (property.PropertyType.IsPrimitive ||
                //    property.PropertyType == typeof (string) ||
                //    property.PropertyType == typeof(int) ||
                //    property.PropertyType == typeof(DateTime))
                //{
                //    modelFormInnerHtml.InnerHtml += formProperty.Generator(property);
                //}
                //else
                if (typeof (IEnumerable).IsAssignableFrom(property.PropertyType))
                {
                    table += formProperty.Generator(property, propValue);
                }
                else
                {
                    modelFormInnerHtml.InnerHtml += formProperty.Generator(property, propValue);
                }
            }

            modelForm.InnerHtml += modelFormInnerHtml + table;
            #endregion

            var submitButton = new TagBuilder(HtmlTags.Button);
            submitButton.AddCssClass("btn btn-success");
            submitButton.Attributes.Add(HtmlTags.Type, "submit");
            submitButton.SetInnerText("Create");


            //const string button = "<br /><input type = \"submit\" value=\"Create\" class=\"btn btn-success\" />";

            modelForm.InnerHtml += submitButton;
            return modelForm.ToString();
        }
    }
}