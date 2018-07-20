using KuchaMobile.Internal;
using KuchaMobile.Logic.Models;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace KuchaMobile.UI
{
    public class ImageUI : ContentPage
    {
        private readonly Editor notesEditor;
        private readonly RelatedImage image;

        double currentScale = 1;
        double startScale = 1;
        double xOffset = 0;
        double yOffset = 0;

        public ImageUI(RelatedImage image)
        {
            this.image = image;
            Title = "Image " + image.shortName;
            StackLayout imageLayout = new StackLayout();
            Image displayImage = new Image
            {
                Source = ImageSource.FromUri(new Uri(Connection.GetPaintedRepresentationImageURL(image.imageID, Helper.ScreenHeight))),
                Aspect = Aspect.AspectFill
            };
            imageLayout.Children.Add(displayImage);
            imageLayout.Padding = new Thickness(0, 10, 0, 20);
            imageLayout.Spacing = 10;
            var pinchGesture = new PinchGestureRecognizer();
            pinchGesture.PinchUpdated += PinchGesture_PinchUpdated;
            imageLayout.GestureRecognizers.Add(pinchGesture);

            Frame editorFrame = new Frame
            {
                HasShadow = true,
                BackgroundColor = Color.White
            };
            StackLayout editorStack = new StackLayout();
            Label notesLabel = new Label
            {
                Text = "Private Notes",
                TextColor = Color.Black,
                FontSize = 20
            };
            editorStack.Children.Add(notesLabel);

            notesEditor = new Editor
            {
                BackgroundColor = Color.White,
                HeightRequest = 100
            };
            var index = Settings.SavedNotesSetting.FindIndex(i => i.ID == image.imageID && i.Type == NotesSaver.NOTES_TYPE.NOTE_TYPE_IMAGE);
            if (index != -1) notesEditor.Text = Settings.SavedNotesSetting[index].Note;
            editorStack.Children.Add(notesEditor);
            editorFrame.Content = editorStack;
            imageLayout.Children.Add(editorFrame);

            ScrollView imageScrollView = new ScrollView
            {
                Content = imageLayout
            };
            Content = imageScrollView;
        }

        private void PinchGesture_PinchUpdated(object sender, PinchGestureUpdatedEventArgs e)
        {
            //From: https://vicenteguzman.mx/2018/07/11/how-to-zoom-in-images-xamarin-forms/
            if (e.Status == GestureStatus.Started)
            {
                startScale = Content.Scale;
                Content.AnchorX = 0;
                Content.AnchorY = 0;
            }
            if (e.Status == GestureStatus.Running)
            {
                currentScale += (e.Scale - 1) * startScale;
                currentScale = Math.Max(1, currentScale);

                double renderedX = Content.X + xOffset;
                double deltaX = renderedX / Width;
                double deltaWidth = Width / (Content.Width * startScale);
                double originX = (e.ScaleOrigin.X - deltaX) * deltaWidth;

                double renderedY = Content.Y + yOffset;
                double deltaY = renderedY / Height;
                double deltaHeight = Height / (Content.Height * startScale);
                double originY = (e.ScaleOrigin.Y - deltaY) * deltaHeight;

                double targetX = xOffset - (originX * Content.Width) * (currentScale - startScale);
                double targetY = yOffset - (originY * Content.Height) * (currentScale - startScale);

                Content.TranslationX = targetX.Clamp(-Content.Width * (currentScale - 1), 0);
                Content.TranslationY = targetY.Clamp(-Content.Height * (currentScale - 1), 0);

                Content.Scale = currentScale;
            }
            if (e.Status == GestureStatus.Completed)
            {
                xOffset = Content.TranslationX;
                yOffset = Content.TranslationY;
            }
        }

        protected override void OnDisappearing()
        {
            var index = Settings.SavedNotesSetting.FindIndex(i => i.ID == image.imageID && i.Type == NotesSaver.NOTES_TYPE.NOTE_TYPE_IMAGE);
            if (index == -1)
            {
                if (!String.IsNullOrEmpty(notesEditor.Text))
                {
                    List<NotesSaver> savedNotes = Settings.SavedNotesSetting;
                    savedNotes.Add(new NotesSaver(NotesSaver.NOTES_TYPE.NOTE_TYPE_IMAGE, image.imageID, notesEditor.Text));
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