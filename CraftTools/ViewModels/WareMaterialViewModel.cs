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

        WareMaterial addedWareMaterial;
        Material selectedMaterial;

        #endregion

        #region Constructors

        public WareMaterialViewModel()
        {
            Materials = new ObservableCollection<Material>();
            Materials.Add(new Material { Name = "Test" });
            AddedWareMaterial = new WareMaterial();
        }

        #endregion

        #region Properties

        public ObservableCollection<Material> Materials { get; set; }

        public Material SelectedMaterial
        {
            get => selectedMaterial;
            set
            {
                selectedMaterial = value;
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

        #endregion

        #region Methods

        #endregion

        #region Commands

        #region Command Fields



        #endregion

        #region Command Properties



        #endregion

        #region Command Methods

        #endregion

        #endregion
    }
}
