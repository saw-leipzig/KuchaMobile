using System;
using System.Collections.Generic;
using System.Text;

namespace KuchaMobile.Logic.Models
{
    public class caveRegionModel
    {
        public int regionID { get; set; }
        public string phoneticName { get; set; }
        public string originalName { get; set; }
        public string englishName { get; set; }
        public int siteID { get; set; }
        public bool openAccess { get; set; }
    }
}
