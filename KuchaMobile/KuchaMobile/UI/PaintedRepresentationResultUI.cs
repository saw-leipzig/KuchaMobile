using KuchaMobile.Logic.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace KuchaMobile.UI
{
    public class PaintedRepresentationResultUI : ContentPage
    {
        public PaintedRepresentationResultUI(List<PaintedRepresentationModel> paintedRepresentationModels)
        {
            Title = "PR-Search: " + paintedRepresentationModels.Count + " Ergebnis(se)";

            ListView resultListView = new ListView();
            resultListView.ItemsSource = paintedRepresentationModels;
            resultListView.ItemTapped += ResultListView_ItemTapped;

            Content = resultListView;
        }

        private void ResultListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            ((ListView)sender).SelectedItem = null;
            var paintedRep = e.Item as PaintedRepresentationModel;
            Navigation.PushAsync(new PaintedRepresentationUI(paintedRep));
        }
    }
}
