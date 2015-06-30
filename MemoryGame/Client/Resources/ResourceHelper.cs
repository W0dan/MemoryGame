using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace MemoryGame.Client.Resources
{
    public enum ImageResources
    {
        Victory,
        Defeat,
        CardBackside
    }

    public static class ResourceHelper
    {
        private static string ImageResourceRoot { get { return "Client/Resources/Images/"; } }

        public static ImageSource GetImage(ImageResources imageResource)
        {
            string imageName;

            switch (imageResource)
            {
                case ImageResources.Victory:
                    imageName = ImageResourceRoot + "victory-02.jpg";
                    break;
                case ImageResources.Defeat:
                    imageName = ImageResourceRoot + "Defeat.jpg";
                    break;
                case ImageResources.CardBackside:
                    imageName = ImageResourceRoot + "000.png";
                    break;
                default:
                    throw new ArgumentOutOfRangeException("imageResource", imageResource, null);
            }

            return GetImageResource(imageName);
        }

        public static ImageSource GetImage(string cardset, int imageNr)
        {
            var psResourceName = string.Format("{0:000}.png", imageNr);

            return GetImageResource(ImageResourceRoot + cardset + "/" + psResourceName);
        }

        private static ImageSource GetImageResource(string psResourceName)
        {
            var oUri = new Uri("pack://application:,,,/MemoryGame;component/" + psResourceName, UriKind.RelativeOrAbsolute);
            return BitmapFrame.Create(oUri);
        }
    }
}