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
    /// Логика взаимодействия для MaterialView.xaml
    /// </summary>
    public partial class MaterialView : UserControl
    {
        public MaterialView()
        {
            InitializeComponent();
        }

        private async void AddMateril_DialogClosing(object sender, MaterialDesignThemes.Wpf.DialogClosingEventArgs eventArgs)
        {
            if (Equals(eventArgs.Parameter, true))
            {
                MaterialViewModel viewModel = (MaterialViewModel)MaterialViewMainGrid.DataContext;
                await viewModel.AddMaterialMethodAsync();
            }
            else
            {
                MaterialViewModel viewModel = (MaterialViewModel)MaterialViewMainGrid.DataContext;
                viewModel.AddedMaterial.Clear();
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (IsLoaded)
            {
                MaterialViewModel viewModel = (MaterialViewModel)MaterialViewMainGrid.DataContext;
                if (!viewModel.IsDataLoaded)
                {
                    System.ComponentModel.BackgroundWorker worker = new System.ComponentModel.BackgroundWorker();
                    worker.DoWork += (obj, ea) => viewModel.LoadData();
                    worker.RunWorkerAsync();
                }
            }
        }

        private void AddMaterialImageButton_MouseLeave(object sender, MouseEventArgs e)
        {
            AddMaterialImageButton.Opacity = 0;
        }

        private void AddMaterialImageButton_MouseEnter(object sender, MouseEventArgs e)
        {
            AddMaterialImageButton.Opacity = 1;
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            TextBox textBox = (sender as TextBox);
            e.Handled = (!(Char.IsDigit(e.Text, 0) && !((textBox.Text.IndexOf("-") == 0) && textBox.SelectionStart == 0))) &&
                ((e.Text.Substring(0, 1) != "-") || (textBox.Text.IndexOf("-") == 0) || textBox.SelectionStart != 0) &&
                ((e.Text.Substring(0, 1) != ".") || (textBox.Text.IndexOf(".") != -1) || (textBox.SelectionStart == 0) || (!Char.IsDigit(textBox.Text.Substring(textBox.SelectionStart - 1, 1), 0)) || ((textBox.Text.IndexOf(",") != -1))) &&
                ((e.Text.Substring(0, 1) != ",") || (textBox.Text.IndexOf(",") != -1) || (textBox.SelectionStart == 0) || (!Char.IsDigit(textBox.Text.Substring(textBox.SelectionStart - 1, 1), 0)) || ((textBox.Text.IndexOf(".") != -1)));
        }
    }
}
