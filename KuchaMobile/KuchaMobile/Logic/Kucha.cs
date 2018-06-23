using KuchaMobile.Internal;
using KuchaMobile.Logic.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using PCLStorage;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace KuchaMobile.Logic
{
    public class Kucha
    {
        static KuchaContainer kuchaContainer;

        public async static Task<bool> RefreshCaveData()
        {
            if (kuchaContainer == null)
                kuchaContainer = new KuchaContainer();
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
            IFolder rootFolder = FileSystem.Current.LocalStorage;
            IFolder kuchaFolder = await rootFolder.CreateFolderAsync("Kucha", CreationCollisionOption.OpenIfExists);
            IFile kuchaContainerStorage = await kuchaFolder.CreateFileAsync("KuchaContainer", CreationCollisionOption.ReplaceExisting);
            await kuchaContainerStorage.WriteAllTextAsync(JsonConvert.SerializeObject(kuchaContainer));

            //Settings.KuchaContainerSetting = kuchaContainer;
            return true;
        }
        public static CaveModel GetCaveByID(int id)
        {
            return kuchaContainer.caves.First(c => c.caveID == id);
        }
        public static DateTime GetDataTimeStamp()
        {
            return kuchaContainer.timeStamp;
        }

        public static void SaveCaveNotes(int caveID, string notes)
        {
            //Device memory
            List<NotesSaver> currentSavedNotes = Settings.SavedNotesSetting;
            var index = currentSavedNotes.FindIndex(id => id.ID == caveID && id.Type == NotesSaver.NOTES_TYPE.NOTE_TYPE_CAVE);
            if(index>-1) //Element found
            {
                currentSavedNotes[index].Note = notes;
            }
            else
            {
                currentSavedNotes.Add(new NotesSaver(NotesSaver.NOTES_TYPE.NOTE_TYPE_CAVE, caveID, notes));
            }
            Settings.SavedNotesSetting = currentSavedNotes;
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

        public static List<PaintedRepresentationModel> GetAllPaintedRepresentations()
        {
            List<PaintedRepresentationModel> resultList = Connection.GetAllPaintedRepresentations();
            return resultList;
        }

        public static List<PaintedRepresentationModel> GetPaintedRepresentationsByIconographies(List<IconographyModel> iconographies, bool exclusive)
        {
            List<PaintedRepresentationModel> resultList = Connection.GetPaintedRepresentationsByFilter(iconographies, exclusive);
            return resultList;
        }

        public async static void LoadPersistantData()
        {
            IFolder rootFolder = FileSystem.Current.LocalStorage;
            ExistenceCheckResult kuchaFolderExists = await rootFolder.CheckExistsAsync("Kucha");
            bool loadSuccess = false;
            if(kuchaFolderExists == ExistenceCheckResult.FolderExists)
            {
                IFolder kuchaFolder = await rootFolder.GetFolderAsync("Kucha");
                IFile kuchaContainerFile;
                if (kuchaFolder != null)
                {
                    ExistenceCheckResult kuchaFileExists = await kuchaFolder.CheckExistsAsync("KuchaContainer");
                    if(kuchaFileExists == ExistenceCheckResult.FileExists)
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

            Device.BeginInvokeOnMainThread(()=> 
            {
                ((App)App.Current).LoadingPersistantDataFinished();
            });           
        }

        public static bool CaveDataIsValid()
        {
            if (kuchaContainer == null)
                return false;
            if (kuchaContainer.caves.Count == 0)
                return false;
            if (kuchaContainer.caveDistricts == new List<CaveDistrictModel>() ||
                kuchaContainer.caveRegions == new List<CaveRegionModel>() ||
                kuchaContainer.caveSites == new List<CaveSiteModel>() ||
                kuchaContainer.caveTypes == new List<CaveTypeModel>() ||
                kuchaContainer.caves == new List<CaveModel>())
                return false;
            else return true;
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
    }
}
