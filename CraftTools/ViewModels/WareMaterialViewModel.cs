using CraftTools.Helpers;
using CraftTools.Models;
using CraftTools.Properties;
using Microsoft.EntityFrameworkCore;
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
        double totalPrice;
        bool isDataLoaded = false;
        bool userPriceMarkupButton = true;
        string userPriceMarkupIcon = "CurrencyUsd";
        DbContextOptionsBuilder builder = new DbContextOptionsBuilder();

        #endregion

        #region Constructors

        public WareMaterialViewModel()
        {
            AddedWareMaterial = new WareMaterial();
            builder.UseNpgsql(Tools.GetConnectionString());
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

        #endregion

        #region Methods

        public async void LoadData()
        {
            List<Material> list;
            using (context = new CraftToolsContext(builder.Options))
            {
                list = await context.Materials.ToListAsync();
            }
            Materials = new ObservableCollection<Material>(list);
            BaseWareViewModel.AddedWare.WareMaterials.CollectionChanged += WareMaterials_CollectionChanged;
            IsDataLoaded = true;
        }

        //  При изменении коллекции происходит подписка и отписка от события PropertyChanged
        //  метода CalcTotalPrice которые суммирует все значения CustomPrice во всей коллекции

        private void WareMaterials_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            BaseWareViewModel.AddedWare.Price = BaseWareViewModel.AddedWare.WareMaterials.Sum(n => n.CustomPrice);
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
            BaseWareViewModel.AddedWare.Price = BaseWareViewModel.AddedWare.WareMaterials.Sum(n => n.CustomPrice);
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

        private void AddUserPriceMarkupMethod()
        {
            if(userPriceMarkupButton)
            {
                if (BaseWareViewModel.AddedWare.WareMaterials.Where(o => o.Name == "Пользовательская наценка").FirstOrDefault() != null)
                {
                    BaseWareViewModel.AddedWare.WareMaterials.Remove(BaseWareViewModel.AddedWare.WareMaterials.FirstOrDefault(n => n.Name == "Пользовательская наценка"));
                }
                BaseWareViewModel.AddedWare.WareMaterials.Add(new WareMaterial { Name = "Пользовательская наценка", Price = 1, HaveSize = false });
                userPriceMarkupButton = false;
                UserPriceMarkupIcon = "CurrencyUsdOff";
            }
            else
            {
                BaseWareViewModel.AddedWare.WareMaterials.Remove(BaseWareViewModel.AddedWare.WareMaterials.FirstOrDefault(n => n.Name == "Пользовательская наценка"));
                userPriceMarkupButton = true;
                UserPriceMarkupIcon = "CurrencyUsd";
            }
        }

        #endregion

        #endregion
    }
}
