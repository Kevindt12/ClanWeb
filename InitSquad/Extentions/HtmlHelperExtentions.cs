using ClanWeb.Web.AppCode.UI.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Routing;

namespace System.Web.Mvc
{
    public static class HtmlHelperExtentions
    {

        /// <summary>
        /// Creates a new instance of famfamfam and returns that
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="html"></param>
        /// <returns></returns>
        public static FamFamFam GetFamFamFam(this HtmlHelper html)
        {
            FamFamFam resultInstance = new FamFamFam();

            return resultInstance;
        }

        //public static IHtmlString ComboBoxFor<TModel, TValue>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TValue>> expression, SelectList ,object htmlAttributes = null)
        //{
        //    ModelMetadata data = ModelMetadata.FromLambdaExpression(expression, helper.ViewData);
        //    string propertyName = data.PropertyName;

        //    // Creeating the input
        //    TagBuilder span = new TagBuilder("input");
        //    if (htmlAttributes != null)
        //    {
        //        RouteValueDictionary attributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
        //        span.MergeAttributes(attributes);
        //    }

        //    TagBuilder datalist

        //    return new MvcHtmlString(span.ToString());


        //}



    }
}