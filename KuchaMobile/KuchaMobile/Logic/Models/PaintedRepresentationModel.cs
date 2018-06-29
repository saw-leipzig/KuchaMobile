using System;
using System.Collections.Generic;

namespace KuchaMobile.Logic.Models
{
    public class PaintedRepresentationModel
    {
        public int depictionID { get; set; }
        public string description { get; set; }
        public int caveID { get; set; }
        public List<RelatedImage> relatedImages { get; set; }
        public string acquiredByExpedition { get; set; }
        public string vendor { get; set; }
        public string currentLocation { get; set; }
        public List<string> Iconography { get; set; }
        public List<string> PictorialElements { get; set; }

        //Not in JSON but manually needed
        public string PRDisplayName
        {
            get
            {
                return String.Format("ID: " + depictionID);
            }
        }

        public string PRDetailDisplayName
        {
            get
            {
                CaveModel c = Kucha.GetCaveByID(caveID);
                if (c != null)
                    return String.Format(Kucha.GetCaveByID(caveID).historicalName);
                else return "Invalid Cave ID";
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