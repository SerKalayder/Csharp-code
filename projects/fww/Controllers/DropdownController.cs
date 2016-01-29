using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using fww.Models;

namespace fww.Controllers
{
    public class DropdownController : Controller
    {
        public ActionResult Index()
        {
            // These countries are equivalent to your states
            var countries = new List<string> { "Ohio", "California", "Florida" };
            var countriesOptions = new SelectList(countries);
            ViewBag.Countries = countriesOptions;
            return View();
        }

        public JsonResult GetRegions(string country)  
        {  
            var regions = new List<string>();  
            switch (country)  
            {  
                case "Florida":
                    regions.Add("City1");
                    regions.Add("City2");
                    regions.Add("City3");  
                    break;  
                case "Ohio":
                    regions.Add("City4");
                    regions.Add("City5");  
                    break;  
                case "California":
                    regions.Add("City6");
                    regions.Add("City7");  
                    break;  
            }  
            //Add JsonRequest behavior to allow retrieving states over http get  
            return Json(regions, JsonRequestBehavior.AllowGet);  
        }  

    }
}
