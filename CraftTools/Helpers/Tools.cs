using CraftTools.Models;
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

        public static WareMaterial ConvertMaterialToWareMaterial(Material material)
        {
            WareMaterial wareMaterial = new WareMaterial
            {
                Name = material.Name,
                Description = material.Description,
                HaveSize = material.HaveSize,
                Length = material.Length,
                Width = material.Width,
                Price = material.Price
            };
            return wareMaterial;
        }

        public static void WareMaterialApplyChanges(ref WareMaterial wareMaterial, ChangerModel model)
        {
            wareMaterial.Length = model.WareMaterial.Length;
            wareMaterial.Width = model.WareMaterial.Width;
            wareMaterial.CustomPrice = model.WareMaterial.CustomPrice;
        }
    }
}
