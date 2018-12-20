﻿using CraftTools.ViewModels;
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
    /// Логика взаимодействия для LossView.xaml
    /// </summary>
    public partial class LossView : UserControl
    {
        public LossView()
        {
            InitializeComponent();
        }

        private async void AddLoss_DialogClosing(object sender, MaterialDesignThemes.Wpf.DialogClosingEventArgs eventArgs)
        {
            if (Equals(eventArgs.Parameter, true))
            {
                LossViewModel viewModel = (LossViewModel)LossViewMainGrid.DataContext;
                await viewModel.AddLossMethodAsync();
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (IsLoaded)
            {
                LossViewModel viewModel = (LossViewModel)LossViewMainGrid.DataContext;
                if (!viewModel.IsDataLoaded)
                {
                    System.ComponentModel.BackgroundWorker worker = new System.ComponentModel.BackgroundWorker();
                    worker.DoWork += (obj, ea) => viewModel.LoadData();
                    worker.RunWorkerAsync();
                }
            }
        }
    }
}
