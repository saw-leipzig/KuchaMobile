using Acr.UserDialogs;
using KuchaMobile.Logic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace KuchaMobile.UI
{
    public class PaintedRepresentationSearchUI : ContentPage
    {
        /// <summary>
        /// UI for the painted representation search
        /// </summary>
        private readonly List<IconographyModel> allIconographies;
        private List<IconographyModel> availableIconographies;
        private List<IconographyModel> selectedIconographies;

        private readonly ListView availableIconsListView;
        private readonly ListView selectedListView;
        private readonly Switch exclusiveSwitch;

        public PaintedRepresentationSearchUI()
        {
            Title = "Painted Representation Search";
            allIconographies = new List<IconographyModel>(KuchaMobile.Logic.Kucha.GetIconographies());
            availableIconographies = new List<IconographyModel>(allIconographies);
            selectedIconographies = new List<IconographyModel>();
            StackLayout contentStack = new StackLayout
            {
                Padding = new Thickness(16, 10, 16, 10)
            };
            Grid listGrid = new Grid
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                ColumnDefinitions = new ColumnDefinitionCollection
            {
                new ColumnDefinition{ Width = new GridLength(1, GridUnitType.Star) },
                new ColumnDefinition{ Width = new GridLength(1, GridUnitType.Star) }
            },
                RowDefinitions = new RowDefinitionCollection
            {
                new RowDefinition { Height = new GridLength(1, GridUnitType.Star)},
                new RowDefinition { Height = new GridLength(9, GridUnitType.Star)}
            }
            };

            Entry searchEntry = new Entry
            {
                Placeholder = "Search here"
            };
            searchEntry.TextChanged += SearchEntry_TextChanged;
            contentStack.Children.Add(searchEntry);

            availableIconsListView = new ListView
            {
                ItemTemplate = new DataTemplate(typeof(TextCell))
            };
            availableIconsListView.ItemTemplate.SetBinding(TextCell.TextProperty, "text");
            availableIconsListView.ItemsSource = allIconographies;
            availableIconsListView.ItemTapped += AvailableIconsListView_ItemTapped;
            listGrid.Children.Add(availableIconsListView, 0, 1);

            Label infoLabel = new Label
            {
                HorizontalOptions = LayoutOptions.Center,
                Text = "Selected Iconographys"
            };
            listGrid.Children.Add(infoLabel, 1, 0);

            Label infoLabel2 = new Label
            {
                HorizontalOptions = LayoutOptions.Center,
                Text = "Available Iconographys"
            };
            listGrid.Children.Add(infoLabel2, 0, 0);

            selectedListView = new ListView
            {
                ItemTemplate = new DataTemplate(typeof(TextCell))
            };
            selectedListView.ItemTemplate.SetBinding(TextCell.TextProperty, "text");
            selectedListView.ItemsSource = selectedIconographies;
            selectedListView.ItemTapped += SelectedListView_ItemTapped;
            listGrid.Children.Add(selectedListView, 1, 1);

            contentStack.Children.Add(listGrid);

            StackLayout exclusiveSearchStack = new StackLayout
            {
                HorizontalOptions = LayoutOptions.Center,
                Orientation = StackOrientation.Horizontal
            };
            Label exclusiveLabel = new Label
            {
                Text = "Exclusive Search"
            };
            exclusiveSearchStack.Children.Add(exclusiveLabel);

            exclusiveSwitch = new Switch
            {
                IsToggled = false
            };
            exclusiveSearchStack.Children.Add(exclusiveSwitch);
            contentStack.Children.Add(exclusiveSearchStack);

            Button searchButton = new Button
            {
                WidthRequest = 150,
                BackgroundColor = Color.Accent,
                TextColor = Color.White,
                HorizontalOptions = LayoutOptions.Center,
                Text = "Search"
            };
            searchButton.Clicked += SearchButton_Clicked;
            contentStack.Children.Add(searchButton);

            Content = contentStack;
        }

        private void SearchButton_Clicked(object sender, EventArgs e)
        {
            if (exclusiveSwitch.IsToggled)
            {
                if (selectedIconographies.Count > 0)
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
                if (selectedIconographies.Count > 0)
                {
                    List<PaintedRepresentationModel> paintedRepresentationModels = Logic.Kucha.GetPaintedRepresentationsByIconographies(selectedIconographies, false);
                    if (paintedRepresentationModels == null)
                        UserDialogs.Instance.Toast("Error");
                    else
                        Navigation.PushAsync(new PaintedRepresentationResultUI(paintedRepresentationModels), true);
                }
                else
                {
                    List<PaintedRepresentationModel> paintedRepresentationModels = Logic.Kucha.GetAllPaintedRepresentations();
                    if (paintedRepresentationModels == null)
                        UserDialogs.Instance.Toast("Error");
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
            List<IconographyModel> newAvailable = new List<IconographyModel>(availableIconographies)
            {
                iconographyModel
            };
            newAvailable.Sort((x, y) => x.text.CompareTo(y.text));
            availableIconsListView.ItemsSource = newAvailable;
            selectedListView.ItemsSource = newSelected;
            selectedIconographies = newSelected;
            availableIconographies = newAvailable;
        }

        private void AvailableIconsListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            IconographyModel iconographyModel = e.Item as IconographyModel;
            List<IconographyModel> newSelected = new List<IconographyModel>(selectedIconographies)
            {
                iconographyModel
            };
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
            foreach (IconographyModel i in allIconographies)
            {
                if (i.text.IndexOf(e.NewTextValue, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    editedList.Add(i);
                }
            }
            availableIconsListView.ItemsSource = editedList;
        }
    }
}