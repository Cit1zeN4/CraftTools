using CraftTools.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CraftTools.ViewModels
{
    public class ProfitViewModel : INotifyPropertyChanged
    {
        #region PropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        protected void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            var handler = PropertyChanged;
            handler?.Invoke(this, e);
        }

        #endregion

        #region Constructors

        public ProfitViewModel()
        {
            Profits = new ObservableCollection<Profit>();

            using (_context = new CraftToolsContext())
            {
                foreach(Profit p in _context.Profits)
                {
                    Profits.Add(p);
                }
            }
        }

        #endregion

        #region Fields

        CraftToolsContext _context;
        Profit _selectedProfit;

        #endregion

        #region Properties

        public ObservableCollection<Profit> Profits { get; set; }

        public Profit SelectedProfit
        {
            get
            {
                if (_selectedProfit == null)
                    return null;
                else
                    return _selectedProfit;
            }
            set
            {
                _selectedProfit = value;
                OnPropertyChanged();
            }
        }

        #endregion
    }
}
