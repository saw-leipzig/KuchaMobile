using KuchaMobile.Logic;
using KuchaMobile.Logic.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KuchaMobile.Internal
{
    static class Connection
    {
        private static string backendURL = "https://kuchatest.saw-leipzig.de/";
        private static HttpClient client = new HttpClient();
        private static string sessionID = String.Empty;

        public static bool Login(string username, string password)
        {
            string hashedPW = Helper.GetMD5Hash(password);

            string data = "";
            HttpStatusCode result = CallAPI("json?login=" + username + "&pw=" + hashedPW, ref data);
            if (result == HttpStatusCode.OK && !String.IsNullOrEmpty(data))
            {
                sessionID = data;
                Settings.LocalTokenSetting = data;
                return true;
            }
            else return false;
        }
        public static bool HasLegitSessionID()
        {
            if (!String.IsNullOrEmpty(sessionID))
            {
                string data = "";
                HttpStatusCode result = CallAPI("json?checkSession&sessionID=" + sessionID, ref data);
                if (result == HttpStatusCode.NoContent) //Should be like this!
                    return true;
                else return false;
            }

            return false;
        }
        public static void LoadCachedSessionID()
        {
            sessionID = Settings.LocalTokenSetting;
        }

        public static List<PaintedRepresentationModel> GetPaintedRepresentationsByFilter(List<IconographyModel> iconographies, bool exclusive)
        {
            if (String.IsNullOrEmpty(sessionID))
                return null;
            string data = "";

            HttpStatusCode result;
            List<int> allIconographiesIDList = new List<int>();
            foreach (IconographyModel i in iconographies)
            {
                allIconographiesIDList.Add(i.iconographyID);
            }
            if (exclusive)
            {
                string queryString = "json?exclusivePaintedRepFromIconographyID=" + String.Join(",", allIconographiesIDList) + "&sessionID=" + sessionID;
                result = CallAPI(queryString, ref data);
            }
            else
            {
                string queryString = "json?paintedRepFromIconographyID=" + String.Join(",", allIconographiesIDList) + "&sessionID=" + sessionID;
                result = CallAPI(queryString, ref data);
            }
            if (result == HttpStatusCode.OK && !String.IsNullOrEmpty(data))
            {
                List<PaintedRepresentationModel> models = JsonConvert.DeserializeObject<List<PaintedRepresentationModel>>(data);
                return models;
            }
            else return null;
        }

        public static List<CaveModel> GetAllCaves()
        {
            if (String.IsNullOrEmpty(sessionID))
                return null;

            string data = "";
            HttpStatusCode result = CallAPI("json?caveID=all&sessionID=" + sessionID, ref data);
            if (result == HttpStatusCode.OK && !String.IsNullOrEmpty(data))
            {
                List<CaveModel> models = JsonConvert.DeserializeObject<List<CaveModel>>(data);
                return models;
            }
            else return null;
        }

        public static List<CaveDistrictModel> GetCaveDistrictModels()
        {
            if (String.IsNullOrEmpty(sessionID))
                return null;

            string data = "";
            HttpStatusCode result = CallAPI("json?districtID=all&sessionID=" + sessionID, ref data);
            if (result == HttpStatusCode.OK && !String.IsNullOrEmpty(data))
            {
                List<CaveDistrictModel> models = JsonConvert.DeserializeObject<List<CaveDistrictModel>>(data);
                return models;
            }
            else return null;
        }

        public static List<CaveRegionModel> GetCaveRegionModels()
        {
            if (String.IsNullOrEmpty(sessionID))
                return null;

            string data = "";
            HttpStatusCode result = CallAPI("json?regionID=all&sessionID=" + sessionID, ref data);
            if (result == HttpStatusCode.OK && !String.IsNullOrEmpty(data))
            {
                List<CaveRegionModel> models = JsonConvert.DeserializeObject<List<CaveRegionModel>>(data);
                return models;
            }
            else return null;
        }

        public static List<CaveSiteModel> GetCaveSiteModels()
        {
            if (String.IsNullOrEmpty(sessionID))
                return null;

            string data = "";
            HttpStatusCode result = CallAPI("json?siteID=all&sessionID=" + sessionID, ref data);
            if (result == HttpStatusCode.OK && !String.IsNullOrEmpty(data))
            {
                List<CaveSiteModel> models = JsonConvert.DeserializeObject<List<CaveSiteModel>>(data);
                return models;
            }
            else return null;
        }

        public static List<CaveTypeModel> GetCaveTypeModels()
        {
            if (String.IsNullOrEmpty(sessionID))
                return null;

            string data = "";
            HttpStatusCode result = CallAPI("json?caveTypeID=all&sessionID=" + sessionID, ref data);
            if (result == HttpStatusCode.OK && !String.IsNullOrEmpty(data))
            {
                List<CaveTypeModel> models = JsonConvert.DeserializeObject<List<CaveTypeModel>>(data);
                return models;
            }
            else return null;
        }

        public static List<IconographyModel> GetIconographyModels()
        {
            if (String.IsNullOrEmpty(sessionID))
                return null;

            string data = "";
            HttpStatusCode result = CallAPI("json?iconographyID=used&sessionID=" + sessionID, ref data);
            if (result == HttpStatusCode.OK && !String.IsNullOrEmpty(data))
            {
                List<IconographyModel> iconographies = JsonConvert.DeserializeObject<List<IconographyModel>>(data);
                return iconographies;
            }
            else return null;
        }

        public static HttpStatusCode CallAPI(string command, ref string data, int timeOut = 10, bool exactURL = false)
        {
            //Get URI:
            string finalURL = command;
            if (!exactURL)
                finalURL = backendURL + finalURL;
            Uri requestUri = new Uri(finalURL);

            //Get request:
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, requestUri);

            //Get response:
            HttpStatusCode result = CallApi(request, out HttpResponseMessage response, timeOut);
            if (response != null)
                System.Diagnostics.Debug.WriteLine($"API GET request: {response.StatusCode}; Query: {command}");

                //Analyze response content:
                if (response != null && response.Content != null)
                data = response.Content.ReadAsStringAsync().Result;

            return result;
        }

        private static HttpStatusCode CallApi(HttpRequestMessage request, out HttpResponseMessage response, int timeOut)
        {
            //Prepare timeout functionality:
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            cancellationTokenSource.CancelAfter(timeOut * 1000);

            //Get response:
            response = null;
            try
            {
                if (timeOut == 0)
                    response = client.SendAsync(request).Result;
                else
                    response = client.SendAsync(request, cancellationTokenSource.Token).Result;
            }
            catch (Exception e)
            {
                if (e.InnerException != null && e.InnerException is TaskCanceledException)  // Timeout
                {
                    System.Diagnostics.Debug.WriteLine($"API request: Timeout; Query: {request.RequestUri}");
                    return HttpStatusCode.RequestTimeout;
                }
                else                                                                        //HTTP error not generated by backend or other error
                {
                    string innerException = String.Empty;
                    if (e.InnerException != null)
                        innerException = e.InnerException.Message;
                    System.Diagnostics.Debug.WriteLine($"API request: Gone; Query: {request.RequestUri}; Exception: {e.Message}; Inner: {innerException}");
                    return HttpStatusCode.Gone;
                }
            }

            //Return:
            return response.StatusCode;
        }

    }
}
