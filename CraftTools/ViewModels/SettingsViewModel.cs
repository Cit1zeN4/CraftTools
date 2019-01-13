using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CraftTools.ViewModels
{
    public class SettingsViewModel : INotifyPropertyChanged
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

        #endregion

        #region Properties

        public string DBServerName
        {
            get => Properties.Settings.Default.DBServerName;
            set
            {
                Properties.Settings.Default.DBServerName = value;
                OnPropertyChanged();
            }
        }

        public string DBDatabaseName
        {
            get => Properties.Settings.Default.DBDatabaseName;
            set
            {
                Properties.Settings.Default.DBDatabaseName = value;
                OnPropertyChanged();
            }
        }

        public bool DBUseIntegratedSecurity
        {
            get => !Properties.Settings.Default.DBUseIntegratedSecurity;
            set
            {
                Properties.Settings.Default.DBUseIntegratedSecurity = !value;
                OnPropertyChanged();
            }
        }

        public string DBUserName
        {
            get => Properties.Settings.Default.DBUserName;
            set
            {
                Properties.Settings.Default.DBUserName = value;
                OnPropertyChanged();
            }
        }

        public string DBPassword
        {
            get => Properties.Settings.Default.DBPassword;
            set
            {
                Properties.Settings.Default.DBPassword = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Methods

        #endregion

        #region Command

        #region Command Fields

        #endregion

        #region Command Properties

        #endregion

        #region Command Methods

        #endregion

        #endregion
    }
}
