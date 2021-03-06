﻿using CraftTools.Helpers;
using CraftTools.Models;
using CraftTools.Views;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CraftTools.ViewModels
{
    public class WareViewModel : INotifyPropertyChanged
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

        public WareViewModel()
        {
            AddedWare = new Ware();
            Changer = new WareMaterialChanger();
            builder.UseNpgsql(Tools.GetConnectionString());
        }

        #endregion

        #region Fields

        ObservableCollection<Ware> wares;
        CraftToolsContext context;
        Ware selectedWare;
        Ware addedWare;
        bool isReadOnly = true;
        bool isDataLoaded = false;
        bool isEnabled = false;
        double progressBarValue = 0;
        string editBoxCurentIcon = "Pencil";
        GridLength editBoxLength = new GridLength(0);
        Visibility isVisible = Visibility.Hidden;
        double lossPrice;
        double profitPrice;
        double incomePrice;
        DbContextOptionsBuilder builder = new DbContextOptionsBuilder();

        #endregion

        #region Properties

        public ObservableCollection<Ware> Wares
        {
            get => wares;
            set
            {
                wares = value;
                OnPropertyChanged();
            }
        }

        public Ware SelectedWare
        {
            get
            {
                if (selectedWare == null)
                    return null;
                else
                    return selectedWare;
            }
            set
            {
                selectedWare = value;
                EditBoxLength = new GridLength(8, GridUnitType.Star);
                OnPropertyChanged();
            }
        }

        public Ware AddedWare
        {
            get => addedWare;
            set
            {
                addedWare = value;
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

        public bool IsEnabled
        {
            get => isEnabled;
            set
            {
                isEnabled = value;
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

        public Visibility IsVisible
        {
            get => isVisible;
            set
            {
                isVisible = value;
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

        public WareMaterialChanger Changer { get; set; }

        #endregion

        #region Methods

        public async void LoadData()
        {
            List<Ware> list;
            List<double> listProfitPrice;
            List<double> listLossPrice;
            using (context = new CraftToolsContext(builder.Options))
            {
                list = await context.Wares.Include(w => w.WareMaterials).ToListAsync();
                listProfitPrice = await context.Profits.Select(o => o.Price).ToListAsync();
                listLossPrice = await context.Losses.Select(o => o.Price).ToListAsync();
            }
            ProgressBarValue = 100;
            Wares = new ObservableCollection<Ware>(list);
            ProfitPrice = listProfitPrice.Sum();
            LossPrice = listLossPrice.Sum();
            IsDataLoaded = true;
        }

        #endregion

        #region Commands

        #region Command Fields

        BaseCommand saveChangesCmd;
        BaseCommand deleteWareCmd;
        BaseCommand addWareImageCmd;
        BaseCommand openWareMaterilCmd;

        #endregion

        #region Command Properties

        public BaseCommand SaveChangesCommand
        {
            get => saveChangesCmd ?? (saveChangesCmd = new BaseCommand(obj => SaveChangesMethodAsync()));
        }

        public BaseCommand DeleteWareCommand
        {
            get => deleteWareCmd ?? (deleteWareCmd = new BaseCommand(obj => DeleteWareMethod()));
        }

        public BaseCommand AddWareImageCommand
        {
            get => addWareImageCmd ?? (addWareImageCmd = new BaseCommand(obj => AddMaterilImageMethod((int)obj)));
        }

        public BaseCommand OpenWareMaterialCommand
        {
            get => openWareMaterilCmd ?? (openWareMaterilCmd = new BaseCommand(obj => OpenWareMaterialViewMethod((bool)obj)));
        }

        #endregion

        #region Command Methods

        private void Comparison(ref Ware Ware)
        {
            if (Ware.WareId != SelectedWare.WareId)
                Ware.WareId = SelectedWare.WareId;

            if (Ware.Image != SelectedWare.Image)
                Ware.Image = SelectedWare.Image;

            if (Ware.Name != SelectedWare.Name)
                Ware.Name = SelectedWare.Name;

            if (Ware.Description != SelectedWare.Description)
                Ware.Description = SelectedWare.Description;

            if (Ware.Price != SelectedWare.Price)
                Ware.Price = SelectedWare.Price;
        }

        private async void SaveChangesMethodAsync()
        {
            using (context = new CraftToolsContext(builder.Options))
            {
                if (IsReadOnly == true)
                {
                    IsEnabled = true;
                    IsVisible = Visibility.Visible;
                    IsReadOnly = false;
                    EditBoxCurentIcon = "Check";
                }
                else
                {
                    IsEnabled = false;
                    IsVisible = Visibility.Hidden;
                    IsReadOnly = true;
                    var Ware = context.Wares
                        .Include(x => x.WareMaterials)
                        .Where(x => x.WareId == SelectedWare.WareId)
                        .FirstOrDefault();
                    Comparison(ref Ware);
                    foreach (ChangerModel cm in Changer.List)
                    {
                        if (cm.Status == ChangerModel.WareMaterialStatus.Added)
                            Ware.WareMaterials.Add(cm.WareMaterial);
                        if (cm.Status == ChangerModel.WareMaterialStatus.Deleted)
                        {
                            context.WareMaterials
                                .Remove(context.WareMaterials
                                .Where(o => o.WareMaterialId == cm.WareMaterial.WareMaterialId).FirstOrDefault());
                        }
                        if (cm.Status == ChangerModel.WareMaterialStatus.Changed)
                        {
                            WareMaterial wareMaterial = context.WareMaterials.Where(o => o.WareMaterialId == cm.WareMaterial.WareMaterialId).FirstOrDefault();
                            Tools.WareMaterialApplyChanges(ref wareMaterial, cm);
                        }
                    }

                    await context.SaveChangesAsync();
                    EditBoxCurentIcon = "Pencil";
                }
            }
        }

        public async Task AddWareMethodAsync()
        {
            using (context = new CraftToolsContext(builder.Options))
            {
                try
                {
                    context.Wares.Add(AddedWare);
                    await context.SaveChangesAsync();
                    Wares.Add(
                        new Ware
                        {
                            Name = AddedWare.Name,
                            Description = AddedWare.Description,
                            Price = AddedWare.Price,
                            WareId = AddedWare.WareId,
                            Image = AddedWare.Image,
                            WareMaterials = AddedWare.WareMaterials
                        });
                    AddedWare.Clear();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка добавление: " + ex.Source + " " + ex.Message);
                }
            }
        }

        public async void DeleteWareMethod()
        {
            using (context = new CraftToolsContext(builder.Options))
            {
                try
                {
                    Ware Ware = context.Wares
                        .Include(o => o.WareMaterials)
                        .FirstOrDefault(o => o.WareId == SelectedWare.WareId);
                    context.Wares.Remove(Ware);
                    await context.SaveChangesAsync();
                    Wares.Remove(SelectedWare);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка удаления: " + ex.Source + " " + ex.Message);
                }
            }
        }

        private void AddMaterilImageMethod(int i)
        {
            OpenFileDialog ofd = new OpenFileDialog
            {
                FilterIndex = 3,
                Filter = "Файлы изображений (*.bmp, *.jpg, *.png)|*.bmp;*.jpg;*.png"
            };
            if (ofd.ShowDialog() == true)
            {
                switch (i)
                {
                    case 1:
                        SelectedWare.Image = Tools.ImageToByteArrayFromFilePath(ofd.FileName);
                        break;
                    case 2:
                        AddedWare.Image = Tools.ImageToByteArrayFromFilePath(ofd.FileName);
                        break;
                    default:
                        MessageBox.Show("Ошибка добавления изображения");
                        break;
                }
            }
        }

        private void OpenWareMaterialViewMethod(bool flag)
        {
            if(flag)
            {
                WareMaterialView wareMaterial = new WareMaterialView();
                WareMaterialViewModel wareMaterialVM = (WareMaterialViewModel)wareMaterial.Resources["wareMaterialVM"];
                wareMaterialVM.BaseWareViewModel = this;
                wareMaterial.ShowDialog();
            }
            else
            {
                SelectedWareMaterialView wareMaterial = new SelectedWareMaterialView();
                SelectedWareMaterialViewModel wareMaterialVM = (SelectedWareMaterialViewModel)wareMaterial.Resources["wareMaterialVM"];
                Changer = new WareMaterialChanger(SelectedWare.WareMaterials);
                wareMaterialVM.Changer = Changer;
                wareMaterialVM.BaseWareViewModel = this;
                wareMaterial.ShowDialog();
            }
        }

        #endregion

        #endregion
    }
}
