using System;
using System.Web.Mvc;

namespace ObjectForm
{
    public static class ObjectFormExtensions
    {
        public static ObjectForm ObjectForm(this HtmlHelper htmlHelper, Type type)
        {
            return new ObjectForm(htmlHelper, type);
        }
    }
}