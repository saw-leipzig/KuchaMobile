using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace KuchaMobile.UI
{
    public class LoadingScreenUI : ContentPage
    {
        public LoadingScreenUI()
        {
            Label textLabel = new Label();
            textLabel.Text = "Kucha Mobile is loading...";
            textLabel.FontSize = 30;
            textLabel.TextColor = Color.Black;
            textLabel.HorizontalOptions = LayoutOptions.Center;
            textLabel.VerticalOptions = LayoutOptions.Center;
            StackLayout contentStack = new StackLayout();
            contentStack.BackgroundColor = Color.White;
            contentStack.Children.Add(textLabel);

            Content = contentStack;
        }
    }
}
