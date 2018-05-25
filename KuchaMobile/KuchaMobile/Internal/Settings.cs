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

        private const string kuchaContainer = "kuchaContainer";
        public static readonly string kuchaContainerDefault = JsonConvert.SerializeObject(new KuchaContainer());

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

        public static KuchaContainer KuchaContainerSetting
        {
            get
            {
                return JsonConvert.DeserializeObject<KuchaContainer>(AppSettings.GetValueOrDefault(kuchaContainer, kuchaContainerDefault));
            }
            set
            {
                AppSettings.AddOrUpdateValue(kuchaContainer, JsonConvert.SerializeObject(value));
            }
        }
    }
}

