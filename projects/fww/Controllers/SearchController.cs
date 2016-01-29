using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using fww.Models;

namespace fww.Controllers
{
    public class SearchController : Controller
    {
        public ActionResult Index(string Keywords, string Theme, string Location, string Popularity)
        {
            using (CommonContext db = new CommonContext())
            {
                List<Ad> sortedModel = new List<Ad>();
                List<Ad> model;
                if (Theme != "Усі теми")
                    model = (from items in db.Ads where items.Theme == Theme select items).ToList();
                else
                    model = (from items in db.Ads select items).ToList();
                if (Location != "Україна")
                    model = (from items in model where items.Country == Location select items).ToList();
                else
                    model = (from items in model select items).ToList();


                if (Keywords != "")
                {
                    if (model.Count != 0)
                    {
                        string[] keys = Keywords.Split(',');
                        for (int i = 0; i < keys.Length; i++)
                        {
                            string key = keys[i].Trim();
                            foreach (var m in model)
                            {
                                if (m.Keywords != null)
                                {
                                    if (m.Keywords.Contains(key))
                                    {
                                        sortedModel.Add(m);
                                    }
                                    
                                }
                            }
                        }
                    }
                }
                else
                    sortedModel = model;

                if (Popularity == "on")
                {
                    List<Ad> helper = (from item in sortedModel
                                       orderby item.Views descending
                                       select item).ToList<Ad>();
                    sortedModel = helper;
                }
                //sortedModel.Reverse();
                ViewBag.IsUnread = IsUnread();
                return View("~/Views/Ads/Index", sortedModel);
            }
        }

        public bool IsUnread()
        {
            bool IsUnread = false;
            using (CommonContext ctx = new CommonContext())
            {
                List<MessageRel> mr = (from items in ctx.MessageRels
                                       where (items.Receiver == User.Identity.Name && items.Unread == true)
                                       select items).ToList<MessageRel>();
                IsUnread = (mr.Count == 0) ? false : true;
            }
            return IsUnread;
        }
    }
}
