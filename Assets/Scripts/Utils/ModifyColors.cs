using UnityEngine;

namespace Utils
{
    public static class ModifyColors
    {
        /// <summary>
        /// Creates color with corrected brightness. Correction factor must be between -1 and 1.
        /// </summary>
        /// <param name="color">Color to correct.</param>
        /// <param name="correctionFactor">The brightness correction factor. Must be between -1 and 1. 
        /// Negative values produce darker colors.</param>
        /// <returns>
        /// Corrected <see cref="Color"/> structure.
        /// </returns>
        public static Color ChangeColorBrightness(Color color, float correctionFactor)
        {
            float red = (float) color.r * 255;
            float green = (float) color.g * 255;
            float blue = (float) color.b * 255;

            if (correctionFactor < 0)
            {
                correctionFactor = 1 + correctionFactor;
                red *= correctionFactor;
                green *= correctionFactor;
                blue *= correctionFactor;
            }
            else
            {
                red = (255 - red) * correctionFactor + red;
                green = (255 - green) * correctionFactor + green;
                blue = (255 - blue) * correctionFactor + blue;
            }

            return new Color(red / 255, green / 255, blue / 255, color.a);
        }
    }
}