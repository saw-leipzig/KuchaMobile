using System;
using System.Collections.Generic;
using System.Text;

namespace KuchaMobile.Internal
{
    public static class ZoomExtension
    {
        /// <summary>
        /// This extension allows a basic zoom interaction on the Images
        /// </summary>
        /// <param name="self"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static double Clamp(this double self, double min, double max)
        {
            return Math.Min(max, Math.Max(self, min));
        }
    }
}
