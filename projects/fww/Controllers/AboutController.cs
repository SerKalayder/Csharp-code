using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using fww.Models;

namespace fww.Controllers
{
    public class AboutController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.IsUnread = IsUnread();
            return View();
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
