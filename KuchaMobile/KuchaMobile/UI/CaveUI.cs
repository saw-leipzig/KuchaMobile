using KuchaMobile.Internal;
using KuchaMobile.Logic;
using KuchaMobile.Logic.Models;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace KuchaMobile.UI
{
    public class CaveUI : ContentPage
    {
        private Editor notesEditor;
        private CaveModel cave;

        public CaveUI(CaveModel cave)
        {
            this.cave = cave;
            Title = "Cave " + cave.caveID;
            StackLayout contentStack = new StackLayout();
            contentStack.Padding = 16;
            Frame generalCaveFrame = new Frame();
            generalCaveFrame.HasShadow = true;
            generalCaveFrame.BackgroundColor = Color.White;

            StackLayout generalCaveStack = new StackLayout();
            Label generalHeadlineLabel = new Label();
            generalHeadlineLabel.FontSize = 20;
            generalHeadlineLabel.Text = "General";
            generalHeadlineLabel.TextColor = Color.Black;
            generalCaveStack.Children.Add(generalHeadlineLabel);
            Label nameLabel = new Label();
            nameLabel.Text = "Historical Name: " + cave.historicalName;
            generalCaveStack.Children.Add(nameLabel);
            if (!String.IsNullOrEmpty(cave.optionalHistoricalName))
            {
                Label optHistoricalNameLabel = new Label();
                optHistoricalNameLabel.Text = "Optional Historical Name: " + cave.optionalHistoricalName;
                generalCaveStack.Children.Add(optHistoricalNameLabel);
            }
            Label siteLabel = new Label();
            siteLabel.Text = "Site: " + Kucha.GetCaveSiteStringByID(cave.siteID);
            generalCaveStack.Children.Add(siteLabel);
            Label districtLabel = new Label();
            districtLabel.Text = "District: " + Kucha.GetCaveDistrictStringByID(cave.districtID);
            generalCaveStack.Children.Add(districtLabel);
            Label regionLabel = new Label();
            regionLabel.Text = "Region: " + Kucha.GetCaveRegionStringByID(cave.regionID);
            generalCaveStack.Children.Add(regionLabel);
            Label typeLabel = new Label();
            typeLabel.Text = "Type: " + Kucha.GetCaveTypeStringByID(cave.caveTypeID);
            generalCaveStack.Children.Add(typeLabel);

            if (!String.IsNullOrEmpty(cave.measurementString))
            {
                Label measurementLabel = new Label();
                measurementLabel.Text = "Measurement: " + cave.measurementString;
                generalCaveStack.Children.Add(measurementLabel);
            }
            generalCaveFrame.Content = generalCaveStack;
            contentStack.Children.Add(generalCaveFrame);

            Frame caveSketchFrame = new Frame();
            caveSketchFrame.HasShadow = true;
            caveSketchFrame.BackgroundColor = Color.White;

            StackLayout caveSketchStack = new StackLayout();
            Label caveSketchHeadline = new Label();
            caveSketchHeadline.FontSize = 20;
            caveSketchHeadline.Text = "Cave Sketch";
            caveSketchHeadline.TextColor = Color.Black;
            caveSketchStack.Children.Add(caveSketchHeadline);
            if (!String.IsNullOrEmpty(cave.optionalCaveSketch))
            {
                Image caveSketch = new Image();
                caveSketch.WidthRequest = 200;
                caveSketch.Aspect = Aspect.AspectFit;
                caveSketch.Source = ImageSource.FromUri(new Uri(Internal.Connection.GetCaveSketchURL(cave.optionalCaveSketch)));
                caveSketchStack.Children.Add(caveSketch);
            }

            Image caveBackground = new Image();
            caveBackground.WidthRequest = 200;
            caveBackground.HeightRequest = 200;
            caveBackground.Source = ImageSource.FromUri(new Uri(Internal.Connection.GetCaveBackgroundImageURL(cave.caveTypeID)));

            caveSketchStack.Children.Add(caveBackground);

            caveSketchFrame.Content = caveSketchStack;
            contentStack.Children.Add(caveSketchFrame);

            Frame notesFrame = new Frame();
            notesFrame.BackgroundColor = Color.White;
            notesFrame.HasShadow = true;
            StackLayout notesStack = new StackLayout();

            Label notesLabel = new Label();
            notesLabel.TextColor = Color.Black;
            notesLabel.FontSize = 20;
            notesLabel.Text = "Private Notes";
            notesStack.Children.Add(notesLabel);

            notesEditor = new Editor();
            notesEditor.BackgroundColor = Color.White;
            notesEditor.HeightRequest = 100;
            var index = Settings.SavedNotesSetting.FindIndex(c => c.ID == cave.caveID && c.Type == NotesSaver.NOTES_TYPE.NOTE_TYPE_CAVE);
            if (index != -1) notesEditor.Text = Settings.SavedNotesSetting[index].Note;
            notesStack.Children.Add(notesEditor);
            notesFrame.Content = notesStack;
            contentStack.Children.Add(notesFrame);

            ScrollView scrollView = new ScrollView();
            scrollView.VerticalOptions = LayoutOptions.FillAndExpand;
            scrollView.Content = contentStack;
            Content = scrollView;
        }

        protected override void OnDisappearing()
        {
            var index = Settings.SavedNotesSetting.FindIndex(c => c.ID == cave.caveID && c.Type == NotesSaver.NOTES_TYPE.NOTE_TYPE_CAVE);
            if (index == -1)
            {
                if (!String.IsNullOrEmpty(notesEditor.Text))
                {
                    List<NotesSaver> savedNotes = Settings.SavedNotesSetting;
                    savedNotes.Add(new NotesSaver(NotesSaver.NOTES_TYPE.NOTE_TYPE_CAVE, cave.caveID, notesEditor.Text));
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
    }
}