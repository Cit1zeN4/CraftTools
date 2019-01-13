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
    public class SelectedWareMaterialViewModel : INotifyPropertyChanged
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
        Material selectedMaterial;
        WareMaterial selectedWareMaterial;
        double totalPrice;
        bool isDataLoaded = false;
        bool userPriceMarkupButton = true;
        string userPriceMarkupIcon = "CurrencyUsd";

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

        public bool IsDataLoaded
        {
            get => isDataLoaded;
            set
            {
                isDataLoaded = value;
                OnPropertyChanged();
            }
        }

        public double TotalPrice
        {
            get => totalPrice;
            set
            {
                totalPrice = value;
                OnPropertyChanged();
            }
        }

        public string UserPriceMarkupIcon
        {
            get => userPriceMarkupIcon;
            set
            {
                userPriceMarkupIcon = value;
                OnPropertyChanged();
            }
        }

        public WareMaterialChanger Changer { get; set; }

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
            BaseWareViewModel.SelectedWare.WareMaterials.CollectionChanged += WareMaterials_CollectionChanged;
            foreach (WareMaterial wm in BaseWareViewModel.SelectedWare.WareMaterials)
                wm.PropertyChanged += CalcTotalPrice;
            IsDataLoaded = true;
        }

        //  При изменении коллекции происходит подписка и отписка от события PropertyChanged
        //  метода CalcTotalPrice которые суммирует все значения CustomPrice во всей коллекции

        private void WareMaterials_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            BaseWareViewModel.SelectedWare.Price = BaseWareViewModel.SelectedWare.WareMaterials.Sum(n => n.CustomPrice);
            if (e.OldItems != null)
            {
                foreach (WareMaterial item in e.OldItems)
                    item.PropertyChanged -= CalcTotalPrice;
            }
            if (e.NewItems != null)
            {
                foreach (WareMaterial item in e.NewItems)
                    item.PropertyChanged += CalcTotalPrice;
            }
        }

        private void CalcTotalPrice(object sender, PropertyChangedEventArgs e)
        {
            BaseWareViewModel.SelectedWare.Price = BaseWareViewModel.SelectedWare.WareMaterials.Sum(n => n.CustomPrice);
            Changer.Change((WareMaterial)sender);
        }

        #endregion

        #region Commands

        #region Command Fields

        BaseCommand fromMaterialToWareMaterialCmd;
        BaseCommand fromWareMaterialToMaterialCmd;
        BaseCommand addUserPriceMarkupCmd;

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

        public BaseCommand AddUserPriceMarkupCommand
        {
            get => addUserPriceMarkupCmd ?? (addUserPriceMarkupCmd = new BaseCommand(obj => AddUserPriceMarkupMethod()));
        }

        #endregion

        #region Command Methods

        private void FromMaterialToWareMaterialMethod()
        {
            try
            {
                BaseWareViewModel.SelectedWare.WareMaterials.Add(Tools.ConvertMaterialToWareMaterial(SelectedMaterial));
                Changer.Add(Tools.ConvertMaterialToWareMaterial(SelectedMaterial));
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ниодин элемент не выбрае");
            }
        }

        private void FromWareMaterialToMaterialMethod()
        {
            try
            {
                Changer.Delete(SelectedWareMaterial);
                baseWareViewModel.SelectedWare.WareMaterials.Remove(SelectedWareMaterial);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ниодин элемент не выбрае");
            }
        }

        private void AddUserPriceMarkupMethod()
        {
            if (userPriceMarkupButton)
            {
                if(BaseWareViewModel.SelectedWare.WareMaterials.Where(o => o.Name == "Пользовательская наценка").FirstOrDefault() != null)
                {
                    Changer.Delete(BaseWareViewModel.SelectedWare.WareMaterials.Where(o => o.Name == "Пользовательская наценка").FirstOrDefault());
                    BaseWareViewModel.SelectedWare.WareMaterials.Remove(BaseWareViewModel.SelectedWare.WareMaterials.FirstOrDefault(n => n.Name == "Пользовательская наценка"));
                }
                WareMaterial wareMaterial = new WareMaterial { Name = "Пользовательская наценка", Price = 1, HaveSize = false };
                BaseWareViewModel.SelectedWare.WareMaterials.Add(wareMaterial);
                Changer.Add(wareMaterial);
                userPriceMarkupButton = false;
                UserPriceMarkupIcon = "CurrencyUsdOff";
            }
            else
            {
                WareMaterial wareMaterial = BaseWareViewModel.SelectedWare.WareMaterials.FirstOrDefault(n => n.Name == "Пользовательская наценка");
                BaseWareViewModel.SelectedWare.WareMaterials.Remove(wareMaterial);
                Changer.Delete(wareMaterial);
                userPriceMarkupButton = true;
                UserPriceMarkupIcon = "CurrencyUsd";
            }
        }

        #endregion

        #endregion

    }
}
