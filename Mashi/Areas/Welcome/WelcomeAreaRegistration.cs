using System.Web.Mvc;

namespace Mashi.Areas.Welcome
{
    public class WelcomeAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Welcome";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Welcome_default",
                "Welcome/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
