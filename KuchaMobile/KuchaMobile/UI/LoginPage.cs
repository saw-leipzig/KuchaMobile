using Acr.UserDialogs;
using KuchaMobile.Internal;
using KuchaMobile.Logic;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace KuchaMobile.UI
{
    public class LoginPage : ContentPage
    {
        Entry nameEntry;
        Entry passwordEntry;
        public LoginPage()
        {

            StackLayout contentStack = new StackLayout();
            nameEntry = new Entry();
            nameEntry.Placeholder = "Nutzername";
            contentStack.Children.Add(nameEntry);

            passwordEntry = new Entry();
            passwordEntry.Placeholder = "Passwort";
            passwordEntry.IsPassword = true;
            contentStack.Children.Add(passwordEntry);

            Button loginButton = new Button();
            loginButton.Clicked += LoginButton_Clicked;
            loginButton.Text = "Login";
            contentStack.Children.Add(loginButton);
            Content = contentStack;
        }

        private void LoginButton_Clicked(object sender, EventArgs e)
        {
            bool loginSuccess = Connection.Login(nameEntry.Text, passwordEntry.Text);
            if(loginSuccess)
            {
                UserDialogs.Instance.Toast("Login erfolgreich!");

                if (Kucha.InitializeDefaults())
                    App.Current.MainPage = new MainPage();
                else UserDialogs.Instance.Toast("Fehler beim Initialisieren!");
            }
            else
            {
                UserDialogs.Instance.Toast("Login fehlgeschlagen!");
            }
            
        }
    }
}
