using Acr.UserDialogs;
using KuchaMobile.UI;
using System;
using Xamarin.Forms;

namespace KuchaMobile
{
    public class MainPage : MasterDetailPage
    {
        /// <summary>
        /// MasterDetailPage is the Xamarin concept of a main menu (master) and actual pages (Detail)
        /// </summary>
        private readonly MainMenuUI mainMenu;

        public MainPage()
        {
            mainMenu = new MainMenuUI();
            Master = mainMenu;
            Detail = new NavigationPage(new CaveSearchUI());
            MasterBehavior = MasterBehavior.Popover;

            mainMenu.ListView.ItemSelected += OnItemSelected;
        }

        private void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem is MasterPageItem item)
            {
                if (item.TargetType.Name == "PaintedRepresentationSearchUI" && Internal.Connection.IsInOfflineMode())
                {
                    UserDialogs.Instance.Toast("Not available in offline mode!");
                    return;
                }
                Detail = new NavigationPage((Page)Activator.CreateInstance(item.TargetType));
                mainMenu.ListView.SelectedItem = null;
                IsPresented = false;
            }
        }
    }
}