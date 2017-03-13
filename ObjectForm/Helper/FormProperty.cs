using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using ObjectForm.Attribute;
using ObjectForm.Options;
using ObjectForm.Settings;

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
            return Generator(property, true, true);
        }

        public TagBuilder Generator(PropertyInfo property, bool withLabel, bool withWraperDiv, bool useId = true)
        {
            TagBuilder propertyHtml;

            var customAttributes = property.CustomAttributes.ToList();

            var isSelect = customAttributes.Any(a => a.AttributeType == typeof(IsSelectAttribute));
            var isRequired = customAttributes.Any(f => f.AttributeType == typeof(RequiredAttribute));

            var rawValue = _htmlHelper.ViewContext.ViewData.Eval(property.Name);

            var isList = property.PropertyType.GetInterface(typeof(IEnumerable<>).FullName) != null && property.PropertyType != typeof(string);

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
                propertyHtml.Attributes.Add(HtmlTags.Class, genaretClass(isRequired));
            }

            if (isRequired)
            {
                propertyHtml.Attributes.Add(HtmlTags.Required, HtmlTags.Required);
            }
            if (useId)
            {
                propertyHtml.Attributes.Add(HtmlTags.Id, property.Name);
            }
            propertyHtml.Attributes.Add(HtmlTags.Type, HtmlTags.Text);
            propertyHtml.Attributes.Add(HtmlTags.Name, property.Name);

            var labelString = string.Empty;
            if (withLabel && !isList && !_labelOption.RemoveLabel)
            {
                labelString = Label(property).ToString();
            }

            var propertyString = propertyHtml;
            if (_propertyOption.DivWrap && !isList)
            {
                var formGroup = new TagBuilder(HtmlTags.Div);
                formGroup.AddCssClass(_propertyOption.DivWrapClass);
                formGroup.InnerHtml = propertyHtml.ToString();
                propertyString = formGroup;
            }
            if (_formOption.DivWrap && withWraperDiv && !isList)
            {
                var formGroup = new TagBuilder(HtmlTags.Div);
                formGroup.AddCssClass(_formOption.DivWrapClass);
                formGroup.InnerHtml = labelString + propertyString;

                var rowProperty = new TagBuilder(HtmlTags.Div);
                rowProperty.AddCssClass("col-sm-6 col-lg-6");
                rowProperty.InnerHtml += formGroup;

                return rowProperty;
            }


            return propertyHtml;
        }

        public TagBuilder Label(PropertyInfo property)
        {
            var propertyLabel = new TagBuilder(HtmlTags.Label);
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
                propertyLabel.Attributes.Add(HtmlTags.Style, _labelOption.InLineStyle);
            }

            if (!_labelOption.DivWrap)
            {
                return propertyLabel;
            }
            else
            {
                var divWrap = new TagBuilder(HtmlTags.Div);

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
                dataTypeValue = (int)dataTypeArg.Value;
            }

            if (dataTypeValue == 0)
            {
                _propertyHtml = new TagBuilder(HtmlTags.Input);
            }
            else
            {
                switch (dataTypeValue)
                {
                    case 9:
                        {
                            _propertyHtml = new TagBuilder(HtmlTags.Textarea);
                            break;
                        }
                    default:
                        {
                            _propertyHtml = new TagBuilder(HtmlTags.Input);
                            break;
                        }
                }
            }
            return _propertyHtml;
        }

        public TagBuilder ForSelect(PropertyInfo intProperty, IEnumerable<SelectListItem> selectListItem)
        {
            _propertyHtml = new TagBuilder(HtmlTags.Select);

            if (selectListItem != null)
            {
                foreach (var listItem in selectListItem)
                {
                    var option = new TagBuilder(HtmlTags.SelectOption) { InnerHtml = listItem.Text };
                    option.Attributes.Add(HtmlTags.Value, listItem.Value);
                    _propertyHtml.InnerHtml += option;
                }
            }
            return _propertyHtml;
        }

        public TagBuilder ForList(PropertyInfo listProperty)
        {
            var tableProperty = new TagBuilder(HtmlTags.Table);

            tableProperty.AddCssClass(HtmlTags.Table);

            var theadProperty = new TagBuilder(HtmlTags.TableHead);
            var tbodyProperty = new TagBuilder(HtmlTags.TableBody);

            var headTrProperty = new TagBuilder(HtmlTags.TableTr);
            var bodyTrProperty = new TagBuilder(HtmlTags.TableTr);

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
                        var thProperty = new TagBuilder(HtmlTags.TableTh)
                        {
                            InnerHtml = propertyInfo.Name
                        };

                        headTrProperty.InnerHtml += thProperty;

                        var tdProperty = new TagBuilder(HtmlTags.TableTd)
                        {
                            InnerHtml = Generator(propertyInfo, false, false, false).ToString()
                        };

                        bodyTrProperty.InnerHtml += tdProperty;
                    }

                    #region "Add Button"

                    var addButton = new TagBuilder(HtmlTags.Button)
                    {
                        InnerHtml = "<span class=\"glyphicon glyphicon-plus-sign\"></span>"
                    };
                    addButton.AddCssClass(BootstrapClass.ButtonDefault + " addRow");
                    addButton.Attributes.Add(HtmlTags.Type, HtmlTags.Button);

                    headTrProperty.InnerHtml += new TagBuilder(HtmlTags.TableTh);

                    var td = new TagBuilder(HtmlTags.TableTd);
                    td.Attributes.Add(HtmlTags.Style, "width:50px");
                    td.InnerHtml = addButton.ToString();


                    bodyTrProperty.InnerHtml += td;

                    #endregion
                }
            }

            theadProperty.InnerHtml += headTrProperty;
            tbodyProperty.InnerHtml += bodyTrProperty;
            tableProperty.InnerHtml += theadProperty.ToString() + tbodyProperty;


            var panel = new TagBuilder(HtmlTags.Div);
            panel.AddCssClass(BootstrapClass.PanelDefault);

            var panelHead = new TagBuilder(HtmlTags.Div);
            panelHead.AddCssClass(BootstrapClass.PanelHeading);
            panelHead.InnerHtml = listProperty.Name;


            var panelBody = new TagBuilder(HtmlTags.Div);
            panelBody.AddCssClass(BootstrapClass.PanelBody);
            panelBody.InnerHtml = tableProperty.ToString();


            panel.InnerHtml = panelHead + panelBody.ToString();

            return panel;
        }

        private string genaretClass(bool isRequired)
        {
            return BootstrapClass.FormControl
                + (isRequired ? " Required" : "");
        }
    }
}