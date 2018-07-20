using KuchaMobile.Logic.Models;
using System.Collections.Generic;
using Xamarin.Forms;

namespace KuchaMobile.UI
{
    public class CaveSearchResultUI : ContentPage
    {
        public CaveSearchResultUI(List<CaveModel> caves)
        {
            Title = "Cavesearch: " + caves.Count + " Result(s)";

            ListView resultListView = new ListView
            {
                ItemTemplate = new DataTemplate(typeof(TextCell))
            };
            resultListView.ItemTemplate.SetBinding(TextCell.TextProperty, "CaveDisplayName");
            resultListView.ItemTemplate.SetBinding(TextCell.DetailProperty, "historicalName");
            resultListView.ItemsSource = caves;
            resultListView.ItemTapped += ResultListView_ItemTapped;

            Content = resultListView;
        }

        private void ResultListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            ((ListView)sender).SelectedItem = null;
            var caveModel = e.Item as CaveModel;
            Navigation.PushAsync(new CaveUI(caveModel));
        }
    }
}