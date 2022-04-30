using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using pizza.Classes;
using pizza.Views.Cart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pizza.Controllers
{
    public class CartController : Controller
    {
        public IHttpContextAccessor accessor;
        //Just taking accessor for session
        public CartController(IHttpContextAccessor _accessor)
        {
            accessor = _accessor;

        }
        [HttpPost]
        public IActionResult PayConfirm(int? idCustomer, int? bankNumber, DateTime? bankDate, int? CVV)
        {
            PayForOrderModel model = new PayForOrderModel(accessor);

            model.payConfirm(idCustomer.Value);

            return Redirect("~/");
        }
        [HttpPost]
        public IActionResult PayForOrder(int? id)
        {
            ViewBagFiller filler = new ViewBagFiller(this, accessor);
            filler.fillViewBag("Cart/OrderConfirm");

            ViewBag.id = id;

            PayForOrderModel model = new PayForOrderModel(accessor);

            model.getOrder();


            return View(model);
        }
        [HttpGet]
        public IActionResult RemovePosition(int? idPos)
        {
            if (idPos.HasValue)
            {
                List<string> lTmp = new List<string>();
                for (int i = 0; true; i++)
                {
                    if (accessor.HttpContext.Session.GetString(i.ToString()) != null)
                    {
                        string a = accessor.HttpContext.Session.GetString(i.ToString());
                        lTmp.Add(a);
                    }
                    else
                    {
                        break;
                    }
                }
                lTmp.RemoveAt(idPos.Value);
                accessor.HttpContext.Session.Clear();
                for(int i=0;i<lTmp.Count ;i++ )
                {
                    accessor.HttpContext.Session.SetString(i.ToString(), lTmp[i]);
                }
            }
            return Redirect("~/Cart/OrderConfirm");
        }
        //Save order to data base
        public IActionResult OrderConfirm()
        {
            OrderConfirmModel model = new OrderConfirmModel(accessor);

            model.getAllPizzasFromSessions();

            ViewBagFiller filler = new ViewBagFiller(this, accessor);
            filler.fillViewBag("Cart/OrderConfirm");

            return View(model);
        }
    }
}
