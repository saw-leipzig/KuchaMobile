using Acr.UserDialogs;
using KuchaMobile.Internal;
using KuchaMobile.Logic;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace KuchaMobile.UI
{
    public class LoginPage : ContentPage
    {
        Entry nameEntry;
        Entry passwordEntry;
        Label loginstatusLabel;
        Label downloadStatusLabel;
        Button loginButton;
        Button downloadDataButton;
        Button continueButton;

        public LoginPage()
        {

            StackLayout contentStack = new StackLayout();
            nameEntry = new Entry();
            nameEntry.Placeholder = "Nutzername";
            if (Connection.HasLegitSessionID())
                nameEntry.IsEnabled = false;
            contentStack.Children.Add(nameEntry);

            passwordEntry = new Entry();
            passwordEntry.Placeholder = "Passwort";
            passwordEntry.IsPassword = true;
            if (Connection.HasLegitSessionID())
                passwordEntry.IsEnabled = false;
            contentStack.Children.Add(passwordEntry);

            loginButton = new Button();
            loginButton.Clicked += LoginButton_Clicked;
            if (Connection.HasLegitSessionID())
                loginButton.IsEnabled = false;
            loginButton.Text = "Login";
            contentStack.Children.Add(loginButton);

            loginstatusLabel = new Label();
            if (Connection.HasLegitSessionID())
                loginstatusLabel.Text = "Eingeloggt mit gültiger Sitzung";
            else
                loginstatusLabel.Text = "Bitte logge dich ein";
            contentStack.Children.Add(loginstatusLabel);

            downloadDataButton = new Button();
            downloadDataButton.Clicked += DownloadDataButton_Clicked;
            downloadDataButton.Text = "Daten runterladen";
            contentStack.Children.Add(downloadDataButton);

            downloadStatusLabel = new Label();
            if (Kucha.CaveDataIsValid())
                downloadStatusLabel.Text = "Bitte initiale Daten downloaden";
            contentStack.Children.Add(downloadStatusLabel);

            continueButton = new Button();
            continueButton.Text = "Fortfahren";
            continueButton.Clicked += ContinueButton_Clicked;
            if (!Connection.HasLegitSessionID() || !Kucha.CaveDataIsValid())
                continueButton.IsEnabled = false;
            contentStack.Children.Add(continueButton);

            Content = contentStack;
        }

        private void ContinueButton_Clicked(object sender, EventArgs e)
        {
            App.Current.MainPage = new MainPage();
        }

        private async void DownloadDataButton_Clicked(object sender, EventArgs e)
        {
            UserDialogs.Instance.ShowLoading("Daten werden heruntergeladen...");
            await Task.Run(() =>
            {
                bool success = Kucha.RefreshCaveData();
                Device.BeginInvokeOnMainThread(() =>
                {
                    UserDialogs.Instance.HideLoading();
                    if(success)
                    {
                        UserDialogs.Instance.Toast("Daten erfolgreich heruntergeladen!");
                        downloadDataButton.IsEnabled = false;
                        if(loginButton.IsEnabled == true)
                        {
                            continueButton.IsEnabled = true;
                        }
                    }else
                    {
                        UserDialogs.Instance.Toast("Daten konnten nicht heruntergeladen werden. Bitte Netzwerkverbindung prüfen.");
                    }
                });
            });

        }

        private void LoginButton_Clicked(object sender, EventArgs e)
        {
            bool loginSuccess = Connection.Login(nameEntry.Text, passwordEntry.Text);
            if(loginSuccess)
            {
                UserDialogs.Instance.Toast("Login erfolgreich!");
                nameEntry.IsEnabled = false;
                passwordEntry.IsEnabled = false;
                loginButton.IsEnabled = false;

                if (Kucha.CaveDataIsValid())
                    continueButton.IsEnabled = true;
            }
            else
            {
                UserDialogs.Instance.Toast("Login fehlgeschlagen!");
            }
            
        }
    }
}
