using System.Web.Mvc;
using System.Web.Security;
using Cms4.TestPlugin.Models;

namespace Cms4.TestPlugin.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet]
        public ActionResult SignIn(string returnUrl)
        {
            if (string.IsNullOrEmpty(returnUrl))
                returnUrl = Request.UrlReferrer.OriginalString;

            return View(new SignInModel{ReturnUrl = returnUrl});
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SignIn(SignInModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            FormsAuthentication.SetAuthCookie(model.Username, false);
            return Redirect(model.ReturnUrl);
        }

    }
}