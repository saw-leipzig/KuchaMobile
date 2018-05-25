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

            StackLayout stackLayout = new StackLayout();
            foreach (PaintedRepresentationModel paintedRepresentationModel in paintedRepresentationModels)
            {
                stackLayout.Children.Add(new PaintedRepresentationResultGrid(paintedRepresentationModel));
            }
            ScrollView scrollView = new ScrollView();
            scrollView.Content = stackLayout;

            Content = scrollView;
        }

        private class PaintedRepresentationResultGrid : Grid
        {
            public PaintedRepresentationResultGrid(PaintedRepresentationModel paintedRepresentation)
            {
                BackgroundColor = Color.LightGray;
                Label nameLabel = new Label();
                nameLabel.Text = paintedRepresentation.shortName;
                Children.Add(nameLabel, 0, 0);
                Label IDLabel = new Label();
                IDLabel.Text = paintedRepresentation.depictionID + "";
                Children.Add(IDLabel, 1, 0);
                Label someOtherLabel = new Label();
                someOtherLabel.Text = paintedRepresentation.locationID + "";
                Children.Add(someOtherLabel, 0, 1);
            }
        }
    }
}
