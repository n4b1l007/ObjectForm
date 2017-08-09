using System;
using System.Web.Mvc;

namespace ObjectForm
{
    public static class ObjectFormExtensions//<TEntity> where TEntity : class
    {
        public static ObjectForm ObjectForm<TEntity>(this HtmlHelper<TEntity> htmlHelper)
        {
            var type = typeof(TEntity);
            var model = htmlHelper.ViewData.Model;
            return new ObjectForm(htmlHelper, type, model);
        }

        public static ObjectForm ObjectForm(this HtmlHelper htmlHelper, Type type)
        {
            var model = htmlHelper.ViewData.Model;
            return new ObjectForm(htmlHelper, type, model);
        }
    }
}