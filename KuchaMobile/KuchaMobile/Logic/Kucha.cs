using KuchaMobile.Internal;
using KuchaMobile.Logic.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace KuchaMobile.Logic
{
    public class Kucha
    {
        static List<CaveDistrictModel> caveDistricts;
        static List<CaveRegionModel> caveRegions;
        static List<CaveTypeModel> caveTypes;
        static List<CaveSiteModel> caveSites;

        static Dictionary<string, CaveTypeModel> caveTypeDictionary;

        private static bool RefreshCaveFilters()
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

            return true;
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

        public static bool InitializeDefaults()
        {
            Connection.LoadCachedSessionID();
            if (Connection.HasLegitSessionID() == false)
                return false;
            if (RefreshCaveFilters() == false)
                return false;


            return true;
        }
    }
}
