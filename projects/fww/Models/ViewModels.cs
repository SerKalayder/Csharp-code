using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace fww.Models
{
    public class AdView
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }
        public string Keywords { get; set; }
        public string Theme { get; set; }
        public string Location { get; set; }
        public string Image { get; set; }
        public int Engaged { get; set; }
        public int Views { get; set; }
        public bool Checked { get; set; }
        public int Rank { get; set; }
        
        // author
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string AuthorImage { get; set; }
        public string Nickname { get; set; }
        public string Occupation { get; set; }
        public DateTime LastTimeOnline { get; set; }

    }
}