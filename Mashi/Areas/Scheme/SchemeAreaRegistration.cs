using System.Web.Mvc;

namespace Mashi.Areas.Scheme
{
    public class SchemeAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Scheme";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Scheme_default",
                "Scheme/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
