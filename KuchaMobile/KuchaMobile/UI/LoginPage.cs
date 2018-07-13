using Acr.UserDialogs;
using KuchaMobile.Internal;
using KuchaMobile.Logic;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace KuchaMobile.UI
{
    public class LoginPage : ContentPage
    {
        private Entry nameEntry;
        private Entry passwordEntry;
        private Label loginstatusLabel;
        private Label downloadStatusLabel;
        private Button loginButton;
        private Button downloadDataButton;
        private Button continueButton;

        public LoginPage()
        {
            StackLayout contentStack = new StackLayout();
            contentStack.Padding = 16;
            Image huLogo = new Image();
            huLogo.Source = "hu_logo.png";
            huLogo.HeightRequest = 100;
            huLogo.Aspect = Aspect.AspectFit;
            contentStack.Children.Add(huLogo);
            nameEntry = new Entry();
            nameEntry.Placeholder = "Username";
            if (Connection.HasLegitSessionID())
                nameEntry.IsEnabled = false;
            contentStack.Children.Add(nameEntry);

            passwordEntry = new Entry();
            passwordEntry.Placeholder = "Password";
            passwordEntry.IsPassword = true;
            if (Connection.HasLegitSessionID())
                passwordEntry.IsEnabled = false;
            contentStack.Children.Add(passwordEntry);

            loginstatusLabel = new Label();
            loginstatusLabel.Margin = new Thickness(0, 10, 0, 0);
            loginstatusLabel.TranslationY += 10;
            if (Connection.HasLegitSessionID())
                loginstatusLabel.Text = "Logged in with valid session";
            else
                loginstatusLabel.Text = "Please log in";
            contentStack.Children.Add(loginstatusLabel);

            loginButton = new Button();
            loginButton.Clicked += LoginButton_Clicked;

            if (Connection.HasLegitSessionID())
                loginButton.IsEnabled = false;
            loginButton.Text = "Login";
            contentStack.Children.Add(loginButton);

            downloadStatusLabel = new Label();
            downloadStatusLabel.Margin = new Thickness(0, 10, 0, 0);
            downloadStatusLabel.TranslationY += 10;
            if (!Kucha.CaveDataIsValid())
                downloadStatusLabel.Text = "Please download initial data";
            else
                downloadStatusLabel.Text = "Data from " + Kucha.GetDataTimeStamp().ToShortDateString();
            contentStack.Children.Add(downloadStatusLabel);

            downloadDataButton = new Button();
            downloadDataButton.Clicked += DownloadDataButton_Clicked;
            downloadDataButton.Text = "Download Data";
            if (Connection.HasLegitSessionID() == false)
                downloadDataButton.IsEnabled = false;
            contentStack.Children.Add(downloadDataButton);

            continueButton = new Button();
            continueButton.Margin = new Thickness(0, 10, 0, 0);
            continueButton.Text = "Continue";
            continueButton.Clicked += ContinueButton_Clicked;
            if (!Connection.HasLegitSessionID() || !Kucha.CaveDataIsValid())
                continueButton.IsEnabled = false;
            contentStack.Children.Add(continueButton);

            ScrollView s = new ScrollView();
            s.Content = contentStack;
            Content = s;
        }

        private void ContinueButton_Clicked(object sender, EventArgs e)
        {
            App.Current.MainPage = new MainPage();
        }

        private async void DownloadDataButton_Clicked(object sender, EventArgs e)
        {
            UserDialogs.Instance.ShowLoading("Downloading Data...");
            await Task.Run(async () =>
            {
                bool success = await Kucha.RefreshCaveData();
                Device.BeginInvokeOnMainThread(() =>
                {
                    UserDialogs.Instance.HideLoading();
                    if (success)
                    {
                        UserDialogs.Instance.Toast("Data successfully downloaded!");
                        downloadDataButton.IsEnabled = false;
                        downloadStatusLabel.Text = "Data from " + Kucha.GetDataTimeStamp().ToShortDateString();
                        if (!loginButton.IsEnabled == true)
                        {
                            continueButton.IsEnabled = true;
                        }
                    }
                    else
                    {
                        UserDialogs.Instance.Toast("Data download failed. Please check your connectivity.");
                    }
                });
            });
        }

        private void LoginButton_Clicked(object sender, EventArgs e)
        {
            if(String.IsNullOrEmpty(nameEntry.Text) || String.IsNullOrEmpty(passwordEntry.Text))
            {
                UserDialogs.Instance.Toast("Please enter a username and a password");
                return;
            }
            UserDialogs.Instance.ShowLoading();
            Task.Run(() =>
            {
                var loginSuccess = Connection.Login(nameEntry.Text, passwordEntry.Text);
                Device.BeginInvokeOnMainThread(() =>
                {
                    UserDialogs.Instance.HideLoading();
                    if (loginSuccess == Connection.LOGIN_STATUS.SUCCESS)
                    {
                        UserDialogs.Instance.Toast("Login successful!");
                        nameEntry.IsEnabled = false;
                        passwordEntry.IsEnabled = false;
                        loginButton.IsEnabled = false;
                        downloadDataButton.IsEnabled = true;

                        if (Kucha.CaveDataIsValid())
                        {
                            continueButton.IsEnabled = true;
                        }
                    }
                    else if(loginSuccess == Connection.LOGIN_STATUS.OFFLINE)
                    {
                        if(Kucha.CaveDataIsValid())
                        {
                            UserDialogs.Instance.Toast("No Connection! Functionality is restricted!");
                            continueButton.IsEnabled = true;
                        }
                        else
                        {
                            UserDialogs.Instance.Toast("No Connection! Login failed!");
                        }
                    }
                    else
                    {
                        UserDialogs.Instance.Toast("Login failed! Wrong username/password!");
                    }
                });
            });
        }
    }
}