using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CraftTools.Models
{
    public class Ware : INotifyPropertyChanged
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

        #region Constractors

        public Ware()
        {
            WareMaterials = new ObservableCollection<WareMaterial>();
        }

        #endregion

        #region Fields

        private byte[] _image;
        private string _name;
        private string _description;
        private double _price;

        #endregion

        #region Properties

        public int WareId { get; set; }

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
                if (_price != value)
                {
                    _price = value;
                    OnPropertyChanged();
                }
            }
        }

        public ObservableCollection<WareMaterial> WareMaterials { get; set; }

        #endregion

        #region Methods

        public void Clear()
        {
            WareId = 0;
            Image = null;
            Name = null;
            Description = null;
            Price = 0;
        }

        #endregion
    }
}
