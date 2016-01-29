using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using fww.Models;
using System.Data.Entity;

namespace fww.Controllers
{
    public class MyAdsController : Controller
    {
        public ActionResult Index()
        {
            using (CommonContext db = new CommonContext())
            {
                var model = (from items in db.Ads where items.OwnerID == User.Identity.Name select items).ToList();
                model.Reverse();
                ViewBag.IsUnread = IsUnread();
                return View(model);
            }
        }

        public ActionResult New()
        {
            ViewBag.IsUnread = IsUnread();
            return View();
        }

        [HttpPost]
        public ActionResult New(Ad advert)
        {
            using (CommonContext db = new CommonContext()) 
            {
                advert.Date = DateTime.Now;
                advert.Engaged = 0;
                advert.Views = 0;
                advert.Theme = advert.Theme;
                advert.OwnerID = User.Identity.Name;
                advert.Country = advert.Country;

                db.Ads.Add(advert);
                db.SaveChanges();
            }            
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            ViewBag.IsUnread = IsUnread();
            if (id == null)
            {
                return HttpNotFound();
            }
            using (CommonContext db = new CommonContext())
            {
                Ad advert = db.Ads.Find(id);
                if (advert != null)
                {
                    return View(advert);
                }

                return HttpNotFound();
            }
        }
        [HttpPost]
        public ActionResult Edit(Ad advert)
        {
            using (CommonContext db = new CommonContext())
            {
                Ad ad = db.Ads.Find(advert.Id);
                ad.Title = advert.Title;
                ad.Text = advert.Text;
                ad.Keywords = advert.Keywords;
                ad.Theme = advert.Theme;
                ad.Date = DateTime.Now;
                ad.Country = advert.Country;
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        
        public ActionResult Delete(int? id)
        {
            using (CommonContext db = new CommonContext())
            {
                Ad ad = db.Ads.Find(id);
                db.Ads.Remove(ad);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
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