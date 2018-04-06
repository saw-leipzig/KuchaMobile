using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace KuchaMobile.UI
{
    public class CaveFilter : ContentPage
    {
        public enum CAVE_FILTER_TYPE
        {
            SITE,
            DISTRICT,
            REGION
        }

        public CaveFilter(CAVE_FILTER_TYPE type, CaveSearchUI parent)
        {
            switch(type)
            {
                case CAVE_FILTER_TYPE.DISTRICT: Title = "Districts"; break;
                case CAVE_FILTER_TYPE.REGION: Title = "Regions"; break;
                case CAVE_FILTER_TYPE.SITE: Title = "Sites"; break;
            }

        }
    }
}
