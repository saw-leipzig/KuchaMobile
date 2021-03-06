﻿using KuchaMobile.Logic;
using KuchaMobile.Logic.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace KuchaMobile.Internal
{
    internal static class Connection
    {
        /// <summary>
        /// This class is responsible for connection handling
        /// </summary>
        private const string backendURL = "https://kuchatest.saw-leipzig.de/";
        private static readonly HttpClient client = new HttpClient();
        private static string sessionID = String.Empty;
        private static bool isInOfflineMode;

        public enum LOGIN_STATUS
        {
            SUCCESS,
            FAILURE,
            OFFLINE
        }

        public static LOGIN_STATUS Login(string username, string password)
        {
            string hashedPW = Helper.GetMD5Hash(password);

            string data = "";
            HttpStatusCode result = CallBackend("json?login=" + username + "&pw=" + hashedPW, ref data);
            if (result == HttpStatusCode.OK && !String.IsNullOrEmpty(data)) //Server returns OK 200 even if login wrong
            {
                sessionID = data;
                Settings.LocalTokenSetting = data;
                isInOfflineMode = false;
                return LOGIN_STATUS.SUCCESS;
            }
            else if (result == HttpStatusCode.RequestTimeout || result == HttpStatusCode.Gone)
            {
                isInOfflineMode = true;
                return LOGIN_STATUS.OFFLINE;
            }
            else
            {
                return LOGIN_STATUS.FAILURE;
            }
        }

        public static bool IsInOfflineMode()
        {
            return isInOfflineMode;
        }

        public static bool HasLegitSessionID()
        {
            if (!String.IsNullOrEmpty(sessionID))
            {
                string data = "";
                HttpStatusCode result = CallBackend("json?checkSession&sessionID=" + sessionID, ref data);
                return result == HttpStatusCode.NoContent;
            }

            return false;
        }

        public static void LoadCachedSessionID()
        {
            sessionID = Settings.LocalTokenSetting;
        }

        public static string GetPaintedRepresentationImageURL(int id, int sizeInPx = 0)
        {
            if (!String.IsNullOrEmpty(sessionID))
            {
                if (sizeInPx == 0)
                {
                    return backendURL + "resource?imageID=" + id + "&sessionID=" + sessionID;
                }
                else
                {
                    return backendURL + "resource?imageID=" + id + "&thumb=" + sizeInPx + "&sessionID=" + sessionID;
                }
            }
            return String.Empty;
        }

        public static string GetCaveSketchURL(string caveSketch)
        {
            if (!String.IsNullOrEmpty(sessionID))
            {
                string returnString = backendURL + "resource?cavesketch=" + caveSketch + "&sessionID=" + sessionID;
                return returnString;
            }
            return String.Empty;
        }

        public static string GetCaveBackgroundImageURL(int caveTypeID)
        {
            if (!String.IsNullOrEmpty(sessionID))
            {
                string caveSketch = Kucha.GetCaveTypeSketchByID(caveTypeID);
                string returnString = backendURL + "resource?background=" + caveSketch + "&sessionID=" + sessionID;
                return returnString;
            }
            return String.Empty;
        }

        public static List<PaintedRepresentationModel> GetAllPaintedRepresentations()
        {
            if (String.IsNullOrEmpty(sessionID))
                return null;
            string data = "";
            HttpStatusCode result;
            string queryString = "json?paintedRepID=all&sessionID=" + sessionID;
            result = CallBackend(queryString, ref data);

            if (result == HttpStatusCode.OK && !String.IsNullOrEmpty(data))
            {
                List<PaintedRepresentationModel> models = JsonConvert.DeserializeObject<List<PaintedRepresentationModel>>(data);
                return models;
            }
            else
            {
                return null;
            }
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
                result = CallBackend(queryString, ref data);
            }
            else
            {
                string queryString = "json?paintedRepFromIconographyID=" + String.Join(",", allIconographiesIDList) + "&sessionID=" + sessionID;
                result = CallBackend(queryString, ref data);
            }
            if (result == HttpStatusCode.OK && !String.IsNullOrEmpty(data))
            {
                List<PaintedRepresentationModel> models = JsonConvert.DeserializeObject<List<PaintedRepresentationModel>>(data);
                return models;
            }
            else
            {
                return null;
            }
        }

        public static List<CaveModel> GetAllCaves()
        {
            if (String.IsNullOrEmpty(sessionID))
                return null;

            string data = "";
            HttpStatusCode result = CallBackend("json?caveID=all&sessionID=" + sessionID, ref data);
            if (result == HttpStatusCode.OK && !String.IsNullOrEmpty(data))
            {
                List<CaveModel> models = JsonConvert.DeserializeObject<List<CaveModel>>(data);
                return models;
            }
            else
            {
                return null;
            }
        }

        public static List<CaveDistrictModel> GetCaveDistrictModels()
        {
            if (String.IsNullOrEmpty(sessionID))
                return null;

            string data = "";
            HttpStatusCode result = CallBackend("json?districtID=all&sessionID=" + sessionID, ref data);
            if (result == HttpStatusCode.OK && !String.IsNullOrEmpty(data))
            {
                List<CaveDistrictModel> models = JsonConvert.DeserializeObject<List<CaveDistrictModel>>(data);
                return models;
            }
            else
            {
                return null;
            }
        }

        public static List<CaveRegionModel> GetCaveRegionModels()
        {
            if (String.IsNullOrEmpty(sessionID))
                return null;

            string data = "";
            HttpStatusCode result = CallBackend("json?regionID=all&sessionID=" + sessionID, ref data);
            if (result == HttpStatusCode.OK && !String.IsNullOrEmpty(data))
            {
                List<CaveRegionModel> models = JsonConvert.DeserializeObject<List<CaveRegionModel>>(data);
                return models;
            }
            else
            {
                return null;
            }
        }

        public static List<CaveSiteModel> GetCaveSiteModels()
        {
            if (String.IsNullOrEmpty(sessionID))
                return null;

            string data = "";
            HttpStatusCode result = CallBackend("json?siteID=all&sessionID=" + sessionID, ref data);
            if (result == HttpStatusCode.OK && !String.IsNullOrEmpty(data))
            {
                List<CaveSiteModel> models = JsonConvert.DeserializeObject<List<CaveSiteModel>>(data);
                return models;
            }
            else
            {
                return null;
            }
        }

        public static List<CaveTypeModel> GetCaveTypeModels()
        {
            if (String.IsNullOrEmpty(sessionID))
                return null;

            string data = "";
            HttpStatusCode result = CallBackend("json?caveTypeID=all&sessionID=" + sessionID, ref data);
            if (result == HttpStatusCode.OK && !String.IsNullOrEmpty(data))
            {
                List<CaveTypeModel> models = JsonConvert.DeserializeObject<List<CaveTypeModel>>(data);
                return models;
            }
            else
            {
                return null;
            }
        }

        public static List<IconographyModel> GetIconographyModels()
        {
            if (String.IsNullOrEmpty(sessionID))
                return null;

            string data = "";
            HttpStatusCode result = CallBackend("json?iconographyID=used&sessionID=" + sessionID, ref data);
            if (result == HttpStatusCode.OK && !String.IsNullOrEmpty(data))
            {
                List<IconographyModel> iconographies = JsonConvert.DeserializeObject<List<IconographyModel>>(data);
                return iconographies;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// This method should be called when adding any server requests
        /// </summary>
        /// <param name="command">The URL endpoint</param>
        /// <param name="data">Will include data from server after the request</param>
        /// <param name="timeOut">Specify a shorter/longer timeOut</param>
        /// <param name="exactURL">Can be used to overwrite the backend url</param>
        /// <returns></returns>
        private static HttpStatusCode CallBackend(string command, ref string data, int timeOut = 10, bool exactURL = false)
        {
            //Get URI:
            string finalURL = command;
            if (!exactURL)
                finalURL = backendURL + finalURL;
            Uri requestUri = new Uri(finalURL);

            //Get request:
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, requestUri);

            //Get response:
            HttpStatusCode result = GetHttpAnswer(request, out HttpResponseMessage response, timeOut);
            if (response != null)
                System.Diagnostics.Debug.WriteLine($"API GET request: {response.StatusCode}; Query: {command}");

            //Analyze response content:
            if (response?.Content != null)
                data = System.Text.Encoding.UTF8.GetString(response.Content.ReadAsByteArrayAsync().Result);

            return result;
        }

        /// <summary>
        /// Should only ever be called by CallBackend
        /// </summary>
        /// <param name="request"></param>
        /// <param name="response"></param>
        /// <param name="timeOut"></param>
        /// <returns></returns>
        private static HttpStatusCode GetHttpAnswer(HttpRequestMessage request, out HttpResponseMessage response, int timeOut)
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