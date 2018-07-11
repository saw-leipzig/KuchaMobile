using Xamarin.Forms;

namespace KuchaMobile.UI
{
    public class LoadingScreenUI : ContentPage
    {
        public LoadingScreenUI()
        {
            Image huLogo = new Image();
            huLogo.VerticalOptions = LayoutOptions.Center;
            huLogo.Source = "hu_logo.png";
            huLogo.Aspect = Aspect.AspectFit;
            huLogo.HeightRequest = 200;

            Label textLabel = new Label();
            textLabel.Text = "Kucha Mobile is loading...";
            textLabel.FontSize = 24;
            textLabel.TextColor = Color.Black;
            textLabel.HorizontalOptions = LayoutOptions.Center;
            textLabel.VerticalOptions = LayoutOptions.Center;
            ActivityIndicator activityIndicator = new ActivityIndicator();
            activityIndicator.IsRunning = true;
            StackLayout contentStack = new StackLayout();
            contentStack.Padding = 16;
            contentStack.BackgroundColor = Color.White;
            contentStack.Children.Add(huLogo);
            contentStack.Children.Add(textLabel);
            contentStack.Children.Add(activityIndicator);

            Content = contentStack;
        }
    }
}