using KuchaMobile.Logic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace KuchaMobile.UI
{
    public class PaintedRepresentationSearchUI : ContentPage
    {
        List<IconographyModel> allIconographies;
        List<IconographyModel> availableIconographies;
        List<IconographyModel> selectedIconographies;

        ListView availableIconsListView;
        ListView selectedListView;

        public PaintedRepresentationSearchUI()
        {
            Title = "Painted Representation Search";
            allIconographies = new List<IconographyModel>(KuchaMobile.Logic.Kucha.GetIconographies());
            availableIconographies = new List<IconographyModel>(allIconographies);
            selectedIconographies = new List<IconographyModel>();
            StackLayout contentStack = new StackLayout();
            Grid listGrid = new Grid();
            listGrid.HorizontalOptions = LayoutOptions.FillAndExpand;
            listGrid.VerticalOptions = LayoutOptions.FillAndExpand;
            listGrid.ColumnDefinitions = new ColumnDefinitionCollection
            {
                new ColumnDefinition{ Width = new GridLength(1, GridUnitType.Star) },
                new ColumnDefinition{ Width = new GridLength(1, GridUnitType.Star) }
            };
            listGrid.RowDefinitions = new RowDefinitionCollection
            {
                new RowDefinition { Height = new GridLength(1, GridUnitType.Star)},
                new RowDefinition { Height = new GridLength(9, GridUnitType.Star)}
            };

            Entry searchEntry = new Entry();
            searchEntry.Placeholder = "Hier suchen";
            searchEntry.TextChanged += SearchEntry_TextChanged;
            listGrid.Children.Add(searchEntry, 0, 0);

            availableIconsListView = new ListView();
            availableIconsListView.ItemTemplate = new DataTemplate(typeof(TextCell));
            availableIconsListView.ItemTemplate.SetBinding(TextCell.TextProperty, "text");
            availableIconsListView.ItemsSource = allIconographies;
            availableIconsListView.ItemTapped += AvailableIconsListView_ItemTapped;
            listGrid.Children.Add(availableIconsListView, 0, 1);


            Label infoLabel = new Label();
            infoLabel.HorizontalOptions = LayoutOptions.Center;
            infoLabel.Text = "Ausgewählte Iconographys";
            listGrid.Children.Add(infoLabel, 1, 0);

            selectedListView = new ListView();
            selectedListView.ItemTemplate = new DataTemplate(typeof(TextCell));
            selectedListView.ItemTemplate.SetBinding(TextCell.TextProperty, "text");
            selectedListView.ItemsSource = selectedIconographies;
            selectedListView.ItemTapped += SelectedListView_ItemTapped;
            listGrid.Children.Add(selectedListView, 1, 1);

            contentStack.Children.Add(listGrid);

            Button anySearchButton = new Button();
            anySearchButton.Text = "Suchen";
            anySearchButton.Clicked += AnySearchButton_Clicked;
            contentStack.Children.Add(anySearchButton);

            Button exclusiveSearchButton = new Button();
            exclusiveSearchButton.Text = "Exklusiv Suche";
            exclusiveSearchButton.Clicked += ExclusiveSearchButton_Clicked;
            contentStack.Children.Add(exclusiveSearchButton);

            Content = contentStack;
        }

        private void SelectedListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            IconographyModel iconographyModel = e.Item as IconographyModel;
            List<IconographyModel> newSelected = new List<IconographyModel>(selectedIconographies);
            newSelected.Remove(iconographyModel);
            List<IconographyModel> newAvailable = new List<IconographyModel>(availableIconographies);
            newAvailable.Add(iconographyModel);
            newAvailable.Sort((x, y) => x.text.CompareTo(y.text));
            availableIconsListView.ItemsSource = newAvailable;
            selectedListView.ItemsSource = newSelected;
            selectedIconographies = newSelected;
            availableIconographies = newAvailable;
        }

        private void AvailableIconsListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            IconographyModel iconographyModel = e.Item as IconographyModel;
            List<IconographyModel> newSelected = new List<IconographyModel>(selectedIconographies);
            newSelected.Add(iconographyModel);
            List<IconographyModel> newAvailable = new List<IconographyModel>(availableIconographies);
            newAvailable.Remove(iconographyModel);
            newSelected.Sort((x, y) => x.text.CompareTo(y.text));
            availableIconsListView.ItemsSource = newAvailable;
            selectedListView.ItemsSource = newSelected;
            selectedIconographies = newSelected;
            availableIconographies = newAvailable;
        }

        private void ExclusiveSearchButton_Clicked(object sender, EventArgs e)
        {
            List<PaintedRepresentationModel> paintedRepresentationModels = Logic.Kucha.GetPaintedRepresentationsByIconographies(selectedIconographies, true);
            Navigation.PushAsync(new PaintedRepresentationResultUI(paintedRepresentationModels), true);
        }

        private void AnySearchButton_Clicked(object sender, EventArgs e)
        {
            List<PaintedRepresentationModel> paintedRepresentationModels = Logic.Kucha.GetPaintedRepresentationsByIconographies(selectedIconographies, false);
            Navigation.PushAsync(new PaintedRepresentationResultUI(paintedRepresentationModels), true);
        }

        private void SearchEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            List<IconographyModel> editedList = new List<IconographyModel>();
            foreach(IconographyModel i in allIconographies)
            {
                if(i.text.ToLower().Contains(e.NewTextValue.ToLower()))
                {
                    editedList.Add(i);
                }
            }
            availableIconsListView.ItemsSource = editedList;
        }
    }
}
