using CraftTools.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CraftTools.Views
{
    /// <summary>
    /// Логика взаимодействия для WareView.xaml
    /// </summary>
    public partial class WareView : UserControl
    {
        public WareView()
        {
            InitializeComponent();
        }

        private async void AddMateril_DialogClosing(object sender, MaterialDesignThemes.Wpf.DialogClosingEventArgs eventArgs)
        {
            if (Equals(eventArgs.Parameter, true))
            {
                WareViewModel viewModel = (WareViewModel)WareViewMainGrid.DataContext;
                await viewModel.AddWareMethodAsync();
            }
            else
            {
                WareViewModel viewModel = (WareViewModel)WareViewMainGrid.DataContext;
                viewModel.AddedWare.Clear();
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (IsLoaded)
            {
                WareViewModel viewModel = (WareViewModel)WareViewMainGrid.DataContext;
                if (!viewModel.IsDataLoaded)
                {
                    System.ComponentModel.BackgroundWorker worker = new System.ComponentModel.BackgroundWorker();
                    worker.DoWork += (obj, ea) => viewModel.LoadData();
                    worker.RunWorkerAsync();
                }
            }
        }

        private void AddWareImageButton_MouseLeave(object sender, MouseEventArgs e)
        {
            AddWareImageButton.Opacity = 0;
        }

        private void AddWareImageButton_MouseEnter(object sender, MouseEventArgs e)
        {
            AddWareImageButton.Opacity = 1;
        }
    }
}
