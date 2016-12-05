using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using ObjectForm.Attribute;
using ObjectForm.Options;

namespace ObjectForm.Helper
{
    public class FormProperty
    {
        private readonly HtmlHelper _htmlHelper;
        private readonly FormOption _formOption;
        private readonly LabelOption _labelOption;
        private readonly PropertyOption _propertyOption;
        private TagBuilder _propertyHtml;

        public FormProperty(HtmlHelper htmlHelper, LabelOption labelOption, FormOption formOption, PropertyOption propertyOption)
        {
            _htmlHelper = htmlHelper;
            _labelOption = labelOption;
            _formOption = formOption;
            _propertyOption = propertyOption;
        }

        public TagBuilder Generator(PropertyInfo property)
        {
            return Generator(property, true);
        }

        public TagBuilder Generator(PropertyInfo property, bool withLabel)
        {
            TagBuilder propertyHtml;
            //var typeName = property.PropertyType.Name;

            var customAttributes = property.CustomAttributes.ToList();

            var isSelect = customAttributes.Any(a => a.AttributeType == typeof(IsSelectAttribute));
            var isRequired = customAttributes.Any(f => f.AttributeType == typeof(RequiredAttribute));

            var rawValue = _htmlHelper.ViewContext.ViewData.Eval(property.Name);

            var isList = property.PropertyType.GetInterface(typeof(IEnumerable<>).FullName) != null && property.PropertyType != typeof(string); //property.PropertyType.Name.Contains("List");//var isList = typeof (IList).IsAssignableFrom(property.PropertyType);

            if (isSelect || rawValue is IEnumerable<SelectListItem>)
            {
                var selectListItem = rawValue as IEnumerable<SelectListItem>;
                propertyHtml = ForSelect(property, selectListItem);
            }
            else if (isList)
            {
                propertyHtml = ForList(property);
            }
            else
            {
                propertyHtml = ForInput(property);
            }

            if (_formOption.IsBootstrap && !isList)
            {
                propertyHtml.Attributes.Add("class", "form-control");
            }

            if (isRequired)
            {
                propertyHtml.Attributes.Add("required", "required");
            }
            propertyHtml.Attributes.Add("type", "text");
            propertyHtml.Attributes.Add("id", property.Name);
            propertyHtml.Attributes.Add("name", property.Name);

            var labelString = string.Empty;
            if (withLabel && !isList && !_labelOption.RemoveLabel)
            {
                labelString = Label(property).ToString();
            }
            //var propertyLabel = formProperty.Label(property);

            if (_propertyOption.DivWrap)
            {
                var formGroup = new TagBuilder("div");
                formGroup.AddCssClass(_propertyOption.DivWrapClass);
                formGroup.InnerHtml = labelString + propertyHtml;

                return formGroup;
            }
            else
            {
                return propertyHtml;
            }
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


            if (!string.IsNullOrEmpty(_labelOption.LabelClass))
            {
                propertyLabel.AddCssClass(_labelOption.LabelClass);
            }
            if (!string.IsNullOrEmpty(_labelOption.InLineStyle))
            {
                propertyLabel.Attributes.Add("style", _labelOption.InLineStyle);
            }

            if (!_labelOption.DivWrap)
            {
                return propertyLabel;
            }
            else
            {
                var divWrap = new TagBuilder("div");

                divWrap.InnerHtml += propertyLabel;

                if (!string.IsNullOrEmpty(_labelOption.DivWrapClass))
                {
                    divWrap.AddCssClass(_labelOption.DivWrapClass);
                }

                return divWrap;
            }
        }

        public TagBuilder ForInput(PropertyInfo stringProperty)
        {
            var dataTypeValue = 0;
            var dataType = stringProperty.CustomAttributes.FirstOrDefault(f => f.AttributeType.Name == "DataTypeAttribute");

            if (dataType != null)
            {
                var dataTypeArg = dataType.ConstructorArguments.FirstOrDefault();
                dataTypeValue = (int) dataTypeArg.Value;
            }

            if (dataTypeValue == 0)
            {
                _propertyHtml = new TagBuilder("input");
            }
            else
            {
                switch (dataTypeValue)
                {
                    case 9:
                    {
                        _propertyHtml = new TagBuilder("textarea");
                    }
                        break;
                    default:
                    {
                        _propertyHtml = new TagBuilder("input");
                    }
                        break;
                }
            }
            return _propertyHtml;
        }

        public TagBuilder ForSelect(PropertyInfo intProperty, IEnumerable<SelectListItem> selectListItem)
        {
            _propertyHtml = new TagBuilder("select");

            if (selectListItem != null)
            {
                foreach (var listItem in selectListItem)
                {
                    var option = new TagBuilder("option") { InnerHtml = listItem.Text };
                    option.Attributes.Add("value", listItem.Value);
                    _propertyHtml.InnerHtml += option;
                }
            }
            return _propertyHtml;
        }

        public TagBuilder ForList(PropertyInfo listProperty)
        {
            var tableProperty = new TagBuilder("table");

            tableProperty.AddCssClass("table");

            var theadProperty = new TagBuilder("thead");
            var tbodyProperty = new TagBuilder("tbody");

            var headTrProperty = new TagBuilder("tr");
            var bodyTrProperty = new TagBuilder("tr");

            var assambliName = listProperty.PropertyType.FullName.Split('[').Last().Split(']').FirstOrDefault();

            //var fullaName = listProperty.PropertyType.FullName;
            //var startIndex = fullaName.IndexOf("[[", StringComparison.Ordinal) + 2 ;
            //var length = fullaName.Length - fullaName.IndexOf("[[", StringComparison.Ordinal) - 4;
            //var piece = fullaName.Substring(startIndex, length);

            //var type2 = Type.GetType(piece);


            if (assambliName != null)
            {
                var type = Type.GetType(assambliName);

                if (type != null)
                {
                    var properties = type.GetProperties();

                    foreach (var propertyInfo in properties)
                    {
                        var thProperty = new TagBuilder("th")
                        {
                            InnerHtml = propertyInfo.Name
                        };

                        headTrProperty.InnerHtml += thProperty;

                        var tdProperty = new TagBuilder("td")
                        {
                            InnerHtml = Generator(propertyInfo, false).ToString()
                    };


                        bodyTrProperty.InnerHtml += tdProperty;
                    }
                }
            }

            theadProperty.InnerHtml += headTrProperty;
            tbodyProperty.InnerHtml += bodyTrProperty;
            tableProperty.InnerHtml += theadProperty.ToString() + tbodyProperty;


            var panel = new TagBuilder("div");
            panel.AddCssClass("panel panel-default");

            var panelHead = new TagBuilder("div");
            panelHead.AddCssClass("panel-heading");
            panelHead.InnerHtml = listProperty.Name;


            var panelBody = new TagBuilder("div");
            panelBody.AddCssClass("panel-body");
            panelBody.InnerHtml = tableProperty.ToString();


            panel.InnerHtml = panelHead + panelBody.ToString();

            return panel;
        }
    }
}