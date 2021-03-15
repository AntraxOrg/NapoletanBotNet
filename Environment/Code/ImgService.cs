using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NapoletanBot.Net.Environment.Code
{
    public static class ImageService
    {
        public static Point CenterPoint((int Width, int Height) affected, (int Width, int Height) maxSpace)
        {
            Point result = new Point(0, 0);
            if (affected.Height > maxSpace.Height || affected.Width > maxSpace.Width)
                throw new Exception("Not enough space.");

            result.X = (maxSpace.Width / 2) - (affected.Width / 2);
            result.Y = (maxSpace.Height / 2) - (affected.Height / 2);
            return result;
        }
        public static Image WriteTextOnImage(Image img, string text, Font font, Color textColor, Point position)
        {
            //first, create a dummy bitmap just to get a graphics object
            SizeF textSize = GetTextSizeF(text, font, Size.Empty);

            //create a new image of the right size
            using (var drawing = Graphics.FromImage(img))
            {
                drawing.SmoothingMode = SmoothingMode.AntiAlias;
                drawing.InterpolationMode = InterpolationMode.High;
                //create a brush for the text
                using (Brush textBrush = new SolidBrush(textColor))
                {
                    if (position == Point.Empty)
                        drawing.DrawString(text, font, textBrush, 0, 0);
                    else
                        drawing.DrawString(text, font, textBrush, position.X, position.Y);
                    drawing.Save();
                }
            }
            return img;
        }

        public static Image WriteImageOnImage(Image img, Image toWriteImg, Point position)
        {
            using (var drawing = Graphics.FromImage(img))
            {
                drawing.SmoothingMode = SmoothingMode.AntiAlias;
                drawing.InterpolationMode = InterpolationMode.High;

                drawing.DrawImage(toWriteImg, position);
            }
            return img;
        }

        /// <summary>
        /// Gets the size of the text depending on the text & font.
        /// </summary>
        public static SizeF GetTextSizeF(string text, Font font, Size minSize)
        {
            SizeF textSize;
            using (var img = new Bitmap(1, 1))
            {
                using (var drawing = Graphics.FromImage(img))
                {
                    //measure the string to see how big the image needs to be
                    textSize = drawing.MeasureString(text, font);
                    if (!minSize.IsEmpty)
                    {
                        textSize.Width = textSize.Width > minSize.Width ? textSize.Width : minSize.Width;
                        textSize.Height = textSize.Height > minSize.Height ? textSize.Height : minSize.Height;
                    }
                }
            }
            return textSize;
        }
    }
}
