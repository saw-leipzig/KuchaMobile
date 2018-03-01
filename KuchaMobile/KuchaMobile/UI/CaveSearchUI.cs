using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace KuchaMobile.UI
{
    public class CaveSearchUI : ContentPage
    {
        Picker caveFilterPicker;

        public CaveSearchUI()
        {
            Title = "Cave Search";
            ToolbarItems.Add(new ToolbarItem("History", null, HistoryButton_Clicked));

            StackLayout contentStackLayout = new StackLayout();
            contentStackLayout.Padding = new Thickness(72, 16, 16, 16);
            contentStackLayout.HorizontalOptions = LayoutOptions.FillAndExpand;
            contentStackLayout.VerticalOptions = LayoutOptions.FillAndExpand;

            Label caveFilterHeadlineLabel = new Label();
            caveFilterHeadlineLabel.FontSize = 20;
            caveFilterHeadlineLabel.Text = "Cave Filter";
            contentStackLayout.Children.Add(caveFilterHeadlineLabel);

            caveFilterPicker = new Picker();
            caveFilterPicker.Items.Add("unknown");
            caveFilterPicker.Items.Add("square cave");  //TODO: Replace with proper enum of models/cave object once available
            caveFilterPicker.SelectedIndex = 0;

            contentStackLayout.Children.Add(caveFilterPicker);

            Label locationFilterHeadlineLabel = new Label();
            locationFilterHeadlineLabel.FontSize = 20;
            locationFilterHeadlineLabel.Text = "Location Filter";
            contentStackLayout.Children.Add(locationFilterHeadlineLabel);

            Label sitesFilterLabel = new Label();
            sitesFilterLabel.FontSize = 12;
            sitesFilterLabel.Text = "Sites";
            contentStackLayout.Children.Add(sitesFilterLabel);

            Label districtsFilterLabel = new Label();
            districtsFilterLabel.FontSize = 12;
            districtsFilterLabel.Text = "Districts";
            contentStackLayout.Children.Add(districtsFilterLabel);

            Label regionsFilterLabel = new Label();
            regionsFilterLabel.FontSize = 12;
            regionsFilterLabel.Text = "Regions";
            contentStackLayout.Children.Add(regionsFilterLabel);

            Button searchButton = new Button();
            searchButton.Text = "Suchen";
            contentStackLayout.Children.Add(searchButton);

            Content = contentStackLayout;
        }

        private void HistoryButton_Clicked()
        {

        }
    }
}
