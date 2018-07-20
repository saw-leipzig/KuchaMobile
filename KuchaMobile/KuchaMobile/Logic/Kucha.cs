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
        private static KuchaContainer kuchaContainer;

        //Caves
        public static CaveModel GetCaveByID(int id)
        {
            return kuchaContainer.caves.Find(c => c.caveID == id);
        }

        public static List<CaveModel> GetCavesByFilters(CaveTypeModel caveTypeModel, List<CaveDistrictModel> pickedDistricts, List<CaveRegionModel> pickedRegions, List<CaveSiteModel> pickedSites)
        {
            List<CaveModel> resultCaves = new List<CaveModel>(kuchaContainer.caves);   //Clone them to not access the full list
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

        public static string GetCaveDistrictStringByID(int id)
        {
            CaveDistrictModel c = kuchaContainer.caveDistricts.Find(x => x.districtID == id);
            if (c == null) return String.Empty;
            return c.name;
        }

        public static string GetCaveRegionStringByID(int id)
        {
            CaveRegionModel c = kuchaContainer.caveRegions.Find(x => x.regionID == id);
            if (c == null) return String.Empty;
            return c.englishName;
        }

        public static string GetCaveSiteStringByID(int id)
        {
            CaveSiteModel c = kuchaContainer.caveSites.Find(x => x.siteID == id);
            if (c == null) return String.Empty;
            return c.name;
        }

        public static string GetCaveTypeStringByID(int id)
        {
            CaveTypeModel c = kuchaContainer.caveTypes.Find(x => x.caveTypeID == id);
            if (c == null) return String.Empty;
            return c.nameEN;
        }

        public static string GetCaveTypeSketchByID(int id)
        {
            CaveTypeModel c = kuchaContainer.caveTypes.Find(x => x.caveTypeID == id);
            if (c == null) return String.Empty;
            return c.sketchName;
        }

        public static void SaveCaveNotes(int caveID, string notes)
        {
            //Device memory
            List<NotesSaver> currentSavedNotes = Settings.SavedNotesSetting;
            var savedNote = currentSavedNotes.Find(id => id.ID == caveID && id.Type == NotesSaver.NOTES_TYPE.NOTE_TYPE_CAVE);
            if (savedNote != null) //Element found
                savedNote.Note = notes;
            else
                currentSavedNotes.Add(new NotesSaver(NotesSaver.NOTES_TYPE.NOTE_TYPE_CAVE, caveID, notes));
            Settings.SavedNotesSetting = currentSavedNotes;
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
            return kuchaContainer.iconographies;
        }

        public static void SavePaintedRepresentationNotes(int paintedRepresentationID, string notes)
        {
            //We dont cache the Painted Representations for now, so only save on device memory
            List<NotesSaver> currentSavedNotes = Settings.SavedNotesSetting;
            var index = currentSavedNotes.FindIndex(id => id.ID == paintedRepresentationID && id.Type == NotesSaver.NOTES_TYPE.NOTE_TYPE_PAINTEDREPRESENTATION);
            if (index > -1) //Element found
            {
                currentSavedNotes[index].Note = notes;
            }
            else
            {
                currentSavedNotes.Add(new NotesSaver(NotesSaver.NOTES_TYPE.NOTE_TYPE_PAINTEDREPRESENTATION, paintedRepresentationID, notes));
            }
            Settings.SavedNotesSetting = currentSavedNotes;
        }

        //Global
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
            kuchaContainer.caveDistricts = caveDistrictModels;

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
                kuchaContainer.caveTypeDictionary = new Dictionary<string, CaveTypeModel>
                {
                    { "Any", null }
                };
                foreach (CaveTypeModel c in caveTypeModels)
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
            if (kuchaContainer.caves.Count == 0)
                return false;
            if (kuchaContainer.caveDistricts == new List<CaveDistrictModel>()
                || kuchaContainer.caveRegions == new List<CaveRegionModel>()
                || kuchaContainer.caveSites == new List<CaveSiteModel>()
                || kuchaContainer.caveTypes == new List<CaveTypeModel>()
                || kuchaContainer.caves == new List<CaveModel>())
                return false;
            if (kuchaContainer.iconographies == new List<IconographyModel>())
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

        public static DateTime GetDataTimeStamp()
        {
            return kuchaContainer.timeStamp;
        }
    }
}