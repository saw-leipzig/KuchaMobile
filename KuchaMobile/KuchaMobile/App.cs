using KuchaMobile.Logic;
using KuchaMobile.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace KuchaMobile
{
    public class App : Application
    {
        public App()
        {
        }

        protected override void OnStart()
        {
            Task.Run(() =>
            {
                Kucha.LoadPersistantData();
            });

            MainPage = new LoadingScreenUI();
        }
        
        public void LoadingPersistantDataFinished()
        {
            if (!Kucha.CaveDataIsValid() || !Internal.Connection.HasLegitSessionID())
                MainPage = new LoginPage();
            else
                MainPage = new MainPage();
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
