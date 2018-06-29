using KuchaMobile.Logic;
using KuchaMobile.Logic.Models;
using System.Collections.Generic;

namespace KuchaMobile.Internal
{
    public class CaveFilter
    {
        public CaveTypeModel caveTypeModel { get; set; }
        public List<CaveDistrictModel> pickedDistricts { get; set; }
        public List<CaveRegionModel> pickedRegions { get; set; }
        public List<CaveSiteModel> pickedSites { get; set; }
        public string FoundResultsString { get; set; }
        public string SearchTimeString { get; set; }
    }
}