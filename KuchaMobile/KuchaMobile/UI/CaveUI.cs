using KuchaMobile.Internal;
using KuchaMobile.Logic;
using KuchaMobile.Logic.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace KuchaMobile.UI
{
    public class CaveUI : ContentPage
    {
        Editor notesEditor;
        CaveModel cave;
        public CaveUI(CaveModel cave)
        {
            this.cave = cave;
            Title = "Cave " + cave.caveID;
            StackLayout contentStack = new StackLayout();
            Label nameLabel = new Label();
            nameLabel.Text = "Historical Name: " + cave.historicalName;
            contentStack.Children.Add(nameLabel);
            if(!String.IsNullOrEmpty(cave.optionalHistoricalName))
            {
                Label optHistoricalNameLabel = new Label();
                optHistoricalNameLabel.Text = "Optional Historical Name: " + cave.optionalHistoricalName;
                contentStack.Children.Add(optHistoricalNameLabel);
            }
            Label siteLabel = new Label();
            siteLabel.Text = "Site: "+Kucha.GetCaveSiteStringByID(cave.siteID);
            contentStack.Children.Add(siteLabel);
            Label districtLabel = new Label();
            districtLabel.Text = "District: " + Kucha.GetCaveDistrictStringByID(cave.districtID);
            contentStack.Children.Add(districtLabel);
            Label regionLabel = new Label();
            regionLabel.Text = "Region: " + Kucha.GetCaveRegionStringByID(cave.regionID);
            contentStack.Children.Add(regionLabel);
            Label typeLabel = new Label();
            typeLabel.Text = "Type: " + Kucha.GetCaveTypeStringByID(cave.caveTypeID);
            contentStack.Children.Add(typeLabel);

            if(!String.IsNullOrEmpty(cave.measurementString))
            {
                Label measurementLabel = new Label();
                measurementLabel.Text = "Measurement: " + cave.measurementString;
                contentStack.Children.Add(measurementLabel);
            }

            if(!String.IsNullOrEmpty(cave.optionalCaveSketch))
            {
                Image caveSketch = new Image();
                caveSketch.WidthRequest = 200;
                caveSketch.Aspect = Aspect.AspectFit;
                caveSketch.Source = ImageSource.FromUri(new Uri(Internal.Connection.GetCaveSketchURL(cave.optionalCaveSketch)));
                contentStack.Children.Add(caveSketch);
            }

            Image caveBackground = new Image();
            caveBackground.WidthRequest = 200;
            caveBackground.HeightRequest = 200;
            caveBackground.Source = ImageSource.FromUri(new Uri(Internal.Connection.GetCaveBackgroundImageURL(cave.caveTypeID)));

            contentStack.Children.Add(caveBackground);

            Label notesLabel = new Label();
            notesLabel.Text = "Private Notizen";
            contentStack.Children.Add(notesLabel);

            notesEditor = new Editor();
            notesEditor.BackgroundColor = Color.White;
            notesEditor.HeightRequest = 100;
            var index = Settings.SavedNotesSetting.FindIndex(c => c.ID == cave.caveID && c.Type == NotesSaver.NOTES_TYPE.NOTE_TYPE_CAVE);
            if (index != -1) notesEditor.Text = Settings.SavedNotesSetting[index].Note;
            contentStack.Children.Add(notesEditor);

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
