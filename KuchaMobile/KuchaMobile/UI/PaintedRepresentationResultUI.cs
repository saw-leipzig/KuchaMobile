using KuchaMobile.Logic.Models;
using System.Collections.Generic;
using Xamarin.Forms;

namespace KuchaMobile.UI
{
    public class PaintedRepresentationResultUI : ContentPage
    {
        /// <summary>
        /// Draws a list with the painted representation search results
        /// </summary>
        /// <param name="paintedRepresentationModels"></param>
        public PaintedRepresentationResultUI(List<PaintedRepresentationModel> paintedRepresentationModels)
        {
            Title = "PR-Search: " + paintedRepresentationModels.Count + " Ergebnis(se)";

            ListView resultListView = new ListView
            {
                ItemTemplate = new DataTemplate(typeof(TextCell))
            };
            resultListView.ItemTemplate.SetBinding(TextCell.TextProperty, "PRDisplayName");
            resultListView.ItemTemplate.SetBinding(TextCell.DetailProperty, "PRDetailDisplayName");
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