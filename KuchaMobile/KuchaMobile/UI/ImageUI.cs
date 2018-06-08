using KuchaMobile.Internal;
using KuchaMobile.Logic.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace KuchaMobile.UI
{
    public class ImageUI : ContentPage
    {
        public ImageUI(RelatedImage image)
        {
            Title = "Image " + image.shortName;
            StackLayout imageLayout = new StackLayout();
            Image displayImage = new Image();
            displayImage.Source = ImageSource.FromUri(new Uri(Connection.GetPaintedRepresentationImageURL(image.imageID, Helper.ScreenHeight)));
            displayImage.Aspect = Aspect.AspectFill;
            imageLayout.Children.Add(displayImage);
            imageLayout.Padding = 0;
            imageLayout.Spacing = 0;

            ScrollView imageScrollView = new ScrollView();
            imageScrollView.Content = imageLayout;
            Content = imageScrollView;
        }
    }
}
