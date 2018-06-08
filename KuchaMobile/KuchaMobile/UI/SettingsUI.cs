using Acr.UserDialogs;
using KuchaMobile.Internal;
using KuchaMobile.Logic;
using PCLStorage;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace KuchaMobile.UI
{
    public class SettingsUI : ContentPage
    {
        public SettingsUI()
        {
            StackLayout contentStack = new StackLayout();

            Button loginButton = new Button();
            loginButton.Text = "Login";
            loginButton.Clicked += LoginButton_Clicked;
            contentStack.Children.Add(loginButton);

            Button deleteLocalFilesButton = new Button();
            deleteLocalFilesButton.Text = "Delete Local Files";
            deleteLocalFilesButton.Clicked += DeleteLocalFilesButton_Clicked; ;
            contentStack.Children.Add(deleteLocalFilesButton);

            Content = contentStack;
        }

        private void DeleteLocalFilesButton_Clicked(object sender, EventArgs e)
        {
            Kucha.RemoveAllData();
        }

        private void LoginButton_Clicked(object sender, EventArgs e)
        {
            App.Current.MainPage = new LoginPage();
        }
    }
}
