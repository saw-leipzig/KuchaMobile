using KuchaMobile.Logic.Models;
using System;
using System.Collections.Generic;

namespace KuchaMobile.Logic
{
    /// <summary>
    /// This class holds all data that is locally available (because saved on the device) during runtime
    /// </summary>
    public class KuchaContainer
    {
        public List<CaveDistrictModel> CaveDistricts { get; set; }
        public List<CaveRegionModel> CaveRegions { get; set; }
        public List<CaveSiteModel> CaveSites { get; set; }
        public List<CaveTypeModel> CaveTypes { get; set; }
        public List<CaveModel> Caves { get; set; }
        public List<IconographyModel> Iconographies { get; set; }
        public Dictionary<string, CaveTypeModel> CaveTypeDictionary { get; set; }
        public DateTime TimeStamp { get; set; }

        public KuchaContainer()
        {
            CaveDistricts = new List<CaveDistrictModel>();
            CaveRegions = new List<CaveRegionModel>();
            CaveSites = new List<CaveSiteModel>();
            CaveTypes = new List<CaveTypeModel>();
            Caves = new List<CaveModel>();
            CaveTypeDictionary = new Dictionary<string, CaveTypeModel>();
            Iconographies = new List<IconographyModel>();
            TimeStamp = DateTime.Now;
        }
    }
}