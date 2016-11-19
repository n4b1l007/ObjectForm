using System;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ObjectForm.Helper;

namespace ObjectForm
{
    public class ObjectForm : IHtmlString
    {
        private readonly HtmlHelper _htmlHelper;
        public FormOption FormOption;
        private readonly Type _type;

        public ObjectForm(HtmlHelper htmlHelper, Type type)
        {
            _htmlHelper = htmlHelper;
            _type = type;
            FormOption = new FormOption();
        }

        public ObjectForm Options(Action<FormOptionBuilder> buildOptions)
        {
            buildOptions(new FormOptionBuilder(FormOption));
            return this;
        }

        public virtual string ToHtmlString()
        {
            return Generator();
        }

        public string Generator()
        {
            var modelForm = new TagBuilder("form");

            if (FormOption.Action != string.Empty) modelForm.Attributes.Add("action", FormOption.Action);
            if (FormOption.AttributeId != string.Empty) modelForm.Attributes.Add("id", FormOption.AttributeId);
            if (FormOption.AttributeClass != string.Empty) modelForm.Attributes.Add("class", FormOption.AttributeClass);

            #region Property Html
            var properties = _type.GetProperties();
            foreach (var property in properties)
            {
                var formProperty = new FormProperty(_htmlHelper);

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
                            propertyHtml.Attributes.Add("class", "form-control");
                            break;
                        }
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


            const string button = "<input type = \"submit\" value=\"Create\" class=\"btn btn-success\" />";

            modelForm.InnerHtml += button;
            return modelForm.ToString();
        }
    }
}