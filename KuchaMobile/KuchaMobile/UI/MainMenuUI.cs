using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace KuchaMobile.UI
{
    public class MainMenuUI : ContentPage
    {
        /// <summary>
        /// Draws the main menu appearing on the left side
        /// </summary>
        public ListView ListView { get; }

        public MainMenuUI()
        {
            var masterPageItems = new List<MasterPageItem>
            {
                new MasterPageItem
                {
                    Title = "Cave",
                    IconSource = "cave.png",
                    TargetType = typeof(CaveSearchUI)
                },
                new MasterPageItem
                {
                    Title = "Painted Representation",
                    IconSource = "image.png",
                    TargetType = typeof(PaintedRepresentationSearchUI)
                },
                new MasterPageItem
                {
                    Title = "Settings",
                    IconSource = "cog.png",
                    TargetType = typeof(SettingsUI)
                }
            };

            ListView = new ListView
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                ItemsSource = masterPageItems,
                ItemTemplate = new DataTemplate(() =>
                {
                    var grid = new Grid { Padding = new Thickness(5, 10) };
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(30) });
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });

                    var image = new Image();
                    image.SetBinding(Image.SourceProperty, "IconSource");
                    var label = new Label { VerticalOptions = LayoutOptions.FillAndExpand };
                    label.FontAttributes = FontAttributes.Bold;
                    label.SetBinding(Label.TextProperty, "Title");

                    grid.Children.Add(image);
                    grid.Children.Add(label, 1, 0);

                    return new ViewCell { View = grid };
                }),
                SeparatorVisibility = SeparatorVisibility.Default
            };

            Icon = "null";
            Title = "Kucha Mobile";
            Frame imageFrame = new Frame
            {
                BackgroundColor = Color.White,
                Padding = 20,
                HasShadow = true
            };

            imageFrame.Content = new Image
            {
                Source = "SAW_logo.png",
                Aspect = Aspect.AspectFit
            };

            Label statusLabel = new Label
            {
                TextColor = Color.LightGray,
                HorizontalOptions = LayoutOptions.Center,
                Margin = new Thickness(0, 0, 0, 10)
            };
            if (Internal.Connection.IsInOfflineMode())
                statusLabel.Text = "App in offline mode";
            else
                statusLabel.Text = "App in online mode";
            Content = new StackLayout
            {
                Children = { imageFrame, ListView, statusLabel }
            };
        }
    }

    public class MasterPageItem
    {
        public string Title { get; set; }

        public string IconSource { get; set; }

        public Type TargetType { get; set; }
    }
}