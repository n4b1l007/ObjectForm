using System.Web.Mvc.Ajax;
using System.Web.Routing;

namespace ObjectForm
{
    public class FormOption
    {
        public static class DefaultDefaults
        {
            public const string DefaultAction = "/";
            public const string DefaultHtmlId = null;
            public const string DefaultHtmlClass = null;
            public const string DefaultLabelBootstrapClass = "col-md-6 col-sm-6 col-xs-12 form-group";
        }

        /// <summary>
        /// The static Defaults class allows you to set defaults for the entire application.
        /// Set values at application startup.
        /// </summary>
        public static class Defaults
        {
            public static string DefaultAction = DefaultDefaults.DefaultAction;
            public static string AttributeId = DefaultDefaults.DefaultHtmlId;
            public static string AttributeClass = DefaultDefaults.DefaultHtmlClass;
            public static string LabelClass = DefaultDefaults.DefaultLabelBootstrapClass;

            public static void Reset()
            {
                DefaultAction = DefaultDefaults.DefaultAction;
                AttributeId = DefaultDefaults.DefaultHtmlId;
                AttributeClass = DefaultDefaults.DefaultHtmlClass;
                LabelClass = DefaultDefaults.DefaultLabelBootstrapClass;
            }
        }

        public string Action { get; set; }
        public string AttributeId { get; set; }
        public string AttributeClass { get; set; }
        public string LabelClass { get; set; }



        public FormOption()
        {
            Action = string.Empty;
            AttributeId = string.Empty;
            AttributeClass = string.Empty;
            LabelClass = string.Empty;
        }
    }
}