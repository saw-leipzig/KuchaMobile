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
        /*static List<CaveDistrictModel> caveDistricts;
        static List<CaveRegionModel> caveRegions;
        static List<CaveSiteModel> caveSites;
        static List<CaveTypeModel> caveTypes;
        static List<CaveModel> caves;

        static Dictionary<string, CaveTypeModel> caveTypeDictionary;*/
        static KuchaContainer kuchaContainer;

        public static bool RefreshCaveData()
        {
            List<CaveDistrictModel> caveDistrictModels = Connection.GetCaveDistrictModels();
            if (caveDistrictModels != null)
            {
                kuchaContainer.caveDistricts = caveDistrictModels;
            }
            else return false;

            List<CaveRegionModel> caveRegionModels = Connection.GetCaveRegionModels();
            if (caveRegionModels != null)
            {
                kuchaContainer.caveRegions = caveRegionModels;
            }
            else return false;

            List<CaveSiteModel> caveSiteModels = Connection.GetCaveSiteModels();
            if (caveSiteModels != null)
            {
                kuchaContainer.caveSites = caveSiteModels;
            }
            else return false;

            List<CaveTypeModel> caveTypeModels = Connection.GetCaveTypeModels();
            if (caveTypeModels != null)
            {
                kuchaContainer.caveTypes = caveTypeModels;
                kuchaContainer.caveTypeDictionary = new Dictionary<string, CaveTypeModel>();
                kuchaContainer.caveTypeDictionary.Add("Egal", null);
                foreach(CaveTypeModel c in caveTypeModels)
                {
                    kuchaContainer.caveTypeDictionary.Add(c.nameEN, c);
                }
            }
            else return false;

            List<CaveModel> caveModels = Connection.GetAllCaves();
            if (caveModels != null)
            {
                kuchaContainer.caves = caveModels;
            }
            else return false;

            List<IconographyModel> iconographieModels = Connection.GetIconographyModels();
            if (iconographieModels != null)
            {
                kuchaContainer.iconographies = iconographieModels;
            }
            else return false;

            kuchaContainer.timeStamp = DateTime.UtcNow;
            Settings.KuchaContainerSetting = kuchaContainer;
            return true;
        }

        public static DateTime GetDataTimeStamp()
        {
            return kuchaContainer.timeStamp;
        }

        public static List<CaveModel> GetCavesByFilters(CaveTypeModel caveTypeModel, List<CaveDistrictModel> pickedDistricts, List<CaveRegionModel> pickedRegions, List<CaveSiteModel> pickedSites)
        {
            List<CaveModel> resultCaves = new List<CaveModel>(kuchaContainer.caves);   //Clone them to not access the full list
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
            return kuchaContainer.caveTypeDictionary;
        }

        public static List<CaveDistrictModel> GetCaveDistricts()
        {
            return kuchaContainer.caveDistricts;
        }

        public static List<CaveRegionModel> GetCaveRegions()
        {
            return kuchaContainer.caveRegions;
        }

        public static List<CaveSiteModel> GetCaveSites()
        {
            return kuchaContainer.caveSites;
        }

        public static List<CaveTypeModel> GetCaveTypes()
        {
            return kuchaContainer.caveTypes;
        }

        public static List<IconographyModel> GetIconographies()
        {
            return kuchaContainer.iconographies;
        }

        public static string GetCaveDistrictStringByID(int id)
        {
            foreach(CaveDistrictModel c in kuchaContainer.caveDistricts)
            {
                if (c.districtID == id)
                    return c.name;
            }
            return "";
        }

        public static string GetCaveRegionStringByID(int id)
        {
            foreach (CaveRegionModel c in kuchaContainer.caveRegions)
            {
                if (c.regionID == id)
                    return c.englishName;
            }
            return "";
        }

        public static string GetCaveSiteStringByID(int id)
        {
            foreach (CaveSiteModel c in kuchaContainer.caveSites)
            {
                if (c.siteID == id)
                    return c.name;
            }
            return "";
        }

        public static string GetCaveTypeStringByID(int id)
        {
            foreach (CaveTypeModel c in kuchaContainer.caveTypes)
            {
                if (c.caveTypeID == id)
                    return c.nameEN;
            }
            return "";
        }

        public static string GetCaveTypeSketchByID(int id)
        {
            foreach (CaveTypeModel c in kuchaContainer.caveTypes)
            {
                if (c.caveTypeID == id)
                    return c.sketchName;
            }
            return "";
        }

        public static List<PaintedRepresentationModel> GetPaintedRepresentationsByIconographies(List<IconographyModel> iconographies, bool exclusive)
        {
            List<PaintedRepresentationModel> resultList = Connection.GetPaintedRepresentationsByFilter(iconographies, exclusive);
            return resultList;
        }

        public static void LoadPersistantData()
        {
            kuchaContainer = Settings.KuchaContainerSetting;

            Connection.LoadCachedSessionID();
        }

        public static bool CaveDataIsValid()
        {
            if (kuchaContainer.caveDistricts == new List<CaveDistrictModel>() ||
                kuchaContainer.caveRegions == new List<CaveRegionModel>() ||
                kuchaContainer.caveSites == new List<CaveSiteModel>() ||
                kuchaContainer.caveTypes == new List<CaveTypeModel>() ||
                kuchaContainer.caves == new List<CaveModel>())
                return false;
            else return true;
        }
    }
}
