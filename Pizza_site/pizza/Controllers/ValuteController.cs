using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pizza.Controllers
{
    public class ValuteController : Controller
    {
        public IHttpContextAccessor accessor;
        //Just taking accessor for session
        public ValuteController(IHttpContextAccessor _accessor)
        {
            accessor = _accessor;

        }
        [HttpGet]
        public ActionResult dol(string path)
        {
            accessor.HttpContext.Session.SetString("Valute", "dol");
            return Redirect("~/"+path);
        }
        [HttpGet]
        public ActionResult uah(string path)
        {
            accessor.HttpContext.Session.SetString("Valute", "uah");
            return Redirect("~/" + path);
        }
        [HttpGet]
        public ActionResult rub(string path)
        {
            accessor.HttpContext.Session.SetString("Valute", "rub");
            return Redirect("~/" + path);
        }


    }
}
