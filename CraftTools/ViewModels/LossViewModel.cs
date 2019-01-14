using CraftTools.Helpers;
using CraftTools.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CraftTools.ViewModels
{
    class LossViewModel : INotifyPropertyChanged
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

        public LossViewModel()
        {
            AddedLoss = new Loss();
        }

        #endregion

        #region Fields

        ObservableCollection<Loss> losses;
        CraftToolsContext context;
        Loss selectedLoss;
        Loss addedLoss;
        bool isReadOnly = true;
        bool isDataLoaded = false;
        double progressBarValue = 0;
        string editBoxCurentIcon = "Pencil";
        GridLength editBoxLength = new GridLength(0);
        double lossPrice;
        double profitPrice;
        double incomePrice;

        #endregion

        #region Properties

        public ObservableCollection<Loss> Losses
        {
            get => losses;
            set
            {
                losses = value;
                OnPropertyChanged();
            }
        }

        public Loss SelectedLoss
        {
            get
            {
                if (selectedLoss == null)
                    return null;
                else
                    return selectedLoss;
            }
            set
            {
                selectedLoss = value;
                EditBoxLength = new GridLength(8, GridUnitType.Star);
                OnPropertyChanged();
            }
        }

        public Loss AddedLoss
        {
            get => addedLoss;
            set
            {
                addedLoss = value;
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

        public double ProgressBarValue
        {
            get => progressBarValue;
            set
            {
                progressBarValue = value;
                OnPropertyChanged();
            }
        }

        public double LossPrice
        {
            get => lossPrice;
            set
            {
                lossPrice = Math.Round(value, 2);
                IncomePrice = ProfitPrice - LossPrice;
                OnPropertyChanged();
            }
        }

        public double ProfitPrice
        {
            get => profitPrice;
            set
            {
                profitPrice = Math.Round(value, 2);
                IncomePrice = ProfitPrice - LossPrice;
                OnPropertyChanged();
            }
        }

        public double IncomePrice
        {
            get => incomePrice;
            set
            {
                incomePrice = Math.Round(value, 2);
                OnPropertyChanged();
            }
        }

        #endregion

        #region Methods

        public async void LoadData()
        {
            List<Loss> list;
            List<double> listProfitPrice;
            using (context = new CraftToolsContext())
            {
                list = await context.Losses.ToListAsync();
                listProfitPrice = await context.Profits.Select(o => o.Price).ToListAsync();
            }
            ProgressBarValue = 100;
            Losses = new ObservableCollection<Loss>(list);
            LossPrice = list.Sum(o => o.Price);
            ProfitPrice = listProfitPrice.Sum();
            IsDataLoaded = true;
        }

        #endregion

        #region Commands

        #region Command Fields

        BaseCommand saveChangesCmd;
        BaseCommand deleteLosssCmd;

        #endregion

        #region Command Properties

        public BaseCommand SaveChangesCommand
        {
            get => saveChangesCmd ?? (saveChangesCmd = new BaseCommand(obj => saveChangesMethodAsync()));
        }

        public BaseCommand DeleteLossCommand
        {
            get => deleteLosssCmd ?? (deleteLosssCmd = new BaseCommand(obj => DeleteLossMethod()));
        }

        #endregion

        #region Command Methods

        private void Comparison(ref Loss loss)
        {
            if (loss.LossId != SelectedLoss.LossId)
                loss.LossId = SelectedLoss.LossId;

            if (loss.Name != SelectedLoss.Name)
                loss.Name = SelectedLoss.Name;

            if (loss.Description != SelectedLoss.Description)
                loss.Description = SelectedLoss.Description;

            if (loss.Price != SelectedLoss.Price)
                loss.Price = SelectedLoss.Price;
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
                    var loss = context.Losses
                        .Where(x => x.LossId == SelectedLoss.LossId)
                        .FirstOrDefault();
                    Comparison(ref loss);
                    await context.SaveChangesAsync();
                    LossPrice = Losses.Sum(o => o.Price);
                    EditBoxCurentIcon = "Pencil";
                }
            }
        }

        public async Task AddLossMethodAsync()
        {
            using (context = new CraftToolsContext())
            {
                try
                {
                    context.Losses.Add(AddedLoss);
                    await context.SaveChangesAsync();
                    Losses.Add(new Loss { Name = AddedLoss.Name, Description = AddedLoss.Description, Price = AddedLoss.Price, LossId = AddedLoss.LossId });
                    AddedLoss.Clear();
                    LossPrice = Losses.Sum(o => o.Price);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка добавление: " + ex.Source + " " + ex.Message);
                }
            }
        }

        public async void DeleteLossMethod()
        {
            using (context = new CraftToolsContext())
            {
                try
                {
                    Loss prof = context.Losses.Where(o => o.LossId == SelectedLoss.LossId).FirstOrDefault();
                    context.Losses.Remove(prof);
                    await context.SaveChangesAsync();
                    Losses.Remove(SelectedLoss);
                    LossPrice = Losses.Sum(o => o.Price);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка удаления: " + ex.Source + " " + ex.Message);
                }
            }
        }

        #endregion

        #endregion
    }
}
