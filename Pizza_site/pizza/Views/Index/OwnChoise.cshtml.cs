using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using pizza.Classes;

namespace pizza.Views.Index
{
    public class OwnChoiseModel : PageModel
    {
        public pizza.Classes.StandartComponents components;
        public OwnChoiseModel()
        {
            components = new StandartComponents();
            components.GetComponents();
        }
        public void OnGet()
        {
            components = new StandartComponents();
            components.GetComponents();
        }
    }
}
