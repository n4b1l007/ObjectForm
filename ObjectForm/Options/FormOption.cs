namespace ObjectForm.Options
{
    public class FormOption
    {
        public static class DefaultDefaults
        {
            public static bool IsBootstrap = false;
            public const string DefaultAction = "/";
            public const string DefaultHtmlId = null;
            public const string DefaultHtmlClass = null;
        }

        /// <summary>
        /// The static Defaults class allows you to set defaults for the entire application.
        /// Set values at application startup.
        /// </summary>
        public static class Defaults
        {
            public static bool IsBootstrap = DefaultDefaults.IsBootstrap;
            public static string DefaultAction = DefaultDefaults.DefaultAction;
            public static string AttributeId = DefaultDefaults.DefaultHtmlId;
            public static string AttributeClass = DefaultDefaults.DefaultHtmlClass;

            public static void Reset()
            {
                IsBootstrap = DefaultDefaults.IsBootstrap;
                DefaultAction = DefaultDefaults.DefaultAction;
                AttributeId = DefaultDefaults.DefaultHtmlId;
                AttributeClass = DefaultDefaults.DefaultHtmlClass;
            }
        }

        public bool IsBootstrap { get; set; }
        public string Action { get; set; }
        public string AttributeId { get; set; }
        public string AttributeClass { get; set; }



        public FormOption()
        {
            IsBootstrap = false;
            Action = string.Empty;
            AttributeId = string.Empty;
            AttributeClass = string.Empty;
        }
    }
}