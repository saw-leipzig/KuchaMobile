using KuchaMobile.Internal;
using KuchaMobile.Logic.Models;
using Newtonsoft.Json;
using PCLStorage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace KuchaMobile.Logic
{
    public static class Kucha
    {
        /// <summary>
        /// This class contains all method to access data for the UI
        /// </summary>
        private static KuchaContainer kuchaContainer;

        //Caves
        public static CaveModel GetCaveByID(int id)
        {
            return kuchaContainer.Caves.Find(c => c.caveID == id);
        }

        public static List<CaveModel> GetCavesByFilters(CaveTypeModel caveTypeModel, List<CaveDistrictModel> pickedDistricts, List<CaveRegionModel> pickedRegions, List<CaveSiteModel> pickedSites)
        {
            List<CaveModel> resultCaves = new List<CaveModel>(kuchaContainer.Caves);   //Clone them to not access the full list
            if (caveTypeModel != null)
            {
                resultCaves.RemoveAll(cave => cave.caveTypeID != caveTypeModel.caveTypeID);
            }

            if (pickedDistricts?.Count > 0)
            {
                resultCaves.RemoveAll(cave => !pickedDistricts.Any(district => cave.districtID == district.districtID));
            }

            if (pickedRegions?.Count > 0)
            {
                resultCaves.RemoveAll(cave => !pickedRegions.Any(region => cave.regionID == region.regionID));
            }

            if (pickedSites?.Count > 0)
            {
                resultCaves.RemoveAll(cave => !pickedSites.Any(site => cave.siteID == site.siteID));
            }
            return resultCaves;
        }

        /// <summary>
        /// We use a dictionary for the Picker Element in CaveSearchUI to bind readable names to CaveTypeModels
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, CaveTypeModel> GetCaveTypeDictionary()
        {
            return kuchaContainer.CaveTypeDictionary;
        }

        public static List<CaveDistrictModel> GetCaveDistricts()
        {
            return kuchaContainer.CaveDistricts;
        }

        public static List<CaveRegionModel> GetCaveRegions()
        {
            return kuchaContainer.CaveRegions;
        }

        public static List<CaveSiteModel> GetCaveSites()
        {
            return kuchaContainer.CaveSites;
        }

        public static List<CaveTypeModel> GetCaveTypes()
        {
            return kuchaContainer.CaveTypes;
        }

        public static string GetCaveDistrictStringByID(int id)
        {
            CaveDistrictModel c = kuchaContainer.CaveDistricts.Find(x => x.districtID == id);
            if (c == null) return String.Empty;
            return c.name;
        }

        public static string GetCaveRegionStringByID(int id)
        {
            CaveRegionModel c = kuchaContainer.CaveRegions.Find(x => x.regionID == id);
            if (c == null) return String.Empty;
            return c.englishName;
        }

        public static string GetCaveSiteStringByID(int id)
        {
            CaveSiteModel c = kuchaContainer.CaveSites.Find(x => x.siteID == id);
            if (c == null) return String.Empty;
            return c.name;
        }

        public static string GetCaveTypeStringByID(int id)
        {
            CaveTypeModel c = kuchaContainer.CaveTypes.Find(x => x.caveTypeID == id);
            if (c == null) return String.Empty;
            return c.nameEN;
        }

        public static string GetCaveTypeSketchByID(int id)
        {
            CaveTypeModel c = kuchaContainer.CaveTypes.Find(x => x.caveTypeID == id);
            if (c == null) return String.Empty;
            return c.sketchName;
        }

        public static void SaveCaveNotes(int caveID, string notes)
        {
            var index = Settings.SavedNotesSetting.FindIndex(c => c.ID == caveID && c.Type == NotesSaver.NOTES_TYPE.NOTE_TYPE_CAVE);
            if (index == -1)
            {
                if (!String.IsNullOrEmpty(notes))
                {
                    List<NotesSaver> savedNotes = Settings.SavedNotesSetting;
                    savedNotes.Add(new NotesSaver(NotesSaver.NOTES_TYPE.NOTE_TYPE_CAVE, caveID, notes));
                    Settings.SavedNotesSetting = savedNotes;
                }
            }
            else
            {
                NotesSaver currentNote = Settings.SavedNotesSetting[index];
                if (currentNote.Note != notes)
                {
                    currentNote.Note = notes;
                    List<NotesSaver> savedNotes = Settings.SavedNotesSetting;
                    savedNotes[index] = currentNote;
                    Settings.SavedNotesSetting = savedNotes;
                }
            }
        }

        //Painted Representations & Iconographies
        public static List<PaintedRepresentationModel> GetAllPaintedRepresentations()
        {
            return Connection.GetAllPaintedRepresentations();
        }

        public static List<PaintedRepresentationModel> GetPaintedRepresentationsByIconographies(List<IconographyModel> iconographies, bool exclusive)
        {
            return Connection.GetPaintedRepresentationsByFilter(iconographies, exclusive);
        }

        public static List<IconographyModel> GetIconographies()
        {
            return kuchaContainer.Iconographies;
        }

        public static void SavePaintedRepresentationNotes(int paintedRepresentationID, string notes)
        {
            var index = Settings.SavedNotesSetting.FindIndex(pr => pr.ID == paintedRepresentationID && pr.Type == NotesSaver.NOTES_TYPE.NOTE_TYPE_PAINTEDREPRESENTATION);
            if (index == -1)
            {
                if (!String.IsNullOrEmpty(notes))
                {
                    List<NotesSaver> savedNotes = Settings.SavedNotesSetting;
                    savedNotes.Add(new NotesSaver(NotesSaver.NOTES_TYPE.NOTE_TYPE_PAINTEDREPRESENTATION, paintedRepresentationID, notes));
                    Settings.SavedNotesSetting = savedNotes;
                }
            }
            else
            {
                NotesSaver currentNote = Settings.SavedNotesSetting[index];
                if (currentNote.Note != notes)
                {
                    currentNote.Note = notes;
                    List<NotesSaver> savedNotes = Settings.SavedNotesSetting;
                    savedNotes[index] = currentNote;
                    Settings.SavedNotesSetting = savedNotes;
                }
            }
        }

        //Image
        public static void SaveImageNotes(int imageID, string notes)
        {
            var index = Settings.SavedNotesSetting.FindIndex(i => i.ID == imageID && i.Type == NotesSaver.NOTES_TYPE.NOTE_TYPE_IMAGE);
            if (index == -1)
            {
                if (!String.IsNullOrEmpty(notes))
                {
                    List<NotesSaver> savedNotes = Settings.SavedNotesSetting;
                    savedNotes.Add(new NotesSaver(NotesSaver.NOTES_TYPE.NOTE_TYPE_IMAGE, imageID, notes));
                    Settings.SavedNotesSetting = savedNotes;
                }
            }
            else
            {
                NotesSaver currentNote = Settings.SavedNotesSetting[index];
                if (currentNote.Note != notes)
                {
                    currentNote.Note = notes;
                    List<NotesSaver> savedNotes = Settings.SavedNotesSetting;
                    savedNotes[index] = currentNote;
                    Settings.SavedNotesSetting = savedNotes;
                }
            }
        }

        //Global
        /// <summary>
        /// Only used during App Launch
        /// </summary>
        public async static void LoadPersistantData()
        {
            IFolder rootFolder = FileSystem.Current.LocalStorage;
            ExistenceCheckResult kuchaFolderExists = await rootFolder.CheckExistsAsync("Kucha");
            bool loadSuccess = false;
            if (kuchaFolderExists == ExistenceCheckResult.FolderExists)
            {
                IFolder kuchaFolder = await rootFolder.GetFolderAsync("Kucha");
                IFile kuchaContainerFile;
                if (kuchaFolder != null)
                {
                    ExistenceCheckResult kuchaFileExists = await kuchaFolder.CheckExistsAsync("KuchaContainer");
                    if (kuchaFileExists == ExistenceCheckResult.FileExists)
                    {
                        kuchaContainerFile = await kuchaFolder.GetFileAsync("KuchaContainer");
                        if (kuchaContainerFile != null)
                        {
                            string fileString = await kuchaContainerFile.ReadAllTextAsync();
                            kuchaContainer = JsonConvert.DeserializeObject<KuchaContainer>(fileString);
                            if (kuchaContainer != null)
                                loadSuccess = true;
                            else
                                kuchaContainer = new KuchaContainer();
                        }
                    }
                    else
                    {
                        await kuchaFolder.CreateFileAsync("KuchaContainer", CreationCollisionOption.ReplaceExisting);
                        kuchaContainer = new KuchaContainer();
                    }
                }
            }
            else
            {
                //Kucha Folder does not exist - Create Folder and empty file
                await rootFolder.CreateFolderAsync("Kucha", CreationCollisionOption.ReplaceExisting);
                IFolder kuchaFolder = await rootFolder.GetFolderAsync("Kucha");
                await kuchaFolder.CreateFileAsync("KuchaContainer", CreationCollisionOption.ReplaceExisting);
            }

            if (!loadSuccess)
                kuchaContainer = new KuchaContainer();
            Connection.LoadCachedSessionID();

            Device.BeginInvokeOnMainThread(() => ((App)App.Current).LoadingPersistantDataFinished());
        }

        public async static Task<bool> RefreshLocalData()
        {
            if (kuchaContainer == null)
                kuchaContainer = new KuchaContainer();
            List<CaveDistrictModel> caveDistrictModels = Connection.GetCaveDistrictModels();
            if (caveDistrictModels == null)
                return false;
            kuchaContainer.CaveDistricts = caveDistrictModels;

            List<CaveRegionModel> caveRegionModels = Connection.GetCaveRegionModels();
            if (caveRegionModels != null)
            {
                kuchaContainer.CaveRegions = caveRegionModels;
            }
            else return false;

            List<CaveSiteModel> caveSiteModels = Connection.GetCaveSiteModels();
            if (caveSiteModels != null)
            {
                kuchaContainer.CaveSites = caveSiteModels;
            }
            else return false;

            List<CaveTypeModel> caveTypeModels = Connection.GetCaveTypeModels();
            if (caveTypeModels != null)
            {
                kuchaContainer.CaveTypes = caveTypeModels;
                kuchaContainer.CaveTypeDictionary = new Dictionary<string, CaveTypeModel>
                {
                    { "Any", null }
                };
                foreach (CaveTypeModel c in caveTypeModels)
                {
                    kuchaContainer.CaveTypeDictionary.Add(c.nameEN, c);
                }
            }
            else return false;

            List<CaveModel> caveModels = Connection.GetAllCaves();
            if (caveModels != null)
            {
                kuchaContainer.Caves = caveModels;
            }
            else return false;

            List<IconographyModel> iconographieModels = Connection.GetIconographyModels();
            if (iconographieModels != null)
            {
                kuchaContainer.Iconographies = iconographieModels;
            }
            else return false;

            kuchaContainer.TimeStamp = DateTime.UtcNow;
            IFolder rootFolder = FileSystem.Current.LocalStorage;
            IFolder kuchaFolder = await rootFolder.CreateFolderAsync("Kucha", CreationCollisionOption.OpenIfExists);
            IFile kuchaContainerStorage = await kuchaFolder.CreateFileAsync("KuchaContainer", CreationCollisionOption.ReplaceExisting);
            await kuchaContainerStorage.WriteAllTextAsync(JsonConvert.SerializeObject(kuchaContainer));

            return true;
        }

        public static bool KuchaContainerIsValid()
        {
            if (kuchaContainer == null)
                return false;
            if (kuchaContainer.Caves.Count == 0)
                return false;
            if (kuchaContainer.CaveDistricts == new List<CaveDistrictModel>()
                || kuchaContainer.CaveRegions == new List<CaveRegionModel>()
                || kuchaContainer.CaveSites == new List<CaveSiteModel>()
                || kuchaContainer.CaveTypes == new List<CaveTypeModel>()
                || kuchaContainer.Caves == new List<CaveModel>())
                return false;
            if (kuchaContainer.Iconographies == new List<IconographyModel>())
                return false;
            return true;
        }

        public async static void RemoveAllData()
        {
            IFolder rootFolder = FileSystem.Current.LocalStorage;
            ExistenceCheckResult kuchaFolderExists = await rootFolder.CheckExistsAsync("Kucha");
            if (kuchaFolderExists == ExistenceCheckResult.FolderExists)
            {
                IFolder kuchaFolder = await rootFolder.GetFolderAsync("Kucha");
                await kuchaFolder.DeleteAsync();
                kuchaContainer = null;
                App.Current.MainPage = new UI.LoginPage();
            }
        }

        /// <summary>
        /// Returns a timestamp of the current used local Database
        /// </summary>
        /// <returns></returns>
        public static DateTime GetDataTimeStamp()
        {
            return kuchaContainer.TimeStamp;
        }
    }
}