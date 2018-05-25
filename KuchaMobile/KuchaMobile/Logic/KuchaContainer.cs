using KuchaMobile.Logic.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace KuchaMobile.Logic
{
    public class KuchaContainer
    {
        public List<CaveDistrictModel> caveDistricts;
        public List<CaveRegionModel> caveRegions;
        public List<CaveSiteModel> caveSites;
        public List<CaveTypeModel> caveTypes;
        public List<CaveModel> caves;

        public Dictionary<string, CaveTypeModel> caveTypeDictionary;

        public KuchaContainer()
        {   
            caveDistricts = new List<CaveDistrictModel>();
            caveRegions = new List<CaveRegionModel>();
            caveSites = new List<CaveSiteModel>();
            caveTypes = new List<CaveTypeModel>();
            caves = new List<CaveModel>();
            caveTypeDictionary = new Dictionary<string, CaveTypeModel>();
        }
    }
}
