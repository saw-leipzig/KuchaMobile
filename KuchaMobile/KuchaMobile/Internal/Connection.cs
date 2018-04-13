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
            if (!String.IsNullOrEmpty(sessionID))   //Todo: AND IS NOT EXPIRED
                return true;
            return false;
        }
        public static void LoadCachedSessionID()
        {
            sessionID = Settings.LocalTokenSetting;
        }


        public static List<caveDistrictModel> GetCaveDistrictModels()
        {
            if (String.IsNullOrEmpty(sessionID))
                return null;

            string data = "";
            HttpStatusCode result = CallAPI("json?districtID=all&sessionID=" + sessionID, ref data);
            if (result == HttpStatusCode.OK && !String.IsNullOrEmpty(data))
            {
                List<caveDistrictModel> models = JsonConvert.DeserializeObject<List<caveDistrictModel>>(data);
                return models;
            }
            else return null;
        }

        public static List<caveRegionModel> GetCaveRegionModels()
        {
            if (String.IsNullOrEmpty(sessionID))
                return null;

            string data = "";
            HttpStatusCode result = CallAPI("json?regionID=all&sessionID=" + sessionID, ref data);
            if (result == HttpStatusCode.OK && !String.IsNullOrEmpty(data))
            {
                List<caveRegionModel> models = JsonConvert.DeserializeObject<List<caveRegionModel>>(data);
                return models;
            }
            else return null;
        }

        public static List<caveSiteModel> GetCaveSiteModels()
        {
            if (String.IsNullOrEmpty(sessionID))
                return null;

            string data = "";
            HttpStatusCode result = CallAPI("json?siteID=all&sessionID=" + sessionID, ref data);
            if (result == HttpStatusCode.OK && !String.IsNullOrEmpty(data))
            {
                List<caveSiteModel> models = JsonConvert.DeserializeObject<List<caveSiteModel>>(data);
                return models;
            }
            else return null;
        }

        public static List<caveTypeModel> GetCaveTypeModels()
        {
            if (String.IsNullOrEmpty(sessionID))
                return null;

            string data = "";
            HttpStatusCode result = CallAPI("json?caveTypeID=all&sessionID=" + sessionID, ref data);
            if (result == HttpStatusCode.OK && !String.IsNullOrEmpty(data))
            {
                List<caveTypeModel> models = JsonConvert.DeserializeObject<List<caveTypeModel>>(data);
                return models;
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
