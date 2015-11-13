using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace CineChat
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            //config.MapHttpAttributeRoutes(); esta a dar erro, comentario a remover depois

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
