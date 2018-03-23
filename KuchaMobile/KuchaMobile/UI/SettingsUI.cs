using Acr.UserDialogs;
using KuchaMobile.Internal;
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

            Content = contentStack;
        }

        private async void LoginButton_Clicked(object sender, EventArgs e)
        {
            LoginResult x = await UserDialogs.Instance.LoginAsync("Login", "Logindaten eintragen");
            if(x.Ok)
            {
                string username = x.LoginText;
                string password = x.Password;

                bool loginSuccess = Connection.Login(username, password);
                UserDialogs.Instance.Toast("Login: " + loginSuccess.ToString());
            }
        }
    }
}
