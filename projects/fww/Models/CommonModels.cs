using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace fww.Models
{
    public class Ad
    {
        public int Id { get; set; }
        public string OwnerID { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }
        public string Keywords { get; set; }
        public string Theme { get; set; }
        public string Country { get; set; }
        public string Region { get; set; }
        public string Image { get; set; }
        public int Engaged { get; set; }
        public int Views { get; set; }
        public bool Checked { get; set; }
        public bool Opened { get; set; }
        public int Rank { get; set; }

    }

    public class Sub
    {
        public int Id { get; set; }
        public int SubID { get; set; }
        public string UserID { get; set; }
    }

    public class UserMessage
    {
        public int Id { get; set; }
        public string Sender { get; set; }
        public string SenderName { get; set; }
        public string Receiver { get; set; }
        public string ReceiverName { get; set; }
        public string Message { get; set; }
        public DateTime Date { get; set; }
    }

    public class MessageRel
    {
        public int Id { get; set; }
        public string Receiver { get; set; }
        public string Sender { get; set; }
        public bool Unread { get; set; }
    }

    public class Interlocutor
    {
        public string UserName { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public bool Unread { get; set; }
    }

    public class CommonContext : DbContext
    {
        public CommonContext()
            : base("DefaultConnection") {}
        public DbSet<Ad> Ads { get; set; }
        public DbSet<Sub> Subs { get; set; }
        public DbSet<UserMessage> UsersMessages { get; set; }
        public DbSet<MessageRel> MessageRels { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<CustomUserInfo> CustomUserInfos { get; set; }
    }
}