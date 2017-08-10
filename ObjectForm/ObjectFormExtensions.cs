using System;
using System.Web.Mvc;

namespace ObjectForm
{
    public static class ObjectFormExtensions//<TEntity> where TEntity : class
    {
       
        public static FormBuilder ObjectForm<TEntity>(this HtmlHelper<TEntity> htmlHelper)
        {
            var type = typeof(TEntity);
            var model = htmlHelper.ViewData.Model;

            return FormBuilder.GetInstance(htmlHelper, type, model);
        }

        public static FormBuilder ObjectForm(this HtmlHelper htmlHelper, Type type)
        {
            var model = htmlHelper.ViewData.Model;
            return FormBuilder.GetInstance(htmlHelper, type, model);
        }
    }
}