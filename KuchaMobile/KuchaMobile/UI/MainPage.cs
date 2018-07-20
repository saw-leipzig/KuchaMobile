using Acr.UserDialogs;
using KuchaMobile.UI;
using System;
using Xamarin.Forms;

namespace KuchaMobile
{
    public class MainPage : MasterDetailPage
    {
        private MainMenuUI mainMenu;

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
            var item = e.SelectedItem as MasterPageItem;
            if (item != null)
            {
                if(item.TargetType.Name == "PaintedRepresentationSearchUI" && Internal.Connection.IsInOfflineMode())
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