using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using fww.Models;

namespace fww.Controllers
{
    public class MySubsController : Controller
    {
        public ActionResult Index()
        {
            using (CommonContext db = new CommonContext())
            {
                var subs = (from items in db.Subs
                            where items.UserID == User.Identity.Name
                            select items).ToList();

                List<Ad> model = new List<Ad>();
                foreach (var sub in subs)
                {
                    List<Ad> ad = (from items in db.Ads
                             where items.Id == sub.SubID
                             select items).ToList<Ad>();
                    if (ad.Count == 1)
                    {
                        Ad a = ad.Single<Ad>();
                        model.Add(a);    
                    }
                    
                }

                CustomUserInfo user = (from items in db.CustomUserInfos
                                       where items.UserName == User.Identity.Name
                                       select items).Single<CustomUserInfo>();

                ViewBag.User = user;
                ViewBag.IsUnread = IsUnread();
                model.Reverse();
                return View(model);
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
