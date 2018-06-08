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

            ListView resultListView = new ListView();
            resultListView.Header = "Ergebnisse";
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
