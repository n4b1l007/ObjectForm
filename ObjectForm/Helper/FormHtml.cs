using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
            var formProperty = new FormProperty(_htmlHelper, _labelOption, _propertyOption);

            if (_formOption.Action != string.Empty)
                modelForm.Attributes.Add("action", _formOption.Action);
            if (_formOption.AttributeId != string.Empty)
                modelForm.Attributes.Add("id", _formOption.AttributeId);
            if (_formOption.AttributeClass != string.Empty)
                modelForm.Attributes.Add("class", _formOption.AttributeClass);


            //var i = Activator.CreateInstance(_type);



            var viewBag = _htmlHelper.ViewContext.ViewBag;
            string jsonString = JsonConvert.SerializeObject(viewBag);
            var jsonObject = JObject.Parse(jsonString);
            #region Property Html

            var properties = _type.GetProperties();
            
            foreach (var property in properties)
            {
                TagBuilder propertyHtml;

                var typeName = property.PropertyType.Name;

                var viewBagJsonObject = jsonObject[property.Name];



                //if (viewBagJsonObject is JArray)
                //{
                //    try
                //    {
                //        propertySelectList = viewBagJsonObject.ToObject<SelectList>();
                //    }
                //    catch
                //    {
                //        // ignored
                //    }
                //}

                
                var contentType = "String";

                if (typeName.Contains("DateTime"))
                {
                    contentType = "DateTime";
                }
                else if (typeName.Contains("Nullable") || typeName.Contains("Int") || typeName.Contains("Decimal") || typeName.Contains("Decimal"))
                {
                    contentType = "Int";
                }

                switch (contentType)
                {
                    case "String":
                    {
                        propertyHtml = formProperty.ForInput(property);
                        break;
                    }
                    case "DateTime":
                    {
                        propertyHtml = formProperty.ForInput(property);
                        break;
                    }
                    case "IList`1":
                    {
                        propertyHtml = formProperty.ForSelect(property);
                        break;
                    }
                    case "Int":
                    {
                        propertyHtml = formProperty.ForSelect(property);
                        break;
                    }
                    default:
                    {
                        propertyHtml = new TagBuilder("br");
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
                propertyHtml.Attributes.Add("type", "text");
                propertyHtml.Attributes.Add("id", property.Name);
                propertyHtml.Attributes.Add("name", property.Name);

                var labelString = string.Empty;
                if (!_labelOption.RemoveLabel)
                {
                    labelString = formProperty.Label(property).ToString();
                }
                //var propertyLabel = formProperty.Label(property);

                modelForm.InnerHtml += "<div class=\"form-group\">" + labelString + propertyHtml + "</div>";
            }

            #endregion

            const string button = "<br /><input type = \"submit\" value=\"Create\" class=\"btn btn-success\" />";

            modelForm.InnerHtml += button;
            return modelForm.ToString();
        }
    }
}