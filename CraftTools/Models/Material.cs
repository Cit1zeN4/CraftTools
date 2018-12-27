using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CraftTools.Models
{
    public class Material : INotifyPropertyChanged
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

        private byte[] _image;
        private string _name;
        private string _description;
        private double _price;
        private double _length;
        private double _width;

        #endregion

        #region Properties

        public int Id { get; set; }

        public byte[] Image
        {
            get => _image;
            set
            {
                _image = value;
                OnPropertyChanged();
            }
        }

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
                if(_price != value)
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

        public bool HaveSize { get; set; }

        #endregion

        #region Methods

        public void Clear()
        {
            Id = 0;
            Image = null; 
            Name = null;
            Description = null;
            Price = 0;
        }

        #endregion
    }
}
