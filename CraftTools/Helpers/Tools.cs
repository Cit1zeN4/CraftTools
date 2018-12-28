using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace CraftTools.Helpers
{
    public class Tools
    {
        public static byte[] ImageToByteArrayFromFilePath(string imagefilePath)
        {
            byte[] imageArray = File.ReadAllBytes(imagefilePath);
            return imageArray;
        }
    }
}
