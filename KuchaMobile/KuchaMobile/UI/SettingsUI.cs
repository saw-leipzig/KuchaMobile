using Acr.UserDialogs;
using KuchaMobile.Internal;
using KuchaMobile.Logic;
using PCLStorage;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace KuchaMobile.UI
{
    public class SettingsUI : ContentPage
    {
        Label downloadStatusLabel;
        public SettingsUI()
        {
            Title = "Einstellungen";
            StackLayout contentStack = new StackLayout();
            contentStack.Padding = 20;
            downloadStatusLabel = new Label();
            downloadStatusLabel.Text = "Daten vom " + Kucha.GetDataTimeStamp().ToShortDateString();
            contentStack.Children.Add(downloadStatusLabel);

            Button updateLocalDatabaseButton = new Button();
            updateLocalDatabaseButton.Text = "Datenbank updaten";
            updateLocalDatabaseButton.Clicked += UpdateLocalDatabaseButton_Clicked;
            contentStack.Children.Add(updateLocalDatabaseButton);

            Button clearAllNotesButton = new Button();
            clearAllNotesButton.Text = "Eigene Notizen löschen";
            clearAllNotesButton.Clicked += ClearAllNotesButton_Clicked;
            contentStack.Children.Add(clearAllNotesButton);

            Button deleteLocalFilesButton = new Button();
            deleteLocalFilesButton.Text = "Delete All Local Files";
            deleteLocalFilesButton.Clicked += DeleteLocalFilesButton_Clicked;
            contentStack.Children.Add(deleteLocalFilesButton);

            Content = contentStack;
        }

        private void ClearAllNotesButton_Clicked(object sender, EventArgs e)
        {
            Settings.SavedNotesSetting = new List<NotesSaver>();
            UserDialogs.Instance.Toast("Eigene Notizen zurückgesetzt!");
        }

        private async void UpdateLocalDatabaseButton_Clicked(object sender, EventArgs e)
        {
            UserDialogs.Instance.ShowLoading("Daten werden heruntergeladen...");
            await Task.Run(async () =>
            {
                bool success = await Kucha.RefreshCaveData();
                Device.BeginInvokeOnMainThread(() =>
                {
                    UserDialogs.Instance.HideLoading();
                    if (success)
                    {
                        UserDialogs.Instance.Toast("Daten erfolgreich heruntergeladen!");
                        downloadStatusLabel.Text = "Daten vom " + Kucha.GetDataTimeStamp().ToShortDateString();
                    }
                    else
                    {
                        UserDialogs.Instance.Toast("Daten konnten nicht heruntergeladen werden. Bitte Netzwerkverbindung prüfen.");
                    }
                });
            });
        }

        private void DeleteLocalFilesButton_Clicked(object sender, EventArgs e)
        {
            Kucha.RemoveAllData();
            UserDialogs.Instance.Toast("Alle lokalen Daten gelöscht!");
        }
    }
}
