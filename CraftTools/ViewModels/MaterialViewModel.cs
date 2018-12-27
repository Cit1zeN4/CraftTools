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
using Windows = System.Windows;

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
        string editBoxCurentIcon = "Pencil";
        Windows.GridLength editBoxLength = new Windows.GridLength(0);

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
                EditBoxLength = new Windows.GridLength(8, Windows.GridUnitType.Star);
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

        public string EditBoxCurentIcon
        {
            get => editBoxCurentIcon;
            set
            {
                editBoxCurentIcon = value;
                OnPropertyChanged();
            }
        }

        public Windows.GridLength EditBoxLength
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
            List<Material> list;
            using (context = new CraftToolsContext())
            {
                list = await context.Materials.ToListAsync();
            }
            Materials = new ObservableCollection<Material>(list);
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
            get => deleteProfitsCmd ?? (deleteProfitsCmd = new BaseCommand(obj => DeleteMaterialMethod()));
        }

        #endregion

        #region Command Methods

        private void Comparison(ref Material material)
        {
            if (material.Id != SelectedMaterial.Id)
                material.Id = SelectedMaterial.Id;

            if (material.Image != SelectedMaterial.Image)
                material.Image = SelectedMaterial.Image;

            if (material.Name != SelectedMaterial.Name)
                material.Name = SelectedMaterial.Name;

            if (material.Description != SelectedMaterial.Description)
                material.Description = SelectedMaterial.Description;

            if (material.Price != SelectedMaterial.Price)
                material.Price = SelectedMaterial.Price;
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
                    var material = context.Materials
                        .Where(x => x.Id == SelectedMaterial.Id)
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
                    Materials.Add(new Material { Name = AddedMaterial.Name, Description = AddedMaterial.Description, Price = AddedMaterial.Price, Id = AddedMaterial.Id, Image = AddedMaterial.Image });
                    AddedMaterial.Clear();
                }
                catch (Exception ex)
                {
                    Windows.MessageBox.Show("Ошибка добавление: " + ex.Source + " " + ex.Message);
                }
            }
        }

        public async void DeleteMaterialMethod()
        {
            using (context = new CraftToolsContext())
            {
                try
                {
                    Material material = context.Materials.Where(o => o.Id == SelectedMaterial.Id).FirstOrDefault();
                    context.Materials.Remove(material);
                    await context.SaveChangesAsync();
                    Materials.Remove(SelectedMaterial);
                }
                catch (Exception ex)
                {
                    Windows.MessageBox.Show("Ошибка удаления: " + ex.Source + " " + ex.Message);
                }
            }
        }

        #endregion

        #endregion
    }
}
