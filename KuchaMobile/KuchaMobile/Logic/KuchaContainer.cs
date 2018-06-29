using KuchaMobile.Logic.Models;
using System;
using System.Collections.Generic;

namespace KuchaMobile.Logic
{
    public class KuchaContainer
    {
        public List<CaveDistrictModel> caveDistricts;
        public List<CaveRegionModel> caveRegions;
        public List<CaveSiteModel> caveSites;
        public List<CaveTypeModel> caveTypes;
        public List<CaveModel> caves;
        public List<IconographyModel> iconographies;

        public Dictionary<string, CaveTypeModel> caveTypeDictionary;

        public DateTime timeStamp;

        public KuchaContainer()
        {
            caveDistricts = new List<CaveDistrictModel>();
            caveRegions = new List<CaveRegionModel>();
            caveSites = new List<CaveSiteModel>();
            caveTypes = new List<CaveTypeModel>();
            caves = new List<CaveModel>();
            caveTypeDictionary = new Dictionary<string, CaveTypeModel>();
            iconographies = new List<IconographyModel>();
            timeStamp = DateTime.Now;
        }
    }
}