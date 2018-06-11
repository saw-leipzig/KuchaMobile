using KuchaMobile.Internal;
using KuchaMobile.Logic;
using KuchaMobile.Logic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace KuchaMobile.UI
{
    public class CaveSearchUI : ContentPage
    {
        Label districtsFilterLabel;
        Label regionsFilterLabel;
        Label sitesFilterLabel;
        public Picker caveFilterPicker;
        public List<CaveDistrictModel> pickedDistricts;
        public List<CaveRegionModel> pickedRegions;
        public List<CaveSiteModel> pickedSites;

        Dictionary<string, CaveTypeModel> caveTypeDictionary;

        public CaveSearchUI()
        {
            Title = "Cave Search";
            ToolbarItems.Add(new ToolbarItem("History", null, HistoryButton_Clicked));

            pickedDistricts = new List<CaveDistrictModel>();
            pickedRegions = new List<CaveRegionModel>();
            pickedSites = new List<CaveSiteModel>();

            StackLayout contentStackLayout = new StackLayout();
            contentStackLayout.Padding = new Thickness(72, 16, 16, 16);
            contentStackLayout.HorizontalOptions = LayoutOptions.FillAndExpand;
            contentStackLayout.VerticalOptions = LayoutOptions.FillAndExpand;

            Label caveFilterHeadlineLabel = new Label();
            caveFilterHeadlineLabel.FontSize = 20;
            caveFilterHeadlineLabel.Text = "Cave Filter";
            contentStackLayout.Children.Add(caveFilterHeadlineLabel);

            caveTypeDictionary = Kucha.GetCaveTypeDictionary();
            caveFilterPicker = new Picker();
            foreach(string name in caveTypeDictionary.Keys)
            {
                caveFilterPicker.Items.Add(name);
            }
 
            caveFilterPicker.SelectedIndex = 0;

            contentStackLayout.Children.Add(caveFilterPicker);

            Label locationFilterHeadlineLabel = new Label();
            locationFilterHeadlineLabel.FontSize = 20;
            locationFilterHeadlineLabel.Text = "Location Filter";
            contentStackLayout.Children.Add(locationFilterHeadlineLabel);

            districtsFilterLabel = new Label();
            districtsFilterLabel.FontSize = 12;
            districtsFilterLabel.Text = "Selektierte Districts: Keine";
            contentStackLayout.Children.Add(districtsFilterLabel);

            Button districtsFilterButton = new Button();
            districtsFilterButton.Text = "Districts auswählen";
            districtsFilterButton.Clicked += DistrictsFilterButton_Clicked;
            contentStackLayout.Children.Add(districtsFilterButton);

            regionsFilterLabel = new Label();
            regionsFilterLabel.FontSize = 12;
            regionsFilterLabel.Text = "Selektierte Regions: Keine";
            contentStackLayout.Children.Add(regionsFilterLabel);

            Button regionsFilterButton = new Button();
            regionsFilterButton.Text = "Regions auswählen";
            regionsFilterButton.Clicked += RegionsFilterButton_Clicked;
            contentStackLayout.Children.Add(regionsFilterButton);

            sitesFilterLabel = new Label();
            sitesFilterLabel.FontSize = 12;
            sitesFilterLabel.Text = "Selektierte Sites: Keine";
            contentStackLayout.Children.Add(sitesFilterLabel);

            Button sitesFilterButton = new Button();
            sitesFilterButton.Text = "Sites auswählen";
            sitesFilterButton.Clicked += SitesFilterButton_Clicked;
            contentStackLayout.Children.Add(sitesFilterButton);

            Button searchButton = new Button();
            searchButton.Text = "Suchen";
            searchButton.Clicked += SearchButton_Clicked;
            contentStackLayout.Children.Add(searchButton);

            Content = contentStackLayout;
        }

        private void SearchButton_Clicked(object sender, EventArgs e)
        {
            string selectedCaveTypeName = caveFilterPicker.Items[caveFilterPicker.SelectedIndex];
            CaveTypeModel caveTypeModel = caveTypeDictionary[selectedCaveTypeName];

            List<CaveModel> caves = Kucha.GetCavesByFilters(caveTypeModel, pickedDistricts, pickedRegions, pickedSites);
            List<CaveFilter> searchHistory = Settings.CaveSearchHistorySetting;
            if (searchHistory == null)
                searchHistory = new List<CaveFilter>();
            CaveFilter caveFilter = new CaveFilter();
            caveFilter.caveTypeModel = caveTypeModel;
            caveFilter.pickedDistricts = pickedDistricts;
            caveFilter.pickedRegions = pickedRegions;
            caveFilter.pickedSites = pickedSites;
            caveFilter.FoundResultsString = "Ergebnisse: "+caves.Count;
            caveFilter.SearchTimeString = "Am "+DateTime.UtcNow.ToString();
            searchHistory.Add(caveFilter);
            var newList = searchHistory.OrderByDescending(x => x.SearchTimeString).ToList();
            Settings.CaveSearchHistorySetting = newList;

            Navigation.PushAsync(new CaveSearchResultUI(caves), true);
        }

        private void SitesFilterButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new CaveFilterUI(CaveFilterUI.CAVE_FILTER_TYPE.SITE, this), true);
        }

        private void RegionsFilterButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new CaveFilterUI(CaveFilterUI.CAVE_FILTER_TYPE.REGION, this), true);
        }

        private void DistrictsFilterButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new CaveFilterUI(CaveFilterUI.CAVE_FILTER_TYPE.DISTRICT, this), true);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if(pickedDistricts.Count == 0)
            {
                districtsFilterLabel.Text = "Selektierte Districts: Keine";
            }
            else
            {
                string labelString = "Selektierte Districts: ";
                foreach(CaveDistrictModel district in pickedDistricts)
                {
                    labelString += district.name + ", ";
                }                
                districtsFilterLabel.Text = labelString.Remove(labelString.Length - 2);
            }

            if (pickedRegions.Count == 0)
            {
                regionsFilterLabel.Text = "Selektierte Regions: Keine";
            }
            else
            {
                string labelString = "Selektierte Regions: ";
                foreach (CaveRegionModel region in pickedRegions)
                {
                    labelString += region.englishName + ", ";
                }               
                regionsFilterLabel.Text = labelString.Remove(labelString.Length - 2);
            }

            if (pickedSites.Count == 0)
            {
                sitesFilterLabel.Text = "Selektierte Sites: Keine";
            }
            else
            {
                string labelString = "Selektierte Sites: ";
                foreach (CaveSiteModel site in pickedSites)
                {
                    labelString += site.name + ", ";
                }              
                sitesFilterLabel.Text = labelString.Remove(labelString.Length - 2);
            }

        }

        private void HistoryButton_Clicked()
        {
            Navigation.PushAsync(new CaveSearchHistoryUI(this));
        }
    }
}
