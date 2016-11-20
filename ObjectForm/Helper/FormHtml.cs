using System;
using System.Linq;
using System.Web.Mvc;

namespace ObjectForm.Helper
{
    public class FormHtml
    {
        private readonly FormOption _formOption;
        private readonly HtmlHelper _htmlHelper;
        private readonly Type _type;

        public FormHtml(FormOption formOption, Type type, HtmlHelper htmlHelper)
        {
            _formOption = formOption;
            _type = type;
            _htmlHelper = htmlHelper;
        }

        public string RetuenHtml()
        {
            var modelForm = new TagBuilder("form");
            var formProperty = new FormProperty(_htmlHelper, _formOption);

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
                var propertyLabel = formProperty.Label(property);

                TagBuilder propertyHtml;

                var typeName = property.PropertyType.Name;

                if (typeName.Contains("Nullable"))
                {
                    typeName = "Int32";
                }
                switch (typeName)
                {
                    case "String":
                    {
                        propertyHtml = formProperty.ForString(property);
                        break;
                    }
                    case "Int32":
                    {
                        propertyHtml = formProperty.ForInt(property);
                        break;
                    }
                    default:
                    {
                        propertyHtml = new TagBuilder("input");
                        break;
                    }
                }

                if (_formOption.IsBootstrap)
                {
                    propertyHtml.Attributes.Add("class", "form-control");
                }

                var isRequired = property.CustomAttributes.Any(f => f.AttributeType.Name == "RequiredAttribute");
                if (isRequired)
                {
                    propertyHtml.Attributes.Add("required", "required");
                }
                //propertyHtml.Attributes.Add("class", "form-control required");
                propertyHtml.Attributes.Add("type", "text");
                propertyHtml.Attributes.Add("id", property.Name);
                propertyHtml.Attributes.Add("name", property.Name);

                modelForm.InnerHtml += propertyLabel + propertyHtml.ToString();
            }

            #endregion

            const string button = "<br /><input type = \"submit\" value=\"Create\" class=\"btn btn-success\" />";

            modelForm.InnerHtml += button;
            return modelForm.ToString();
        }
    }
}