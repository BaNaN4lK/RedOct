﻿using RedOct.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace RedOct.View
{
    /// <summary>
    /// Логика взаимодействия для MainView.xaml
    /// </summary>
    public partial class MainView : Window
    {
        public MainView()
        {
            InitializeComponent();
            MainFrame.Navigate(new AuthorizationView());
            Manager.MainFrame = MainFrame;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.GoBack();
        }

        private void MainFrame_ContentRender(object sender, EventArgs e)
        {
            if (MainFrame.CanGoBack)
            {
                BatonBack.Visibility = Visibility.Visible;
            }
            else
            {
                BatonBack.Visibility = Visibility.Hidden;
            }
        }
    }
}
