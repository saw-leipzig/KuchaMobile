using System;
using System.Collections.Generic;
using System.Text;

namespace KuchaMobile.Logic.Models
{
    public class IconographyRootCategory
    {
        public int iconographyID { get; set; }
        public int parentID { get; set; }
        public string text { get; set; }
        public List<IconographySubcategory> children { get; set; }   //Children are included in the JSON to allow hierarchybased selecting, we wont use this in the app however
        public bool openAccess { get; set; }

        public class IconographySubcategory
        {
            public int iconographyID { get; set; }
            public int parentID { get; set; }
            public string text { get; set; }
            public List<Iconography> children { get; set; }
            public bool openAccess { get; set; }
        }

        public class Iconography
        {
            public int iconographyID { get; set; }
            public int parentID { get; set; }
            public string text { get; set; }
            public List<object> children { get; set; }
            public bool openAccess { get; set; }
        }
    }
}
