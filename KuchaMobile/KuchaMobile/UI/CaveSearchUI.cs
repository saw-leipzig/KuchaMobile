using KuchaMobile.Internal;
using KuchaMobile.Logic;
using KuchaMobile.Logic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace KuchaMobile.UI
{
    public class CaveSearchUI : ContentPage
    {
        private Label districtsFilterLabel;
        private Label regionsFilterLabel;
        private Label sitesFilterLabel;
        public Picker caveFilterPicker;
        public List<CaveDistrictModel> pickedDistricts;
        public List<CaveRegionModel> pickedRegions;
        public List<CaveSiteModel> pickedSites;

        private Dictionary<string, CaveTypeModel> caveTypeDictionary;

        public CaveSearchUI()
        {
            Title = "Cave Search";
            ToolbarItems.Add(new ToolbarItem("History", null, HistoryButton_Clicked));

            pickedDistricts = new List<CaveDistrictModel>();
            pickedRegions = new List<CaveRegionModel>();
            pickedSites = new List<CaveSiteModel>();

            StackLayout contentStackLayout = new StackLayout();
            contentStackLayout.Padding = new Thickness(16, 16, 16, 16);
            contentStackLayout.HorizontalOptions = LayoutOptions.FillAndExpand;
            contentStackLayout.VerticalOptions = LayoutOptions.FillAndExpand;

            Frame caveTypeFrame = new Frame();
            caveTypeFrame.BackgroundColor = Color.White;
            caveTypeFrame.HasShadow = true;

            StackLayout caveTypeStack = new StackLayout();

            Label caveFilterHeadlineLabel = new Label();
            caveFilterHeadlineLabel.FontSize = 20;
            caveFilterHeadlineLabel.Text = "Cave Type";
            caveFilterHeadlineLabel.TextColor = Color.Black;
            caveTypeStack.Children.Add(caveFilterHeadlineLabel);

            caveTypeDictionary = Kucha.GetCaveTypeDictionary();
            caveFilterPicker = new Picker();
            foreach (string name in caveTypeDictionary.Keys)
            {
                caveFilterPicker.Items.Add(name);
            }

            caveFilterPicker.SelectedIndex = 0;

            caveTypeStack.Children.Add(caveFilterPicker);
            caveTypeFrame.Content = caveTypeStack;
            contentStackLayout.Children.Add(caveTypeFrame);

            Frame caveLocationFrame = new Frame();
            caveLocationFrame.HasShadow = true;
            caveLocationFrame.BackgroundColor = Color.White;

            StackLayout caveLocationStack = new StackLayout();

            Label locationFilterHeadlineLabel = new Label();
            locationFilterHeadlineLabel.FontSize = 20;
            locationFilterHeadlineLabel.TextColor = Color.Black;
            locationFilterHeadlineLabel.Text = "Location Filter";
            caveLocationStack.Children.Add(locationFilterHeadlineLabel);

            districtsFilterLabel = new Label();
            districtsFilterLabel.FontSize = 12;
            districtsFilterLabel.Text = "Selected Districts: None";
            caveLocationStack.Children.Add(districtsFilterLabel);

            Button districtsFilterButton = new Button();
            districtsFilterButton.Text = "Select Districts";
            districtsFilterButton.Clicked += DistrictsFilterButton_Clicked;
            caveLocationStack.Children.Add(districtsFilterButton);

            regionsFilterLabel = new Label();
            regionsFilterLabel.FontSize = 12;
            regionsFilterLabel.Text = "Selected Regions: None";
            caveLocationStack.Children.Add(regionsFilterLabel);

            Button regionsFilterButton = new Button();
            regionsFilterButton.Text = "Select Regions";
            regionsFilterButton.Clicked += RegionsFilterButton_Clicked;
            caveLocationStack.Children.Add(regionsFilterButton);

            sitesFilterLabel = new Label();
            sitesFilterLabel.FontSize = 12;
            sitesFilterLabel.Text = "Selected Sites: None";
            caveLocationStack.Children.Add(sitesFilterLabel);

            Button sitesFilterButton = new Button();
            sitesFilterButton.Text = "Select Sites";
            sitesFilterButton.Clicked += SitesFilterButton_Clicked;
            caveLocationStack.Children.Add(sitesFilterButton);

            caveLocationFrame.Content = caveLocationStack;
            contentStackLayout.Children.Add(caveLocationFrame);

            Frame buttonFrame = new Frame();
            buttonFrame.HasShadow = true;
            buttonFrame.BackgroundColor = Color.White;

            StackLayout buttonStack = new StackLayout();
            Label buttonHeadlineLabel = new Label();
            buttonHeadlineLabel.FontSize = 20;
            buttonHeadlineLabel.Text = "Start searching";
            buttonHeadlineLabel.TextColor = Color.Black;
            buttonStack.Children.Add(buttonHeadlineLabel);

            Button searchButton = new Button();
            searchButton.BackgroundColor = Color.FromHex("2196f3");
            searchButton.TextColor = Color.White;
            searchButton.Text = "Search";
            searchButton.Clicked += SearchButton_Clicked;
            buttonStack.Children.Add(searchButton);
            buttonFrame.Content = buttonStack;

            contentStackLayout.Children.Add(buttonFrame);

            ScrollView scrollView = new ScrollView();
            scrollView.Content = contentStackLayout;

            Content = scrollView;
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
            caveFilter.FoundResultsString = "Results: " + caves.Count;
            caveFilter.SearchTimeString = "At " + DateTime.UtcNow.ToString();
            searchHistory.Add(caveFilter);
            var newList = searchHistory.OrderByDescending(x => x.SearchTimeString).ToList();
            Settings.CaveSearchHistorySetting = newList;

            Navigation.PushAsync(new CaveSearchResultUI(caves), true);
        }

        private void SitesFilterButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new CaveFilterUI(CaveFilterUI.CAVE_FILTER_TYPE.SITE, this), true);
        }

        private void RegionsFilterButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new CaveFilterUI(CaveFilterUI.CAVE_FILTER_TYPE.REGION, this), true);
        }

        private void DistrictsFilterButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new CaveFilterUI(CaveFilterUI.CAVE_FILTER_TYPE.DISTRICT, this), true);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (pickedDistricts.Count == 0)
            {
                districtsFilterLabel.Text = "Selected Districts: None";
            }
            else
            {
                string labelString = "Selected Districts: ";
                foreach (CaveDistrictModel district in pickedDistricts)
                {
                    labelString += district.name + ", ";
                }
                districtsFilterLabel.Text = labelString.Remove(labelString.Length - 2);
            }

            if (pickedRegions.Count == 0)
            {
                regionsFilterLabel.Text = "Selected Regions: None";
            }
            else
            {
                string labelString = "Selected Regions: ";
                foreach (CaveRegionModel region in pickedRegions)
                {
                    labelString += region.englishName + ", ";
                }
                regionsFilterLabel.Text = labelString.Remove(labelString.Length - 2);
            }

            if (pickedSites.Count == 0)
            {
                sitesFilterLabel.Text = "Selected Sites: None";
            }
            else
            {
                string labelString = "Selected Sites: ";
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