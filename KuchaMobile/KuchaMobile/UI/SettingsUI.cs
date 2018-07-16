using Acr.UserDialogs;
using KuchaMobile.Internal;
using KuchaMobile.Logic;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace KuchaMobile.UI
{
    public class SettingsUI : ContentPage
    {
        private Label downloadStatusLabel;

        public SettingsUI()
        {
            Title = "Settings";
            StackLayout contentStack = new StackLayout();
            contentStack.Padding = 16;
            downloadStatusLabel = new Label();
            downloadStatusLabel.Text = "Data from " + Kucha.GetDataTimeStamp().ToShortDateString();
            contentStack.Children.Add(downloadStatusLabel);

            if(Connection.IsInOfflineMode())
            {
                Button showLoginScreenButton = new Button();
                showLoginScreenButton.Text = "Go to Login Screen";
                showLoginScreenButton.Clicked += ShowLoginScreenButton_Clicked;
                contentStack.Children.Add(showLoginScreenButton);
            }

            Button updateLocalDatabaseButton = new Button();
            updateLocalDatabaseButton.Text = "Update Database";
            updateLocalDatabaseButton.Clicked += UpdateLocalDatabaseButton_Clicked;
            contentStack.Children.Add(updateLocalDatabaseButton);

            Button clearAllNotesButton = new Button();
            clearAllNotesButton.Text = "Clear private notes";
            clearAllNotesButton.Clicked += ClearAllNotesButton_Clicked;
            contentStack.Children.Add(clearAllNotesButton);

            Button deleteLocalFilesButton = new Button();
            deleteLocalFilesButton.Text = "Delete all local files";
            deleteLocalFilesButton.Clicked += DeleteLocalFilesButton_Clicked;
            contentStack.Children.Add(deleteLocalFilesButton);

            Label licenseLabel = new Label();
            licenseLabel.TextColor = Color.LightGray;
            licenseLabel.Text = "Icon licenses\n\ncave by Alexander Skowalsky from the Noun Project\nImage by Viktor Vorobyev from the Noun Project\ncog by Vicons Design from the Noun Project\n\nAll icons issued under Creative Commons license.";
            licenseLabel.Margin = new Thickness(0, 50, 0, 0);
            contentStack.Children.Add(licenseLabel);
            Content = contentStack;
        }

        private void ShowLoginScreenButton_Clicked(object sender, EventArgs e)
        {
            App.Current.MainPage = new LoginPage();
        }

        private void ClearAllNotesButton_Clicked(object sender, EventArgs e)
        {
            Settings.SavedNotesSetting = new List<NotesSaver>();
            UserDialogs.Instance.Toast("Successfully cleard private notes!");
        }

        private async void UpdateLocalDatabaseButton_Clicked(object sender, EventArgs e)
        {
            UserDialogs.Instance.ShowLoading("Downloading Data...");
            await Task.Run(async () =>
            {
                bool success = await Kucha.RefreshLocalData();
                Device.BeginInvokeOnMainThread(() =>
                {
                    UserDialogs.Instance.HideLoading();
                    if (success)
                    {
                        UserDialogs.Instance.Toast("Download successful!");
                        downloadStatusLabel.Text = "Data from " + Kucha.GetDataTimeStamp().ToShortDateString();
                    }
                    else
                    {
                        UserDialogs.Instance.Toast("Download failed. Please check connectivity.");
                    }
                });
            });
        }

        private void DeleteLocalFilesButton_Clicked(object sender, EventArgs e)
        {
            Kucha.RemoveAllData();
            UserDialogs.Instance.Toast("All local files deleted!");
        }
    }
}