﻿using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace MemoryGame.Client.Resources
{
    public static class ResourceHelper
    {
        public static string ImageResourceRoot { get { return "Client/Resources/Images/"; } }

        public static ImageSource GetImage(string psResourceName)
        {
            return GetImage("MemoryGame", psResourceName);
        }

        public static ImageSource GetImageRelative(string cardset, string psResourceName)
        {
            return GetImage("MemoryGame", ImageResourceRoot + cardset + "/" + psResourceName);
        }

        public static ImageSource GetImage(string psAssemblyName, string psResourceName)
        {
            var oUri = new Uri("pack://application:,,,/" + psAssemblyName + ";component/" + psResourceName, UriKind.RelativeOrAbsolute);
            return BitmapFrame.Create(oUri);
        }
    }
}