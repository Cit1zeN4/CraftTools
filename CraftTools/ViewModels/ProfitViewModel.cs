using CraftTools.Helpers;
using CraftTools.Models;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Data.Entity;

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
            AddedProfit = new Profit();
        }

        #endregion

        #region Fields

        ObservableCollection<Profit> profits;
        CraftToolsContext context;
        Profit selectedProfit;
        Profit addedProfit;
        bool isReadOnly = true;
        bool isDataLoaded = false;
        string editBoxCurentIcon = "Pencil";
        GridLength editBoxLength = new GridLength(0);

        #endregion

        #region Properties

        public ObservableCollection<Profit> Profits
        {
            get => profits;
            set
            {
                profits = value;
                OnPropertyChanged();
            }
        }

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
                EditBoxLength = new GridLength(8, GridUnitType.Star);
                OnPropertyChanged();
            }
        }

        public Profit AddedProfit
        {
            get => addedProfit;
            set
            {
                addedProfit = value;
                OnPropertyChanged();
            }
        }

        public bool IsDataLoaded
        {
            get => isDataLoaded;
            set
            {
                isDataLoaded = value;
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

        public GridLength EditBoxLength
        {
            get => editBoxLength;
            set
            {
                editBoxLength = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Methods

        public async void LoadData()
        {
            List<Profit> list;
            using (context = new CraftToolsContext())
            {
               list = await context.Profits.ToListAsync();
            }
            Profits = new ObservableCollection<Profit>(list);
            IsDataLoaded = true;
        }

        #endregion

        #region Commands

        #region Command Fields

        BaseCommand saveChangesCmd;
        BaseCommand deleteProfitsCmd;

        #endregion

        #region Command Properties

        public BaseCommand SaveChangesCommand
        {
            get => saveChangesCmd ?? (saveChangesCmd = new BaseCommand(obj => saveChangesMethodAsync()));
        }

        public BaseCommand DeleteProfitCommand
        {
            get => deleteProfitsCmd ?? (deleteProfitsCmd = new BaseCommand(obj => DeleteProfitMethod()));
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

        private async void saveChangesMethodAsync()
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
                    await context.SaveChangesAsync();
                    EditBoxCurentIcon = "Pencil";
                }
            }
        }

        public async Task AddProfitMethodAsync()
        {
            using (context = new CraftToolsContext())
            {
                try
                {
                    context.Profits.Add(AddedProfit);
                    await context.SaveChangesAsync();
                    Profits.Add(new Profit { Name = AddedProfit.Name, Description = AddedProfit.Description, Price = AddedProfit.Price, Id = AddedProfit.Id });       
                    AddedProfit.Clear();
                }
                catch(Exception ex)
                {
                    MessageBox.Show("Ошибка добавление: " + ex.Source + " " + ex.Message);
                }
            }
        }

        public async void DeleteProfitMethod()
        {
            using (context = new CraftToolsContext())
            {
                try
                {
                    Profit prof = context.Profits.Where(o => o.Id == SelectedProfit.Id).FirstOrDefault();
                    context.Profits.Remove(prof);
                    await context.SaveChangesAsync();
                    Profits.Remove(SelectedProfit);
                }
                catch(Exception ex)
                {
                    MessageBox.Show("Ошибка удаления: " + ex.Source + " " + ex.Message);
                }
            }
        }

        #endregion

        #endregion
    }
}
