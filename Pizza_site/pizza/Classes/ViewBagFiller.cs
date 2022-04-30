using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pizza.Classes
{
    public class ViewBagFiller
    {
        Controller controller;
        IHttpContextAccessor accessor;
        public ViewBagFiller(Controller _c, IHttpContextAccessor _accessor)
        {
            accessor = _accessor;
            controller = _c;
        }
        public void fillViewBag(string path)
        {
            #region AddNumberToCart(ViewBag)
            int count = 0;
            for (int i = 0; accessor.HttpContext.Session.GetString(i.ToString()) != null; i++)
            {
                count++;
            }
            controller.ViewBag.cart = count;
            #endregion
            #region AddValuteToViewBag
            controller.ViewBag.valute = accessor.HttpContext.Session.GetString("Valute");
            controller.ViewBag.path = path;
            #endregion
        }
    }
}
