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
        private readonly Label districtsFilterLabel;
        private readonly Label regionsFilterLabel;
        private readonly Label sitesFilterLabel;
        public Picker caveFilterPicker;
        public List<CaveDistrictModel> pickedDistricts;
        public List<CaveRegionModel> pickedRegions;
        public List<CaveSiteModel> pickedSites;

        private readonly Dictionary<string, CaveTypeModel> caveTypeDictionary;

        public CaveSearchUI()
        {
            Title = "Cave Search";
            ToolbarItems.Add(new ToolbarItem("History", null, HistoryButton_Clicked));

            pickedDistricts = new List<CaveDistrictModel>();
            pickedRegions = new List<CaveRegionModel>();
            pickedSites = new List<CaveSiteModel>();

            StackLayout contentStackLayout = new StackLayout
            {
                Padding = new Thickness(16, 16, 16, 16),
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand
            };

            Frame caveTypeFrame = new Frame
            {
                BackgroundColor = Color.White,
                HasShadow = true
            };

            StackLayout caveTypeStack = new StackLayout();

            Label caveFilterHeadlineLabel = new Label
            {
                FontSize = 20,
                Text = "Cave Type",
                TextColor = Color.Black
            };
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

            Frame caveLocationFrame = new Frame
            {
                HasShadow = true,
                BackgroundColor = Color.White
            };

            StackLayout caveLocationStack = new StackLayout();

            Label locationFilterHeadlineLabel = new Label
            {
                FontSize = 20,
                TextColor = Color.Black,
                Text = "Location Filter"
            };
            caveLocationStack.Children.Add(locationFilterHeadlineLabel);

            districtsFilterLabel = new Label
            {
                FontSize = 12,
                Text = "Selected Districts: None"
            };
            caveLocationStack.Children.Add(districtsFilterLabel);

            Button districtsFilterButton = new Button
            {
                Text = "Select Districts"
            };
            districtsFilterButton.Clicked += DistrictsFilterButton_Clicked;
            caveLocationStack.Children.Add(districtsFilterButton);

            regionsFilterLabel = new Label
            {
                FontSize = 12,
                Text = "Selected Regions: None"
            };
            caveLocationStack.Children.Add(regionsFilterLabel);

            Button regionsFilterButton = new Button
            {
                Text = "Select Regions"
            };
            regionsFilterButton.Clicked += RegionsFilterButton_Clicked;
            caveLocationStack.Children.Add(regionsFilterButton);

            sitesFilterLabel = new Label
            {
                FontSize = 12,
                Text = "Selected Sites: None"
            };
            caveLocationStack.Children.Add(sitesFilterLabel);

            Button sitesFilterButton = new Button
            {
                Text = "Select Sites"
            };
            sitesFilterButton.Clicked += SitesFilterButton_Clicked;
            caveLocationStack.Children.Add(sitesFilterButton);

            caveLocationFrame.Content = caveLocationStack;
            contentStackLayout.Children.Add(caveLocationFrame);

            Frame buttonFrame = new Frame
            {
                HasShadow = true,
                BackgroundColor = Color.White
            };

            StackLayout buttonStack = new StackLayout();
            Label buttonHeadlineLabel = new Label
            {
                FontSize = 20,
                Text = "Start searching",
                TextColor = Color.Black
            };
            buttonStack.Children.Add(buttonHeadlineLabel);

            Button searchButton = new Button
            {
                BackgroundColor = Color.Accent,
                TextColor = Color.White,
                Text = "Search"
            };
            searchButton.Clicked += SearchButton_Clicked;
            buttonStack.Children.Add(searchButton);
            buttonFrame.Content = buttonStack;

            contentStackLayout.Children.Add(buttonFrame);

            ScrollView scrollView = new ScrollView
            {
                Content = contentStackLayout
            };

            Content = scrollView;
        }

        private void SearchButton_Clicked(object sender, EventArgs e)
        {
            string selectedCaveTypeName = caveFilterPicker.Items[caveFilterPicker.SelectedIndex];
            CaveTypeModel caveTypeModel = caveTypeDictionary[selectedCaveTypeName];

            List<CaveModel> caves = Kucha.GetCavesByFilters(caveTypeModel, pickedDistricts, pickedRegions, pickedSites);
            List<CaveFilter> searchHistory = Settings.CaveSearchHistorySetting ?? new List<CaveFilter>();
            CaveFilter caveFilter = new CaveFilter
            {
                caveTypeModel = caveTypeModel,
                pickedDistricts = pickedDistricts,
                pickedRegions = pickedRegions,
                pickedSites = pickedSites,
                FoundResultsString = "Results: " + caves.Count,
                SearchTimeString = "At " + DateTime.UtcNow.ToString()
            };
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