using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

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
                if (_price != value)
                {
                    _price = value;
                    OnPropertyChanged();
                }
            }
        }

        public double Length
        {
            get => _length;
            set
            {
                _length = value;
                OnPropertyChanged();
            }
        }

        public double Width
        {
            get => _width;
            set
            {
                _width = value;
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
