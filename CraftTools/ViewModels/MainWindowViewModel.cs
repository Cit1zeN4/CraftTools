using CraftTools.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CraftTools.ViewModels
{
    public class MainWindowViewModel
    {
        public MainWindowViewModel()
        {
            var context = new CraftToolsContext();
            Material material = new Material { Name = "Name 1", Description = "Descrition 1", Price = 2.4f };
            context.Materials.Add(material);
            context.SaveChanges();
        }
    }

    public partial class MainWindow
    {

    }
}
