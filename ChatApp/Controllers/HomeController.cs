using ChatApp.Data;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;

namespace ChatApp.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            using (var db = new DataContext())
            {
                var messages = db.Messages.OrderBy(x => x.DateCreated).ToList();
                return View(messages);
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        #region AjaxRequest
        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(string userName)
        {
            FormsAuthentication.SetAuthCookie(userName, true);
            return Json(true);
        }
        #endregion
    }
}