using KuchaMobile.Internal;
using KuchaMobile.Logic;
using KuchaMobile.Logic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace KuchaMobile.UI
{
    public class PaintedRepresentationUI : ContentPage
    {
        public CaveModel cave { get; set; }

        Editor notesEditor;

        PaintedRepresentationModel paintedRepresentation;
        public PaintedRepresentationUI(PaintedRepresentationModel paintedRepresentation)
        {
            this.paintedRepresentation = paintedRepresentation;
            this.cave = Kucha.GetCaveByID(paintedRepresentation.caveID);
            Title = "Painted Representation " + paintedRepresentation.depictionID;

            StackLayout contentStack = new StackLayout();
            contentStack.Padding = 20;
            Frame generalFrame = new Frame();
            generalFrame.HasShadow = true;
            generalFrame.BackgroundColor = Color.White;
            StackLayout generalStack = new StackLayout();

            Label generalInfoLabel = new Label();
            generalInfoLabel.Text = "Generelle Infos";
            generalInfoLabel.FontSize = 20;
            generalInfoLabel.TextColor = Color.Black;
            generalStack.Children.Add(generalInfoLabel);
            if(!String.IsNullOrEmpty(paintedRepresentation.description))
            {
                Label descriptionLabel = new Label();
                descriptionLabel.Text = "Description: "+paintedRepresentation.description;
                generalStack.Children.Add(descriptionLabel);
            }
            if (!String.IsNullOrEmpty(paintedRepresentation.acquiredByExpedition))
            {
                Label aquiredByExpeditionLabel = new Label();
                aquiredByExpeditionLabel.Text = "Aquired by Expedition: " + paintedRepresentation.acquiredByExpedition;
                generalStack.Children.Add(aquiredByExpeditionLabel);
            }

            if (!String.IsNullOrEmpty(paintedRepresentation.currentLocation))
            {
                Label currentLocationLabel = new Label();
                currentLocationLabel.Text = "Current Location: " + paintedRepresentation.currentLocation;
                generalStack.Children.Add(currentLocationLabel);
            }
            if (!String.IsNullOrEmpty(paintedRepresentation.vendor))
            {
                Label vendorLabel = new Label();
                vendorLabel.Text = "Vendor: " + paintedRepresentation.vendor;
                generalStack.Children.Add(vendorLabel);
            }
            if (paintedRepresentation.Iconography.Any())
            {
                Label iconographyLabel = new Label();
                iconographyLabel.Text = "Iconography: ";
                foreach (string i in paintedRepresentation.Iconography)
                {
                    iconographyLabel.Text += i + "\n";
                }
                generalStack.Children.Add(iconographyLabel);
            }
            if (paintedRepresentation.PictorialElements.Any())
            {
                Label pictorialElementsLabel = new Label();
                pictorialElementsLabel.Text = "Pictorial Elements: ";
                foreach (string i in paintedRepresentation.PictorialElements)
                {
                    pictorialElementsLabel.Text += i + "\n";
                }
                generalStack.Children.Add(pictorialElementsLabel);
            }
            generalFrame.Content = generalStack;
            contentStack.Children.Add(generalFrame);

            Frame caveFrame = new Frame();
            caveFrame.BackgroundColor = Color.White;
            caveFrame.HasShadow = true;

            StackLayout caveStack = new StackLayout();
            caveStack.Spacing = 2;
            if(cave == null)
            {
                Label caveInfoLabel = new Label();
                caveInfoLabel.Text = "Cave could not be loaded - the backend probably sent an invalid ID or the local database needs to be updated. (ID "+ paintedRepresentation.caveID+")";
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
                caveInfoLabel.Text = "Cave Infos";
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
            notesLabel.Text = "Private Notizen";
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
            if(index==-1)
            {               
                if(!String.IsNullOrEmpty(notesEditor.Text))
                {
                    List<NotesSaver> savedNotes = Settings.SavedNotesSetting;
                    savedNotes.Add(new NotesSaver(NotesSaver.NOTES_TYPE.NOTE_TYPE_PAINTEDREPRESENTATION, paintedRepresentation.depictionID, notesEditor.Text));
                    Settings.SavedNotesSetting = savedNotes;
                }
            }
            else
            {
                NotesSaver currentNote = Settings.SavedNotesSetting[index];
                if(currentNote.Note != notesEditor.Text)
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

        class RelatedImageStack : StackLayout
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
