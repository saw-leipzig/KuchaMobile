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
        private readonly Editor notesEditor;
        private readonly CaveModel cave;

        public CaveUI(CaveModel cave)
        {
            this.cave = cave;
            Title = "Cave " + cave.caveID;
            StackLayout contentStack = new StackLayout
            {
                Padding = 16
            };
            Frame generalCaveFrame = new Frame
            {
                HasShadow = true,
                BackgroundColor = Color.White
            };

            StackLayout generalCaveStack = new StackLayout();
            Label generalHeadlineLabel = new Label
            {
                FontSize = 20,
                Text = "General",
                TextColor = Color.Black
            };
            generalCaveStack.Children.Add(generalHeadlineLabel);

            Label nameLabel = new Label();
            var nameText = new FormattedString();
            nameText.Spans.Add(new Span { Text = "Historical Name: ", FontAttributes = FontAttributes.Bold });
            nameText.Spans.Add(new Span { Text = cave.historicalName });
            nameLabel.FormattedText = nameText;
            generalCaveStack.Children.Add(nameLabel);

            if (!String.IsNullOrEmpty(cave.optionalHistoricalName))
            {
                Label optHistoricalNameLabel = new Label();
                var optNameText = new FormattedString();
                optNameText.Spans.Add(new Span { Text = "Optional Historical Name: ", FontAttributes = FontAttributes.Bold });
                optNameText.Spans.Add(new Span { Text = cave.optionalHistoricalName });
                optHistoricalNameLabel.FormattedText = optNameText;
                generalCaveStack.Children.Add(optHistoricalNameLabel);
            }

            Label siteLabel = new Label();
            var siteText = new FormattedString();
            siteText.Spans.Add(new Span { Text = "Site: ", FontAttributes = FontAttributes.Bold });
            siteText.Spans.Add(new Span { Text = Kucha.GetCaveSiteStringByID(cave.siteID) });
            siteLabel.FormattedText = siteText;
            generalCaveStack.Children.Add(siteLabel);

            Label districtLabel = new Label();
            var districtText = new FormattedString();
            districtText.Spans.Add(new Span { Text = "District: ", FontAttributes = FontAttributes.Bold });
            districtText.Spans.Add(new Span { Text = Kucha.GetCaveDistrictStringByID(cave.districtID) });
            districtLabel.FormattedText = districtText;
            generalCaveStack.Children.Add(districtLabel);

            Label regionLabel = new Label();
            var regionText = new FormattedString();
            regionText.Spans.Add(new Span { Text = "Region: ", FontAttributes = FontAttributes.Bold });
            regionText.Spans.Add(new Span { Text = Kucha.GetCaveRegionStringByID(cave.regionID) });
            regionLabel.FormattedText = regionText;
            generalCaveStack.Children.Add(regionLabel);

            Label typeLabel = new Label();
            var typeText = new FormattedString();
            typeText.Spans.Add(new Span { Text = "Type: ", FontAttributes = FontAttributes.Bold });
            typeText.Spans.Add(new Span { Text = Kucha.GetCaveTypeStringByID(cave.caveTypeID) });
            typeLabel.FormattedText = typeText;
            generalCaveStack.Children.Add(typeLabel);

            if (!String.IsNullOrEmpty(cave.measurementString))
            {
                Label measurementLabel = new Label();
                var measurementText = new FormattedString();
                measurementText.Spans.Add(new Span { Text = "Measurement:\n", FontAttributes = FontAttributes.Bold });
                measurementText.Spans.Add(new Span { Text = cave.measurementString });
                measurementLabel.FormattedText = measurementText;
                generalCaveStack.Children.Add(measurementLabel);
            }
            generalCaveFrame.Content = generalCaveStack;
            contentStack.Children.Add(generalCaveFrame);

            Frame caveSketchFrame = new Frame
            {
                HasShadow = true,
                BackgroundColor = Color.White
            };

            StackLayout caveSketchStack = new StackLayout();
            Label caveSketchHeadline = new Label
            {
                FontSize = 20,
                Text = "Cave Sketch",
                TextColor = Color.Black
            };
            caveSketchStack.Children.Add(caveSketchHeadline);
            if (!String.IsNullOrEmpty(cave.optionalCaveSketch))
            {
                Image caveSketch = new Image
                {
                    WidthRequest = 200,
                    Aspect = Aspect.AspectFit,
                    Source = ImageSource.FromUri(new Uri(Internal.Connection.GetCaveSketchURL(cave.optionalCaveSketch)))
                };
                caveSketchStack.Children.Add(caveSketch);
            }

            Image caveBackground = new Image
            {
                WidthRequest = 200,
                HeightRequest = 200,
                Source = ImageSource.FromUri(new Uri(Internal.Connection.GetCaveBackgroundImageURL(cave.caveTypeID)))
            };

            caveSketchStack.Children.Add(caveBackground);

            caveSketchFrame.Content = caveSketchStack;
            contentStack.Children.Add(caveSketchFrame);

            Frame notesFrame = new Frame
            {
                BackgroundColor = Color.White,
                HasShadow = true
            };
            StackLayout notesStack = new StackLayout();

            Label notesLabel = new Label
            {
                TextColor = Color.Black,
                FontSize = 20,
                Text = "Private Notes"
            };
            notesStack.Children.Add(notesLabel);

            notesEditor = new Editor
            {
                BackgroundColor = Color.White,
                HeightRequest = 100
            };
            var index = Settings.SavedNotesSetting.FindIndex(c => c.ID == cave.caveID && c.Type == NotesSaver.NOTES_TYPE.NOTE_TYPE_CAVE);
            if (index != -1) notesEditor.Text = Settings.SavedNotesSetting[index].Note;
            notesStack.Children.Add(notesEditor);
            notesFrame.Content = notesStack;
            contentStack.Children.Add(notesFrame);

            ScrollView scrollView = new ScrollView
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                Content = contentStack
            };
            Content = scrollView;
        }

        protected override void OnDisappearing()
        {
            Kucha.SaveCaveNotes(cave.caveID, notesEditor.Text);
            base.OnDisappearing();
        }
    }
}