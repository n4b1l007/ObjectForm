using System;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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
            var modelForm = new TagBuilder("form");

            if (FormOption.Action != string.Empty) modelForm.Attributes.Add("action", FormOption.Action);
            if (FormOption.AttributeId != string.Empty) modelForm.Attributes.Add("id", FormOption.AttributeId);
            if (FormOption.AttributeClass != string.Empty) modelForm.Attributes.Add("class", FormOption.AttributeClass);

            #region Property Html
            var properties = _type.GetProperties();
            foreach (var property in properties)
            {
                var propertyLabelName = string.Empty;
                var propertyLabel = new TagBuilder("label");
                //propertyLabel.Attributes.Add("class", "col-md-6 col-sm-6 col-xs-12 form-group");
                var labelProperty = property.CustomAttributes.FirstOrDefault(f => f.AttributeType.Name == "DisplayNameAttribute");
                if (labelProperty != null)
                {
                    var labelName = labelProperty.ConstructorArguments.FirstOrDefault();
                    propertyLabelName = labelName.Value.ToString();
                    propertyLabel.SetInnerText(propertyLabelName);
                }
                else
                {
                    propertyLabel.SetInnerText(property.Name);
                }

                var dataTypeValue = 0;
                var dataType = property.CustomAttributes.FirstOrDefault(f => f.AttributeType.Name == "DataTypeAttribute");

                if (dataType != null)
                {
                    var dataTypeArg = dataType.ConstructorArguments.FirstOrDefault();
                    dataTypeValue = (int)dataTypeArg.Value;
                }


                var isRequired = property.CustomAttributes.Any(f => f.AttributeType.Name == "RequiredAttribute");

                //var propertyInnerDiv = new TagBuilder("div");
                //propertyInnerDiv.Attributes.Add("class", "col-md-6 col-sm-6 col-xs-12");

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

                            if (dataTypeValue == 0)
                            {
                                propertyHtml = new TagBuilder("input");
                                propertyHtml.Attributes.Add("class", "form-control");
                            }
                            else
                            {
                                switch (dataTypeValue)
                                {
                                    case 9:
                                        {
                                            propertyHtml = new TagBuilder("textarea");
                                            propertyHtml.Attributes.Add("class", "form-control");
                                        }
                                        break;
                                    default:
                                        {
                                            propertyHtml = new TagBuilder("input");
                                            propertyHtml.Attributes.Add("class", "form-control");
                                        }
                                        break;
                                }
                            }
                            break;
                        }
                    case "Int32":
                        {
                            var attrHeader = property.CustomAttributes.FirstOrDefault(a => a.AttributeType.Name == "IsSelectAttribute");

                            var isSelect = attrHeader != null;

                            var propertyString = isSelect ? "select" : "input";

                            propertyHtml = new TagBuilder(propertyString);


                            if (isSelect)
                            {
                                var viewBag = _htmlHelper.ViewContext.ViewBag;
                                if (isRequired)
                                    propertyHtml.Attributes.Add("class", "required");

                                var propertyName = property.Name;
                                string jsonString = JsonConvert.SerializeObject(viewBag);
                                var jsonObject = JObject.Parse(jsonString);

                                var propertyJson = jsonObject[propertyName];


                                if (propertyJson != null)
                                {
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


                                        var option = new TagBuilder("option") { InnerHtml = text.ToString() };
                                        option.Attributes.Add("value", value.ToString());
                                        fullList.AppendLine(option.ToString());
                                    }

                                    propertyHtml.InnerHtml = fullList.ToString();
                                }
                            }
                            else
                            {
                                propertyHtml.Attributes.Add("class", "form-control");
                            }
                            break;
                        }
                    default:
                        {
                            propertyHtml = new TagBuilder("input");
                            propertyHtml.Attributes.Add("class", "form-control");
                            break;
                        }
                }
                if (isRequired)
                {
                    propertyHtml.Attributes.Add("required", "required");
                    //propertyHtml.Attributes.Add("data-validetta", "required,minLength[2],maxLength[20]");
                }
                //propertyHtml.Attributes.Add("class", "form-control required");
                propertyHtml.Attributes.Add("type", "text");
                propertyHtml.Attributes.Add("id", property.Name);
                propertyHtml.Attributes.Add("name", property.Name);
                //propertyInnerDiv.InnerHtml = propertyHtml.ToString();



                modelForm.InnerHtml += propertyLabel + propertyHtml.ToString();
            }
            #endregion


            const string button = "<input type = \"submit\" value=\"Create\" class=\"btn btn-success\" />";

            modelForm.InnerHtml += button;
            return modelForm.ToString();
        }
    }
}