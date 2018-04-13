using KuchaMobile.Internal;
using KuchaMobile.Logic.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace KuchaMobile.Logic
{
    public class Kucha
    {
        static List<caveDistrictModel> caveDistricts;
        static List<caveRegionModel> caveRegions;
        static List<caveTypeModel> caveTypes;
        static List<caveSiteModel> caveSites;

        private static bool RefreshCaveFilters()
        {
            List<caveDistrictModel> caveDistrictModels = Connection.GetCaveDistrictModels();
            if (caveDistrictModels != null)
            {
                caveDistricts = caveDistrictModels;
            }
            else return false;

            List<caveRegionModel> caveRegionModels = Connection.GetCaveRegionModels();
            if (caveRegionModels != null)
            {
                caveRegions = caveRegionModels;
            }
            else return false;

            List<caveSiteModel> caveSiteModels = Connection.GetCaveSiteModels();
            if (caveSiteModels != null)
            {
                caveSites = caveSiteModels;
            }
            else return false;

            List<caveTypeModel> caveTypeModels = Connection.GetCaveTypeModels();
            if (caveTypeModels != null)
            {
                caveTypes = caveTypeModels;
            }
            else return false;

            return true;
        }

        public static List<caveSiteModel> GetCaveSites()
        {
            return caveSites;
        }

        public static List<caveTypeModel> GetCaveTypes()
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
