using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CraftTools.Models
{
    public class WareMaterial : INotifyPropertyChanged
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

        private string _name;
        private string _description;
        private double _price;
        private double _length;
        private double _width;
        private bool _haveSize;
        private double _cubicCM;
        private double _customPrice;

        #endregion

        #region Properties

        public int WareMaterialId { get; set; }

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }

        public string Description
        {
            get => _description;
            set
            {
                _description = value;
                OnPropertyChanged();
            }
        }

        public double Price
        {
            get => _price;
            set
            {
                _price = value;
                CubicCM = Price / (Length * Width);
                OnPropertyChanged();
            }
        }

        public double Length
        {
            get => _length;
            set
            {
                _length = value;
                CustomPrice = CubicCM * Length * Width;
                OnPropertyChanged();
            }
        }

        public double Width
        {
            get => _width;
            set
            {
                _width = value;
                CustomPrice = CubicCM * Length * Width;
                OnPropertyChanged();
            }
        }

        public bool HaveSize
        {
            get => _haveSize;
            set
            {
                _haveSize = value;
                OnPropertyChanged();
            }
        }

        public Ware Ware { get; set; }

        [NotMapped]
        public double CubicCM
        {
            get => _cubicCM;
            set
            {
                _cubicCM = value;
                CustomPrice = CubicCM * Length * Width;
                OnPropertyChanged();
            }
        }

        [NotMapped]
        public double CustomPrice
        {
            get => _customPrice;
            set
            {
                _customPrice = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Methods

        public void Clear()
        {
            WareMaterialId = 0;
            Name = null;
            HaveSize = false;
            Length = 0;
            Width = 0;
            Description = null;
            Price = 0;
        }

        #endregion
    }
}
