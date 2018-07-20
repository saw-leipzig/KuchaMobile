using Newtonsoft.Json;
using Plugin.Settings;
using Plugin.Settings.Abstractions;
using System;
using System.Collections.Generic;

namespace KuchaMobile.Internal
{
    /// <summary>
    /// This is the Settings static class that can be used in your Core solution or in any
    /// of your client applications. All settings are laid out the same exact way with getters
    /// and setters. From https://github.com/jamesmontemagno/SettingsPlugin
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

        private const string savedNotes = "savedNotes";
        public static readonly string savedNotesDefault = JsonConvert.SerializeObject(new List<NotesSaver>());

        private const string caveSearchHistory = "caveSearchHistory";
        public static readonly string caveSearchHistoryDefault = JsonConvert.SerializeObject(new List<CaveFilter>());

        private const string showPreviewPictures = "showPreviewPictures";
        public static readonly bool showPreviewPicturesDefault = true;

        #endregion Setting Constants

        public static bool showPreviewPicturesSetting
        {
            get
            {
                return AppSettings.GetValueOrDefault(showPreviewPictures, showPreviewPicturesDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(showPreviewPictures, value);
            }
        }

        public static List<CaveFilter> CaveSearchHistorySetting
        {
            get
            {
                return JsonConvert.DeserializeObject<List<CaveFilter>>(AppSettings.GetValueOrDefault(
                    caveSearchHistory,
                    caveSearchHistoryDefault
                    ));
            }
            set
            {
                AppSettings.AddOrUpdateValue(caveSearchHistory, JsonConvert.SerializeObject(value));
            }
        }

        public static List<NotesSaver> SavedNotesSetting
        {
            get
            {
                return JsonConvert.DeserializeObject<List<NotesSaver>>(AppSettings.GetValueOrDefault(
                    savedNotes,
                    savedNotesDefault
                    ));
            }
            set
            {
                AppSettings.AddOrUpdateValue(savedNotes, JsonConvert.SerializeObject(value));
            }
        }

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
    }
}