using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace KuchaMobile.UI
{
    public class MainMenuUI : ContentPage
    {
        public ListView ListView { get { return listView; } }

        private ListView listView;

        public MainMenuUI()
        {
            var masterPageItems = new List<MasterPageItem>();
            masterPageItems.Add(new MasterPageItem
            {
                Title = "Cave",
                IconSource = "cave.png",
                TargetType = typeof(CaveSearchUI)
            });
            masterPageItems.Add(new MasterPageItem
            {
                Title = "Painted Representation",
                IconSource = "image.png",
                TargetType = typeof(PaintedRepresentationSearchUI)
            });
            masterPageItems.Add(new MasterPageItem
            {
                Title = "Settings",
                IconSource = "cog.png",
                TargetType = typeof(SettingsUI)
            });

            listView = new ListView
            {
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
            Frame imageFrame = new Frame();
            imageFrame.BackgroundColor = Color.White;
            imageFrame.Padding = 20;
            imageFrame.HasShadow = true;

            Image huLogo = new Image();
            huLogo.Source = "hu_logo.png";
            huLogo.Aspect = Aspect.AspectFit;
            imageFrame.Content = huLogo;
            Content = new StackLayout
            {
                Children = { imageFrame, listView }
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