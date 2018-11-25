using CraftTools.Helpers;
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

        CraftToolsContext context;
        Profit selectedProfit;
        bool isReadOnly = true;
        string editBoxCurentIcon = "Pencil";

        #endregion

        #region Properties

        public ObservableCollection<Profit> Profits { get; set; }

        public Profit SelectedProfit
        {
            get
            {
                if (selectedProfit == null)
                    return null;
                else
                    return selectedProfit;
            }
            set
            {
                selectedProfit = value;
                OnPropertyChanged();
            }
        }

        public bool IsReadOnly
        {
            get => isReadOnly;
            set
            {
                isReadOnly = value;
                OnPropertyChanged();
            }
        }

        public string EditBoxCurentIcon
        {
            get => editBoxCurentIcon;
            set
            {
                editBoxCurentIcon = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Commands

        #region Command Fields

        BaseCommand saveChangesCmd;

        #endregion

        #region Command Properties

        public BaseCommand SaveChanges
        {
            get => saveChangesCmd ?? (saveChangesCmd = new BaseCommand(obj => saveChangesMethod()));
        }

        #endregion

        #region Command Methods

        private void comparison(ref Profit profit)
        {
            if (profit.Id != SelectedProfit.Id)
                profit.Id = SelectedProfit.Id;

            if (profit.Name != SelectedProfit.Name)
                profit.Name = SelectedProfit.Name;

            if (profit.Description != SelectedProfit.Description)
                profit.Description = SelectedProfit.Description;

            if (profit.Price != SelectedProfit.Price)
                profit.Price = SelectedProfit.Price;
        }

        private void saveChangesMethod()
        {
            using (context = new CraftToolsContext())
            {
                if (IsReadOnly == true)
                {
                    IsReadOnly = false;
                    EditBoxCurentIcon = "Check";
                }
                else
                {
                    IsReadOnly = true;
                    var prof = context.Profits
                        .Where(x => x.Id == SelectedProfit.Id)
                        .FirstOrDefault();
                    comparison(ref prof);
                    context.SaveChanges();
                    EditBoxCurentIcon = "Pencil";
                }
            }
        }

        #endregion

        #endregion
    }
}
