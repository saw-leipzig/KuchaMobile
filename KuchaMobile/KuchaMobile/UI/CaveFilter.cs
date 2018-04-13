using KuchaMobile.Logic;
using KuchaMobile.Logic.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace KuchaMobile.UI
{
    public class CaveFilter : ContentPage
    {
        StackLayout listStack;
        public enum CAVE_FILTER_TYPE
        {
            SITE,
            DISTRICT,
            REGION
        }
        CAVE_FILTER_TYPE type;
        CaveSearchUI parent;

        public CaveFilter(CAVE_FILTER_TYPE type, CaveSearchUI parent)
        {
            this.type = type;
            this.parent = parent;
            switch(type)
            {
                case CAVE_FILTER_TYPE.DISTRICT: Title = "Districts"; break;
                case CAVE_FILTER_TYPE.REGION: Title = "Regions"; break;
                case CAVE_FILTER_TYPE.SITE: Title = "Sites"; break;
            }
            StackLayout finalStack = new StackLayout();

            listStack = new StackLayout();
            if(type== CAVE_FILTER_TYPE.DISTRICT)
            {
                foreach(CaveDistrictModel caveDistrict in Kucha.GetCaveDistricts())
                {
                    listStack.Children.Add(new CaveDistrictGrid(caveDistrict, parent.pickedDistricts.Contains(caveDistrict)));
                }
            }
            else if(type == CAVE_FILTER_TYPE.REGION)
            {
                foreach (CaveRegionModel caveRegion in Kucha.GetCaveRegions())
                {
                    listStack.Children.Add(new CaveRegionGrid(caveRegion, parent.pickedRegions.Contains(caveRegion)));
                }
            }
            else if (type == CAVE_FILTER_TYPE.SITE)
            {
                foreach (CaveSiteModel caveSite in Kucha.GetCaveSites())
                {
                    listStack.Children.Add(new CaveSiteGrid(caveSite, parent.pickedSites.Contains(caveSite)));
                }
            }
            ScrollView scrollView = new ScrollView();
            scrollView.Content = listStack;

            finalStack.Children.Add(scrollView);
            Button doneButton = new Button();
            doneButton.Text = "Fertig";
            doneButton.Clicked += DoneButton_Clicked;
            finalStack.Children.Add(doneButton);

            Content = finalStack;
        }

        private void DoneButton_Clicked(object sender, EventArgs e)
        {
            if(type == CAVE_FILTER_TYPE.DISTRICT)
            {
                List<CaveDistrictModel> selectedModels = new List<CaveDistrictModel>();
                foreach(CaveDistrictGrid grid in listStack.Children)
                {
                    if ((grid.Children[1] as Switch).IsToggled)
                        selectedModels.Add(grid.caveDistrictModel);
                }
                parent.pickedDistricts = selectedModels;
                Navigation.PopModalAsync();
            }
            else if(type == CAVE_FILTER_TYPE.REGION)
            {
                List<CaveRegionModel> selectedModels = new List<CaveRegionModel>();
                foreach (CaveRegionGrid grid in listStack.Children)
                {
                    if ((grid.Children[1] as Switch).IsToggled)
                        selectedModels.Add(grid.caveRegionModel);
                }
                parent.pickedRegions = selectedModels;
                Navigation.PopModalAsync();
            }
            else if(type == CAVE_FILTER_TYPE.SITE)
            {
                List<CaveSiteModel> selectedModels = new List<CaveSiteModel>();
                foreach (CaveSiteGrid grid in listStack.Children)
                {
                    if ((grid.Children[1] as Switch).IsToggled)
                        selectedModels.Add(grid.caveSiteModel);
                }
                parent.pickedSites = selectedModels;
                Navigation.PopModalAsync();
            }
        }

        private class CaveDistrictGrid : Grid
        {
            public CaveDistrictModel caveDistrictModel;
            public CaveDistrictGrid(CaveDistrictModel caveDistrictModel, bool enabled)
            {
                this.caveDistrictModel = caveDistrictModel;
                Label nameLabel = new Label();
                nameLabel.Text = caveDistrictModel.name;
                Children.Add(nameLabel, 0, 0);

                Switch enabledSwitch = new Switch();
                enabledSwitch.IsToggled = enabled;
                Children.Add(enabledSwitch, 1, 0);
            }
        }

        private class CaveRegionGrid : Grid
        {
            public CaveRegionModel caveRegionModel;
            public CaveRegionGrid(CaveRegionModel caveRegionModel, bool enabled)
            {
                this.caveRegionModel = caveRegionModel;
                Label nameLabel = new Label();
                nameLabel.Text = caveRegionModel.englishName;
                Children.Add(nameLabel, 0, 0);

                Switch enabledSwitch = new Switch();
                enabledSwitch.IsToggled = enabled;
                Children.Add(enabledSwitch, 1, 0);
            }
        }

        private class CaveSiteGrid : Grid
        {
            public CaveSiteModel caveSiteModel;
            public CaveSiteGrid(CaveSiteModel caveSiteModel, bool enabled)
            {
                this.caveSiteModel = caveSiteModel;
                Label nameLabel = new Label();
                nameLabel.Text = caveSiteModel.name;
                Children.Add(nameLabel, 0, 0);

                Switch enabledSwitch = new Switch();
                enabledSwitch.IsToggled = enabled;
                Children.Add(enabledSwitch, 1, 0);
            }
        }
    }
}
