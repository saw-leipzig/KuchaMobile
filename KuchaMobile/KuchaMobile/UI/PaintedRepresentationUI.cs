using KuchaMobile.Logic;
using KuchaMobile.Logic.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace KuchaMobile.UI
{
    public class PaintedRepresentationUI : ContentPage
    {
        public CaveModel cave { get; set; }
        public PaintedRepresentationUI(PaintedRepresentationModel paintedRepresentation)
        {
            this.cave = paintedRepresentation.cave;
            Title = "Painted Representation " + paintedRepresentation.depictionID;

            StackLayout contentStack = new StackLayout();
            Label generalInfoLabel = new Label();
            generalInfoLabel.Text = "Generelle Infos";
            generalInfoLabel.FontSize = 20;
            contentStack.Children.Add(generalInfoLabel);
            if(!String.IsNullOrEmpty(paintedRepresentation.description))
            {
                Label descriptionLabel = new Label();
                descriptionLabel.Text = paintedRepresentation.description;
                contentStack.Children.Add(descriptionLabel);
            }

            StackLayout caveStack = new StackLayout();
            caveStack.Spacing = 2;
            caveStack.BackgroundColor = Color.White;
            TapGestureRecognizer caveTap = new TapGestureRecognizer();
            caveTap.Tapped += CaveTap_Tapped;
            caveStack.GestureRecognizers.Add(caveTap);

            Label caveInfoLabel = new Label();
            caveInfoLabel.Text = "Cave Infos";
            caveInfoLabel.FontSize = 20;
            caveStack.Children.Add(caveInfoLabel);
            string caveInfoString = "Located in Cave: " + Kucha.GetCaveSiteStringByID(paintedRepresentation.cave.siteID) + ": " + paintedRepresentation.cave.caveID + " " + paintedRepresentation.cave.optionalHistoricName;
            Label caveLabel = new Label();
            caveLabel.Text = caveInfoString;
            caveStack.Children.Add(caveLabel);

            if (!String.IsNullOrEmpty(paintedRepresentation.cave.optionalCaveSketch))
            {
                Image caveSketch = new Image();
                caveSketch.WidthRequest = 150;
                caveSketch.Aspect = Aspect.AspectFit;
                caveSketch.Source = ImageSource.FromUri(new Uri(Internal.Connection.GetCaveSketchURL(paintedRepresentation.cave.optionalCaveSketch)));
                caveStack.Children.Add(caveSketch);
            }

            Image caveBackground = new Image();
            caveBackground.WidthRequest = 150;
            caveBackground.HeightRequest = 150;
            caveBackground.Source = ImageSource.FromUri(new Uri(Internal.Connection.GetCaveBackgroundImageURL(paintedRepresentation.cave.caveTypeID)));
            caveStack.Children.Add(caveBackground);
            contentStack.Children.Add(caveStack);

            Label imageInfoLabel = new Label();
            imageInfoLabel.Text = "Image Infos";
            imageInfoLabel.FontSize = 20;
            contentStack.Children.Add(imageInfoLabel);

            foreach (RelatedImage image in paintedRepresentation.relatedImages)
            {               
                RelatedImageStack imageStack = new RelatedImageStack(image);
                contentStack.Children.Add(imageStack);

                TapGestureRecognizer imageTap = new TapGestureRecognizer();
                imageTap.Tapped += ImageTap_Tapped;
                imageStack.GestureRecognizers.Add(imageTap);
            }

            ScrollView contentScrollView = new ScrollView();
            contentScrollView.HorizontalOptions = LayoutOptions.FillAndExpand;
            contentScrollView.VerticalOptions = LayoutOptions.FillAndExpand;
            contentScrollView.Content = contentStack;

            Content = contentScrollView;
        }

        private void CaveTap_Tapped(object sender, EventArgs e)
        {
            Navigation.PushAsync(new CaveUI(cave));
        }

        private void ImageTap_Tapped(object sender, EventArgs e)
        {
            RelatedImageStack stack = sender as RelatedImageStack;
            Navigation.PushAsync(new ImageUI(stack.relatedImage));
        }

        class RelatedImageStack : StackLayout
        {
            public RelatedImage relatedImage { get; set; }
            public RelatedImageStack(RelatedImage image)
            {
                this.relatedImage = image;
                Spacing = 2;
                BackgroundColor = Color.White;

                Label idLabel = new Label();
                idLabel.Text = "ImageID: " + image.imageID;
                Children.Add(idLabel);

                if(!String.IsNullOrEmpty(image.title))
                {
                    Label titleLabel = new Label();
                    titleLabel.Text = "Titel: "+image.title;
                    Children.Add(titleLabel);
                }

                if (!String.IsNullOrEmpty(image.shortName))
                {
                    Label shortNameLabel = new Label();
                    shortNameLabel.Text = "ShortName: " + image.shortName;
                    Children.Add(shortNameLabel);
                }

                if (!String.IsNullOrEmpty(image.copyright))
                {
                    Label copyRightLabel = new Label();
                    copyRightLabel.Text = "Copyright: "+image.copyright;
                    Children.Add(copyRightLabel);
                }

                if (!String.IsNullOrEmpty(image.comment))
                {
                    Label commentLabel = new Label();
                    commentLabel.Text = "Comment: " + image.comment;
                    Children.Add(commentLabel);
                }
                if (!String.IsNullOrEmpty(image.date))
                {
                    Label dateLabel = new Label();
                    dateLabel.Text = "Date: " + image.date;
                    Children.Add(dateLabel);
                }


                Image prImage = new Image();
                prImage.WidthRequest = 150;
                prImage.HeightRequest = 150;
                prImage.Source = ImageSource.FromUri(new Uri(Internal.Connection.GetPaintedRepresentationImageURL(image.imageID, 150)));
                Children.Add(prImage);
            }
        }
    }
}
