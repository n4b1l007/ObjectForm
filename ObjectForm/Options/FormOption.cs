namespace ObjectForm.Options
{
    public class FormOption
    {
        public FormOption()
        {
            IsBootstrap = false;
            Action = string.Empty;
            AttributeId = string.Empty;
            AttributeClass = string.Empty;
            DivWrap = false;
            DivWrapClass = string.Empty;
        }

        public bool IsBootstrap { get; set; }
        public string Action { get; set; }
        public string AttributeId { get; set; }
        public string AttributeClass { get; set; }

        public bool DivWrap { get; set; }
        public string DivWrapClass { get; set; }

        public static class DefaultDefaults
        {
            public static bool IsBootstrap = false;
            public const string DefaultAction = "/";
            public const string DefaultHtmlId = null;
            public const string DefaultHtmlClass = null;
            public static bool DivWrap = false;
            public static string DivWrapClass = string.Empty;
        }

        public static class Defaults
        {
            public static bool IsBootstrap = DefaultDefaults.IsBootstrap;
            public static string DefaultAction = DefaultDefaults.DefaultAction;
            public static string AttributeId = DefaultDefaults.DefaultHtmlId;
            public static string AttributeClass = DefaultDefaults.DefaultHtmlClass;
            public static bool DivWrap = DefaultDefaults.DivWrap;
            public static string DivWrapClass = DefaultDefaults.DivWrapClass;

            public static void Reset()
            {
                IsBootstrap = DefaultDefaults.IsBootstrap;
                DefaultAction = DefaultDefaults.DefaultAction;
                AttributeId = DefaultDefaults.DefaultHtmlId;
                AttributeClass = DefaultDefaults.DefaultHtmlClass;
                DivWrap = DefaultDefaults.DivWrap;
                DivWrapClass = DefaultDefaults.DivWrapClass;
            }
        }
    }
}