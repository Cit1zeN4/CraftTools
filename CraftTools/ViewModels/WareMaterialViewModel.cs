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
    public class WareMaterialViewModel : INotifyPropertyChanged
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

        #region Fields

        WareViewModel baseWareViewModel;
        ObservableCollection<Material> materials;
        CraftToolsContext context;
        WareMaterial addedWareMaterial;
        Material selectedMaterial;
        WareMaterial selectedWareMaterial;
        bool isDataLoaded = false;

        #endregion

        #region Constructors

        public WareMaterialViewModel()
        {
            AddedWareMaterial = new WareMaterial();
        }

        #endregion

        #region Properties

        public WareViewModel BaseWareViewModel
        {
            get => baseWareViewModel;
            set
            {
                baseWareViewModel = value;
                OnPropertyChanged();
            }
        }

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
            get => selectedMaterial;
            set
            {
                selectedMaterial = value;
                OnPropertyChanged();
            }
        }

        public WareMaterial SelectedWareMaterial
        {
            get => selectedWareMaterial;
            set
            {
                selectedWareMaterial = value;
                OnPropertyChanged();
            }
        }

        public WareMaterial AddedWareMaterial
        {
            get => addedWareMaterial;
            set
            {
                addedWareMaterial = value;
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

        BaseCommand fromMaterialToWareMaterialCmd;
        BaseCommand fromWareMaterialToMaterialCmd;

        #endregion

        #region Command Properties

        public BaseCommand FromMaterialToWareMaterialCommand
        {
            get => fromMaterialToWareMaterialCmd ?? (fromMaterialToWareMaterialCmd = new BaseCommand(obj => FromMaterialToWareMaterialMethod()));
        }

        public BaseCommand FromWareMaterialToMaterialCommand
        {
            get => fromWareMaterialToMaterialCmd ?? (fromWareMaterialToMaterialCmd = new BaseCommand(obj => FromWareMaterialToMaterialMethod()));
        }

        #endregion

        #region Command Methods

        private void FromMaterialToWareMaterialMethod()
        {
            try
            {
                BaseWareViewModel.AddedWare.WareMaterials.Add(Tools.ConvertMaterialToWareMaterial(SelectedMaterial));
            }
            catch(Exception ex)
            {
                MessageBox.Show("Ниодин элемент не выбрае");
            }
        }

        private void FromWareMaterialToMaterialMethod()
        {
            try
            {
                baseWareViewModel.AddedWare.WareMaterials.Remove(SelectedWareMaterial);
            }
            catch(Exception ex)
            {
                MessageBox.Show("Ниодин элемент не выбрае");
            }
        }

        #endregion

        #endregion
    }
}
