using CraftTools.Helpers;
using CraftTools.Models;
using Microsoft.Win32;
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

        #endregion

        #region Methods

        public async void LoadData()
        {
            List<Ware> list;
            using (context = new CraftToolsContext())
            {
                list = await context.Wares.ToListAsync();
            }
            ProgressBarValue = 100;
            Wares = new ObservableCollection<Ware>(list);
            IsDataLoaded = true;
        }

        #endregion

        #region Commands

        #region Command Fields

        BaseCommand saveChangesCmd;
        BaseCommand deleteWareCmd;
        BaseCommand addWareImageCmd;

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
            using (context = new CraftToolsContext())
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
                        .Where(x => x.WareId == SelectedWare.WareId)
                        .FirstOrDefault();
                    Comparison(ref Ware);
                    await context.SaveChangesAsync();
                    EditBoxCurentIcon = "Pencil";
                }
            }
        }

        public async Task AddWareMethodAsync()
        {
            using (context = new CraftToolsContext())
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
                            Image = AddedWare.Image
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
            using (context = new CraftToolsContext())
            {
                try
                {
                    Ware Ware = context.Wares.Where(o => o.WareId == SelectedWare.WareId).FirstOrDefault();
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

        #endregion

        #endregion
    }
}
