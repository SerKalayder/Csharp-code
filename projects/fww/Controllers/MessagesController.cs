using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using fww.Models;

namespace fww.Controllers
{
    public class MessagesController : Controller
    {
        public ActionResult Index()
        {
            List<Interlocutor> Interlocutors = new List<Interlocutor>();
            using (CommonContext ctx = new CommonContext())
            {
                List<MessageRel> mr = (from items in ctx.MessageRels 
                                           where items.Receiver == User.Identity.Name
                                       select items).ToList<MessageRel>();
                List<CustomUserInfo> users = new List<CustomUserInfo>();
                foreach (MessageRel item in mr)
                {
                    CustomUserInfo user = (from us in ctx.CustomUserInfos
                                           where us.UserName == item.Sender
                                           select us).Single<CustomUserInfo>();
                    if (user != null)
                        users.Add(user);
                }
                for (int i = 0; i < mr.Count; i++)
                {
                    Interlocutor inter = new Interlocutor();
                    inter.Unread = mr.ElementAt<MessageRel>(i).Unread;
                    inter.Name = users.ElementAt<CustomUserInfo>(i).Nickname;
                    inter.UserName = users.ElementAt<CustomUserInfo>(i).UserName;
                    inter.Image = users.ElementAt<CustomUserInfo>(i).Image;
                    Interlocutors.Add(inter);
                }
            }

            ViewBag.IsUnread = IsUnread();
            return View(Interlocutors);
        }

        [HttpGet]
        public ActionResult ConvoWith(string interlocutor)
        {
            interlocutor = (string)RouteData.Values["id"];
            List<UserMessage> messages;
            CustomUserInfo user; ;
            using(CommonContext ctx = new CommonContext())
            {
                messages = (from items in ctx.UsersMessages 
                                                  where ((items.Sender == interlocutor && 
                                                        items.Receiver == User.Identity.Name) ||
                                                        (items.Receiver == interlocutor &&
                                                        items.Sender == User.Identity.Name))
                                                    select items).ToList<UserMessage>();
            
                List<MessageRel> mrl = (from items in ctx.MessageRels 
                                     where (items.Receiver == User.Identity.Name && items.Sender == interlocutor)
                                 select items).ToList<MessageRel>();
                if (mrl.Count != 0)
                {
                    MessageRel mr = mrl.Single<MessageRel>();
                    mr.Unread = false;
                    ctx.Entry(mr).State = System.Data.EntityState.Modified;
                    ctx.SaveChanges();   
                }

                user = (from items in ctx.CustomUserInfos
                        where items.UserName == interlocutor
                        select items).Single<CustomUserInfo>();
            }
            messages.Reverse();
            ViewBag.Messages = messages;
            ViewBag.Interlocutor = user;
            ViewBag.IsUnread = IsUnread();
            return View();
        }

        [HttpPost]
        public ActionResult ConvoWith()
        {
            string message = Request.Form["MessageText"];
            DateTime date = DateTime.Now;
            string sender = User.Identity.Name;
            string receiver = Request.Form["MessageReceiver"];

            using (CommonContext ctx = new CommonContext())
            {
                CustomUserInfo senderInfo = (from items in ctx.CustomUserInfos
                                             where items.UserName == sender
                                             select items).Single<CustomUserInfo>();
                CustomUserInfo receiverInfo = (from items in ctx.CustomUserInfos
                                               where items.UserName == receiver
                                             select items).Single<CustomUserInfo>();

                UserMessage msg = new UserMessage();
                msg.Sender = sender;
                msg.SenderName = senderInfo.Nickname;
                msg.Receiver = receiver;
                msg.ReceiverName = receiverInfo.Nickname;
                msg.Message = message;
                msg.Date = date;

                List<MessageRel> ml = (from items in ctx.MessageRels
                                       where items.Receiver == receiver
                                       select items).ToList<MessageRel>();
                MessageRel seekingMR = null;
                if (ml.Count != 0)
                {
                    foreach (MessageRel item in ml)
                    {
                        if (item.Sender == sender)
                        {
                            seekingMR = item;
                            seekingMR.Unread = true;
                            ctx.Entry(seekingMR).State = System.Data.EntityState.Modified;
                        }
                    }
                    if (seekingMR == null)
                    {
                        seekingMR = new MessageRel();
                        seekingMR.Receiver = receiver;
                        seekingMR.Sender = sender;
                        seekingMR.Unread = true;
                        ctx.Entry(seekingMR).State = System.Data.EntityState.Added;
                    }
                }
                else
                {
                    seekingMR = new MessageRel();
                    seekingMR.Receiver = receiver;
                    seekingMR.Sender = sender;
                    seekingMR.Unread = true;
                    ctx.Entry(seekingMR).State = System.Data.EntityState.Added;
                }

                ctx.Entry(msg).State = System.Data.EntityState.Added;
                ctx.SaveChanges();
            }

            return RedirectToAction("ConvoWith/" + receiver);
        }

        [HttpGet]
        public ActionResult New(string r)
        {
            ViewBag.Receiver = (string)RouteData.Values["id"];
            ViewBag.IsUnread = IsUnread();
            return View();
        }
        [HttpPost]
        public ActionResult New()
        {
            string message = Request.Form["MessageText"];
            DateTime date = DateTime.Now;
            string sender = User.Identity.Name;
            string receiver = Request.Form["MessageReceiver"];

            using (CommonContext ctx = new CommonContext())
            {
                UserMessage msg = new UserMessage();
                msg.Sender = sender;
                msg.Receiver = receiver;
                msg.Message = message;
                msg.Date = date;

                List<MessageRel> ml = (from items in ctx.MessageRels 
                                           where items.Receiver == receiver
                                       select items).ToList<MessageRel>();
                MessageRel seekingMR = null;
                if (ml.Count != 0)
                {
                    foreach (MessageRel item in ml)
                    {
                        if (item.Sender == sender)
                        {
                            seekingMR = item;
                            seekingMR.Unread = true;
                            ctx.Entry(seekingMR).State = System.Data.EntityState.Modified;
                        }
                    }
                    if (seekingMR == null)
                    {
                        seekingMR = new MessageRel();
                        seekingMR.Receiver = receiver;
                        seekingMR.Sender = sender;
                        seekingMR.Unread = true;
                        ctx.Entry(seekingMR).State = System.Data.EntityState.Added;
                    }
                }
                else
                {
                    seekingMR = new MessageRel();
                    seekingMR.Receiver = receiver;
                    seekingMR.Sender = sender;
                    seekingMR.Unread = true;
                    ctx.Entry(seekingMR).State = System.Data.EntityState.Added;
                }

                ctx.Entry(msg).State = System.Data.EntityState.Added;
                ctx.SaveChanges();
            }

            return RedirectToAction("Index");
        }
        public ActionResult Sent()
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
