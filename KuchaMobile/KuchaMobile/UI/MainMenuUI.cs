using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace KuchaMobile.UI
{
    public class MainMenuUI : ContentPage
    {
        public ListView ListView { get { return listView; } }

        ListView listView;

        public MainMenuUI()
        {
            var masterPageItems = new List<MasterPageItem>();
            masterPageItems.Add(new MasterPageItem
            {
                Title = "Cave",
                //IconSource = "contacts.png",
                TargetType = typeof(CaveSearchUI)
            });
            masterPageItems.Add(new MasterPageItem
            {
                Title = "Painted Representation",
               // IconSource = "todo.png",
                TargetType = typeof(PaintedRepresentationSearchUI)
            });
            masterPageItems.Add(new MasterPageItem
            {
                Title = "Settings",
                //IconSource = "reminders.png",
                TargetType = typeof(SettingsUI)
            });

            listView = new ListView
            {
                ItemsSource = masterPageItems,
                ItemTemplate = new DataTemplate(() => {
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

            Content = new StackLayout
            {
                Children = { listView }
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
