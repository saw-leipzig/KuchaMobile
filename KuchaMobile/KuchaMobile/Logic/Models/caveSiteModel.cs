using System;
using System.Collections.Generic;
using System.Text;

namespace KuchaMobile.Logic.Models
{
    public class CaveSiteModel
    {
        public int siteID { get; set; }
        public string name { get; set; }
        public string alternativeName { get; set; }
        public string shortName { get; set; }
        public bool openAccess { get; set; }
    }
}
