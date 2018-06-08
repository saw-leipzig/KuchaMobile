using System;
using System.Collections.Generic;
using System.Text;

namespace KuchaMobile.Logic.Models
{
    public class CaveModel
    {
        public int caveID { get; set; }
        public string officialNumber { get; set; }
        public string historicName { get; set; }
        public string optionalHistoricName { get; set; }
        public int caveTypeID { get; set; }
        public int siteID { get; set; }
        public int districtID { get; set; }
        public int regionID { get; set; }
        public int orientationID { get; set; }
        public int preservationClassificationID { get; set; }
        public int caveGroupID { get; set; }
        public string stateOfPerservation { get; set; }
        public string findings { get; set; }
        public string firstDocumentedBy { get; set; }
        public int firstDocumentedInYear { get; set; }
        public string optionalCaveSketch { get; set; }
        public bool hasVolutedHorseShoeArch { get; set; }
        public bool hasSculptures { get; set; }
        public bool hasClayFigures { get; set; }
        public bool hasImmitationOfMountains { get; set; }
        public bool hasHolesForFixationOfPlasticalItems { get; set; }
        public bool hasWoodenConstruction { get; set; }
        public List<object> caveAreaList { get; set; }
        public List<object> wallList { get; set; }
        public List<object> c14AnalysisUrlList { get; set; }
        public List<object> c14DocumentList { get; set; }
        public bool openAccess { get; set; }
        public string caveLayoutComments { get; set; }

        //Not in JSON but manually needed
        public string CaveDisplayName
        {
            get
            {
                return string.Format("Cave: {0}", caveID);
            }
        }
        public string Notes { get; set; }
    }
}
