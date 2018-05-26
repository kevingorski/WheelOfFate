using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace WheelOfFate.Web.Controllers
{
    /// <summary>
    /// Home controller.
    /// </summary>
    public class HomeController : Controller
    {
        /// <summary>
        /// Render the default view.
        /// </summary>
        /// <returns>The index view.</returns>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Render the error view.
        /// </summary>
        /// <returns>The error view.</returns>
        public IActionResult Error()
        {
            return View();
        }
    }
}
