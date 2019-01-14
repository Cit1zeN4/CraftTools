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
    class MaterialViewModel : INotifyPropertyChanged
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

        public MaterialViewModel()
        {
            AddedMaterial = new Material();
        }

        #endregion

        #region Fields

        ObservableCollection<Material> materials;
        CraftToolsContext context;
        Material selectedMaterial;
        Material addedMaterial;
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

        #endregion

        #region Properties

        public ObservableCollection<Material> Materials
        {
            get => materials;
            set
            {
                materials = value;
                OnPropertyChanged();
            }
        }

        public Material SelectedMaterial
        {
            get
            {
                if (selectedMaterial == null)
                    return null;
                else
                    return selectedMaterial;
            }
            set
            {
                selectedMaterial = value;
                EditBoxLength = new GridLength(8, GridUnitType.Star);
                OnPropertyChanged();
            }
        }

        public Material AddedMaterial
        {
            get => addedMaterial;
            set
            {
                addedMaterial = value;
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

        #endregion

        #region Methods

        public async void LoadData()
        {
            List<Material> list;
            List<double> listProfitPrice;
            List<double> listLossPrice;
            using (context = new CraftToolsContext())
            {
                list = await context.Materials.ToListAsync();
                listProfitPrice = await context.Profits.Select(o => o.Price).ToListAsync();
                listLossPrice = await context.Losses.Select(o => o.Price).ToListAsync();
            }
            ProgressBarValue = 100;
            Materials = new ObservableCollection<Material>(list);
            ProfitPrice = listProfitPrice.Sum();
            LossPrice = listLossPrice.Sum();
            IsDataLoaded = true;
        }

        #endregion

        #region Commands

        #region Command Fields

        BaseCommand saveChangesCmd;
        BaseCommand deleteMaterialCmd;
        BaseCommand addMaterialImageCmd;

        #endregion

        #region Command Properties

        public BaseCommand SaveChangesCommand
        {
            get => saveChangesCmd ?? (saveChangesCmd = new BaseCommand(obj => SaveChangesMethodAsync()));
        }

        public BaseCommand DeleteMaterialCommand
        {
            get => deleteMaterialCmd ?? (deleteMaterialCmd = new BaseCommand(obj => DeleteMaterialMethod()));
        }

        public BaseCommand AddMaterialImageCommand
        {
            get => addMaterialImageCmd ?? (addMaterialImageCmd = new BaseCommand(obj => AddMaterilImageMethod((int)obj)));
        }

        #endregion

        #region Command Methods

        private void Comparison(ref Material material)
        {
            if (material.MaterialId != SelectedMaterial.MaterialId)
                material.MaterialId = SelectedMaterial.MaterialId;

            if (material.Image != SelectedMaterial.Image)
                material.Image = SelectedMaterial.Image;

            if (material.Name != SelectedMaterial.Name)
                material.Name = SelectedMaterial.Name;

            if (material.Description != SelectedMaterial.Description)
                material.Description = SelectedMaterial.Description;

            if (material.Price != SelectedMaterial.Price)
                material.Price = SelectedMaterial.Price;

            if (material.HaveSize != SelectedMaterial.HaveSize)
                material.HaveSize = SelectedMaterial.HaveSize;

            if (material.Length != SelectedMaterial.Length)
                material.Length = SelectedMaterial.Length;

            if (material.Width != SelectedMaterial.Width)
                material.Width = SelectedMaterial.Width;
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
                    var material = context.Materials
                        .Where(x => x.MaterialId == SelectedMaterial.MaterialId)
                        .FirstOrDefault();
                    Comparison(ref material);
                    await context.SaveChangesAsync();
                    EditBoxCurentIcon = "Pencil";
                }
            }
        }

        public async Task AddMaterialMethodAsync()
        {
            using (context = new CraftToolsContext())
            {
                try
                {
                    context.Materials.Add(AddedMaterial);
                    await context.SaveChangesAsync();
                    Materials.Add(
                        new Material
                        {
                            Name = AddedMaterial.Name,
                            Description = AddedMaterial.Description,
                            Price = AddedMaterial.Price,
                            MaterialId = AddedMaterial.MaterialId,
                            Image = AddedMaterial.Image,
                            HaveSize = AddedMaterial.HaveSize,
                            Length = AddedMaterial.Length,
                            Width = AddedMaterial.Width
                        });
                    AddedMaterial.Clear();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка добавление: " + ex.Source + " " + ex.Message);
                }
            }
        }

        public async void DeleteMaterialMethod()
        {
            using (context = new CraftToolsContext())
            {
                try
                {
                    Material material = context.Materials.Where(o => o.MaterialId == SelectedMaterial.MaterialId).FirstOrDefault();
                    context.Materials.Remove(material);
                    await context.SaveChangesAsync();
                    Materials.Remove(SelectedMaterial);
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
                switch(i)
                {
                    case 1:
                        SelectedMaterial.Image = Tools.ImageToByteArrayFromFilePath(ofd.FileName);
                        break;
                    case 2:
                        AddedMaterial.Image = Tools.ImageToByteArrayFromFilePath(ofd.FileName);
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
