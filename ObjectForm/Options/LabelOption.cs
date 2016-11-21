namespace ObjectForm.Options
{
    public class LabelOption
    {
        public LabelOption()
        {
            RemoveLabel = false;
            LabelClass = string.Empty;
            InLineStyle = string.Empty;
            DivWrap = false;
            DivWrapClass = string.Empty;
        }

        public bool RemoveLabel { get; set; }
        public string LabelClass { get; set; }
        public string InLineStyle { get; set; }
        public bool DivWrap { get; set; }
        public string DivWrapClass { get; set; }

        public static class DefaultDefaults
        {
            public static bool RemoveLabel = false;
            public static string LabelClass = string.Empty;
            public static string InLineStyle = string.Empty;
            public static bool DivWrap = false;
            public static string DivWrapClass = string.Empty;
        }

        /// <summary>
        ///     The static Defaults class allows you to set defaults for the entire application.
        ///     Set values at application startup.
        /// </summary>
        public static class Defaults
        {
            public static bool RemoveLabel = DefaultDefaults.RemoveLabel;
            public static string LabelClass = DefaultDefaults.LabelClass;
            public static string InLineStyle = DefaultDefaults.InLineStyle;
            public static bool DivWrap = DefaultDefaults.DivWrap;
            public static string DivWrapClass = DefaultDefaults.DivWrapClass;

            public static void Reset()
            {
                RemoveLabel = DefaultDefaults.RemoveLabel;
                LabelClass = DefaultDefaults.LabelClass;
                InLineStyle = DefaultDefaults.InLineStyle;
                DivWrap = DefaultDefaults.DivWrap;
                DivWrapClass = DefaultDefaults.DivWrapClass;
            }
        }
    }
}