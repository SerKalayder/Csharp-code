using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using fww.Models;
using PagedList;
using System.IO;

namespace fww.Controllers
{
    public class AdsController : Controller
    {
        public ActionResult Index(int page = 1)
        {
            int pageSize = 5; // количество объектов на страницу

            if (User.Identity.IsAuthenticated)
            {
                ViewBag.Us = User.Identity.IsAuthenticated;
                using (CommonContext db = new CommonContext())
                {
                    var model = db.Ads.ToList();
                    model.Reverse();
                    IEnumerable<Ad> adsPerPages = model.Skip((page - 1) * pageSize).Take(pageSize);
                    PageInfo pageInfo = new PageInfo { PageNumber = page, PageSize = pageSize, TotalItems = model.Count };
                    IndexViewModel ivm = new IndexViewModel { PageInfo = pageInfo, Ads = adsPerPages };
                    ViewBag.IsUnread = IsUnread();
                    return View(ivm);
                }   
            }

            return View("Guest");
            
        }

        public JsonResult GetRegions(string country)
        {
            var regions = new List<string>();
            switch (country)
            {
                case "not selected":
                        regions.Add("not selected");
                    break;
                case "USA":
                    using (StreamReader sr = new StreamReader(Server.MapPath("~/App_Data/USA.txt")))
                    {
                        while (!sr.EndOfStream)
                        {
                            string region = sr.ReadLine();
                            regions.Add(region);
                        }
                    }
                    break;
                case "Ukraine":
                    using (StreamReader sr = new StreamReader(Server.MapPath("~/App_Data/Ukraine.txt")))
                    {
                        while (!sr.EndOfStream)
                        {
                            string region = sr.ReadLine();
                            regions.Add(region);
                        }
                    }
                    break;
                case "Russia":
                    using (StreamReader sr = new StreamReader(Server.MapPath("~/App_Data/Russia.txt")))
                    {
                        while (!sr.EndOfStream)
                        {
                            string region = sr.ReadLine();
                            regions.Add(region);
                        }
                    }
                    break;
            }
            //Add JsonRequest behavior to allow retrieving states over http get  
            return Json(regions, JsonRequestBehavior.AllowGet);
        }  


        [HttpGet]
        public ActionResult No(int id)
        {
            ViewBag.IsUnread = IsUnread();
            AdView ad = new AdView();
            using (CommonContext db = new CommonContext())
            {
                var model = db.Ads.Find(id);
                model.Views++;
                db.SaveChanges();

                int subsID = Convert.ToInt32(id);

                List<Sub> subscribers;
                subscribers = (from items in db.Subs where items.SubID == subsID select items).ToList<Sub>();
                List<CustomUserInfo> users = new List<CustomUserInfo>();
                if (subscribers.Count != 0)
                    using (CommonContext ctx = new CommonContext())
                    {
                        foreach (var item in subscribers)
                        {
                            var user = (from items in ctx.CustomUserInfos where items.UserName == item.UserID select items).Single();
                            users.Add(user);
                        }
                    }
                ViewBag.Subscribers = users;

                ViewBag.Flag = false;

                foreach (var item in subscribers)
                    if (item.UserID == User.Identity.Name)
                        ViewBag.Flag = true;

                CustomUserInfo author = (from items in db.CustomUserInfos
                                         where items.UserName == model.OwnerID
                                         select items).Single<CustomUserInfo>();
                ad.Id = model.Id;
                ad.Title = model.Title;
                ad.Text = model.Text;
                ad.Date = model.Date;
                ad.Keywords = model.Keywords;
                ad.Theme = model.Theme;
                ad.Location = model.Country;
                ad.Image = model.Image;
                ad.Engaged = model.Engaged;
                ad.Views = model.Views;
                // author
                ad.UserName = author.UserName;
                ad.FirstName = author.FirstName;
                ad.LastName = author.LastName;
                ad.AuthorImage = author.Image;
                ad.Nickname = author.Nickname;
                ad.Occupation = author.Occupation;
                ad.LastTimeOnline = author.LastTimeOnline;

                return View("Ad", ad);
            }
        }


        [HttpPost]
        public ActionResult Subscribe()
        {
            string UserID = Request.Form["UserID"];
            string path = "No/" + Request.Form["SubscriptionID"];
            int subsID = Convert.ToInt32(Request.Form["SubscriptionID"]);

            using (CommonContext db = new CommonContext())
            {
                Sub sub = new Sub();
                sub.SubID = subsID;
                sub.UserID = UserID;

                var entry = (from item in db.Subs
                             where
                                 (item.UserID == UserID &&
                                 item.SubID == subsID)
                             select item).ToList();
                if (entry.Count == 0)
                {
                    db.Subs.Add(sub);
                    db.SaveChanges();
                    using (CommonContext ctx = new CommonContext())
                    {
                        Ad ad = ctx.Ads.Find(subsID);
                        ad.Engaged++;
                        ctx.SaveChanges();
                    }
                }
            }
            return RedirectToAction(path);
        }

        public ActionResult Search(string Keywords, string Theme, string Location, string Popularity, int page = 1)
        {
            //this.TempData["Keywords"] = Keywords;
            ViewBag.Keywords = Keywords;
            ViewBag.Theme = Theme;
            //ViewBag.Location = Location;
            ViewBag.Popularity = Popularity;

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

                int pageSize = 2;
                IEnumerable<Ad> adsPerPages = model.Skip((page - 1) * pageSize).Take(pageSize);
                PageInfo pageInfo = new PageInfo { PageNumber = page, PageSize = pageSize, TotalItems = model.Count };
                IndexViewModel ivm = new IndexViewModel { PageInfo = pageInfo, Ads = sortedModel };

                //sortedModel.Reverse();
                ViewBag.IsUnread = IsUnread();
                return View(ivm);
            }
        }

        public ActionResult preSearch(string Keywords, int page = 1)
        {
            ViewBag.Keywords = Keywords;

            using (CommonContext db = new CommonContext())
            {
                List<Ad> sortedModel = new List<Ad>();
                List<Ad> model;
                model = (from items in db.Ads select items).ToList();

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
                
                int pageSize = 2;
                IEnumerable<Ad> adsPerPages = model.Skip((page - 1) * pageSize).Take(pageSize);
                PageInfo pageInfo = new PageInfo { PageNumber = page, PageSize = pageSize, TotalItems = model.Count };
                IndexViewModel ivm = new IndexViewModel { PageInfo = pageInfo, Ads = sortedModel };
                ViewBag.IsUnread = IsUnread();
                return View("Search", ivm);
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
