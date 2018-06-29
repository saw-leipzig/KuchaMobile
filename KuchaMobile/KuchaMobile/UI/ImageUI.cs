using KuchaMobile.Internal;
using KuchaMobile.Logic.Models;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace KuchaMobile.UI
{
    public class ImageUI : ContentPage
    {
        private Editor notesEditor;
        private RelatedImage image;

        public ImageUI(RelatedImage image)
        {
            this.image = image;
            Title = "Image " + image.shortName;
            StackLayout imageLayout = new StackLayout();
            Image displayImage = new Image();
            displayImage.Source = ImageSource.FromUri(new Uri(Connection.GetPaintedRepresentationImageURL(image.imageID, Helper.ScreenHeight)));
            displayImage.Aspect = Aspect.AspectFill;
            imageLayout.Children.Add(displayImage);
            imageLayout.Padding = new Thickness(0, 10, 0, 20);
            imageLayout.Spacing = 10;

            Frame editorFrame = new Frame();
            editorFrame.HasShadow = true;
            editorFrame.BackgroundColor = Color.White;
            StackLayout editorStack = new StackLayout();
            Label notesLabel = new Label();
            notesLabel.Text = "Private Notizen";
            notesLabel.TextColor = Color.Black;
            notesLabel.FontSize = 20;
            editorStack.Children.Add(notesLabel);

            notesEditor = new Editor();
            notesEditor.BackgroundColor = Color.White;
            notesEditor.HeightRequest = 100;
            var index = Settings.SavedNotesSetting.FindIndex(i => i.ID == image.imageID && i.Type == NotesSaver.NOTES_TYPE.NOTES_TYPE_IMAGE);
            if (index != -1) notesEditor.Text = Settings.SavedNotesSetting[index].Note;
            editorStack.Children.Add(notesEditor);
            editorFrame.Content = editorStack;
            imageLayout.Children.Add(editorFrame);

            ScrollView imageScrollView = new ScrollView();
            imageScrollView.Content = imageLayout;
            Content = imageScrollView;
        }

        protected override void OnDisappearing()
        {
            var index = Settings.SavedNotesSetting.FindIndex(i => i.ID == image.imageID && i.Type == NotesSaver.NOTES_TYPE.NOTES_TYPE_IMAGE);
            if (index == -1)
            {
                if (!String.IsNullOrEmpty(notesEditor.Text))
                {
                    List<NotesSaver> savedNotes = Settings.SavedNotesSetting;
                    savedNotes.Add(new NotesSaver(NotesSaver.NOTES_TYPE.NOTES_TYPE_IMAGE, image.imageID, notesEditor.Text));
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