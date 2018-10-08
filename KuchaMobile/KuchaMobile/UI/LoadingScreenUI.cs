using Xamarin.Forms;

namespace KuchaMobile.UI
{
    public class LoadingScreenUI : ContentPage
    {
        /// <summary>
        /// Shows a loading screen while the local files are being loaded during app launch
        /// </summary>
        public LoadingScreenUI()
        {
            Image huLogo = new Image
            {
                VerticalOptions = LayoutOptions.Center,
                Source = "SAW_logo.png",
                Aspect = Aspect.AspectFit,
                HeightRequest = 200
            };

            Label textLabel = new Label
            {
                Text = "Kucha Mobile is loading...",
                FontSize = 24,
                TextColor = Color.Black,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            };
            ActivityIndicator activityIndicator = new ActivityIndicator
            {
                IsRunning = true
            };
            StackLayout contentStack = new StackLayout
            {
                Padding = 16,
                BackgroundColor = Color.White
            };
            contentStack.Children.Add(huLogo);
            contentStack.Children.Add(textLabel);
            contentStack.Children.Add(activityIndicator);

            Content = contentStack;
        }
    }
}