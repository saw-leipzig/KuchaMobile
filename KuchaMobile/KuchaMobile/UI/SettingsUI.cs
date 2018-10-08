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
        /// <summary>
        /// UI for the Settings Page
        /// </summary>
        private readonly Label downloadStatusLabel;

        public SettingsUI()
        {
            Title = "Settings";
            StackLayout contentStack = new StackLayout
            {
                Padding = 16
            };

            Grid previewPicGrid = new Grid();
            Label previewPicLabel = new Label
            {
                Text = "Enable Preview Images in Painted Representations"
            };
            Switch previewPicSwitch = new Switch
            {
                IsToggled = Settings.ShowPreviewPicturesSetting
            };
            previewPicSwitch.Toggled += PreviewPicSwitch_Toggled;
            previewPicGrid.Children.Add(previewPicLabel, 0, 0);
            previewPicGrid.Children.Add(previewPicSwitch, 1, 0);
            previewPicGrid.HorizontalOptions = LayoutOptions.CenterAndExpand;
            contentStack.Children.Add(previewPicGrid);

            if (Connection.IsInOfflineMode())
            {
                Button showLoginScreenButton = new Button
                {
                    Text = "Go to Login Screen"
                };
                showLoginScreenButton.Clicked += ShowLoginScreenButton_Clicked;
                contentStack.Children.Add(showLoginScreenButton);
            }

            downloadStatusLabel = new Label
            {
                Text = "Data from " + Kucha.GetDataTimeStamp().ToShortDateString()
            };
            contentStack.Children.Add(downloadStatusLabel);

            Button updateLocalDatabaseButton = new Button
            {
                Text = "Update Database"
            };
            updateLocalDatabaseButton.Clicked += UpdateLocalDatabaseButton_Clicked;
            contentStack.Children.Add(updateLocalDatabaseButton);

            Button clearAllNotesButton = new Button
            {
                Text = "Clear private notes"
            };
            clearAllNotesButton.Clicked += ClearAllNotesButton_Clicked;
            contentStack.Children.Add(clearAllNotesButton);

            Button deleteLocalFilesButton = new Button
            {
                Text = "Delete all local files"
            };
            deleteLocalFilesButton.Clicked += DeleteLocalFilesButton_Clicked;
            contentStack.Children.Add(deleteLocalFilesButton);

            Label licenseLabel = new Label
            {
                TextColor = Color.LightGray,
                Text = "Icon licenses\n\ncave by Alexander Skowalsky from the Noun Project\nImage by Viktor Vorobyev from the Noun Project\ncog by Vicons Design from the Noun Project\n\nAll icons issued under Creative Commons license.",
                Margin = new Thickness(0, 50, 0, 0)
            };
            contentStack.Children.Add(licenseLabel);
            Content = contentStack;
        }

        private void PreviewPicSwitch_Toggled(object sender, ToggledEventArgs e)
        {
            Settings.ShowPreviewPicturesSetting = e.Value;
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