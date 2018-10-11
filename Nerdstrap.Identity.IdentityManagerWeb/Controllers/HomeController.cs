using Nerdstrap.Identity.IdentityManagerWeb.Constants;
using log4net;
using System.Web.Mvc;

namespace Nerdstrap.Identity.IdentityManagerWeb.Controllers
{
    public class HomeController : Controller, IHomeController
    {
        private static ILog _logger;

        public ILog Logger
        {
            get { return _logger ?? (_logger = LogManager.GetLogger(typeof(HomeController))); }
            set { _logger = value; }
        }

        public HomeController() { }

        [Route("", Name = HomeControllerRoute.GetIndex)]
        public ActionResult Index()
        {
            return this.View(HomeControllerAction.Index);
        }
    }
}
