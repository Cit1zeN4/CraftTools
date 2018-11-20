using CraftTools.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CraftTools.ViewModels
{
    public class ProfitViewModel
    {
        CraftToolsContext context;

        #region Constructors

        public ProfitViewModel()
        {
            Profits = new ObservableCollection<Profit>();

            using (context = new CraftToolsContext())
            {
                foreach(Profit p in context.Profits)
                {
                    Profits.Add(p);
                }
            }
        }

        #endregion

        #region Fields

        #endregion

        #region Properties

        public ObservableCollection<Profit> Profits { get; set; }

        #endregion
    }
}
