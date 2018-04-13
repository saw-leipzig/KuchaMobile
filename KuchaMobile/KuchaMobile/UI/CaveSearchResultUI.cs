using KuchaMobile.Logic.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace KuchaMobile.UI
{
    public class CaveSearchResultUI : ContentPage
    {
        public CaveSearchResultUI(List<CaveModel> caves)
        {
            Title = "Cavesearch: " + caves.Count + " Ergebnis(se)";

            StackLayout stackLayout = new StackLayout();
            foreach(CaveModel cave in caves)
            {
                stackLayout.Children.Add(new CaveResultGrid(cave));
            }
            ScrollView scrollView = new ScrollView();
            scrollView.Content = stackLayout;

            Content = scrollView;
        }

        private class CaveResultGrid : Grid
        {
            public CaveResultGrid(CaveModel cave)
            {
                BackgroundColor = Color.LightGray;
                Label nameLabel = new Label();
                nameLabel.Text = cave.historicName;
                Children.Add(nameLabel, 0, 0);
                Label IDLabel = new Label();
                IDLabel.Text = cave.caveID+"";
                Children.Add(IDLabel, 1, 0);
                Label someOtherLabel = new Label();
                someOtherLabel.Text = cave.siteID+"";
                Children.Add(someOtherLabel, 0, 1);
            }
        }
    }
}
