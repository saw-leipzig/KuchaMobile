using KuchaMobile.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace KuchaMobile
{
    public class MainPage : MasterDetailPage
    {
        MainMenuUI mainMenu;

        public MainPage()
        {
            mainMenu = new MainMenuUI();
            Master = mainMenu;
            Detail = new NavigationPage(new CaveSearchUI());
            MasterBehavior = MasterBehavior.Popover;

            mainMenu.ListView.ItemSelected += OnItemSelected;
        }

        void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as MasterPageItem;
            if (item != null)
            {
                Detail = new NavigationPage((Page)Activator.CreateInstance(item.TargetType));
                mainMenu.ListView.SelectedItem = null;
                IsPresented = false;
            }
        }
    }
}

