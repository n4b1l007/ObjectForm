using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using ObjectForm.Attribute;
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

        public FormHtml(FormOption formOption, Type type, HtmlHelper htmlHelper, LabelOption labelOption, PropertyOption propertyOption)
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
            var formProperty = new FormProperty(_htmlHelper, _labelOption, _formOption);

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
                //TagBuilder propertyHtml;
                ////var typeName = property.PropertyType.Name;

                //var customAttributes = property.CustomAttributes.ToList();

                //var isSelect = customAttributes.Any(a => a.AttributeType == typeof(IsSelectAttribute));
                //var isRequired = customAttributes.Any(f => f.AttributeType == typeof(RequiredAttribute));

                //var rawValue = _htmlHelper.ViewContext.ViewData.Eval(property.Name);

                ////var isList = typeof (IList).IsAssignableFrom(property.PropertyType);
                //var isList = property.PropertyType.Name.Contains("List");

                //if (isSelect || rawValue is IEnumerable<SelectListItem>)
                //{
                //    var selectListItem = rawValue as IEnumerable<SelectListItem>;
                //    propertyHtml = formProperty.ForSelect(property, selectListItem);
                //}
                //else if (isList)
                //{
                //    propertyHtml = formProperty.ForList(property);
                //}
                //else
                //{
                //    propertyHtml = formProperty.ForInput(property);
                //}

                //if (_formOption.IsBootstrap && !isList)
                //{
                //    propertyHtml.Attributes.Add("class", "form-control");
                //}

                //if (isRequired)
                //{
                //    propertyHtml.Attributes.Add("required", "required");
                //}
                //propertyHtml.Attributes.Add("type", "text");
                //propertyHtml.Attributes.Add("id", property.Name);
                //propertyHtml.Attributes.Add("name", property.Name);

                //var labelString = string.Empty;
                //if (!isList && !_labelOption.RemoveLabel)
                //{
                //    labelString = formProperty.Label(property).ToString();
                //}
                ////var propertyLabel = formProperty.Label(property);

                //var formGroup = new TagBuilder("div");
                //formGroup.AddCssClass("form-group");
                //formGroup.InnerHtml = labelString + propertyHtml;

                //modelForm.InnerHtml += formGroup;
            }

            #endregion

            const string button = "<br /><input type = \"submit\" value=\"Create\" class=\"btn btn-success\" />";

            modelForm.InnerHtml += button;
            return modelForm.ToString();
        }


    }
}