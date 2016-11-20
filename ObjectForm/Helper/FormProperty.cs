using System.Linq;
using System.Reflection;
using System.Text;
using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ObjectForm.Helper
{
    public class FormProperty
    {
        private readonly HtmlHelper _htmlHelper;
        private TagBuilder _propertyHtml;

        public FormProperty(HtmlHelper htmlHelper)
        {
            _htmlHelper = htmlHelper;
        }

        public TagBuilder Label(PropertyInfo property)
        {
            var propertyLabel = new TagBuilder("label");
            var labelProperty = property.CustomAttributes.FirstOrDefault(f => f.AttributeType.Name == "DisplayNameAttribute");
            if (labelProperty != null)
            {
                var labelName = labelProperty.ConstructorArguments.FirstOrDefault();
                var propertyLabelName = labelName.Value.ToString();
                propertyLabel.SetInnerText(propertyLabelName);
            }
            else
            {
                propertyLabel.SetInnerText(property.Name);
            }
            return propertyLabel;
        }

        public TagBuilder ForString(PropertyInfo stringProperty)
        {
            var dataTypeValue = 0;
            var dataType =
                stringProperty.CustomAttributes.FirstOrDefault(f => f.AttributeType.Name == "DataTypeAttribute");

            if (dataType != null)
            {
                var dataTypeArg = dataType.ConstructorArguments.FirstOrDefault();
                dataTypeValue = (int) dataTypeArg.Value;
            }

            if (dataTypeValue == 0)
            {
                _propertyHtml = new TagBuilder("input");
                _propertyHtml.Attributes.Add("class", "form-control");
            }
            else
            {
                switch (dataTypeValue)
                {
                    case 9:
                    {
                        _propertyHtml = new TagBuilder("textarea");
                        _propertyHtml.Attributes.Add("class", "form-control");
                    }
                        break;
                    default:
                    {
                        _propertyHtml = new TagBuilder("input");
                        _propertyHtml.Attributes.Add("class", "form-control");
                    }
                        break;
                }
            }
            return _propertyHtml;
        }

        public TagBuilder ForInt(PropertyInfo intProperty)
        {
            var attrHeader =
                intProperty.CustomAttributes.FirstOrDefault(a => a.AttributeType.Name == "IsSelectAttribute");

            var isRequired = intProperty.CustomAttributes.Any(f => f.AttributeType.Name == "RequiredAttribute");

            var isSelect = attrHeader != null;

            var propertyString = isSelect ? "select" : "input";

            _propertyHtml = new TagBuilder(propertyString);


            if (isSelect)
            {
                var viewBag = _htmlHelper.ViewContext.ViewBag;
                if (isRequired)
                    _propertyHtml.Attributes.Add("class", "required");

                var propertyName = intProperty.Name;
                string jsonString = JsonConvert.SerializeObject(viewBag);
                var jsonObject = JObject.Parse(jsonString);

                var propertyJson = jsonObject[propertyName];


                if (propertyJson != null)
                {
                    var labelProperty =
                        intProperty.CustomAttributes.FirstOrDefault(f => f.AttributeType.Name == "DisplayNameAttribute");
                    if (labelProperty != null)
                    {
                        var labelName = labelProperty.ConstructorArguments.FirstOrDefault();
                        var propertyLabelName = labelName.Value.ToString();

                        var fullList = new StringBuilder();
                        var selectLable = propertyLabelName != string.Empty
                            ? propertyLabelName
                            : propertyName;
                        var placeHolderoption = new TagBuilder("option")
                        {
                            InnerHtml = "Select " + selectLable
                        };
                        placeHolderoption.Attributes.Add("value", "");
                        fullList.AppendLine(placeHolderoption.ToString());

                        var hh = propertyJson.Children();
                        foreach (var v in hh)
                        {
                            var value = v["Value"];
                            var text = v["Text"];


                            var option = new TagBuilder("option") {InnerHtml = text.ToString()};
                            option.Attributes.Add("value", value.ToString());
                            fullList.AppendLine(option.ToString());
                        }

                        _propertyHtml.InnerHtml = fullList.ToString();
                    }
                }
            }
            else
            {
                _propertyHtml.Attributes.Add("class", "form-control");
            }
            return _propertyHtml;
        }
    }
}