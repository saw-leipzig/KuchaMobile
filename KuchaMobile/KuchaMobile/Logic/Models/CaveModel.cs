using System;
using System.Collections.Generic;
using System.Text;

namespace KuchaMobile.Logic.Models
{
    public class CaveModel
    {
        public int caveID { get; set; }
        public string officialNumber { get; set; }
        public string historicalName { get; set; }
        public string optionalHistoricalName { get; set; }
        public int caveTypeID { get; set; }
        public int siteID { get; set; }
        public int districtID { get; set; }
        public int regionID { get; set; }
        public string optionalCaveSketch { get; set; }
        public string measurementString { get; set; }

        //Not in JSON but manually needed
        public string CaveDisplayName
        {
            get
            {
                return string.Format("Cave: {0}", caveID);
            }
        }
    }
}
