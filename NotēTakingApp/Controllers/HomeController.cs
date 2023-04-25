using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace NoteTakingApp.Controllers
{
    public class HomeController : Controller
    {
        [Route("/Home/Error")]
        public IActionResult Error()
        {

            IExceptionHandlerPathFeature iExceptionHandlerFeature
                    = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            if (iExceptionHandlerFeature != null)
            {
                string path = iExceptionHandlerFeature.Path;
                Exception exception =
                iExceptionHandlerFeature.Error;
                //Write code here to log the exception details
                return View("Error",
                iExceptionHandlerFeature);
            }
            return View();

        }

        [Route("/Home/HandleError/{code:int}")]
        public IActionResult HandleError(int code)
        {
            string errormessage = "";
            if(code == 404)
            {
                errormessage = $"Page Not Found";
            }
            else
            {
                errormessage = "Something went wrong";
            }
            ViewData["ErrorMessage"] =errormessage;
            return View("~/Views/Shared/HandleError.cshtml");
        }
    }
}
