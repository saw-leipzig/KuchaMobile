using Acr.UserDialogs;
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
        Switch exclusiveSwitch;

        public PaintedRepresentationSearchUI()
        {
            Title = "Painted Representation Search";
            allIconographies = new List<IconographyModel>(KuchaMobile.Logic.Kucha.GetIconographies());
            availableIconographies = new List<IconographyModel>(allIconographies);
            selectedIconographies = new List<IconographyModel>();
            StackLayout contentStack = new StackLayout();
            contentStack.Padding = new Thickness(20, 10, 20, 10);
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
            contentStack.Children.Add(searchEntry);

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

            Label infoLabel2 = new Label();
            infoLabel2.HorizontalOptions = LayoutOptions.Center;
            infoLabel2.Text = "Verfügbare Iconographys";
            listGrid.Children.Add(infoLabel2, 0, 0);

            selectedListView = new ListView();
            selectedListView.ItemTemplate = new DataTemplate(typeof(TextCell));
            selectedListView.ItemTemplate.SetBinding(TextCell.TextProperty, "text");
            selectedListView.ItemsSource = selectedIconographies;
            selectedListView.ItemTapped += SelectedListView_ItemTapped;
            listGrid.Children.Add(selectedListView, 1, 1);

            contentStack.Children.Add(listGrid);

            StackLayout exclusiveSearchStack = new StackLayout();
            exclusiveSearchStack.HorizontalOptions = LayoutOptions.Center;
            exclusiveSearchStack.Orientation = StackOrientation.Horizontal;
            Label exclusiveLabel = new Label();
            exclusiveLabel.Text = "Exklusive Suche";
            exclusiveSearchStack.Children.Add(exclusiveLabel);

            exclusiveSwitch = new Switch();
            exclusiveSwitch.IsToggled = false;
            exclusiveSearchStack.Children.Add(exclusiveSwitch);
            contentStack.Children.Add(exclusiveSearchStack);

            Button searchButton = new Button();
            searchButton.WidthRequest = 150;
            searchButton.BackgroundColor = Color.FromHex("2196f3");
            searchButton.TextColor = Color.White;
            searchButton.HorizontalOptions = LayoutOptions.Center;
            searchButton.Text = "Suchen";
            searchButton.Clicked += SearchButton_Clicked;
            contentStack.Children.Add(searchButton);

            Content = contentStack;
        }

        private void SearchButton_Clicked(object sender, EventArgs e)
        {
            if(exclusiveSwitch.IsToggled)
            {
                if (selectedIconographies.Any())
                {
                    List<PaintedRepresentationModel> paintedRepresentationModels = Logic.Kucha.GetPaintedRepresentationsByIconographies(selectedIconographies, true);
                    Navigation.PushAsync(new PaintedRepresentationResultUI(paintedRepresentationModels), true);
                }
                else
                {
                    List<PaintedRepresentationModel> paintedRepresentationModels = Logic.Kucha.GetAllPaintedRepresentations();
                    Navigation.PushAsync(new PaintedRepresentationResultUI(paintedRepresentationModels), true);
                }
            }
            else
            {
                if (selectedIconographies.Any())
                {
                    List<PaintedRepresentationModel> paintedRepresentationModels = Logic.Kucha.GetPaintedRepresentationsByIconographies(selectedIconographies, false);
                    if (paintedRepresentationModels == null)
                        UserDialogs.Instance.Toast("Fehler");
                    else
                        Navigation.PushAsync(new PaintedRepresentationResultUI(paintedRepresentationModels), true);
                }
                else
                {
                    List<PaintedRepresentationModel> paintedRepresentationModels = Logic.Kucha.GetAllPaintedRepresentations();
                    if (paintedRepresentationModels == null)
                        UserDialogs.Instance.Toast("Fehler");
                    else
                        Navigation.PushAsync(new PaintedRepresentationResultUI(paintedRepresentationModels), true);
                }
            }
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
