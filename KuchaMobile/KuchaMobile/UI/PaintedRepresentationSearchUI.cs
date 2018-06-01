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

        StackLayout availableIconsStack;
        StackLayout selectedIconsStack;
        public PaintedRepresentationSearchUI()
        {
            Title = "Painted Representation Search";
            allIconographies = new List<IconographyModel>(KuchaMobile.Logic.Kucha.GetIconographies());
            StackLayout contentStack = new StackLayout();
            Entry searchEntry = new Entry();
            searchEntry.Placeholder = "Hier suchen";
            searchEntry.TextChanged += SearchEntry_TextChanged;
            contentStack.Children.Add(searchEntry);
            ScrollView availableIconsScrollView = new ScrollView();
            availableIconsStack = new StackLayout();
            foreach(IconographyModel iconography in allIconographies)
            {
                IconStack i = new IconStack(iconography);
                availableIconsStack.Children.Add(i);
                TapGestureRecognizer availableIconTap = new TapGestureRecognizer();
                availableIconTap.Tapped += AvailableIconTap_Tapped;
                i.GestureRecognizers.Add(availableIconTap);
                i.selected = false;

            }
            availableIconsScrollView.Content = availableIconsStack;
            contentStack.Children.Add(availableIconsScrollView);
            Label infoLabel = new Label();
            infoLabel.Text = "Ausgewählte Iconographys";
            contentStack.Children.Add(infoLabel);
            ScrollView selectedIconsScrollView = new ScrollView();
            selectedIconsStack = new StackLayout();
            selectedIconsScrollView.Content = selectedIconsStack;
            contentStack.Children.Add(selectedIconsScrollView);

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

        private void ExclusiveSearchButton_Clicked(object sender, EventArgs e)
        {
            List<IconographyModel> iconographies = new List<IconographyModel>();
            foreach (IconStack iconStack in selectedIconsStack.Children)
            {
                iconographies.Add(iconStack.iconography);
            }
            List<PaintedRepresentationModel> paintedRepresentationModels = Logic.Kucha.GetPaintedRepresentationsByIconographies(iconographies, true);
            Navigation.PushAsync(new PaintedRepresentationResultUI(paintedRepresentationModels), true);
        }

        private void AnySearchButton_Clicked(object sender, EventArgs e)
        {
            List<IconographyModel> iconographies = new List<IconographyModel>();
            foreach(IconStack iconStack in selectedIconsStack.Children)
            {
                iconographies.Add(iconStack.iconography);
            }
            List<PaintedRepresentationModel> paintedRepresentationModels = Logic.Kucha.GetPaintedRepresentationsByIconographies(iconographies, false);
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
            availableIconsStack.Children.Clear();
            foreach (IconographyModel iconography in editedList)
            {
                IconStack i = new IconStack(iconography);
                availableIconsStack.Children.Add(i);
                TapGestureRecognizer availableIconTap = new TapGestureRecognizer();
                availableIconTap.Tapped += AvailableIconTap_Tapped;
                i.GestureRecognizers.Add(availableIconTap);
                i.selected = false;
            }
        }

        private void AvailableIconTap_Tapped(object sender, EventArgs e)
        {
            IconStack i = sender as IconStack;
            if(!i.selected)
            {
                i.selected=true;
                availableIconsStack.Children.Remove(i);
                selectedIconsStack.Children.Add(i);
            }
            else
            {
                i.selected = false;
                selectedIconsStack.Children.Remove(i);
                availableIconsStack.Children.Add(i);
            }

        }

        private class IconStack : StackLayout
        {
            public bool selected;
            public IconographyModel iconography;
            public IconStack(IconographyModel iconographyModel)
            {
                iconography = iconographyModel;
                BackgroundColor = Color.Orange;
                Label nameLabel = new Label();
                nameLabel.Text = iconographyModel.text;
                Children.Add(nameLabel);
                selected = false;
            }
        }
    }
}
