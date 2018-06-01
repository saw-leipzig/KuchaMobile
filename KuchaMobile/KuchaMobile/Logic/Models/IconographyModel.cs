using System;
using System.Collections.Generic;
using System.Text;

namespace KuchaMobile.Logic.Models
{
    public class IconographyModel
    {
        public int iconographyID { get; set; }
        public int parentID { get; set; }
        public string text { get; set; }
        public bool openAccess { get; set; }
    }
}
