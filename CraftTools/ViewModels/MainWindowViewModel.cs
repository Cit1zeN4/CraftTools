using CraftTools.Helpers;
using CraftTools.Models;
using CraftTools.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace CraftTools.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
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

        #region Fileds

        UserControl currentView;

        #endregion

        #region Properties

        public UserControl CurrentView
        {
            get => currentView;
            set
            {
                currentView = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Commands

        #region Command Fields

        BaseCommand changeCurrentViewCmd;

        #endregion

        #region Command Properties

        public BaseCommand ChangeCurrentViewCommand
        {
            get => changeCurrentViewCmd ?? (changeCurrentViewCmd = new BaseCommand(obj => ChangeCurrentView((int)obj)));
        }

        #endregion

        #region Command Methods

        private void ChangeCurrentView(int i)
        {
            switch (i)
            {
                case 1:
                    CurrentView = new ProfitView();
                    break;
                case 2:
                    CurrentView = new LossView();
                    break;
                case 3:
                    CurrentView = new MaterialView();
                    break;
                case 4:
                    CurrentView = new WareView();
                    break;
                default:
                    MessageBox.Show("Ошибка открытия view");
                    break;
            }
        }

        #endregion

        #endregion
    }
}

    public partial class MainWindow
    {

    }
