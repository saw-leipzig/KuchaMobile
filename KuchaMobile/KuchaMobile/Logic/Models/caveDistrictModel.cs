using System;
using System.Collections.Generic;
using System.Text;

namespace KuchaMobile.Logic.Models
{
    public class caveDistrictModel
    {
        public int districtID { get; set; }
        public string name { get; set; }
        public int siteID { get; set; }
        public string description { get; set; }
        public string map { get; set; }
        public string arialMap { get; set; }
        public bool openAccess { get; set; }
    }
}
