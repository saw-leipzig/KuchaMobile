using KuchaMobile.Internal;
using System.Collections.Generic;
using Xamarin.Forms;

namespace KuchaMobile.UI
{
    public class CaveSearchHistoryUI : ContentPage
    {
        private readonly CaveSearchUI parent;

        public CaveSearchHistoryUI(CaveSearchUI parent)
        {
            this.parent = parent;
            Title = "Search History";
            ToolbarItems.Add(new ToolbarItem("Clear History", null, Clear_List));

            StackLayout contentStack = new StackLayout
            {
                Padding = 0
            };
            ListView listView = new ListView
            {
                ItemTemplate = new DataTemplate(typeof(TextCell))
            };
            listView.ItemTemplate.SetBinding(TextCell.TextProperty, "SearchTimeString");
            listView.ItemTemplate.SetBinding(TextCell.DetailProperty, "FoundResultsString");
            listView.ItemsSource = Settings.CaveSearchHistorySetting;
            listView.ItemTapped += ListView_ItemTapped;

            contentStack.Children.Add(listView);
            Content = contentStack;
        }

        private void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            CaveFilter caveFilter = e.Item as CaveFilter;
            parent.pickedDistricts = caveFilter.pickedDistricts;
            parent.pickedRegions = caveFilter.pickedRegions;
            parent.pickedSites = caveFilter.pickedSites;

            var index = -1;
            if (caveFilter.caveTypeModel != null)
                index = parent.caveFilterPicker.Items.IndexOf(caveFilter.caveTypeModel.nameEN);
            if (index != -1)
            {
                parent.caveFilterPicker.SelectedIndex = index;
            }

            Navigation.PopAsync();
        }

        private async void Clear_List()
        {
            bool response = await DisplayAlert("Clear History?", "Do you really want to clear your search history?", "Yes", "No");
            if (response)
            {
                Settings.CaveSearchHistorySetting = new List<CaveFilter>();
                await Navigation.PopAsync();
            }
        }
    }
}