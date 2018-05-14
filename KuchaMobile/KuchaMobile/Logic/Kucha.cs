using KuchaMobile.Internal;
using KuchaMobile.Logic.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace KuchaMobile.Logic
{
    public class Kucha
    {
        static List<CaveDistrictModel> caveDistricts;
        static List<CaveRegionModel> caveRegions;
        static List<CaveSiteModel> caveSites;
        static List<CaveTypeModel> caveTypes;
        static List<CaveModel> caves;

        static Dictionary<string, CaveTypeModel> caveTypeDictionary;

        public static bool RefreshCaveData()
        {
            List<CaveDistrictModel> caveDistrictModels = Connection.GetCaveDistrictModels();
            if (caveDistrictModels != null)
            {
                caveDistricts = caveDistrictModels;
            }
            else return false;

            List<CaveRegionModel> caveRegionModels = Connection.GetCaveRegionModels();
            if (caveRegionModels != null)
            {
                caveRegions = caveRegionModels;
            }
            else return false;

            List<CaveSiteModel> caveSiteModels = Connection.GetCaveSiteModels();
            if (caveSiteModels != null)
            {
                caveSites = caveSiteModels;
            }
            else return false;

            List<CaveTypeModel> caveTypeModels = Connection.GetCaveTypeModels();
            if (caveTypeModels != null)
            {
                caveTypes = caveTypeModels;
                caveTypeDictionary = new Dictionary<string, CaveTypeModel>();
                caveTypeDictionary.Add("Egal", null);
                foreach(CaveTypeModel c in caveTypeModels)
                {
                    caveTypeDictionary.Add(c.nameEN, c);
                }
            }
            else return false;

            List<CaveModel> caveModels = Connection.GetAllCaves();
            if (caveModels != null)
            {
                caves = caveModels;
            }
            else return false;

            return true;
        }

        public static List<CaveModel> GetCavesByFilters(CaveTypeModel caveTypeModel, List<CaveDistrictModel> pickedDistricts, List<CaveRegionModel> pickedRegions, List<CaveSiteModel> pickedSites)
        {
            List<CaveModel> resultCaves = new List<CaveModel>(caves);   //Clone them to not access the full list
            if(caveTypeModel!=null)
            {
                resultCaves.RemoveAll(cave => cave.caveTypeID != caveTypeModel.caveTypeID);
            }

            if(pickedDistricts != null && pickedDistricts.Count>0)
            {
                resultCaves.RemoveAll(cave => pickedDistricts.Any(district => cave.districtID == district.districtID)==false);
            }

            if(pickedRegions != null && pickedRegions.Count>0)
            {
                resultCaves.RemoveAll(cave => pickedRegions.Any(region => cave.regionID == region.regionID)==false);
            }

            if(pickedSites != null && pickedSites.Count > 0)
            {
                resultCaves.RemoveAll(cave => pickedSites.Any(site => cave.siteID == site.siteID)==false);
            }
            return resultCaves;
        }

        public static Dictionary<string, CaveTypeModel> GetCaveTypeDictionary()
        {
            return caveTypeDictionary;
        }

        public static List<CaveDistrictModel> GetCaveDistricts()
        {
            return caveDistricts;
        }

        public static List<CaveRegionModel> GetCaveRegions()
        {
            return caveRegions;
        }

        public static List<CaveSiteModel> GetCaveSites()
        {
            return caveSites;
        }

        public static List<CaveTypeModel> GetCaveTypes()
        {
            return caveTypes;
        }

        public static void LoadPersistantData()
        {
            caveDistricts = Settings.caveDistrictsSetting;
            caveRegions = Settings.caveRegionSetting;
            caveSites = Settings.caveSiteSetting;
            caveTypes = Settings.caveTypeSetting;
            caveTypeDictionary = new Dictionary<string, CaveTypeModel>();
            caveTypeDictionary.Add("Egal", null);
            foreach (CaveTypeModel c in caveTypes)
            {
                caveTypeDictionary.Add(c.nameEN, c);
            }
            caves = Settings.caveSetting;

            Connection.LoadCachedSessionID();
        }

        public static bool CaveDataIsValid()
        {
            if (caveDistricts == new List<CaveDistrictModel>() ||
                caveRegions == new List<CaveRegionModel>() ||
                caveSites == new List<CaveSiteModel>() ||
                caveTypes == new List<CaveTypeModel>() ||
                caves == new List<CaveModel>())
                return false;
            else return true;
        }
    }
}
