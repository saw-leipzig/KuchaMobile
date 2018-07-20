using KuchaMobile.Internal;
using KuchaMobile.Logic;
using KuchaMobile.Logic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace KuchaMobile.UI
{
    public class PaintedRepresentationUI : ContentPage
    {
        public CaveModel cave { get; set; }

        private Editor notesEditor;

        private PaintedRepresentationModel paintedRepresentation;

        public PaintedRepresentationUI(PaintedRepresentationModel paintedRepresentation)
        {
            this.paintedRepresentation = paintedRepresentation;
            this.cave = Kucha.GetCaveByID(paintedRepresentation.caveID);
            Title = "Painted Representation " + paintedRepresentation.depictionID;

            StackLayout contentStack = new StackLayout();
            contentStack.Padding = 16;
            Frame generalFrame = new Frame();
            generalFrame.HasShadow = true;
            generalFrame.BackgroundColor = Color.White;
            StackLayout generalStack = new StackLayout();

            Label generalInfoLabel = new Label();
            generalInfoLabel.Text = "General Information";
            generalInfoLabel.FontSize = 20;
            generalInfoLabel.TextColor = Color.Black;
            generalStack.Children.Add(generalInfoLabel);
            if (!String.IsNullOrEmpty(paintedRepresentation.description))
            {
                Label descriptionLabel = new Label();
                var descriptionText = new FormattedString();
                descriptionText.Spans.Add(new Span { Text = "Description: ", FontAttributes = FontAttributes.Bold });
                descriptionText.Spans.Add(new Span { Text = paintedRepresentation.description });
                descriptionLabel.FormattedText = descriptionText;
                generalStack.Children.Add(descriptionLabel);
            }
            if (!String.IsNullOrEmpty(paintedRepresentation.acquiredByExpedition))
            {
                Label aquiredByExpeditionLabel = new Label();
                var descriptionText = new FormattedString();
                descriptionText.Spans.Add(new Span { Text = "Aquired by Expedition: ", FontAttributes = FontAttributes.Bold });
                descriptionText.Spans.Add(new Span { Text = paintedRepresentation.acquiredByExpedition });
                aquiredByExpeditionLabel.FormattedText = descriptionText;
                generalStack.Children.Add(aquiredByExpeditionLabel);
            }

            if (!String.IsNullOrEmpty(paintedRepresentation.currentLocation))
            {
                Label currentLocationLabel = new Label();
                var descriptionText = new FormattedString();
                descriptionText.Spans.Add(new Span { Text = "Current Location: ", FontAttributes = FontAttributes.Bold });
                descriptionText.Spans.Add(new Span { Text = paintedRepresentation.currentLocation });
                currentLocationLabel.FormattedText = descriptionText;
                generalStack.Children.Add(currentLocationLabel);
            }
            if (!String.IsNullOrEmpty(paintedRepresentation.vendor))
            {
                Label vendorLabel = new Label();
                var descriptionText = new FormattedString();
                descriptionText.Spans.Add(new Span { Text = "Vendor: ", FontAttributes = FontAttributes.Bold });
                descriptionText.Spans.Add(new Span { Text = paintedRepresentation.vendor });
                vendorLabel.FormattedText = descriptionText;
                generalStack.Children.Add(vendorLabel);
            }
            if (paintedRepresentation.Iconography.Any())
            {
                Label iconographyLabel = new Label();
                var descriptionText = new FormattedString();
                descriptionText.Spans.Add(new Span { Text = "Iconography:\n", FontAttributes = FontAttributes.Bold });
                foreach (string i in paintedRepresentation.Iconography)
                {
                    descriptionText.Spans.Add(new Span { Text = i+"\n" });
                }
                iconographyLabel.FormattedText = descriptionText;
                generalStack.Children.Add(iconographyLabel);
            }
            if (paintedRepresentation.PictorialElements.Any())
            {
                Label pictorialElementsLabel = new Label();
                var descriptionText = new FormattedString();
                descriptionText.Spans.Add(new Span { Text = "Pictorial Elements:\n", FontAttributes = FontAttributes.Bold });
                foreach (string i in paintedRepresentation.PictorialElements)
                {
                    descriptionText.Spans.Add(new Span { Text = i + "\n" });
                }
                pictorialElementsLabel.FormattedText = descriptionText;
                generalStack.Children.Add(pictorialElementsLabel);
            }
            generalFrame.Content = generalStack;
            contentStack.Children.Add(generalFrame);

            Frame caveFrame = new Frame();
            caveFrame.BackgroundColor = Color.White;
            caveFrame.HasShadow = true;

            StackLayout caveStack = new StackLayout();
            caveStack.Spacing = 2;
            if (cave == null)
            {
                Label caveInfoLabel = new Label();
                caveInfoLabel.Text = "Cave could not be loaded - the backend probably sent an invalid ID or the local database needs to be updated. (ID " + paintedRepresentation.caveID + ")";
                caveInfoLabel.FontSize = 20;
                caveInfoLabel.TextColor = Color.Black;
                caveStack.Children.Add(caveInfoLabel);
            }
            else
            {
                TapGestureRecognizer caveTap = new TapGestureRecognizer();
                caveTap.Tapped += CaveTap_Tapped;
                caveFrame.GestureRecognizers.Add(caveTap);

                Label caveInfoLabel = new Label();
                caveInfoLabel.Text = "Cave Information";
                caveInfoLabel.FontSize = 20;
                caveInfoLabel.TextColor = Color.Black;
                caveStack.Children.Add(caveInfoLabel);
                string caveInfoString = "Located in Cave: " + Kucha.GetCaveSiteStringByID(cave.siteID) + ": " + cave.caveID + " " + cave.optionalHistoricalName;
                Label caveLabel = new Label();
                caveLabel.Text = caveInfoString;
                caveStack.Children.Add(caveLabel);

                if (!String.IsNullOrEmpty(cave.optionalCaveSketch))
                {
                    Image caveSketch = new Image();
                    caveSketch.WidthRequest = 150;
                    caveSketch.Aspect = Aspect.AspectFit;
                    caveSketch.Source = ImageSource.FromUri(new Uri(Internal.Connection.GetCaveSketchURL(cave.optionalCaveSketch)));
                    caveStack.Children.Add(caveSketch);
                }

                Image caveBackground = new Image();
                caveBackground.WidthRequest = 150;
                caveBackground.HeightRequest = 150;
                caveBackground.Source = ImageSource.FromUri(new Uri(Internal.Connection.GetCaveBackgroundImageURL(cave.caveTypeID)));
                caveStack.Children.Add(caveBackground);
            }

            caveFrame.Content = caveStack;
            contentStack.Children.Add(caveFrame);

            foreach (RelatedImage image in paintedRepresentation.relatedImages)
            {
                Frame imageFrame = new Frame();
                imageFrame.HasShadow = true;
                imageFrame.BackgroundColor = Color.White;
                RelatedImageStack imageStack = new RelatedImageStack(image);
                imageFrame.Content = imageStack;
                contentStack.Children.Add(imageFrame);

                TapGestureRecognizer imageTap = new TapGestureRecognizer();
                imageTap.Tapped += ImageTap_Tapped;
                imageStack.GestureRecognizers.Add(imageTap);
            }

            Frame notesFrame = new Frame();
            notesFrame.HasShadow = true;
            notesFrame.BackgroundColor = Color.White;
            StackLayout notesStack = new StackLayout();

            Label notesLabel = new Label();
            notesLabel.TextColor = Color.Black;
            notesLabel.FontSize = 20;
            notesLabel.Text = "Private Notes";
            notesStack.Children.Add(notesLabel);

            notesEditor = new Editor();
            notesEditor.BackgroundColor = Color.White;
            notesEditor.HeightRequest = 100;
            var index = Settings.SavedNotesSetting.FindIndex(pr => pr.ID == paintedRepresentation.depictionID && pr.Type == NotesSaver.NOTES_TYPE.NOTE_TYPE_PAINTEDREPRESENTATION);
            if (index != -1) notesEditor.Text = Settings.SavedNotesSetting[index].Note;
            notesStack.Children.Add(notesEditor);
            notesFrame.Content = notesStack;
            contentStack.Children.Add(notesFrame);

            ScrollView contentScrollView = new ScrollView();
            contentScrollView.HorizontalOptions = LayoutOptions.FillAndExpand;
            contentScrollView.VerticalOptions = LayoutOptions.FillAndExpand;
            contentScrollView.Content = contentStack;

            Content = contentScrollView;
        }

        protected override void OnDisappearing()
        {
            var index = Settings.SavedNotesSetting.FindIndex(pr => pr.ID == paintedRepresentation.depictionID && pr.Type == NotesSaver.NOTES_TYPE.NOTE_TYPE_PAINTEDREPRESENTATION);
            if (index == -1)
            {
                if (!String.IsNullOrEmpty(notesEditor.Text))
                {
                    List<NotesSaver> savedNotes = Settings.SavedNotesSetting;
                    savedNotes.Add(new NotesSaver(NotesSaver.NOTES_TYPE.NOTE_TYPE_PAINTEDREPRESENTATION, paintedRepresentation.depictionID, notesEditor.Text));
                    Settings.SavedNotesSetting = savedNotes;
                }
            }
            else
            {
                NotesSaver currentNote = Settings.SavedNotesSetting[index];
                if (currentNote.Note != notesEditor.Text)
                {
                    currentNote.Note = notesEditor.Text;
                    List<NotesSaver> savedNotes = Settings.SavedNotesSetting;
                    savedNotes[index] = currentNote;
                    Settings.SavedNotesSetting = savedNotes;
                }
            }
            base.OnDisappearing();
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

        private class RelatedImageStack : StackLayout
        {
            public RelatedImage relatedImage { get; set; }

            public RelatedImageStack(RelatedImage image)
            {
                this.relatedImage = image;
                Spacing = 2;
                BackgroundColor = Color.White;

                Label idLabel = new Label();
                idLabel.TextColor = Color.Black;
                idLabel.FontSize = 20;
                var idText = new FormattedString();
                idText.Spans.Add(new Span { Text = "ImageID: ", FontAttributes = FontAttributes.Bold });
                idText.Spans.Add(new Span { Text = image.imageID+""});
                idLabel.FormattedText = idText;
                Children.Add(idLabel);

                if (!String.IsNullOrEmpty(image.title))
                {
                    Label titleLabel = new Label();
                    var descriptionText = new FormattedString();
                    descriptionText.Spans.Add(new Span { Text = "Title: ", FontAttributes = FontAttributes.Bold });
                    descriptionText.Spans.Add(new Span { Text = image.title + "" });
                    titleLabel.FormattedText = descriptionText;
                    Children.Add(titleLabel);
                }

                if (!String.IsNullOrEmpty(image.shortName))
                {
                    Label shortNameLabel = new Label();
                    var descriptionText = new FormattedString();
                    descriptionText.Spans.Add(new Span { Text = "Shortname: ", FontAttributes = FontAttributes.Bold });
                    descriptionText.Spans.Add(new Span { Text = image.shortName + "" });
                    shortNameLabel.FormattedText = descriptionText;
                    Children.Add(shortNameLabel);
                }

                if (!String.IsNullOrEmpty(image.copyright))
                {
                    Label copyRightLabel = new Label();
                    var descriptionText = new FormattedString();
                    descriptionText.Spans.Add(new Span { Text = "Copyright: ", FontAttributes = FontAttributes.Bold });
                    descriptionText.Spans.Add(new Span { Text = image.copyright + "" });
                    copyRightLabel.FormattedText = descriptionText;
                    Children.Add(copyRightLabel);
                }

                if (!String.IsNullOrEmpty(image.comment))
                {
                    Label commentLabel = new Label();
                    var descriptionText = new FormattedString();
                    descriptionText.Spans.Add(new Span { Text = "Comment: ", FontAttributes = FontAttributes.Bold });
                    descriptionText.Spans.Add(new Span { Text = image.comment + "" });
                    commentLabel.FormattedText = descriptionText;
                    Children.Add(commentLabel);
                }
                if (!String.IsNullOrEmpty(image.date))
                {
                    Label dateLabel = new Label();
                    var descriptionText = new FormattedString();
                    descriptionText.Spans.Add(new Span { Text = "Date: ", FontAttributes = FontAttributes.Bold });
                    descriptionText.Spans.Add(new Span { Text = image.date + "" });
                    dateLabel.FormattedText = descriptionText;
                    Children.Add(dateLabel);
                }
                if (Settings.showPreviewPicturesSetting)
                {
                    Image prImage = new Image();
                    prImage.WidthRequest = 150;
                    prImage.HeightRequest = 150;
                    prImage.Source = ImageSource.FromUri(new Uri(Internal.Connection.GetPaintedRepresentationImageURL(image.imageID, 150)));
                    Children.Add(prImage);
                }
                else
                {
                    Label l = new Label();
                    l.Text = "Preview pictures are disabled in Settings";
                    l.TextColor = Color.LightGray;
                    l.HorizontalOptions = LayoutOptions.Center;
                    Children.Add(l);
                }
            }
        }
    }
}