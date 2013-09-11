using System;
using System.Web.Mvc;
using Raven.Client;
using StructureMap;

namespace RavenDBUnitOfWork.Filters
{
    [AttributeUsage(AttributeTargets.Class, Inherited = true)]
    public class RavenDbDocumentSessionAttribute : FilterAttribute, IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (filterContext.Exception != null)
                return; // don't commit changes if an exception was thrown

            using (var session = ObjectFactory.GetInstance<IDocumentSession>())
                session.SaveChanges();
        }
    }
}