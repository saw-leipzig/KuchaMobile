using System;
using System.Collections.Generic;
using System.Text;

namespace KuchaMobile.Logic.Models
{
    public class PaintedRepresentationModel
    {
        public int depictionID { get; set; }
        public int styleID { get; set; }
        public string inscriptions { get; set; }
        public string separateAksaras { get; set; }
        public string dating { get; set; }
        public string description { get; set; }
        public string backgroundColour { get; set; }
        public string generalRemarks { get; set; }
        public string otherSuggestedIdentifications { get; set; }
        public double width { get; set; }
        public double height { get; set; }
        public int expeditionID { get; set; }
        public int locationID { get; set; }
        public string inventoryNumber { get; set; }
        public int vendorID { get; set; }
        public int storyID { get; set; }
        public CaveModel cave { get; set; }
        public int wallID { get; set; }
        public int absoluteLeft { get; set; }
        public int absoluteTop { get; set; }
        public int modeOfRepresentationID { get; set; }
        public string shortName { get; set; }
        public int masterImageID { get; set; }
        public List<RelatedImage> relatedImages { get; set; }
        public List<object> preservationAttributesList { get; set; }
        public List<object> relatedBibliographyList { get; set; }
        public bool openAccess { get; set; }

        //Not in JSON but manually needed
        public string PRDisplayName
        {
            get
            {
                return String.Format("ID: "+depictionID);
            }
        }
        public string PRDetailDisplayName
        {
            get
            {
                return String.Format(cave.historicName);
            }
        }
    }

    public class RelatedImage
    {
        public int imageID { get; set; }
        public int photographerID { get; set; }
        public int imageTypeID { get; set; }
        public string copyright { get; set; }
        public string comment { get; set; }
        public string filename { get; set; }
        public string title { get; set; }
        public string shortName { get; set; }
        public bool openAccess { get; set; }
        public string date { get; set; }
    }

}
