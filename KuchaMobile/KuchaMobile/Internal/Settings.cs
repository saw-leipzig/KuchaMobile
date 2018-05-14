using System;
using System.Collections.Generic;
using System.Text;
using KuchaMobile.Logic;
using KuchaMobile.Logic.Models;
using Newtonsoft.Json;
using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace KuchaMobile.Internal
{
    /// <summary>
    /// This is the Settings static class that can be used in your Core solution or in any
    /// of your client applications. All settings are laid out the same exact way with getters
    /// and setters. 
    /// </summary>
    public static class Settings
    {
        private static ISettings AppSettings
        {
            get
            {
                return CrossSettings.Current;
            }
        }

        #region Setting Constants


        private const string localToken = "localToken";
        public static readonly string localTokenDefault = String.Empty;

        private const string caveDistricts = "caveDistricts";
        public static readonly string caveDistrictsDefault = JsonConvert.SerializeObject(new List<CaveDistrictModel>());

        private const string caveRegions = "caveRegions";
        public static readonly string caveRegionsDefault = JsonConvert.SerializeObject(new List<CaveRegionModel>());

        private const string caveSites = "caveSites";
        public static readonly string caveSitesDefault = JsonConvert.SerializeObject(new List<CaveSiteModel>());

        private const string caveTypes = "caveTypes";
        public static readonly string caveTypesDefault = JsonConvert.SerializeObject(new List<CaveTypeModel>());

        private const string caves = "caves";
        public static readonly string cavesDefault = JsonConvert.SerializeObject(new List<CaveModel>());

        #endregion

        public static string LocalTokenSetting
        {
            get
            {
                return AppSettings.GetValueOrDefault(localToken, localTokenDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(localToken, value);
            }
        }

        public static List<CaveModel> caveSetting
        {
            get
            {
                return JsonConvert.DeserializeObject<List<CaveModel>>(AppSettings.GetValueOrDefault(caves, cavesDefault));
            }
            set
            {
                AppSettings.AddOrUpdateValue(caves, JsonConvert.SerializeObject(value));
            }
        }

        public static List<CaveDistrictModel> caveDistrictsSetting
        {
            get
            {
                return JsonConvert.DeserializeObject<List<CaveDistrictModel>>(AppSettings.GetValueOrDefault(caveDistricts, caveDistrictsDefault));
            }
            set
            {
                AppSettings.AddOrUpdateValue(caveDistricts, JsonConvert.SerializeObject(value));
            }
        }

        public static List<CaveRegionModel> caveRegionSetting
        {
            get
            {
                return JsonConvert.DeserializeObject<List<CaveRegionModel>>(AppSettings.GetValueOrDefault(caveRegions, caveRegionsDefault));
            }
            set
            {
                AppSettings.AddOrUpdateValue(caveRegions, JsonConvert.SerializeObject(value));
            }
        }

        public static List<CaveSiteModel> caveSiteSetting
        {
            get
            {
                return JsonConvert.DeserializeObject<List<CaveSiteModel>>(AppSettings.GetValueOrDefault(caveSites, caveSitesDefault));
            }
            set
            {
                AppSettings.AddOrUpdateValue(caveSites, JsonConvert.SerializeObject(value));
            }
        }

        public static List<CaveTypeModel> caveTypeSetting
        {
            get
            {
                return JsonConvert.DeserializeObject<List<CaveTypeModel>>(AppSettings.GetValueOrDefault(caveTypes, caveTypesDefault));
            }
            set
            {
                AppSettings.AddOrUpdateValue(caveTypes, JsonConvert.SerializeObject(value));
            }
        }
    }
}

