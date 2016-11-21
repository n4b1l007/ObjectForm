namespace ObjectForm.Options
{
    public class PropertyOption
    {
        public PropertyOption()
        {
            PropertyClass = string.Empty;
            InLineStyle = string.Empty;
            DivWrap = false;
            DivWrapClass = string.Empty;
        }

        public string PropertyClass { get; set; }
        public string InLineStyle { get; set; }
        public bool DivWrap { get; set; }
        public string DivWrapClass { get; set; }

        public static class DefaultDefaults
        {
            public static string PropertyClass = string.Empty;
            public static string InLineStyle = string.Empty;
            public static bool DivWrap = false;
            public static string DivWrapClass = string.Empty;
        }

        public static class Defaults
        {
            public static string PropertyClass = DefaultDefaults.PropertyClass;
            public static string InLineStyle = DefaultDefaults.InLineStyle;
            public static bool DivWrap = DefaultDefaults.DivWrap;
            public static string DivWrapClass = DefaultDefaults.DivWrapClass;

            public static void Reset()
            {
                PropertyClass = DefaultDefaults.PropertyClass;
                InLineStyle = DefaultDefaults.InLineStyle;
                DivWrap = DefaultDefaults.DivWrap;
                DivWrapClass = DefaultDefaults.DivWrapClass;
            }
        }
    }
}