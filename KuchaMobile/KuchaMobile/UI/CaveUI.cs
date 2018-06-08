using KuchaMobile.Logic;
using KuchaMobile.Logic.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace KuchaMobile.UI
{
    public class CaveUI : ContentPage
    {
        public CaveUI(CaveModel cave)
        {
            Title = "Cave " + cave.caveID;
            StackLayout contentStack = new StackLayout();
            Label nameLabel = new Label();
            nameLabel.Text = "Historical Name: " + cave.historicName;
            contentStack.Children.Add(nameLabel);
            if(!String.IsNullOrEmpty(cave.optionalHistoricName))
            {
                Label optHistoricalNameLabel = new Label();
                optHistoricalNameLabel.Text = "Optional Historical Name: " + cave.optionalHistoricName;
                contentStack.Children.Add(optHistoricalNameLabel);
            }
            Label siteLabel = new Label();
            siteLabel.Text = "Site: "+Kucha.GetCaveSiteStringByID(cave.siteID);
            contentStack.Children.Add(siteLabel);
            Label districtLabel = new Label();
            districtLabel.Text = "District: " + Kucha.GetCaveDistrictStringByID(cave.districtID);
            contentStack.Children.Add(districtLabel);
            Label regionLabel = new Label();
            regionLabel.Text = "Region: " + Kucha.GetCaveRegionStringByID(cave.regionID);
            contentStack.Children.Add(regionLabel);
            Label typeLabel = new Label();
            typeLabel.Text = "Type: " + Kucha.GetCaveTypeStringByID(cave.caveTypeID);
            contentStack.Children.Add(typeLabel);

            if(!String.IsNullOrEmpty(cave.optionalCaveSketch))
            {
                Image caveSketch = new Image();
                caveSketch.WidthRequest = 200;
                caveSketch.Aspect = Aspect.AspectFit;
                caveSketch.Source = ImageSource.FromUri(new Uri(Internal.Connection.GetCaveSketchURL(cave.optionalCaveSketch)));
                contentStack.Children.Add(caveSketch);
            }

            Image caveBackground = new Image();
            caveBackground.WidthRequest = 200;
            caveBackground.HeightRequest = 200;
            caveBackground.Source = ImageSource.FromUri(new Uri(Internal.Connection.GetCaveBackgroundImageURL(cave.caveTypeID)));

            contentStack.Children.Add(caveBackground);

            ScrollView scrollView = new ScrollView();
            scrollView.VerticalOptions = LayoutOptions.FillAndExpand;
            scrollView.Content = contentStack;
            Content = scrollView;
        }
    }
}
