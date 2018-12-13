using CraftTools.Models;
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
    /// Логика взаимодействия для ProfitView.xaml
    /// </summary>
    public partial class ProfitView : UserControl
    {
        public ProfitView()
        {
            InitializeComponent();
        }

        private async void AddProfit_DialogClosing(object sender, MaterialDesignThemes.Wpf.DialogClosingEventArgs eventArgs)
        {
            if (Equals(eventArgs.Parameter, true))
            {
                ProfitViewModel viewModel = (ProfitViewModel)ProfitViewMainGrid.DataContext;
                await viewModel.AddProfitMethodAsync();
            }
        }
    }
}
